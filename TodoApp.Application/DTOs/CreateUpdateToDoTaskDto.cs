namespace TodoApp.Application.DTOs
{
    public class CreateUpdateToDoTaskDto
    {
        public Guid? Id { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public DateTime DueDate { get; set; }
        public Domain.Entities.TaskStatus Status { get; set; }
    }
}