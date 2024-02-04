using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NESTCOOKING_API.DataAccess.Models;

namespace NESTCOOKING_API.DataAccess.Data
{
	public class ApplicationDbContext : IdentityDbContext<User>
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
			: base(options)
		{
		}

		public DbSet<User> Users { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<Response> Responses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<User>(user =>
			{
				// Each User can have only one Role
				user.HasOne<IdentityRole>().WithMany().HasForeignKey(u => u.RoleId).IsRequired();
				user.ToTable("Users");
			});
			modelBuilder.Entity<IdentityRole>(role =>
			{
				// Each Role can have many Users
				role.HasMany<User>().WithOne().HasForeignKey(u => u.RoleId).IsRequired();
				role.ToTable("Roles");
			});
			modelBuilder.Entity<IdentityUserToken<string>>().ToTable("UserTokens");
			modelBuilder.Entity<Response>();

		}
	}
}
