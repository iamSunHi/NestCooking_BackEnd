using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NESTCOOKING_API.DataAccess.Data;
using NESTCOOKING_API.DataAccess.Models;
using NESTCOOKING_API.DataAccess.Repositories.IRepositories;
using NESTCOOKING_API.Utility;
using System.Text.RegularExpressions;
using System.Text;
using Microsoft.AspNetCore.WebUtilities;

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
            var user = _context.Users.FirstOrDefault(u => u.UserName == username);
            if (user != default)
            {
                bool isValid = await _userManager.CheckPasswordAsync(user, password);
                if (!isValid)
                {
                    return null;
                }
            }
            return user;
        }

        public async Task<string> Register(User newUser, string password)
        {
            try
            {
                var result = await _userManager.CreateAsync(newUser, password);
                if (result.Succeeded)
                {
                    if (!_roleManager.RoleExistsAsync(StaticDetails.Role_Admin).GetAwaiter().GetResult())
                    {
                        await _roleManager.CreateAsync(new IdentityRole(StaticDetails.Role_Admin));
                        await _roleManager.CreateAsync(new IdentityRole(StaticDetails.Role_User));
                        await _roleManager.CreateAsync(new IdentityRole(StaticDetails.Role_Chef));
                    }
                    //await _userManager.AddToRoleAsync(newUser, StaticDetails.Role_Admin);
                    await _userManager.AddToRoleAsync(newUser, StaticDetails.Role_User);

                    var userToReturn = await _context.Users.FirstOrDefaultAsync(u => u.UserName == newUser.UserName);
                    if (userToReturn == null)
                    {
                        return "Error while registering!";
                    }
                }
                else
                {
                    string error = result.Errors.FirstOrDefault()?.Description;
                    return error;
                }
            }
            catch (Exception ex)
            {
            }

            return string.Empty;
        }

        public async Task UpdateAsync(User entity)
        {
            throw new NotImplementedException();
        }
    }
}
