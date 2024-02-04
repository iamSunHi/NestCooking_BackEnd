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
		// Entities for Recipe
		public DbSet<Category> Categories { get; set; }
		public DbSet<IngredientTipContent> IngredientTipContents { get; set; }
		public DbSet<IngredientTip> IngredientTips { get; set; }
		public DbSet<Ingredient> Ingredients { get; set; }
		public DbSet<Instructor> Instructors { get; set; }
		public DbSet<Recipe> Recipes { get; set; }

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
		}
	}
}
