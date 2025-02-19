using TaskManagerAPI.Models;

namespace TaskManagerAPI.DTOs
{
    public class TaskItemDTO
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public Status Status { get; set; } = Status.Pending;
    }
}
