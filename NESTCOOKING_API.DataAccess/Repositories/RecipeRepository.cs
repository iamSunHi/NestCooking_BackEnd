﻿using Microsoft.EntityFrameworkCore;
using NESTCOOKING_API.DataAccess.Data;
using NESTCOOKING_API.DataAccess.Models;
using NESTCOOKING_API.DataAccess.Repositories.IRepositories;

namespace NESTCOOKING_API.DataAccess.Repositories
{
	public class RecipeRepository : Repository<Recipe>, IRecipeRepository
	{
		public RecipeRepository(ApplicationDbContext context) : base(context)
		{
		}

		public async Task<IEnumerable<Recipe>> GetRecipesWithPaginationAsync(int pageNumber, int pageSize)
		{
			var skipNumber = (pageNumber - 1) * pageSize;
			var query = _dbSet.AsQueryable<Recipe>();

			var recipes = await query.Skip(skipNumber).Take(pageSize).ToListAsync();
			return recipes;
		}

		public async Task UpdateAsync(Recipe recipe)
		{
			var recipeFromDb = await this.GetAsync(r => r.Id == recipe.Id);

			if (recipeFromDb != null)
			{
				if (_context.Entry(recipeFromDb).State == EntityState.Detached)
				{
					_context.Attach(recipeFromDb);
				}

				recipeFromDb.Title = recipe.Title;
				recipeFromDb.Description = recipe.Description;
				recipeFromDb.ThumbnailUrl = recipe.ThumbnailUrl;
				recipeFromDb.CookingTime = recipe.CookingTime;
				recipeFromDb.Portion = recipe.Portion;
				recipeFromDb.UpdatedAt = recipe.UpdatedAt;

				await _context.SaveChangesAsync();
			}
		}

	}
}