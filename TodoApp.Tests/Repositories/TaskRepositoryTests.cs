using Microsoft.EntityFrameworkCore;
using TodoApp.Domain.Entities;
using TodoApp.Domain.Repositories;
using TodoApp.Infrastructure.Data;
using TodoApp.Infrastructure.Repositories;
using Xunit;

namespace TodoApp.Tests.Repositories
{
    public class TaskRepositoryTests
    {
        private readonly ToDoDbContext _context;
        private readonly TaskRepository _repository;

        public TaskRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<ToDoDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new ToDoDbContext(options);
            _repository = new TaskRepository(_context);
        }

        private ToDoTask CreateTask(string title = "Test", Domain.Entities.TaskStatus status = Domain.Entities.TaskStatus.Pending)
        {
            var task = new ToDoTask(title, "Desc", DateTime.Today);
            task.UpdateStatus(status);
            return task;
        }
        [Fact]
        public async Task AddAsync_Should_Add_Task()
        {
            var task = CreateTask();
            await _repository.AddAsync(task);

            Assert.Equal(1, _context.Tasks.Count());
        }

        [Fact]
        public async Task GetByIdAsync_Should_Return_Task()
        {
            var task = CreateTask();
            await _context.Tasks.AddAsync(task);
            await _context.SaveChangesAsync();

            var result = await _repository.GetByIdAsync(task.Id);
            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetByDueDateAsync_Should_Filter_By_Date()
        {
            var task = CreateTask();
            await _context.Tasks.AddAsync(task);
            await _context.SaveChangesAsync();

            var result = await _repository.GetByDueDateAsync(DateTime.Today);
            Assert.Single(result);
        }

        [Fact]
        public async Task GetByStatusAsync_Should_Filter_By_Status()
        {
            var task = CreateTask(status: Domain.Entities.TaskStatus.InProgress);
            await _context.Tasks.AddAsync(task);
            await _context.SaveChangesAsync();

            var result = await _repository.GetByStatusAsync(Domain.Entities.TaskStatus.InProgress);
            Assert.Single(result);
        }

        [Fact]
        public async Task RemoveAsync_Should_Remove_Task()
        {
            var task = CreateTask();
            await _context.Tasks.AddAsync(task);
            await _context.SaveChangesAsync();

            await _repository.RemoveAsync(task.Id);
            Assert.Empty(_context.Tasks);
        }

        [Fact]
        public async Task RemoveAsync_Should_Ignore_If_Not_Exists()
        {
            await _repository.RemoveAsync(Guid.NewGuid());
            // Should not throw exception
        }

        [Fact]
        public async Task UpdateAsync_Should_Update_Task()
        {
            var task = CreateTask();
            await _context.Tasks.AddAsync(task);
            await _context.SaveChangesAsync();

            task.UpdateDetails("Updated", "New Desc", DateTime.Today.AddDays(1));
            await _repository.UpdateAsync(task);

            var updated = await _context.Tasks.FindAsync(task.Id);
            Assert.Equal("Updated", updated!.Title);
        }

        [Fact]
        public async Task GetAllAsync_Should_Paginate_And_Filter()
        {
            var task1 = new ToDoTask("Task A", "A", DateTime.Today);
            task1.UpdateStatus(Domain.Entities.TaskStatus.Pending);

            var task2 = new ToDoTask("Task B", "B", DateTime.Today.AddDays(1));
            task2.UpdateStatus(Domain.Entities.TaskStatus.Completed);
            await _context.Tasks.AddRangeAsync(task1, task2);
            await _context.SaveChangesAsync();

            var filter = new TaskFilter
            {
                Title = "Task",
                Status = null,
                Page = 1,
                PageSize = 1
            };

            var result = await _repository.GetAllAsync(filter);
            Assert.Equal(2, result.TotalCount);
            Assert.Single(result.Items);
        }

        [Fact]
        public void Query_Should_Return_IQueryable()
        {
            var query = _repository.Query();
            Assert.NotNull(query);
        }
    }
}
