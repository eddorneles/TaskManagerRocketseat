using TaskManager.Communication.Requests;
using TaskManager.Repositories;

namespace TaskManager.Application.UseCases.Tasks;

public class UpdateTaskUseCase {
    public async Task<bool> Execute( int id, TaskRequest request) {
        var repository = new TaskRepository();
        bool wasUpdated = await repository.UpdateByIdAsync(id, request);
        return wasUpdated;
    }
}