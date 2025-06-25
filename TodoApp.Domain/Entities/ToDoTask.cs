namespace TodoApp.Domain.Entities
{
    public enum TaskStatus
    {
        Pending,
        InProgress,
        Completed
    }

    public class ToDoTask
    {
        public Guid Id { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public TaskStatus Status { get; private set; }
        public DateTime DueDate { get; private set; }

        public ToDoTask(Guid id, string title, string description, DateTime dueDate)
        {
            Id = id;
            Title = title ?? throw new ArgumentNullException(nameof(title));
            Description = description ?? string.Empty;
            Status = TaskStatus.Pending;
            DueDate = dueDate;
        }

        public ToDoTask(string title, string description, DateTime dueDate)
        {
            Id = Guid.NewGuid();
            Title = title ?? throw new ArgumentNullException(nameof(title));
            Description = description ?? string.Empty;
            Status = TaskStatus.Pending;
            DueDate = dueDate;
        }

        public void UpdateDetails(string title, string description, DateTime dueDate)
        {
            Title = title ?? throw new ArgumentNullException(nameof(title));
            Description = description ?? string.Empty;
            DueDate = dueDate;
        }

        public void UpdateStatus(TaskStatus status)
        {
            Status = status;
        }
    }
}