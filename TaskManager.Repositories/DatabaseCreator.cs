

using Microsoft.Data.Sqlite;

namespace TaskManager.Repositories;

public static class DatabaseCreator {
    public static readonly string ConnectionString = "Data Source=..\\local-database.db";
    public static void Create() {
        using var connection = new SqliteConnection(ConnectionString);
        connection.OpenAsync();
        var command = connection.CreateCommand();
        command.CommandText = @"
            CREATE TABLE IF NOT EXISTS Task (
                id INTEGER PRIMARY KEY AUTOINCREMENT,
                name TEXT,
                description TEXT,
                priority int,
                deadline DATE,
                status int
            );
        ";
        command.ExecuteNonQuery();
        connection.CloseAsync();
    }
}