using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ToDoApi.Data;
using ToDoApi.Models;

namespace ToDoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILogger<TodoController> _logger;

        public TodoController(AppDbContext context, ILogger<TodoController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetTasks()
        {
            _logger.LogInformation("Fetching all tasks from the database.");
            var tasks = await _context.TodoItems.ToListAsync();
            return Ok(tasks);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTask([FromBody] TodoItem item)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state while creating a task.");
                return BadRequest(ModelState);
            }

            item.IsCompleted = false;
            _context.TodoItems.Add(item);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Task created with ID {item.Id}.");
            return Ok(item);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(int id, [FromBody] TodoItem updatedItem)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state while updating a task.");
                return BadRequest(ModelState);
            }

            var task = await _context.TodoItems.FindAsync(id);
            if (task == null)
            {
                _logger.LogWarning($"Task with ID {id} not found for update.");
                return NotFound();
            }

            task.Name = updatedItem.Name;
            task.Description = updatedItem.Description;
            task.DueDate = updatedItem.DueDate;
            task.IsCompleted = updatedItem.IsCompleted;

            await _context.SaveChangesAsync();
            _logger.LogInformation($"Task with ID {id} updated.");
            return Ok(task);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var task = await _context.TodoItems.FindAsync(id);
            if (task == null)
            {
                _logger.LogWarning($"Task with ID {id} not found for deletion.");
                return NotFound();
            }

            _context.TodoItems.Remove(task);
            await _context.SaveChangesAsync();
            _logger.LogInformation($"Task with ID {id} deleted.");
            return NoContent();
        }
    }
}
