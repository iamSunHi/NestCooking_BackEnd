using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NESTCOOKING_API.DataAccess.Data;
using NESTCOOKING_API.DataAccess.Models;
using NESTCOOKING_API.DataAccess.Repositories.IRepositories;
using NESTCOOKING_API.Utility;

namespace NESTCOOKING_API.DataAccess.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserRepository(ApplicationDbContext context,
            UserManager<User> userManager, RoleManager<IdentityRole> roleManager) : base(context)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }


        public bool IsUniqueEmail(string email)
        {
            var existedUser = _context.Users.FirstOrDefault(x => x.Email == email);
            if (existedUser == null)
            {
                return true;
            }
            return false;
        }

        public bool IsUniqueUserName(string username)
        {
            var user = _context.Users.FirstOrDefault(x => x.UserName == username);
            if (user == null)
            {
                return true;
            }
            return false;
        }

        public async Task<User> Login(string username, string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserName == username || u.Email == username);
            if (user != null)
            {
                bool isValid = await _userManager.CheckPasswordAsync(user, password);
                if (!isValid)
                {
                    if (!_userManager.IsLockedOutAsync(user).Result)
                    {
                        await _userManager.AccessFailedAsync(user);
                        if (await _userManager.GetAccessFailedCountAsync(user) == 3)
                        {
                            await _userManager.SetLockoutEndDateAsync(user, DateTime.UtcNow.AddMinutes(30));
                            await _userManager.ResetAccessFailedCountAsync(user);
                        }
                    }
                    return null;
                }
            }
            return user;
        }

        public async Task<bool> Register(User newUser, string password)
        {
            if (!_roleManager.RoleExistsAsync(StaticDetails.Role_Admin).GetAwaiter().GetResult())
            {
                await _roleManager.CreateAsync(new IdentityRole(StaticDetails.Role_Admin)).ConfigureAwait(false);
                await _roleManager.CreateAsync(new IdentityRole(StaticDetails.Role_User)).ConfigureAwait(false);
                await _roleManager.CreateAsync(new IdentityRole(StaticDetails.Role_Chef)).ConfigureAwait(false);
            }

            var result = await _userManager.CreateAsync(newUser, password).ConfigureAwait(false);

            if (!result.Succeeded)
            {
                throw new Exception(result.Errors.FirstOrDefault()?.Description);
            }

            await _userManager.AddToRoleAsync(newUser, StaticDetails.Role_User).ConfigureAwait(false);

            var userExists = await _context.Users.AnyAsync(u => u.UserName == newUser.UserName).ConfigureAwait(false);
            if (!userExists)
            {
                throw new Exception(AppString.SomethingWrongMessage);
            }

            return true;
        }

        public async Task UpdateAsync(User entity)
        {
            throw new NotImplementedException();
        }
    }
}
