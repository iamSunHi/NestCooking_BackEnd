using Microsoft.AspNetCore.Identity;

namespace NESTCOOKING_API.DataAccess.Repositories.IRepositories
{
	public interface IRoleRepository : IRepository<IdentityRole>
	{
		Task<string> GetRoleNameByIdAsync(string roleId);
		Task<string> GetRoleIdByNameAsync(string roleName);
		Task<bool> ChangeRoleAsync(string username, string roleName);
	}
}
