using Microsoft.EntityFrameworkCore;
using OnlyTodo.Data;
using TodoTask = OnlyTodo.Models.Task;

namespace OnlyTodo.Services;

public class TaskService
{
    private readonly OnlyTodoContext _context;

    public TaskService(OnlyTodoContext context)
    {
        _context = context;
    }

    public Task<List<TodoTask>> GetAllAsync()
    {
        return _context.Tasks.ToListAsync();
    }

    public Task<TodoTask?> FindAsync(Guid id)
    {
        var query = from task in _context.Tasks where task.Id == id select task;
        return Task.FromResult<TodoTask?>(query.First());
    }

    public Task AddAsync(TodoTask task)
    {
        _context.Add(task);
        _context.SaveChanges();
        return Task.CompletedTask;
    }

    public Task<TodoTask?> RemoveAsync(Guid id)
    {
        TodoTask task = (
            from t in _context.Tasks
            where t.Id == id
            select t
        ).First();

        _context.Remove(task);
        _context.SaveChanges();
        return Task.FromResult<TodoTask?>(task);
    }

    // public async Task<TodoTask?> UpdateAsync(TodoTask task)
    // {
    //     var taskToUpdate = await FindAsync(task.Id);
    //     bool success = tasks.Remove(taskToUpdate);
    //     if (success) tasks.Add(task);

    //     return Task.;
    // }
}