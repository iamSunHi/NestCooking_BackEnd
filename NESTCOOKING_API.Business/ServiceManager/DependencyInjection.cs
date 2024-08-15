using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using NESTCOOKING_API.Business.Authorization;
using NESTCOOKING_API.Business.BackgroundServices;
using NESTCOOKING_API.Business.DTOs.EmailDTOs;
using NESTCOOKING_API.Business.Libraries;
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
		public void ConfigureServices(IServiceCollection service, IConfiguration configuration, string databaseName)
		{
			#region Repositories

			service.AddScoped<IUserRepository, UserRepository>();
			service.AddScoped<IRoleRepository, RoleRepository>();
			service.AddScoped<IResponseRepository, ResponseRepository>();
			service.AddScoped<IRequestBecomeChefRepository, RequestBecomeChefRepository>();
			service.AddScoped<ICategoryRepository, CategoryRepository>();
			service.AddScoped<IIngredientTipContentRepository, IngredientTipContentRepository>();
			service.AddScoped<IIngredientTipRepository, IngredientTipRepository>();
			service.AddScoped<IIngredientRepository, IngredientRepository>();
			service.AddScoped<IRecipeRepository, RecipeRepository>();
			service.AddScoped<ICategoryRecipeRepository, CategoryRecipeRepository>();
			service.AddScoped<IInstructorRepository, InstructorRepository>();
			service.AddScoped<IFavoriteRecipeRepository, FavoriteRecipeRepository>();
			service.AddScoped<IOAuthRepository, OAuthRepository>();
			service.AddScoped<IReportRepository, ReportRepository>();
			service.AddScoped<IReactionRepository, ReactionRepository>();
			service.AddScoped<ICommentRepository, CommentRepository>();
			service.AddScoped<IUserConnectionRepository, UserConnectionRepository>();
			service.AddScoped<ITransactionRepository, TransactionRepository>();
			service.AddScoped<INotificationRepository, NotificationRepository>();
			service.AddScoped<IPurchasedRecipesRepository, PurchasedRecipesRepository>();
			service.AddScoped<IBookingLineRepository, BookingLineRepository>();
			service.AddScoped<IBookingRepository, BookingRepository>();

			// For admin statistic
			service.AddSingleton<IStatisticRepository, StatisticRepository>();
			service.AddScoped<IBookingStatisticRepository, BookingStatisticRepository>();
			service.AddScoped<IChefStatisticRepository, ChefStatisticRepository>();
			service.AddScoped<IViolationStatisticRepository, ViolationStatisticRepository>();
			service.AddScoped<IUserStatisticRepository, UserStatisticRepository>();
			service.AddScoped<IRevenueStatisticRepository, RevenueStatisticRepository>();

			#endregion Repositories

			#region Services

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
			service.AddScoped<IReactionService, ReactionService>();
			service.AddScoped<ICommentService, CommentService>();
			service.AddScoped<IUserConnectionService, UserConnectionService>();
			service.AddScoped<IPaymentService, PaymentService>();
			service.AddScoped<VnPayLibrary>();
			service.AddScoped<ITransactionService, TransactionService>();
			service.AddScoped<INotificationService, NotificationService>();
			service.AddScoped<IPurchasedRecipesService, PurchasedRecipesService>();
			service.AddScoped<IBookingService, BookingService>();
			service.AddScoped<IBookingLineService, BookingLineService>();
			// For admin statistic
			//service.AddHostedService<StatisticBackgroundService>();
			service.AddScoped<IStatisticService, StatisticService>();

			#endregion Services

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

			service.AddSingleton(new EmailRequestDTO
			{
				From = configuration["APPSETTING_EMAIL_FROM"],
				SmtpServer = configuration["APPSETTING_EMAIL_SMTP_SERVER"],
				Port = int.Parse(configuration["APPSETTING_EMAIL_PORT"]),
				UserName = configuration["APPSETTING_EMAIL_USERNAME"],
				Password = configuration["APPSETTING_EMAIL_PASSWORD"]
			});

			// DBContext and Identity
			service.AddDbContext<ApplicationDbContext>(options =>
			{
				options.UseSqlServer(configuration.GetConnectionString(databaseName), options =>
				{
					options.EnableRetryOnFailure();
				});
			});

			// Auto update database
			using (var applicationDbContext = service.BuildServiceProvider().GetService<ApplicationDbContext>())
			{
				applicationDbContext.Database.Migrate();
			}

			service.AddIdentityCore<User>(options =>
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
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["APPSETTING_API_SECRET"])),
				};
			})
			.AddGoogle(GoogleDefaults.AuthenticationScheme, options =>
			{
				options.ClientId = configuration["APPSETTING_GOOGLE_CLIENT_ID"];
				options.ClientSecret = configuration["APPSETTING_GOOGLE_CLIENT_SECRET"];
			})
			.AddFacebook(FacebookDefaults.AuthenticationScheme, options =>
			{
				options.AppId = configuration["APPSETTING_FACEBOOK_APP_ID"];
				options.AppSecret = configuration["APPSETTING_FACEBOOK_APP_SECRET"];
			});
		}
	}
}
