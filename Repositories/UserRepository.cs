using Microsoft.EntityFrameworkCore;
using TaskManagerAPI.Data;
using TaskManagerAPI.Models;

namespace TaskManagerAPI.Repositories
{
    public class UserRepository : BaseRepository<User>,IUserRepository
    {
        public UserRepository(ApplicationDbContext context) :base (context){ }
        public async Task<User> GetUserByEmailAsync(string email) => await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

    }
}
