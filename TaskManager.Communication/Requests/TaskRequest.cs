namespace TaskManager.Communication.Requests;

public class TaskRequest {
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateOnly Deadline { get; set; }
    public TaskStatus Status { get; set; }
    public TaskPriority Priority { get; set; }
}