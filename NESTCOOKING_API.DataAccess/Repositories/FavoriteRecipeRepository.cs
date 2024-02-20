using NESTCOOKING_API.DataAccess.Data;
using NESTCOOKING_API.DataAccess.Models;
using NESTCOOKING_API.DataAccess.Repositories.IRepositories;

namespace NESTCOOKING_API.DataAccess.Repositories
{
	public class FavoriteRecipeRepository : Repository<FavoriteRecipe>, IFavoriteRecipeRepository
	{
		public FavoriteRecipeRepository(ApplicationDbContext context) : base(context)
		{
		}
	}
}
