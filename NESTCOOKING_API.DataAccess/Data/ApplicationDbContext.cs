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
        public DbSet<Reaction> Reaction { get; set; }

        public DbSet<Comment> Comments { get; set; }
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
        }
                favoriteRecipe.HasOne(fr => fr.Recipe).WithMany().HasForeignKey(fr => fr.RecipeId).IsRequired(true).OnDelete(DeleteBehavior.NoAction);
            });
            SeedData(modelBuilder);
        }
        private void SeedData(ModelBuilder modelBuilder)
        {       
                modelBuilder.Entity<Reaction>().HasData(new Reaction { Id = Guid.NewGuid().ToString(), Emoji = "like"});
                 
                modelBuilder.Entity<Reaction>().HasData(new Reaction { Id = Guid.NewGuid().ToString(), Emoji = "favorite"});
                      
                modelBuilder.Entity<Reaction>().HasData(new Reaction {Id = Guid.NewGuid().ToString(), Emoji = "haha"});
            
        }
    }
}
