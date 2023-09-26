using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace OnlyTodo.Models;

public record Task
{
    [BindNever]
    public Guid Id { get; set; }
    public string Title { get; set; } = "";
    public bool Completed { get; set; }
}
