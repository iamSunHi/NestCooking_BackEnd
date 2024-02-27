using AutoMapper;
using NESTCOOKING_API.Business.DTOs;
using NESTCOOKING_API.Business.DTOs.RecipeDTOs;
using NESTCOOKING_API.Business.Services.IServices;
using NESTCOOKING_API.DataAccess.Models;
using NESTCOOKING_API.DataAccess.Repositories;
using NESTCOOKING_API.DataAccess.Repositories.IRepositories;

namespace NESTCOOKING_API.Business.Services
{
	public class CategoryService : ICategoryService
	{
		private readonly IMapper _mapper;
		private readonly ICategoryRepository _categoryRepository;

		public CategoryService(IMapper mapper, ICategoryRepository categoryRepository)
		{
			_mapper = mapper;
			_categoryRepository = categoryRepository;
		}

		public async Task<IEnumerable<CategoryDTO>> GetAllCategoriesAsync()
		{
			var categories = await _categoryRepository.GetAllAsync();
			return _mapper.Map<IEnumerable<CategoryDTO>>(categories);
		}

		public async Task<(int, int, IEnumerable<CategoryDTO>)> GetCategoriesAsync(PaginationInfoDTO paginationInfo)
		{
			var totalItems = (await _categoryRepository.GetAllAsync()).Count();
			var totalPages = (int)Math.Ceiling((double)totalItems / paginationInfo.PageSize);
			var categoriesFromDb = await _categoryRepository.GetCategoriesWithPaginationAsync(paginationInfo.PageNumber, paginationInfo.PageSize);
			if (categoriesFromDb == null)
			{
				return (totalItems, totalPages, null);
			}

			var categoryList = _mapper.Map<IEnumerable<CategoryDTO>>(categoriesFromDb);
			return (totalItems, totalPages, categoryList);
		}

		public async Task<CategoryDTO> GetCategoryByIdAsync(int id)
		{
			var category = await _categoryRepository.GetAsync(c => c.Id == id);
			return _mapper.Map<CategoryDTO>(category);
		}

		public async Task<CategoryDTO> CreateCategoryAsync(CategoryDTO categoryDTO)
		{
			if (await IsUnique(categoryDTO.Name))
			{
				Category category = new()
				{
					Name = categoryDTO.Name,
					ImageUrl = categoryDTO.ImageUrl
				};
				await _categoryRepository.CreateAsync(category);

				var createdCategory = await _categoryRepository.GetAsync(c => c.Name == categoryDTO.Name);
				return _mapper.Map<CategoryDTO>(createdCategory);
			}
			return null;
		}

		public async Task UpdateCategoryAsync(CategoryDTO categoryDTO)
		{
			await _categoryRepository.UpdateAsync(_mapper.Map<Category>(categoryDTO));
		}
		public async Task DeleteCategoryAsync(int id)
		{
			var categoryFromDb = await _categoryRepository.GetAsync(c => c.Id == id);
			if (categoryFromDb != null)
			{
				await _categoryRepository.RemoveAsync(categoryFromDb);
			}
		}

		private async Task<bool> IsUnique(string categoryName)
		{
			var category = await _categoryRepository.GetAsync(c => c.Name == categoryName);
			if (category == null)
			{
				return true;
			}
			return false;
		}
	}
}
