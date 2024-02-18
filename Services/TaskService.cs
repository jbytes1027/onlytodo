using Microsoft.EntityFrameworkCore;
using OnlyTodo.Data;
using OnlyTodo.Models;

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

    public async Task<TodoTask?> FindAsync(Guid id)
    {
        return await _context.Tasks.FindAsync(id);
    }

    public async Task<TodoTask> AddAsync(TodoTask task)
    {
        // Don't trust the given guid
        task = new TodoTask(Guid.NewGuid(), task.Title, task.Completed);

        _context.Add(task);
        await _context.SaveChangesAsync();

        return task;
    }

    public async Task<TodoTask> UpdateAsync(TodoTask taskUpdates)
    {
        TodoTask task = _context.Tasks.Find(taskUpdates.Id) ?? throw new Exception();

        _context.Update(task);
        await _context.SaveChangesAsync();

        return task;
    }

    public async Task RemoveAsync(Guid id)
    {
        TodoTask task = _context.Tasks.Find(id) ?? throw new NullReferenceException();

        _context.Remove(task);
        await _context.SaveChangesAsync();
    }
}
