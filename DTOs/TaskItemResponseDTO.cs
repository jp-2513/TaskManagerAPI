using TaskManagerAPI.Models;

namespace TaskManagerAPI.DTOs
{
    public class TaskItemResponseDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Status Status { get; set; }
        public int CreatedById { get; set; }
        public string CreatedByName { get; set; } 
    }
}