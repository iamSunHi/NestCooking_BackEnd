using Microsoft.AspNetCore.Identity;

namespace NESTCOOKING_API.DataAccess.Repositories.IRepositories
{
	public interface IRoleRepository : IRepository<IdentityRole>
	{
		Task<string> GetRoleNameAsync(string roleId);
		Task<string> GetRoleIdAsync(string roleName);
		Task<bool> ChangeRoleAsync(string username, string roleName);
	}
}
