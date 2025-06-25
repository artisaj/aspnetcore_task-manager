using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TodoApp.API.Controllers;
using TodoApp.Application.DTOs;
using TodoApp.Application.Services;
using TodoApp.Domain.Common;
using TodoApp.Domain.Entities;
using TodoApp.Domain.Repositories;
using Xunit;

public class TasksControllerTests
{
    private readonly Mock<ITaskService> _taskServiceMock;
    private readonly TasksController _controller;

    public TasksControllerTests()
    {
        _taskServiceMock = new Mock<ITaskService>();
        _controller = new TasksController(_taskServiceMock.Object);
    }

    [Fact]
    public async Task GetAll_ReturnsOk_WithTasks()
    {
        var paginatedResult = new PaginatedResult<ToDoTaskDto>
        {
            Items = new List<ToDoTaskDto>
            {
                new ToDoTaskDto(new ToDoTask("Test Task", "Test Description", DateTime.Now))
            },
            TotalCount = 1
        };

        _taskServiceMock
            .Setup(s => s.GetAllAsync(It.IsAny<TaskFilter>()))
            .ReturnsAsync(paginatedResult);

        var filtersDto = new TaskFilterDto();

        var result = await _controller.GetAll(filtersDto);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnValue = Assert.IsType<PaginatedResult<ToDoTaskDto>>(okResult.Value);
        Assert.Single(returnValue.Items);
    }

    [Fact]
    public async Task GetById_ReturnsOk_WhenFound()
    {
        var id = Guid.NewGuid();
        var taskDto = new ToDoTaskDto(new ToDoTask(id, "Test Task", "Test Description", DateTime.Now));
        _taskServiceMock.Setup(s => s.GetByIdAsync(id)).ReturnsAsync(taskDto);

        var result = await _controller.GetById(id);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnValue = Assert.IsType<ToDoTaskDto>(okResult.Value);
        Assert.Equal(id, returnValue.Id);
    }

    [Fact]
    public async Task GetById_ReturnsNotFound_WhenNotFound()
    {
        _taskServiceMock.Setup(s => s.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((ToDoTaskDto?)null);

        var result = await _controller.GetById(Guid.NewGuid());

        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task Create_ReturnsCreatedAtAction_WithCreatedTask()
    {
        var taskDto = new ToDoTaskDto(new ToDoTask("Test Task", "Test Description", DateTime.Now));
        var createDto = new CreateUpdateToDoTaskDto { Title = "New Task", Description = "Desc", DueDate = DateTime.Today, Status = TodoApp.Domain.Entities.TaskStatus.Pending };

        _taskServiceMock.Setup(s => s.CreateAsync(createDto)).ReturnsAsync(taskDto);

        var result = await _controller.Create(createDto);

        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal(nameof(TasksController.GetById), createdAtActionResult.ActionName);
        Assert.Equal(taskDto.Id, ((ToDoTaskDto)createdAtActionResult.Value!).Id);
    }

    [Fact]
    public async Task Update_ReturnsBadRequest_WhenIdMismatch()
    {
        var id = Guid.NewGuid();
        var updateDto = new CreateUpdateToDoTaskDto { Id = Guid.NewGuid(), Title = "Test", Description = "Desc", DueDate = DateTime.Today, Status = TodoApp.Domain.Entities.TaskStatus.Pending };

        var result = await _controller.Update(id, updateDto);

        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Id mismatch", badRequest.Value);
    }

    [Fact]
    public async Task Update_ReturnsNotFound_WhenTaskNotFound()
    {
        var id = Guid.NewGuid();
        var updateDto = new CreateUpdateToDoTaskDto { Id = id, Title = "Test", Description = "Desc", DueDate = DateTime.Today, Status = TodoApp.Domain.Entities.TaskStatus.Pending };

        _taskServiceMock.Setup(s => s.GetByIdAsync(id)).ReturnsAsync((ToDoTaskDto?)null);

        var result = await _controller.Update(id, updateDto);

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Update_ReturnsNoContent_WhenSuccessful()
    {
        var id = Guid.NewGuid();
        var updateDto = new CreateUpdateToDoTaskDto { Id = id, Title = "Test", Description = "Desc", DueDate = DateTime.Today, Status = TodoApp.Domain.Entities.TaskStatus.Pending };
        var existingTask = new ToDoTaskDto(new ToDoTask("Test Task", "Test Description", DateTime.Now));

        _taskServiceMock.Setup(s => s.GetByIdAsync(id)).ReturnsAsync(existingTask);
        _taskServiceMock.Setup(s => s.UpdateAsync(id, updateDto)).ReturnsAsync(existingTask);

        var result = await _controller.Update(id, updateDto);

        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task Delete_ReturnsNotFound_WhenTaskNotFound()
    {
        var id = Guid.NewGuid();
        _taskServiceMock.Setup(s => s.GetByIdAsync(id)).ReturnsAsync((ToDoTaskDto?)null);

        var result = await _controller.Delete(id);

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Delete_ReturnsNoContent_WhenSuccessful()
    {
        var id = Guid.NewGuid();
        var existingTask = new ToDoTaskDto(new ToDoTask("Test Task", "Test Description", DateTime.Now));

        _taskServiceMock.Setup(s => s.GetByIdAsync(id)).ReturnsAsync(existingTask);
        _taskServiceMock.Setup(s => s.DeleteAsync(id)).Returns(Task.CompletedTask);

        var result = await _controller.Delete(id);

        Assert.IsType<NoContentResult>(result);
    }
}
