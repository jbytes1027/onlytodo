using Microsoft.AspNetCore.Mvc;
using OnlyTodo.Models;
using OnlyTodo.Services;
using OnlyTodo.Helpers;

namespace OnlyTodo.Controllers;

[ApiController]
[Route("[controller]")]
public class TasksController : ControllerBase
{
    private readonly TaskService _taskService;

    public TasksController(TaskService taskService)
    {
        _taskService = taskService;
    }

    [HttpGet]
    public async Task<ActionResult<List<TodoTaskDTO>>> GetAll()
    {
        List<TodoTask> tasks = await _taskService.GetAllAsync();

        try
        {
            IEnumerable<TodoTaskDTO> taskDTOs = tasks.Select(t => t.ToDTO());
            return Ok(taskDTOs);
        }
        catch
        {
            return Problem();
        }
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<ActionResult<TodoTaskDTO>> Get(Guid id)
    {
        TodoTask? task = await _taskService.FindAsync(id);

        if (task is null)
            return NotFound();

        return Ok(task);
    }

    [HttpPost]
    public async Task<ActionResult<Task>> Create(TodoTaskDTO dto)
    {
        if (dto.Title is null)
            return UnprocessableEntity();

        TodoTask parsedTask = new(new Guid(), dto.Title, dto?.Completed ?? false);

        TodoTask createdTask = await _taskService.AddAsync(parsedTask);

        if (createdTask is null)
            return BadRequest();

        return Created($"/tasks/{createdTask.Id}", createdTask);
    }

    [HttpPatch]
    [Route("{id}")]
    public async Task<ActionResult<Task>> Update([FromRoute] Guid id, TodoTaskDTO taskUpdates)
    {
        TodoTask? ogTask = await _taskService.FindAsync(id);

        if (ogTask is null)
            return NotFound();

        if (taskUpdates.Title is not null)
            ogTask.Title = taskUpdates.Title;
        if (taskUpdates.Completed is not null)
            ogTask.Completed = (bool)taskUpdates.Completed;

        TodoTask updatedTask = await _taskService.UpdateAsync(ogTask);

        return Ok(updatedTask);
    }

    [HttpDelete]
    [Route("{id}")]
    public async Task<ActionResult> Remove(Guid id)
    {
        try
        {
            await _taskService.RemoveAsync(id);
        }
        catch
        {
            return NotFound();
        }

        return NoContent();
    }
}
