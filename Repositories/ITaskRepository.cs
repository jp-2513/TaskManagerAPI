using TaskManagerAPI.Models;

namespace TaskManagerAPI.Repositories
{
    public interface ITaskRepository : IBaseRepository<TaskItem>
    {
        Task<IEnumerable<TaskItem>> GetTasksByStatusAsync(Status status);
    }
}