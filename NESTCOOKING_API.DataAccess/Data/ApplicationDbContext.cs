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

		public DbSet<Booking> Bookings { get; set; }
		public DbSet<Category> Categories { get; set; }
		public DbSet<Instructor> Instructors { get; set; }
		public DbSet<Comment> Comments { get; set; }
		public DbSet<CommentHierarchy> CommentHierarchies { get; set; }
		public DbSet<CommentReaction> CommentReactions { get; set; }
		public DbSet<Ingredient> Ingredients { get; set; }
		public DbSet<Message> Messages { get; set; }
		public DbSet<Notification> Notifications { get; set; }
		public DbSet<Payment> Payments { get; set; }
		public DbSet<Recipe> Recipes { get; set; }
		public DbSet<PurchasedRecipe> PurchasedRecipes { get; set; }
		public DbSet<Reaction> Reactions { get; set; }
		public DbSet<Report> Reports { get; set; }
		public DbSet<RequestToBecomeChef> RequestsToBecomeChef { get; set; }
		public DbSet<Response> Responses { get; set; }
		public DbSet<User> Users { get; set; }
		public DbSet<UserConnection> UserConnections { get; set; }

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
