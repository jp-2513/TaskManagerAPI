namespace TaskManagerAPI.Models
{
    public class TaskItem : BaseEntity
    {

        public string Title { get; set; }
        public string Description { get; set; }
        public Status Status { get; set; } = Status.Pending;
        public int CreatedById { get; set; }
        public User CreatedBy { get; set; }
    }

    public enum Status
    {
        Pending,
        InProgress,
        Completed
    }
}
