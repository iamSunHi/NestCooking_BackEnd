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
		public DbSet<RequestToBecomeChef> RequestToBecomeChefs { get; set; }

		// Entities for Recipe
		public DbSet<Category> Categories { get; set; }
		public DbSet<IngredientTipContent> IngredientTipContents { get; set; }
		public DbSet<IngredientTip> IngredientTips { get; set; }
		public DbSet<Ingredient> Ingredients { get; set; }
		public DbSet<Instructor> Instructors { get; set; }
		public DbSet<Recipe> Recipes { get; set; }
		public DbSet<CategoryRecipe> CategoryRecipe { get; set; }
		public DbSet<FavoriteRecipe> FavoriteRecipes { get; set; }

		public DbSet<Report> Reports { get; set; }
		public DbSet<Response> Responses { get; set; }

		public DbSet<RecipeReaction> RecipeReaction { get; set; }
		public DbSet<CommentReaction> CommentReaction { get; set; }
		public DbSet<Reaction> Reactions { get; set; }

		public DbSet<Comment> Comments { get; set; }

		public DbSet<UserConnection> UserConnections { get; set; }
		public DbSet<Transaction> Transactions { get; set; }

		public DbSet<PurchasedRecipe> PurchasedRecipes { get; set; }

		public DbSet<Notification> Notifications { get; set; }

		public DbSet<Booking> Bookings { get; set; }
		public DbSet<BookingLine> BookingLines { get; set; }

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
			});

			modelBuilder.Entity<CategoryRecipe>().HasKey(cr => new { cr.CategoryId, cr.RecipeId });
			modelBuilder.Entity<IngredientTip>(ingredientTip =>
			{
				ingredientTip.HasMany<IngredientTipContent>().WithOne().HasForeignKey(ingredientTipContent => ingredientTipContent.IngredientTipId).IsRequired(true);
				ingredientTip.HasMany<Ingredient>().WithOne().HasForeignKey(ingredient => ingredient.IngredientTipId).IsRequired(false);
			});
			modelBuilder.Entity<Recipe>(recipe =>
			{
				recipe.HasMany<Ingredient>().WithOne().HasForeignKey(ingredient => ingredient.RecipeId).IsRequired(true);
				recipe.HasMany<Instructor>().WithOne().HasForeignKey(instructor => instructor.RecipeId).IsRequired(true);
			});

			modelBuilder.Entity<User>(user =>
			{
				user.HasMany<Recipe>().WithOne().HasForeignKey(recipe => recipe.UserId).IsRequired(true);
				user.HasMany<IngredientTip>().WithOne().HasForeignKey(ingredientTip => ingredientTip.UserId).IsRequired(true);
			});

			modelBuilder.Entity<FavoriteRecipe>(favoriteRecipe =>
			{
				favoriteRecipe.HasKey(fr => new { fr.UserId, fr.RecipeId });
				favoriteRecipe.HasOne(fr => fr.User).WithMany().HasForeignKey(fr => fr.UserId).IsRequired(true).OnDelete(DeleteBehavior.NoAction);
				favoriteRecipe.HasOne(fr => fr.Recipe).WithMany().HasForeignKey(fr => fr.RecipeId).IsRequired(true).OnDelete(DeleteBehavior.NoAction);
			});

			modelBuilder.Entity<Reaction>(reaction =>
			{
				reaction.HasData(new Reaction { Id = "2c01c25c-11b6-40df-a60e-d8bf59beb411", Emoji = "like" });

				reaction.HasData(new Reaction { Id = "e3fd7581-60e2-40aa-960b-e006586e63b5", Emoji = "favorite" });

				reaction.HasData(new Reaction { Id = "42996687-e4a0-4168-b03d-bd09a7534773", Emoji = "haha" });
			});

			modelBuilder.Entity<Comment>(comment =>
			{
				comment.ToTable("Comments");
				comment.HasKey(comment => comment.CommentId);
				comment.HasOne<Recipe>(c => c.Recipe)
					.WithMany(r => r.Comments)
					.HasForeignKey(c => c.RecipeId)
					.IsRequired();
				comment.HasOne<User>(c => c.User)
					.WithMany(u => u.Comments)
					.HasForeignKey(c => c.UserId)
					.IsRequired();
			});

			modelBuilder.Entity<UserConnection>(connection =>
			{
				connection.HasKey(uc => new { uc.UserId, uc.FollowingUserId });
				connection.HasOne<User>().WithMany().HasForeignKey(uc => uc.UserId).IsRequired(true).OnDelete(DeleteBehavior.NoAction);
				connection.HasOne<User>().WithMany().HasForeignKey(uc => uc.FollowingUserId).IsRequired(true).OnDelete(DeleteBehavior.NoAction);
			});

			modelBuilder.Entity<Notification>(notification =>
			{
				notification.HasOne<User>().WithMany().HasForeignKey(n => n.SenderId).IsRequired(false).OnDelete(DeleteBehavior.NoAction);
				notification.HasOne<User>().WithMany().HasForeignKey(n => n.ReceiverId).IsRequired(false).OnDelete(DeleteBehavior.NoAction);
			});

			modelBuilder.Entity<Booking>(booking =>
			{
				booking.ToTable("Bookings");
				booking.HasOne(b => b.User).WithMany(user => user.Bookings).HasForeignKey(b => b.UserId).IsRequired();
			});

			modelBuilder.Entity<BookingLine>()
				 .ToTable("BookingLines")
				 .HasKey(bl => new { bl.RecipeId, bl.BookingId });

			modelBuilder.Entity<BookingLine>()
				.HasOne(bl => bl.Recipe)
				.WithMany(r => r.BookingLines)
				.HasForeignKey(bl => bl.RecipeId);

			modelBuilder.Entity<BookingLine>()
				.HasOne(bl => bl.Booking)
				.WithMany(b => b.BookingLines)
				.HasForeignKey(bl => bl.BookingId);

		}
	}
}
