using TaskManager.Communication.Responses;
using TaskManager.Repositories;

namespace TaskManager.Application.UseCases.Tasks;

public class RetrieveTaskByIdUseCase {

    public async Task<TaskResponse> Execute( int id ) {
        var repository = new TaskRepository();
        var taskRetrieved = await repository.SelectByIdAsync(id);
        return taskRetrieved;
    }
    
}//END class