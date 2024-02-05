using NESTCOOKING_API.DataAccess.Models;

namespace NESTCOOKING_API.DataAccess.Repositories.IRepositories
{
	public interface IIngredientTipRepository : IRepository<IngredientTip>
	{
		Task<IEnumerable<IngredientTip>> GetIngredientTipsWithPaginationAsync(int pageNumber, int pageSize, string? includeProperties = null);
		Task UpdateAsync(IngredientTip ingredientTip);
	}
}
