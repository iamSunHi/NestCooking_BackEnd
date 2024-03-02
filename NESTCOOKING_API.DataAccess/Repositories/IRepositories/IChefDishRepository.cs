using NESTCOOKING_API.DataAccess.Models;

namespace NESTCOOKING_API.DataAccess.Repositories.IRepositories
{
	public interface IChefDishRepository : IRepository<ChefDish>
	{
		Task UpdateAsync(ChefDish chefDish);
	}
}
