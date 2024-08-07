using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Application.UseCases.Tasks;
using TaskManager.Communication.Requests;
using TaskManager.Communication.Responses;

namespace TaskManager.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        
        
        [HttpPost]
        [ProducesResponseType(typeof(TaskResponse), StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateNewTask( [FromBody] TaskRequest request ) {
            var useCase = new CreateTaskUseCase();
            TaskResponse task = await useCase.Execute(request);
            return base.Created(string.Empty, task);

        }

        [HttpGet]
        [ProducesResponseType(typeof(MultipleTasksResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> GetAll() {
            var useCase = new RetrieveAllTasksUseCase();
            var tasksResponse = await useCase.Execute();
            if (tasksResponse.Tasks.Any()){
                return Ok(tasksResponse);
            }
            return NoContent();
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(TaskResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(TaskResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById( [FromRoute] int id ) {
            var useCase = new RetrieveTaskByIdUseCase();
            var taskRetrieved = await useCase.Execute( id );
            if (taskRetrieved != null){
                return Ok(taskRetrieved);
            }
            return NotFound();
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponse) , StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PutById([FromRoute] int id, [FromBody] TaskRequest request) {
            var useCase = new UpdateTaskUseCase();
            try{
                await useCase.Execute(id, request);
                return NoContent();
            }catch(Exception e){
                return BadRequest();
            }
        }

        [HttpDelete("id")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteById(int id) {
            var useCase = new DeleteTaskUseCase();
            try{
                await useCase.Execute(id);
                return NoContent();
            }catch (Exception e){
                return NotFound();
            }
        }
    }
}//END class