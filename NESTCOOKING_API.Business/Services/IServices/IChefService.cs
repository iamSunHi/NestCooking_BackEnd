using NESTCOOKING_API.Business.DTOs.BookingDTOs;

namespace NESTCOOKING_API.Business.Services.IServices
{
	public interface IChefService
	{
		// Chef Dish Service
		Task<IEnumerable<ChefDishDTO>> GetAllChefDishesAsync();
		Task<ChefDishDTO> GetChefDishByIdAsync(string id);
		Task<IEnumerable<ChefDishDTO>> GetChefDishByChefIdAsync(string chefId);
		Task CreateChefDishAsync(ChefDishDTO chefDishDTO);
		Task<ChefDishDTO> UpdateChefDishAsync(ChefDishDTO chefDishDTO);
		Task RemoveChefDishAsync(string id);

		// Other
	}
}
