namespace TaskManager.Communication.Responses;

public class MultipleTasksResponse {
    public List<TaskResponse> Tasks { get; set; }= [];
}