using Microsoft.AspNetCore.Mvc;
using TodoTask = OnlyTodo.Models.Task;
using OnlyTodo.Services;

namespace OnlyTodo.Controllers;

[ApiController]
[Route("[controller]")]
public class TasksController : ControllerBase
{
    TaskService taskService = new();

    [HttpGet]
    public async Task<ActionResult<List<TodoTask>>> GetAll() {
        return await taskService.GetAllAsync();
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<ActionResult<TodoTask>> Get(Guid id)
    {
        var task = await taskService.FindAsync(id);
        if (task is null)
            return NotFound();

        return Ok(task);
    }

    [HttpPost]
    public async Task<ActionResult<TodoTask>> Create(TodoTask task)
    {
        await taskService.AddAsync(task);
        return CreatedAtAction(nameof(Create), task);
    }

    [HttpDelete]
    [Route("{id}")]
    public async Task<ActionResult<TodoTask>> Remove(Guid id)
    {
        var task = await taskService.RemoveAsync(id);
        if (task is null)
            return NotFound();

        return NoContent();
    }
}