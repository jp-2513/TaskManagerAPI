using Microsoft.EntityFrameworkCore;
using TaskManagerAPI.Data;
using TaskManagerAPI.Models;

namespace TaskManagerAPI.Repositories
{

    public class TaskRepository : BaseRepository<TaskItem>, ITaskRepository
    {
        public TaskRepository(ApplicationDbContext context) : base(context) { }

        public async Task<IEnumerable<TaskItem>> GetTasksByStatusAsync(Status status)
        {
            return await _dbSet.Where(t => t.Status == status).Include(t=>t.CreatedBy).ToListAsync();
        }
        public async Task<TaskItem> GetByIdAsync(int id)
        {
            return await _context.Tasks
                .Include(t => t.CreatedBy) 
                .FirstOrDefaultAsync(t => t.Id == id);
        }
    }
}
