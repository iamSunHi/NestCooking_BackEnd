using AutoMapper;
using Microsoft.AspNetCore.Identity;
using NESTCOOKING_API.Business.DTOs;
using NESTCOOKING_API.Business.DTOs.RecipeDTOs;
using NESTCOOKING_API.Business.Services.IServices;
using NESTCOOKING_API.DataAccess.Models;
using NESTCOOKING_API.DataAccess.Repositories.IRepositories;

namespace NESTCOOKING_API.Business.Services
{
	public class RecipeService : IRecipeService
	{
		private readonly IMapper _mapper;
		private readonly IRecipeRepository _recipeRepository;
		private readonly ICategoryRepository _categoryRepository;
		private readonly UserManager<User> _userManager;

        public RecipeService(IMapper mapper,
			IRecipeRepository recipeRepository, ICategoryRepository categoryRepository,
			UserManager<User> userManager)
        {
			_mapper = mapper;
            _recipeRepository = recipeRepository;
			_categoryRepository = categoryRepository;
			_userManager = userManager;
		}

		public async Task<IEnumerable<RecipeDTO>> GetAllRecipesAsync()
		{
			var recipesFromDb = await _recipeRepository.GetAllAsync(includeProperties: "User");
			return _mapper.Map<IEnumerable<RecipeDTO>>(recipesFromDb);
		}

		public async Task<IEnumerable<RecipeDTO>> GetRecipesAsync(PaginationInfoDTO paginationInfo)
		{
			var recipesFromDb = await _recipeRepository.GetRecipesWithPaginationAsync(paginationInfo.PageNumber, paginationInfo.PageSize);
			return _mapper.Map<IEnumerable<RecipeDTO>>(recipesFromDb);
		}

		public async Task<RecipeDetailDTO> GetRecipeByIdAsync(string id)
		{
			var recipeFromDb = await _recipeRepository.GetRecipeByIdAsync(id);
			if (recipeFromDb == null)
			{
				return null;
			}
			return _mapper.Map<RecipeDetailDTO>(recipeFromDb);
		}

		public async Task<IEnumerable<RecipeDTO>> GetRecipesByCategoryIdAsync(int categoryId)
		{
			var recipeList = await _recipeRepository.GetRecipesByCategoryIdAsync(categoryId);
			return _mapper.Map<IEnumerable<RecipeDTO>>(recipeList);
		}

		public async Task CreateRecipeAsync(RecipeDetailDTO recipeDetailDTO)
		{
			var recipe = _mapper.Map<Recipe>(recipeDetailDTO);
			var user = await _userManager.FindByIdAsync(recipeDetailDTO.User.Id);
			recipe.User = user;
			recipe.CreatedAt = DateTime.Now;
			recipe.Id = Guid.NewGuid().ToString();

			await _recipeRepository.CreateAsync(recipe);
		}

		public async Task UpdateRecipeAsync(RecipeDetailDTO recipeDetailDTO)
		{
			var recipe = _mapper.Map<Recipe>(recipeDetailDTO);
			await _recipeRepository.UpdateAsync(recipe);
		}

		public async Task DeleteRecipeAsync(string id)
		{
			var recipeFromDb = await _recipeRepository.GetAsync(r => r.Id == id);
			if (recipeFromDb != null)
			{
				await _recipeRepository.RemoveAsync(recipeFromDb);
			}
		}
	}
}
