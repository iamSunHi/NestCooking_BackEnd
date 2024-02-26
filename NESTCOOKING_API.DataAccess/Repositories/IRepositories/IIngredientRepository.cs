using NESTCOOKING_API.DataAccess.Models;

namespace NESTCOOKING_API.DataAccess.Repositories.IRepositories
{
	public interface IIngredientRepository : IRepository<Ingredient>
	{
		Task UpdateAsync(Ingredient ingredient);
	}
}
