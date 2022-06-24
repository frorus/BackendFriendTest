using BackendFriend.Data;
using BackendFriend.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace BackendFriend.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly BackendFriendContext _context;

        public UserRepository(BackendFriendContext context)
        {
            _context = context;
        }

        public async Task Create(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task<User> GetUserById(Guid id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<User> GetUserByLogin(string login)
        {
            return await _context.Users.FirstOrDefaultAsync(l => l.Login == login);
        }

        public Task<bool> UserExists(string login)
        {
            return Task.FromResult((_context.Users?.Any(e => e.Login == login)).GetValueOrDefault());
        }
    }
}
