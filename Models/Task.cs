namespace OnlyTodo.Models;

public record Task
{
    public Task(Guid id, string title, bool completed) {
        Id = id;
        Title = title;
        Completed = completed;
    }

    public Guid Id { get; set; }

    public string Title { get; set; }

    public bool Completed { get; set; }
}
