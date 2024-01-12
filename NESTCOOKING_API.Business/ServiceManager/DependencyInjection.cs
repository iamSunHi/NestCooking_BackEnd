using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NESTCOOKING_API.Business.Mapping;
using NESTCOOKING_API.Business.Services;
using NESTCOOKING_API.Business.Services.IServices;
using NESTCOOKING_API.DataAccess.Data;
using NESTCOOKING_API.DataAccess.Models;
using NESTCOOKING_API.DataAccess.Repositories;
using NESTCOOKING_API.DataAccess.Repositories.IRepositories;

namespace NESTCOOKING_API.Business.ServiceManager
{
	public class DependencyInjection
	{
		public void ConfigureServices(IServiceCollection service)
		{
			var configBuilder = new ConfigurationBuilder()
					   .SetBasePath(Directory.GetCurrentDirectory())
					   .AddJsonFile("appsettings.json");
			var configurationRoot = configBuilder.Build();

			// Add repositories to the container.
			service.AddScoped<IUserRepository, UserRepository>();

			// Add services to the container.
			service.AddScoped<IAuthService, AuthService>();
			service.AddScoped<IUserService, UserService>();

			//DBContext and Identity
			service.AddDbContext<ApplicationDbContext>(options =>
			{
				options.UseSqlServer(configurationRoot.GetConnectionString("Default"));
			});
			service.AddIdentityCore<User>().AddRoles<IdentityRole>()
				.AddEntityFrameworkStores<ApplicationDbContext>();
			service.AddAutoMapper(typeof(AutoMapperProfile));
		}
	}
}
