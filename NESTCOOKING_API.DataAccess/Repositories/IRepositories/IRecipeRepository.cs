using NESTCOOKING_API.DataAccess.Models;
using System.Linq.Expressions;

namespace NESTCOOKING_API.DataAccess.Repositories.IRepositories
{
	public interface IRecipeRepository : IRepository<Recipe>
	{
		Task<IEnumerable<Recipe>> GetRecipesWithPaginationAsync(int pageNumber, int pageSize);
	}
}
