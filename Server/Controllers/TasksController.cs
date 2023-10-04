using Microsoft.AspNetCore.Mvc;
using TodoTask = OnlyTodo.Models.Task;
using OnlyTodo.Services;
using OnlyTodo.Models;

namespace OnlyTodo.Controllers;

[ApiController]
[Route("[controller]")]
public class TasksController : ControllerBase
{
    readonly TaskService _taskService;

    public TasksController(TaskService taskService)
    {
        _taskService = taskService;
    }

    [HttpGet]
    public async Task<ActionResult<List<TaskSchema>>> GetAll()
    {
        return await _taskService.GetAllAsync();
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<ActionResult<TaskSchema>> Get(Guid id)
    {
        var task = await _taskService.FindAsync(id);
        if (task is null)
            return NotFound();

        return Ok(task);
    }

    [HttpPost]
    public async Task<ActionResult<TaskSchema>> Create(TodoTask task)
    {
        TaskSchema? createdTask = await _taskService.AddAsync(task);

        if (createdTask is null)
            return BadRequest();

        return Created($"/tasks/{createdTask.Id}", createdTask);
    }

    [HttpPatch]
    [Route("{id}")]
    public async Task<ActionResult<TaskSchema>> Update([FromRoute] Guid id, TodoTask taskUpdates)
    {
        taskUpdates.Id = id;

        var updatedTask = await _taskService.UpdateAsync(taskUpdates);
        if (updatedTask is null)
            return NotFound();

        return Ok(updatedTask);
    }

    [HttpDelete]
    [Route("{id}")]
    public async Task<ActionResult<TaskSchema>> Remove(Guid id)
    {
        var task = await _taskService.RemoveAsync(id);
        if (task is null)
            return NotFound();

        return NoContent();
    }
}