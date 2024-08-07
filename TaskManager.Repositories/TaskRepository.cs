using System.Data;
using Microsoft.Data.Sqlite;
using TaskManager.Communication;
using TaskManager.Communication.Requests;
using TaskManager.Communication.Responses;
using TaskStatus = TaskManager.Communication.TaskStatus;

namespace TaskManager.Repositories;

public class TaskRepository {
    private readonly string LocalDBPath = DatabaseCreator.ConnectionString;

    public async Task<TaskResponse> InsertAsync( TaskRequest newTask ) {
        var insertQuery = @"INSERT INTO task ( name, description, priority, deadline, status ) VALUES 
                                ($name, $description, $priority, $deadline, $status );
                            SELECT last_insert_rowid();";
        await using var dbConnection = new SqliteConnection(this.LocalDBPath);
        await dbConnection.OpenAsync();
        var command = dbConnection.CreateCommand();
        this.SetParametersToInsertOrUpdate( command.Parameters, newTask );
        command.CommandText = insertQuery;
        int registeredTaskId = Convert.ToInt16( await command.ExecuteScalarAsync() );
        var createdTask = new TaskResponse()
        {
            Name = newTask.Name,
            Description = newTask.Description,
            Status = (TaskStatus) newTask.Status,
            Deadline = newTask.Deadline,
            Priority = (TaskPriority) newTask.Priority,
            Id = registeredTaskId,
        };
        return createdTask;
    }
    
    public async Task<bool> UpdateByIdAsync( int id, TaskRequest task ) {
        var updateQuery = @"UPDATE task SET 
                                name = $name, 
                                description = $description, 
                                priority = $priority , 
                                deadline = $deadline, 
                                status = $status
                            WHERE id = $id;";
        await using var dbConnection = new SqliteConnection(this.LocalDBPath);
        await dbConnection.OpenAsync();
        var command = dbConnection.CreateCommand();
        this.SetParametersToInsertOrUpdate( command.Parameters, task );
        command.Parameters.AddWithValue( "$id", id );
        command.CommandText = updateQuery;
        var result = await command.ExecuteNonQueryAsync();
        return result > 0;
    }

    private void SetParametersToInsertOrUpdate( SqliteParameterCollection parameters,  TaskRequest task) {
        parameters.AddWithValue("$name", task.Name);
        parameters.AddWithValue("$description", task.Description);
        parameters.AddWithValue("$priority", task.Priority);
        parameters.AddWithValue("$deadline", task.Deadline);
        parameters.AddWithValue("$status", task.Status);
    }

    public async Task<List<TaskResponse>> SelectAllAsync() {
        var selectAllQuery = @"SELECT id, name, description, priority, deadline, status FROM task;";
        
        await using var dbConnection = new SqliteConnection(this.LocalDBPath);
        await dbConnection.OpenAsync();
        var command = dbConnection.CreateCommand();
        command.CommandText = selectAllQuery;
        var dbReader = await command.ExecuteReaderAsync();
        var retrievedTasks = new List<TaskResponse>();
        while ( await dbReader.ReadAsync() ){
            retrievedTasks.Add( await this.ConvertDatabaseTypesToEntity(dbReader) );
        }
        return retrievedTasks;
    }
    
    public async Task<TaskResponse> SelectByIdAsync( int id ) {
        var selectByIdQuery = @"SELECT id, name, description, priority, deadline, status FROM task WHERE id = $id;";
        
        await using var dbConnection = new SqliteConnection(this.LocalDBPath);
        await dbConnection.OpenAsync();
        var command = dbConnection.CreateCommand();
        command.CommandText = selectByIdQuery;
        command.Parameters.AddWithValue($"$id", id);
        var dbReader = await command.ExecuteReaderAsync();
        if (dbReader.HasRows){
            while (await dbReader.ReadAsync()){
                var retrievedTask = await this.ConvertDatabaseTypesToEntity( dbReader );
                return retrievedTask;
            }
        }
        return null;
    }

    private async Task<TaskResponse> ConvertDatabaseTypesToEntity(SqliteDataReader dbReader) {
        var retrievedTask = new TaskResponse();
        
        retrievedTask.Id = await dbReader.GetFieldValueAsync<int>("id");
        retrievedTask.Name = await dbReader.GetFieldValueAsync<string>("name");
        retrievedTask.Description = await dbReader.GetFieldValueAsync<string>("description");
        retrievedTask.Status = (TaskStatus) await dbReader.GetFieldValueAsync<int>("status");
        retrievedTask.Priority = (TaskPriority) await dbReader.GetFieldValueAsync<int>("priority");
        retrievedTask.Deadline = DateOnly.Parse( await dbReader.GetFieldValueAsync<string>("deadline"));
        
        return retrievedTask;
    }
    
    public async Task<bool> DeleteByIdAsync(int id) {
        var deleteQuery = @"DELETE FROM task WHERE id = $id;";
        await using var dbConnection = new SqliteConnection(this.LocalDBPath);
        await dbConnection.OpenAsync();
        var command =  dbConnection.CreateCommand();
        command.CommandText = deleteQuery;
        command.Parameters.AddWithValue("$id", id);
        int result = await command.ExecuteNonQueryAsync();
        return result > 0;
    }

    
}//END class