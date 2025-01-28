using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ToDoApi.Controllers;
using ToDoApi.Data;
using ToDoApi.Models;
using Xunit;

namespace ToDoApi.Tests
{
    public class TodoControllerTests
    {
        private readonly TodoController _controller;
        private readonly AppDbContext _context;

        public TodoControllerTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase("TodoTestDb")
                .Options;

            _context = new AppDbContext(options);
            var logger = new LoggerFactory().CreateLogger<TodoController>();
            _controller = new TodoController(_context, logger);

            _context.TodoItems.RemoveRange(_context.TodoItems);
            _context.SaveChanges();

            _context.TodoItems.AddRange(
                new TodoItem { Id = 1, Name = "Task 1", Description = "Test Task 1", DueDate = DateTime.Now.AddDays(1) },
                new TodoItem { Id = 2, Name = "Task 2", Description = "Test Task 2", DueDate = DateTime.Now.AddDays(2) }
            );
            _context.SaveChanges();
        }

        [Fact]
        public async Task GetTasks_ReturnsOkResult_WithListOfTasks()
        {
            var result = await _controller.GetTasks();

            var okResult = Assert.IsType<OkObjectResult>(result);
            var tasks = Assert.IsAssignableFrom<List<TodoItem>>(okResult.Value);
            Assert.Equal(2, tasks.Count);
        }

        [Fact]
        public async Task CreateTask_ReturnsCreatedResult_WhenTaskIsValid()
        {
            var newTask = new TodoItem { Name = "New Task", Description = "New Task Description", DueDate = DateTime.Now.AddDays(3) };

            var result = await _controller.CreateTask(newTask);

            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var createdTask = Assert.IsType<TodoItem>(createdResult.Value);

            Assert.Equal("New Task", createdTask.Name);
            Assert.False(createdTask.IsCompleted);
        }


        [Fact]
        public async Task UpdateTask_ReturnsOkResult_WhenTaskIsUpdated()
        {
            var taskId = 1;
            var updatedTask = new TodoItem
            {
                Name = "Updated Task",
                Description = "Updated Description",
                DueDate = DateTime.Now.AddDays(5)
            };

            var result = await _controller.UpdateTask(taskId, updatedTask);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var updated = Assert.IsType<TodoItem>(okResult.Value);
            Assert.Equal("Updated Task", updated.Name);
            Assert.Equal("Updated Description", updated.Description);
        }

        [Fact]
        public async Task UpdateTask_ReturnsNotFound_WhenTaskDoesNotExist()
        {
            var updatedTask = new TodoItem
            {
                Name = "Nonexistent Task",
                Description = "Nonexistent Description",
                DueDate = DateTime.Now.AddDays(5)
            };

            var result = await _controller.UpdateTask(999, updatedTask);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeleteTask_ReturnsNoContent_WhenTaskExists()
        {
            var taskId = 1;

            var result = await _controller.DeleteTask(taskId);

            Assert.IsType<NoContentResult>(result);
            Assert.Null(await _context.TodoItems.FindAsync(taskId));
        }

        [Fact]
        public async Task DeleteTask_ReturnsNotFound_WhenTaskDoesNotExist()
        {
            var result = await _controller.DeleteTask(999);

            Assert.IsType<NotFoundResult>(result);
        }
    }
}
