using NESTCOOKING_API.DataAccess.Models;

namespace NESTCOOKING_API.DataAccess.Repositories.IRepositories
{
    public interface IUserRepository : IRepository<User>
    {
        bool IsUniqueEmail(string email);
        bool IsUniqueUserName(string username);
        Task UpdateAsync(User entity);
        Task<User> Login(string username, string password);
        Task<string> Register(User newUser, string password);
    }
}
