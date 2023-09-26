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
        query.DefaultIfEmpty(null);
        return query.FirstOrDefaultAsync();
    }

    public Task AddAsync(TodoTask task)
    {
        task.Id = Guid.NewGuid();
        _context.Add(task);
        _context.SaveChanges();
        return Task.CompletedTask;
    }

    public Task<TodoTask?> RemoveAsync(Guid id)
    {
        var query = from t in _context.Tasks where t.Id == id select t;
        query.DefaultIfEmpty(null);
        TodoTask? task = query.FirstOrDefault();

        if (task is not null)
        {
            _context.Remove(task);
            _context.SaveChanges();
        }

        return Task.FromResult(task);
    }

    // public async Task<TodoTask?> UpdateAsync(TodoTask task)
    // {
    //     var taskToUpdate = await FindAsync(task.Id);
    //     bool success = tasks.Remove(taskToUpdate);
    //     if (success) tasks.Add(task);

    //     return Task.;
    // }
}