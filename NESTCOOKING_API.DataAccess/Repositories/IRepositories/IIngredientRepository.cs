using NESTCOOKING_API.DataAccess.Models;

namespace NESTCOOKING_API.DataAccess.Repositories.IRepositories
{
	public interface IIngredientRepository : IRepository<Ingredient>
	{
		Task<IEnumerable<Ingredient>> GetIngredientsWithPaginationAsync(int pageNumber, int pageSize, string includeProperties);
		Task UpdateAsync(Ingredient ingredient);
	}
}
