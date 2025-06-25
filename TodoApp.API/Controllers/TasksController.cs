using Microsoft.AspNetCore.Mvc;
using TodoApp.Domain.Entities;
using TodoApp.Application.Services;
using TodoApp.Application.DTOs;
using TodoApp.Domain.Repositories;

namespace TodoApp.API.Controllers
{
    [ApiController]
    [Route("api/tasks")]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TasksController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ToDoTask>>> GetAll(
            [FromQuery] TaskFilterDto filters
        )
        {
            var domainFilter = new TaskFilter
            {
                Title = filters.Title,
                Status = filters.Status,
                DueDateMin = filters.DueDateMin,
                DueDateMax = filters.DueDateMax,
                Page = filters.Page,
                PageSize = filters.PageSize
            };

            var tasks = await _taskService.GetAllAsync(domainFilter);
            return Ok(tasks);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ToDoTask>> GetById(Guid id)
        {
            var task = await _taskService.GetByIdAsync(id);
            if (task == null)
                return NotFound();

            return Ok(task);
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] CreateUpdateToDoTaskDto dto)
        {
            var task = await _taskService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = task.Id }, task);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult> Update(Guid id, [FromBody] CreateUpdateToDoTaskDto task)
        {
            if (id != task.Id)
                return BadRequest("Id mismatch");

            var existingTask = await _taskService.GetByIdAsync(id);
            if (existingTask == null)
                return NotFound();

            await _taskService.UpdateAsync(id, task);
            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var existingTask = await _taskService.GetByIdAsync(id);
            if (existingTask == null)
                return NotFound();

            await _taskService.DeleteAsync(id);
            return NoContent();
        }
    }
}
