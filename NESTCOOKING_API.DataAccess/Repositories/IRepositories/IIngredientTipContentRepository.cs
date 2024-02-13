using NESTCOOKING_API.DataAccess.Models;

namespace NESTCOOKING_API.DataAccess.Repositories.IRepositories
{
	public interface IIngredientTipContentRepository : IRepository<IngredientTipContent>
	{
		Task<IEnumerable<IngredientTipContent>> GetIngredientTipContentsWithPaginationAsync(int pageNumber, int pageSize);
		Task UpdateAsync(IngredientTipContent ingredientTipContent);
	}
}
