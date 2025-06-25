using TodoApp.Domain.Entities;

namespace TodoApp.Application.DTOs
{
    public class ToDoTaskDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Domain.Entities.TaskStatus Status { get; set; }

        public string StatusText => Status switch
        {
            Domain.Entities.TaskStatus.Pending => "Pendente",
            Domain.Entities.TaskStatus.InProgress => "Em andamento",
            Domain.Entities.TaskStatus.Completed => "Concluído",
            _ => "Desconhecido"
        };
        public DateTime DueDate { get; set; }

        public ToDoTaskDto(ToDoTask task)
        {
            Id = task.Id;
            Title = task.Title;
            Description = task.Description;
            DueDate = task.DueDate;
            Status = task.Status;
        }
    }
}