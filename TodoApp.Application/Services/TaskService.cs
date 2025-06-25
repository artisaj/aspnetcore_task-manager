using TodoApp.Application.DTOs;
using TodoApp.Domain.Common;
using TodoApp.Domain.Entities;
using TodoApp.Domain.Repositories;

namespace TodoApp.Application.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;

        public TaskService(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task<ToDoTaskDto> CreateAsync(CreateUpdateToDoTaskDto taskDto)
        {
            var task = new ToDoTask(taskDto.Title, taskDto.Description, taskDto.DueDate);
            if (taskDto.Status != Domain.Entities.TaskStatus.Pending)
            {
                task.UpdateStatus(taskDto.Status);
            }
            await _taskRepository.AddAsync(task);
            return MapToDto(task);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _taskRepository.RemoveAsync(id);
        }

        public async Task<PaginatedResult<ToDoTaskDto>> GetAllAsync(TaskFilter filters)
        {
            var domainFilter = new TaskFilter
            {
                Title = filters.Title,
                DueDateMin = filters.DueDateMin,
                DueDateMax = filters.DueDateMax,
                Status = filters.Status,
                Page = filters.Page,
                PageSize = filters.PageSize
            };

            var result = await _taskRepository.GetAllAsync(domainFilter);
            return new PaginatedResult<ToDoTaskDto>
            {
                Items = result.Items.Select(t => new ToDoTaskDto(t)).ToList(),
                TotalCount = result.TotalCount
            };
        }

        public async Task<ToDoTaskDto?> GetByIdAsync(Guid id)
        {
            var task = await _taskRepository.GetByIdAsync(id);
            return task == null ? null : MapToDto(task);
        }

        public async Task<IEnumerable<ToDoTaskDto>> GetByDueDateAsync(DateTime dueDate)
        {
            var tasks = await _taskRepository.GetByDueDateAsync(dueDate);
            return MapToDtoList(tasks);
        }

        public async Task<IEnumerable<ToDoTaskDto>> GetByStatusAsync(Domain.Entities.TaskStatus status)
        {
            var tasks = await _taskRepository.GetByStatusAsync(status);
            return MapToDtoList(tasks);
        }

        public async Task<ToDoTaskDto?> UpdateAsync(Guid id, CreateUpdateToDoTaskDto taskDto)
        {
            var task = await _taskRepository.GetByIdAsync(id);
            if (task == null) return null;

            task.UpdateDetails(taskDto.Title, taskDto.Description, taskDto.DueDate);
            task.UpdateStatus(taskDto.Status);

            await _taskRepository.UpdateAsync(task);
            return MapToDto(task);
        }

        private ToDoTaskDto MapToDto(ToDoTask task)
        {
            return new ToDoTaskDto(task);
        }

        private IEnumerable<ToDoTaskDto> MapToDtoList(IEnumerable<ToDoTask> tasks)
        {
            foreach (var task in tasks)
            {
                yield return MapToDto(task);
            }
        }
    }
}