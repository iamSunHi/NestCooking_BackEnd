using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using NESTCOOKING_API.Business.Authorization;
using NESTCOOKING_API.Business.DTOs.EmailDTOs;
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
	public class AzureDependencyInjection
	{
		public void ConfigureServices(IServiceCollection service)
		{
			// Config for github CI/CD
			string dbConnectionStringServer = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING_SERVER");
			string emailFrom = Environment.GetEnvironmentVariable("EMAIL_FROM");
			string emailSmtpServer = Environment.GetEnvironmentVariable("EMAIL_SMTP_SERVER");
			int emailPort = Convert.ToInt32(Environment.GetEnvironmentVariable("EMAIL_PORT"));
			string emailUsername = Environment.GetEnvironmentVariable("EMAIL_USERNAME");
			string emailPassword = Environment.GetEnvironmentVariable("EMAIL_PASSWORD");
			string apiSecret = Environment.GetEnvironmentVariable("API_SECRET");
			string facebookAppId = Environment.GetEnvironmentVariable("FACEBOOK_APP_ID");
			string facebookAppSecret = Environment.GetEnvironmentVariable("FACEBOOK_APP_SECRET");
			string googleClientId = Environment.GetEnvironmentVariable("GOOGLE_CLIENT_ID");
			string googleClientSecret = Environment.GetEnvironmentVariable("GOOGLE_CLIENT_SECRET");

			// Add repositories to the container.
			service.AddScoped<IUserRepository, UserRepository>();
			service.AddScoped<IRoleRepository, RoleRepository>();
			service.AddScoped<IReportRepository, ReportRepository>();
			service.AddScoped<IResponseRepository, ResponseRepository>();
			service.AddScoped<IRequestBecomeChefRepository, RequestBecomeChefRepository>();
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
			service.AddScoped<IReportService, ReportService>();
			service.AddScoped<IResponseService, ResponseService>();
			service.AddScoped<IRequestBecomeChefService, RequestBecomeChefService>();
			service.AddScoped<ICategoryService, CategoryService>();
			service.AddScoped<IIngredientTipService, IngredientTipService>();
			service.AddScoped<IIngredientService, IngredientService>();
			service.AddScoped<IRecipeService, RecipeService>();
			service.AddScoped<ICloudinaryService, CloudinaryService>();
			service.AddScoped<ISearchService, SearchService>();
			service.AddScoped<IPaymentService, PaymentService>();

			service.AddHttpClient();

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

			// Configure for email
			service.Configure<IdentityOptions>(options => options.SignIn.RequireConfirmedEmail = true);
			// Set time Token for Email Confirm
			service.Configure<DataProtectionTokenProviderOptions>(options => options.TokenLifespan = TimeSpan.FromMinutes(20));

			var emailConfiguration = new EmailRequestDTO
			{
				From = emailFrom,
				SmtpServer = emailSmtpServer,
				Port = emailPort,
				UserName = emailUsername,
				Password = emailPassword
			};
			service.AddSingleton(emailConfiguration);

			// DBContext and Identity
			service.AddDbContext<ApplicationDbContext>(options =>
			{
				options.UseSqlServer(dbConnectionStringServer);
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
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(apiSecret)),
				};
			})
			.AddGoogle(GoogleDefaults.AuthenticationScheme, options =>
			{
				options.ClientId = googleClientId;
				options.ClientSecret = googleClientSecret;
			})
			.AddFacebook(FacebookDefaults.AuthenticationScheme, options =>
			{
				options.AppId = facebookAppId;
				options.AppSecret = facebookAppSecret;
			});
		}
	}
}
