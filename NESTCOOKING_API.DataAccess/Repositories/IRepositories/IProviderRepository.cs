using Microsoft.AspNetCore.Identity;
using NESTCOOKING_API.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NESTCOOKING_API.DataAccess.Repositories.IRepositories
{
    public interface IProviderRepository : IRepository<User>
    {
        Task<IdentityResult> RegisterByGoogle(User newUser);
        Task<IdentityResult> AddLogin(User newUser,UserLoginInfo userLoginInfo);
    }

}
