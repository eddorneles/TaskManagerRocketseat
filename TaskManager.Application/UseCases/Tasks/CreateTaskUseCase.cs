using TaskManager.Communication.Requests;
using TaskManager.Communication.Responses;
using TaskManager.Repositories;

namespace TaskManager.Application.UseCases.Tasks;

public class CreateTaskUseCase {


    public async Task<TaskResponse> Execute( TaskRequest newTask ) {
        var repository = new TaskRepository();
        var taskCreated = await repository.InsertAsync( newTask );
        return taskCreated;
    }
}