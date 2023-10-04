using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace OnlyTodo.Models;

public record Task
{
    [BindNever]
    public Guid? Id { get; set; }
    public string? Title { get; set; }
    public bool? Completed { get; set; }
}

public record TaskSchema
{
    public TaskSchema(Guid id, string title, bool completed = false)
    {
        Id = id;
        Title = title;
        Completed = completed;
    }

    [BindNever]
    public Guid Id { get; set; }
    public string Title { get; set; }
    public bool Completed { get; set; }
}