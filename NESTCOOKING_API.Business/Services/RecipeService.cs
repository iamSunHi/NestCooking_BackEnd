using AutoMapper;
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
		private readonly IUserRepository _userRepository;

        public RecipeService(IMapper mapper,
			IRecipeRepository recipeRepository, IUserRepository userRepository)
        {
			_mapper = mapper;
            _recipeRepository = recipeRepository;
			_userRepository = userRepository;
        }

		public async Task<IEnumerable<RecipeDTO>> GetRecipesAsync(PaginationInfoDTO paginationInfo)
		{
			var recipeList = await _recipeRepository.GetRecipesWithPaginationAsync(paginationInfo.PageNumber, paginationInfo.PageSize);
			
			return null;
		}

		public async Task CreateRecipeAsync(RecipeCreationDTO recipeDTO)
		{
			var recipe = _mapper.Map<Recipe>(recipeDTO);
			var user = await _userRepository.GetAsync(u => u.Id == recipeDTO.UserId);
			recipe.User = user;
			recipe.CreatedAt = DateTime.Now;
			recipe.Id = Guid.NewGuid().ToString();

			await _recipeRepository.CreateAsync(recipe);
		}
	}
}
