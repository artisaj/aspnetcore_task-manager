using TodoApp.Application.DTOs;
using TodoApp.Domain.Common;
using TodoApp.Domain.Repositories;

namespace TodoApp.Application.Services
{
    public interface ITaskService
    {
        Task<ToDoTaskDto?> GetByIdAsync(Guid id);
        Task<PaginatedResult<ToDoTaskDto>> GetAllAsync(TaskFilter filters);
        Task<IEnumerable<ToDoTaskDto>> GetByStatusAsync(Domain.Entities.TaskStatus status);
        Task<IEnumerable<ToDoTaskDto>> GetByDueDateAsync(DateTime dueDate);
        Task<ToDoTaskDto> CreateAsync(CreateUpdateToDoTaskDto taskDto);
        Task<ToDoTaskDto?> UpdateAsync(Guid id, CreateUpdateToDoTaskDto taskDto);
        Task DeleteAsync(Guid id);
    }
}