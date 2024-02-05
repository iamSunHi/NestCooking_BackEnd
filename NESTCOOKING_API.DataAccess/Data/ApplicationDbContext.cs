using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NESTCOOKING_API.DataAccess.Models;
using static NESTCOOKING_API.Utility.StaticDetails;

namespace NESTCOOKING_API.DataAccess.Data
{
	public class ApplicationDbContext : IdentityDbContext<User>
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
			: base(options)
		{
		}

		public DbSet<User> Users { get; set; }
		public DbSet<RequestToBecomeChef> RequestToBecomeChefs { get; set; }
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<User>(user =>
			{
				// Each User can have only one Role
				user.HasOne<IdentityRole>().WithMany().HasForeignKey(u => u.RoleId).IsRequired();
				user.ToTable("Users");
			});
			//Each User can have many RequestToBecomeAchef
			modelBuilder.Entity<User>()
			.HasMany(u => u.RequestsToBecomeChefs)
			.WithOne(r => r.User)
			.HasForeignKey(r => r.UserID);

			modelBuilder.Entity<IdentityRole>(role =>
			{
				// Each Role can have many Users
				role.HasMany<User>().WithOne().HasForeignKey(u => u.RoleId).IsRequired();
				role.ToTable("Roles");
			});
			modelBuilder.Entity<IdentityUserToken<string>>().ToTable("UserTokens");
			modelBuilder.Entity<RequestToBecomeChef>(request =>
			{
				request.ToTable("RequestToBecomeChefs");
				request.HasKey(r => r.RequestChefId);

				// Each RequestToBecomeAChef can have only one User
				request.HasOne<User>(r => r.User)
					.WithMany(u => u.RequestsToBecomeChefs)
					.HasForeignKey(r => r.UserID)
					.IsRequired();

				// Property
				request.Property(r => r.Skills).IsRequired();
				request.Property(r => r.Reasons).IsRequired();
				request.Property(r => r.AchievementImageUrls).IsRequired();
				request.Property(r => r.AchievementDescriptions).IsRequired();
				request.Property(r => r.IdentityImage).IsRequired();
				request.Property(r => r.FullName).IsRequired();
				request.Property(r => r.ResponseId).IsRequired(false);
				request.Property(r => r.Status).IsRequired();
				request.Property(r => r.CreatedAt).IsRequired();
			});
		}
	}
}
