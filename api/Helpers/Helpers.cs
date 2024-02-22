using OnlyTodo.Models;

namespace OnlyTodo.Helpers;

public static class Helplers
{
    public static TodoTaskDTO ToDTO(this TodoTask todoTask)
    {
        return new()
        {
            Id = todoTask.Id,
            Title = todoTask.Title,
            Completed = todoTask.Completed,
        };
    }
}
