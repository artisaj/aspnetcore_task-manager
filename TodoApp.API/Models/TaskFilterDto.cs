using Microsoft.AspNetCore.Mvc;

public class TaskFilterDto
{
    [FromQuery(Name = "title")]
    public string? Title { get; set; }

    [FromQuery(Name = "status")]
    public TodoApp.Domain.Entities.TaskStatus? Status { get; set; }

    [FromQuery(Name = "due_date_min")]
    public DateTime? DueDateMin { get; set; }

    [FromQuery(Name = "due_date_max")]
    public DateTime? DueDateMax { get; set; }

    [FromQuery(Name = "page")]
    public int Page { get; set; } = 1;

    [FromQuery(Name = "page_size")]
    public int PageSize { get; set; } = 10;
}