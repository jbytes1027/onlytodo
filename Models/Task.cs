namespace OnlyTodo.Models;

public record TodoTaskDTO
{
    public Guid? Id { get; set; }
    public string? Title { get; set; }
    public bool? Completed { get; set; }
}

public record TodoTask
{
    public TodoTask(Guid id, string title, bool completed = false)
    {
        Id = id;
        Title = title;
        Completed = completed;
    }

    public Guid Id { get; set; }
    public string Title { get; set; }
    public bool Completed { get; set; }
}
