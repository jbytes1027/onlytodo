using Microsoft.EntityFrameworkCore;
using OnlyTodo.Data;
using OnlyTodo.Models;
using TodoTask = OnlyTodo.Models.Task;
using Task = System.Threading.Tasks.Task;

namespace OnlyTodo.Services;

public class TaskService
{
    private readonly OnlyTodoContext _context;

    public TaskService(OnlyTodoContext context)
    {
        _context = context;
    }

    public Task<List<TaskSchema>> GetAllAsync()
    {
        return _context.Tasks.ToListAsync();
    }

    public Task<TaskSchema?> FindAsync(Guid id)
    {
        var query = from task in _context.Tasks where task.Id == id select task;
        query.DefaultIfEmpty(null);
        return query.FirstOrDefaultAsync();
    }

    public Task<TaskSchema?> AddAsync(TodoTask task)
    {
        TaskSchema? taskToAdd = null;

        if (task.Title is not null && task.Completed is not null)
        {
            taskToAdd = new TaskSchema(Guid.NewGuid(), task.Title, (bool)task.Completed);
            _context.Add(taskToAdd);
            _context.SaveChanges();
        }

        return Task.FromResult(taskToAdd);
    }

    public Task<TaskSchema?> UpdateAsync(TodoTask taskUpdates)
    {
        var entity = _context.Tasks
            .DefaultIfEmpty(null)
            .FirstOrDefault(t => t != null && t.Id == taskUpdates.Id);

        if (entity is not null)
        {
            if (taskUpdates.Title is not null)
                entity.Title = taskUpdates.Title;
            if (taskUpdates.Completed is not null)
                entity.Completed = (bool)taskUpdates.Completed;

            _context.Update(taskUpdates);
            _context.SaveChanges();
        }

        return Task.FromResult(entity);
    }

    public Task<TaskSchema?> RemoveAsync(Guid id)
    {
        var query = from t in _context.Tasks where t.Id == id select t;
        query.DefaultIfEmpty(null);
        TaskSchema? task = query.FirstOrDefault();

        if (task is not null)
        {
            _context.Remove(task);
            _context.SaveChanges();
        }

        return Task.FromResult(task);
    }
}