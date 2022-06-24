using BackendFriend.Data.Models;

namespace BackendFriend.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetUserById(Guid id);
        Task<User> GetUserByLogin(string login);
        Task Create(User user);
        Task<bool> UserExists(string login);
    }
}
