using NESTCOOKING_API.Business.DTOs;
using NESTCOOKING_API.Business.DTOs.CategoryDTOs;

namespace NESTCOOKING_API.Business.Services.IServices
{
	public interface ICategoryService
	{
		Task<IEnumerable<CategoryDTO>> GetAllCategoriesAsync();
		Task<IEnumerable<CategoryDTO>> GetCategoriesAsync(PaginationInfoDTO paginationInfo);
		Task<CategoryDTO> GetCategoryByIdAsync(int id);
		Task<CategoryDTO> CreateCategoryAsync(CategoryDTO categoryDTO);
		Task UpdateCategoryAsync(CategoryDTO categoryDTO);
		Task DeleteCategoryAsync(int id);
	}
}
