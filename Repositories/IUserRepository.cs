using TaskManagerAPI.Models;

namespace TaskManagerAPI.Repositories
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task<User> GetUserByEmailAsync(string email);
    }
}
