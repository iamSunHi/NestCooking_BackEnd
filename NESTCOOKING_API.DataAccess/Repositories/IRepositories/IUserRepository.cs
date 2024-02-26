using NESTCOOKING_API.DataAccess.Models;

namespace NESTCOOKING_API.DataAccess.Repositories.IRepositories
{
    public interface IUserRepository : IRepository<User>
	{
		Task<string> GetRoleAsync(string userId);
		bool IsUniqueEmail(string email);
        bool IsUniqueUserName(string username);
        Task<User> Login(string username, string password);
        Task<bool> Register(User newUser, string password);
        Task<IEnumerable<User>> GetUsersByCriteriaAsync(string criteria, string? userId = null);
	}
}