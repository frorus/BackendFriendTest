using BackendFriend.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace BackendFriend.Data
{
    public class BackendFriendContext : DbContext
    {
        public DbSet<User> Users { get; set; } = null!;

        public BackendFriendContext(DbContextOptions<BackendFriendContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
