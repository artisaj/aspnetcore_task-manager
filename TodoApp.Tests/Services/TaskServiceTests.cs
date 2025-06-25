using Moq;
using TodoApp.Application.Services;
using TodoApp.Application.DTOs;
using TodoApp.Domain.Entities;
using TodoApp.Domain.Repositories;
using TodoApp.Domain.Common;

namespace TodoApp.Tests.Services
{
    public class TaskServiceTests
    {
        private readonly Mock<ITaskRepository> _taskRepositoryMock;
        private readonly TaskService _taskService;

        public TaskServiceTests()
        {
            _taskRepositoryMock = new Mock<ITaskRepository>();
            _taskService = new TaskService(_taskRepositoryMock.Object);
        }

        [Fact]
        public async Task CreateAsync_Should_Create_Task_With_Pending_Status()
        {
            var dto = new CreateUpdateToDoTaskDto
            {
                Title = "Test",
                Description = "Desc",
                DueDate = DateTime.Today,
                Status = Domain.Entities.TaskStatus.Pending
            };

            var result = await _taskService.CreateAsync(dto);

            Assert.Equal(dto.Title, result.Title);
            Assert.Equal(dto.Description, result.Description);
            Assert.Equal(dto.DueDate.Date, result.DueDate.Date);
            Assert.Equal(Domain.Entities.TaskStatus.Pending, result.Status);
            _taskRepositoryMock.Verify(r => r.AddAsync(It.IsAny<ToDoTask>()), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_Should_Set_Status_If_Not_Pending()
        {
            var dto = new CreateUpdateToDoTaskDto
            {
                Title = "Test",
                Description = "Desc",
                DueDate = DateTime.Today,
                Status = Domain.Entities.TaskStatus.Completed
            };

            var result = await _taskService.CreateAsync(dto);

            Assert.Equal(Domain.Entities.TaskStatus.Completed, result.Status);
        }

        [Fact]
        public async Task DeleteAsync_Should_Call_Repository()
        {
            var id = Guid.NewGuid();

            await _taskService.DeleteAsync(id);

            _taskRepositoryMock.Verify(r => r.RemoveAsync(id), Times.Once);
        }

        [Fact]
        public async Task GetAllAsync_Should_Return_Mapped_Result()
        {
            var filters = new TaskFilter
            {
                Title = "Test",
                Page = 1,
                PageSize = 10
            };

            var tasks = new List<ToDoTask>
            {
                new("A", "B", DateTime.Today),
                new("X", "Y", DateTime.Today)
            };

            var paginated = new PaginatedResult<ToDoTask>
            {
                Items = tasks,
                TotalCount = tasks.Count
            };

            _taskRepositoryMock.Setup(r => r.GetAllAsync(It.IsAny<TaskFilter>()))
                .ReturnsAsync(paginated);

            var result = await _taskService.GetAllAsync(filters);

            Assert.Equal(tasks.Count, result.TotalCount);
            Assert.All(result.Items, item => Assert.IsType<ToDoTaskDto>(item));
        }

        [Fact]
        public async Task GetByIdAsync_Should_Return_Null_If_Not_Found()
        {
            var id = Guid.NewGuid();
            _taskRepositoryMock.Setup(r => r.GetByIdAsync(id))
                .ReturnsAsync((ToDoTask?)null);

            var result = await _taskService.GetByIdAsync(id);

            Assert.Null(result);
        }

        [Fact]
        public async Task GetByIdAsync_Should_Return_Task()
        {
            var task = new ToDoTask("T", "D", DateTime.Today);
            _taskRepositoryMock.Setup(r => r.GetByIdAsync(task.Id)).ReturnsAsync(task);

            var result = await _taskService.GetByIdAsync(task.Id);

            Assert.NotNull(result);
            Assert.Equal(task.Title, result!.Title);
        }

        [Fact]
        public async Task GetByDueDateAsync_Should_Return_List()
        {
            var dueDate = DateTime.Today;
            var tasks = new List<ToDoTask> { new("1", "2", dueDate) };

            _taskRepositoryMock.Setup(r => r.GetByDueDateAsync(dueDate)).ReturnsAsync(tasks);

            var result = await _taskService.GetByDueDateAsync(dueDate);

            Assert.Single(result);
        }

        [Fact]
        public async Task GetByStatusAsync_Should_Return_List()
        {
            var tasks = new List<ToDoTask> { new("a", "b", DateTime.Today) };
            _taskRepositoryMock.Setup(r => r.GetByStatusAsync(Domain.Entities.TaskStatus.Pending)).ReturnsAsync(tasks);

            var result = await _taskService.GetByStatusAsync(Domain.Entities.TaskStatus.Pending);

            Assert.Single(result);
        }

        [Fact]
        public async Task UpdateAsync_Should_Return_Null_If_Not_Found()
        {
            var id = Guid.NewGuid();
            _taskRepositoryMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((ToDoTask?)null);

            var dto = new CreateUpdateToDoTaskDto
            {
                Title = "Dummy Title",
                Description = "Dummy Description",
                DueDate = DateTime.Today,
                Status = Domain.Entities.TaskStatus.Pending
            };

            var result = await _taskService.UpdateAsync(id, dto);

            Assert.Null(result);
        }

        [Fact]
        public async Task UpdateAsync_Should_Update_And_Return_Dto()
        {
            var id = Guid.NewGuid();
            var task = new ToDoTask("old", "desc", DateTime.Today);
            typeof(ToDoTask).GetProperty("Id")!.SetValue(task, id);

            _taskRepositoryMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(task);

            var dto = new CreateUpdateToDoTaskDto
            {
                Title = "new",
                Description = "updated",
                DueDate = DateTime.Today.AddDays(1),
                Status = Domain.Entities.TaskStatus.InProgress
            };

            var result = await _taskService.UpdateAsync(id, dto);

            Assert.NotNull(result);
            Assert.Equal(dto.Title, result!.Title);
            Assert.Equal(dto.Status, result.Status);
            _taskRepositoryMock.Verify(r => r.UpdateAsync(It.IsAny<ToDoTask>()), Times.Once);
        }
    }
}
