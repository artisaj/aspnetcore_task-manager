using Microsoft.EntityFrameworkCore;
using TodoApp.Domain.Common;
using TodoApp.Domain.Entities;
using TodoApp.Domain.Repositories;
using TodoApp.Infrastructure.Data;

namespace TodoApp.Infrastructure.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly ToDoDbContext _context;

        public TaskRepository(ToDoDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(ToDoTask task)
        {
            await _context.Tasks.AddAsync(task);
            await _context.SaveChangesAsync();
        }

        public async Task<PaginatedResult<ToDoTask>> GetAllAsync(TaskFilter filters)
        {
            var query = _context.Tasks.AsQueryable();

            if (!string.IsNullOrWhiteSpace(filters.Title))
                query = query.Where(t => t.Title.Contains(filters.Title));

            if (filters.DueDateMin.HasValue)
                query = query.Where(t => t.DueDate >= filters.DueDateMin.Value);

            if (filters.DueDateMax.HasValue)
                query = query.Where(t => t.DueDate <= filters.DueDateMax.Value);

            if (filters.Status.HasValue)
                query = query.Where(t => t.Status == filters.Status.Value);

            var totalCount = await query.CountAsync();

            var items = await query
                .Skip((filters.Page - 1) * filters.PageSize)
                .Take(filters.PageSize)
                .ToListAsync();

            return new PaginatedResult<ToDoTask>
            {
                Items = items,
                TotalCount = totalCount
            };
        }

        public async Task<ToDoTask?> GetByIdAsync(Guid id)
        {
            return await _context.Tasks.FindAsync(id);
        }

        public async Task<IEnumerable<ToDoTask>> GetByDueDateAsync(DateTime dueDate)
        {
            return await _context.Tasks
                .AsNoTracking()
                .Where(t => t.DueDate.Date == dueDate.Date)
                .ToListAsync();
        }

        public async Task<IEnumerable<ToDoTask>> GetByStatusAsync(Domain.Entities.TaskStatus status)
        {
            return await _context.Tasks
                .AsNoTracking()
                .Where(t => t.Status == status)
                .ToListAsync();
        }

        public async Task RemoveAsync(Guid id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null) return;
            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(ToDoTask task)
        {
            _context.Tasks.Update(task);
            await _context.SaveChangesAsync();
        }

        public IQueryable<ToDoTask> Query()
        {
            return _context.Tasks.AsQueryable();
        }
    }
}
