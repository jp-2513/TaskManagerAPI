namespace TaskManagerAPI.Models
{
    public class User : BaseEntity
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public List<TaskItem> Tasks { get; set; } = new List<TaskItem>();
    }
}
