using TodoTask = OnlyTodo.Models.Task;

namespace OnlyTodo.Services;

public class TaskService
{
    List<TodoTask> tasks = new();

    public Task<List<TodoTask>> GetAllAsync() {
        foreach (var task in tasks) Console.WriteLine(task);
        return Task.FromResult(tasks);
    }

    public Task<TodoTask?> FindAsync(Guid id)
    {
        foreach (var task in tasks)
        {
            if (task.Id == id)
            {
                return Task.FromResult<TodoTask?>(task);
            }
        }
        return Task.FromResult<TodoTask?>(null);
    }

    public Task AddAsync(TodoTask task)
    {
        tasks.Add(task);
        return Task.CompletedTask;
    }

    public async Task<TodoTask?> RemoveAsync(Guid id)
    {
        var task = await FindAsync(id);
        if (task is not null) tasks.Remove(task);
        return task;
    }

    // public async Task<TodoTask?> UpdateAsync(TodoTask task)
    // {
    //     var taskToUpdate = await FindAsync(task.Id);
    //     bool success = tasks.Remove(taskToUpdate);
    //     if (success) tasks.Add(task);

    //     return Task.;
    // }
}