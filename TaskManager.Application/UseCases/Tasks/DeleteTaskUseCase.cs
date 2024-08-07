using TaskManager.Repositories;

namespace TaskManager.Application.UseCases.Tasks;

public class DeleteTaskUseCase {
    public async Task<bool> Execute( int id ) {
        var repository = new TaskRepository();
        bool wasDeleted = await repository.DeleteByIdAsync(id);
        return wasDeleted;
    }
}