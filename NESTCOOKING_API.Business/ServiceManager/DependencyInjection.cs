using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NESTCOOKING_API.Business.Mapping;
using NESTCOOKING_API.Business.Services;
using NESTCOOKING_API.Business.Services.IServices;
using NESTCOOKING_API.DataAccess.Data;
using NESTCOOKING_API.DataAccess.Models;
using NESTCOOKING_API.DataAccess.Repositories;
using NESTCOOKING_API.DataAccess.Repositories.IRepositories;
using System.Text;



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
            service.AddScoped<UserService>();

            service.AddCors(options => options.AddDefaultPolicy(policy => policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

            //DBContext and Identity
            service.AddDbContext<ApplicationDbContext>(options =>    
            {
                options.UseSqlServer(configurationRoot.GetConnectionString("Default"));
            });
            service
                .AddIdentityCore<User>()
                .AddRoles<IdentityRole>()
                .AddSignInManager<SignInManager<User>>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            service.AddAutoMapper(typeof(AutoMapperProfile));


            service.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme= CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                //options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme; // Use Google scheme for external logins
            })
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = configurationRoot["JWT:ValidAudience"],
                    ValidIssuer = configurationRoot["JWT:ValidIssuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configurationRoot["JWT:Secret"])),
                };
            })

            .AddCookie()
            .AddGoogle(options =>
              {
                  //options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
                  options.ClientId = configurationRoot["Authentication:Google:ClientId"];
                  options.ClientSecret = configurationRoot["Authentication:Google:ClientSecret"];
              })
            .AddFacebook(FacebookDefaults.AuthenticationScheme, options =>
            {
                options.AppId = configurationRoot["Authentication:Facebook:AppId"];
                options.AppSecret = configurationRoot["Authentication:Facebook:AppSecret"];
                options.SaveTokens = true;
            });



        }
    }
}
