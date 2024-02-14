using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NESTCOOKING_API.Business.Authorization;
using NESTCOOKING_API.Business.DTOs.EmailDTOs;
using NESTCOOKING_API.Business.Mapping;
using NESTCOOKING_API.Business.Services.IServices;
using NESTCOOKING_API.Business.Services;
using NESTCOOKING_API.DataAccess.Data;
using NESTCOOKING_API.DataAccess.Models;
using NESTCOOKING_API.DataAccess.Repositories.IRepositories;
using NESTCOOKING_API.DataAccess.Repositories;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace NESTCOOKING_API.Business.ServiceManager
{
	public class AzureDependencyInjection
	{
		public void ConfigureServices(IServiceCollection service)
		{
			// Config for github CI/CD
			string appSettingsJson = Environment.GetEnvironmentVariable("APPSETTINGS");
			var appSettings = JsonSerializer.Deserialize<Dictionary<string, dynamic>>(appSettingsJson);

			// Add repositories to the container.
			service.AddScoped<IUserRepository, UserRepository>();
			service.AddScoped<IRoleRepository, RoleRepository>();
			service.AddScoped<IReportRepository, ReportRepository>();
			service.AddScoped<IResponseRepository, ResponseRepository>();
			service.AddScoped<IChefRequestRepository, ChefRequestRepository>();

			// Add services to the container.
			service.AddScoped<IJwtUtils, JwtUtils>();
			service.AddScoped<IAuthService, AuthService>();
			service.AddScoped<IUserService, UserService>();
			service.AddScoped<IEmailService, EmailService>();
			service.AddScoped<IReportService, ReportService>();
			service.AddScoped<IResponseService, ResponseService>();
			service.AddScoped<IRequestBecomeChefService, RequestBecomeChefService>();
			service.AddScoped<IRepository<Report>, ReportRepository>();

			service.AddCors(options =>
			{
				options.AddPolicy("AllowAnyOrigin", builder =>
				{
					builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
				});
			});
			service.AddAuthorization(options =>
			{
				options.AddPolicy("admin", policy => policy.RequireRole("admin"));
			});
			// Add repositories to the container.
			service.AddScoped<IUserRepository, UserRepository>();
			service.AddScoped<IRoleRepository, RoleRepository>();
			service.AddScoped<IChefRequestRepository, ChefRequestRepository>();
			service.AddScoped<ICategoryRepository, CategoryRepository>();
			service.AddScoped<IIngredientTipContentRepository, IngredientTipContentRepository>();
			service.AddScoped<IIngredientTipRepository, IngredientTipRepository>();
			service.AddScoped<IIngredientRepository, IngredientRepository>();
			service.AddScoped<IRecipeRepository, RecipeRepository>();
			service.AddScoped<ICategoryRecipeRepository, CategoryRecipeRepository>();
			service.AddScoped<IInstructorRepository, InstructorRepository>();
			service.AddScoped<IOAuthRepository, OAuthRepository>();

			// Add services to the container.
			service.AddScoped<IJwtUtils, JwtUtils>();
			service.AddScoped<IAuthService, AuthService>();
			service.AddScoped<IUserService, UserService>();
			service.AddScoped<IEmailService, EmailService>();
			service.AddScoped<IRequestBecomeChefService, RequestBecomeChefService>();
			service.AddScoped<ICategoryService, CategoryService>();
			service.AddScoped<IIngredientTipService, IngredientTipService>();
			service.AddScoped<IIngredientService, IngredientService>();
			service.AddScoped<IRecipeService, RecipeService>();
			service.AddScoped<ICloudinaryService, CloudinaryService>();


			service.AddHttpClient();

			service.AddCors(options =>
			{
				options.AddPolicy("AllowAnyOrigin", builder =>
				{
					builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
				});
			});

			// Configure for email
			service.Configure<IdentityOptions>(options => options.SignIn.RequireConfirmedEmail = true);
			// Set time Token for Email Confirm
			service.Configure<DataProtectionTokenProviderOptions>(options => options.TokenLifespan = TimeSpan.FromMinutes(20));

			var emailConfiguration = new EmailRequestDTO
			{
				From = appSettings["EmailConfiguration:From"],
				SmtpServer = appSettings["EmailConfiguration:SmtpServer"],
				Port = Convert.ToInt32(appSettings["EmailConfiguration:Port"]),
				UserName = appSettings["EmailConfiguration:Username"],
				Password = appSettings["EmailConfiguration:Password"]
			};
			service.AddSingleton(emailConfiguration);

			// DBContext and Identity
			string connectionString = appSettings["ConnectionStrings:Server"];
			service.AddDbContext<ApplicationDbContext>(options =>
			{
				options.UseSqlServer(connectionString);
			});

			service
				.AddIdentityCore<User>(options =>
				{
					options.Lockout.AllowedForNewUsers = true;
					options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
					options.Lockout.MaxFailedAccessAttempts = 3;
				})
				.AddRoles<IdentityRole>()
				.AddSignInManager<SignInManager<User>>()
				.AddEntityFrameworkStores<ApplicationDbContext>()
				.AddDefaultTokenProviders();
			service.AddAutoMapper(typeof(AutoMapperProfile));
			service
				.AddIdentityCore<User>(options =>
				{
					options.Lockout.AllowedForNewUsers = true;
					options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
					options.Lockout.MaxFailedAccessAttempts = 3;
				})
				.AddRoles<IdentityRole>()
				.AddSignInManager<SignInManager<User>>()
				.AddEntityFrameworkStores<ApplicationDbContext>()
				.AddDefaultTokenProviders();
			service.AddAutoMapper(typeof(AutoMapperProfile));

			service.AddAuthentication(options =>
			{
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
				// For Google & Facebook login
				options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
				options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
			})
			.AddCookie()
			.AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
			{
				options.SaveToken = true;
				options.RequireHttpsMetadata = false;
				options.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuer = false,
					ValidateAudience = false,
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appSettings["ApiSettings:Secret"])),
				};
			})
			.AddGoogle(GoogleDefaults.AuthenticationScheme, options =>
			{
				options.ClientId = appSettings["Authentication:Google:ClientId"];
				options.ClientSecret = appSettings["Authentication:Google:ClientSecret"];
			})
			.AddFacebook(FacebookDefaults.AuthenticationScheme, options =>
			{
				options.AppId = appSettings["Authentication:Facebook:AppId"];
				options.AppSecret = appSettings["Authentication:Facebook:AppSecret"];
			});
		}
	}
}
