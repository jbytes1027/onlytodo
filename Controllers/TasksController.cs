using Microsoft.AspNetCore.Mvc;
using TodoTask = OnlyTodo.Models.Task;
using OnlyTodo.Services;

namespace OnlyTodo.Controllers;

[ApiController]
[Route("[controller]")]
public class TasksController : ControllerBase
{
    TaskService _taskService;

    public TasksController(TaskService taskService)
    {
        _taskService = taskService;
    }

    [HttpGet]
    public async Task<ActionResult<List<TodoTask>>> GetAll()
    {
        return await _taskService.GetAllAsync();
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<ActionResult<TodoTask>> Get(Guid id)
    {
        var task = await _taskService.FindAsync(id);
        if (task is null)
            return NotFound();

        return Ok(task);
    }

    [HttpPost]
    public async Task<ActionResult<TodoTask>> Create(TodoTask task)
    {
        await _taskService.AddAsync(task);
        return CreatedAtAction(nameof(Create), task);
    }

    [HttpDelete]
    [Route("{id}")]
    public async Task<ActionResult<TodoTask>> Remove(Guid id)
    {
        var task = await _taskService.RemoveAsync(id);
        if (task is null)
            return NotFound();

        return NoContent();
    }
}