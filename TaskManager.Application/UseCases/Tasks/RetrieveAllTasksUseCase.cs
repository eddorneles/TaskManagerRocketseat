using TaskManager.Communication.Responses;
using TaskManager.Repositories;

namespace TaskManager.Application.UseCases.Tasks;

public class RetrieveAllTasksUseCase {
    public async Task<MultipleTasksResponse> Execute() {
        var multiTasks = new MultipleTasksResponse();
        var repository = new TaskRepository();
        multiTasks.Tasks = await repository.SelectAllAsync();
        return multiTasks;
    }
}