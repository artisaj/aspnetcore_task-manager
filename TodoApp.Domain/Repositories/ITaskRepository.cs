using TodoApp.Domain.Common;
using TodoApp.Domain.Entities;

namespace TodoApp.Domain.Repositories
{
    public class TaskFilter
    {
        public string? Title { get; set; }
        public Entities.TaskStatus? Status { get; set; }
        public DateTime? DueDateMin { get; set; }
        public DateTime? DueDateMax { get; set; }

        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
    public interface ITaskRepository
    {
        Task<ToDoTask?> GetByIdAsync(Guid id);
        Task<PaginatedResult<ToDoTask>> GetAllAsync(TaskFilter filters);
        Task<IEnumerable<ToDoTask>> GetByStatusAsync(Entities.TaskStatus status);
        Task<IEnumerable<ToDoTask>> GetByDueDateAsync(DateTime dueDate);
        Task AddAsync(ToDoTask task);
        Task UpdateAsync(ToDoTask task);
        Task RemoveAsync(Guid id);
        IQueryable<ToDoTask> Query();
    }
}