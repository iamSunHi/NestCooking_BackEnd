using Microsoft.AspNetCore.Identity;
using NESTCOOKING_API.DataAccess.Data;
using NESTCOOKING_API.DataAccess.Models;
using NESTCOOKING_API.DataAccess.Repositories.IRepositories;
using NESTCOOKING_API.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NESTCOOKING_API.DataAccess.Repositories
{
    public class ProviderRepository : Repository<User>, IProviderRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        //private readonly UserManager<IdentityUserLogin<string>> _userLoginManager;

        public ProviderRepository(ApplicationDbContext context,
            UserManager<User> userManager, RoleManager<IdentityRole> roleManager) : base(context)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IdentityResult> RegisterByGoogle(User newUser)
        {
            var result = await _userManager.CreateAsync(newUser);

            return result;
        }
        public async Task<IdentityResult> AddLogin(User newUser, UserLoginInfo userLoginInfo)
        {

            var result = await _userManager.AddLoginAsync(newUser, userLoginInfo);

            return result;
        }
    }


}
