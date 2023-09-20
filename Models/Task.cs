namespace OnlyTodo.Models;

public record Task
{
    public Guid Id {get; init;}
    public string Name {get; init;} = "";
    public bool IsDone {get; set;}
}