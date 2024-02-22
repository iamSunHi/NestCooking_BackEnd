using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using NESTCOOKING_API.Business.DTOs;
using NESTCOOKING_API.Business.DTOs.RecipeDTOs;
using NESTCOOKING_API.Business.DTOs.UserDTOs;
using NESTCOOKING_API.Business.Services.IServices;
using NESTCOOKING_API.DataAccess.Models;
using NESTCOOKING_API.DataAccess.Repositories.IRepositories;

namespace NESTCOOKING_API.Business.Services
{
	public class SearchService : ISearchService
	{
		private readonly IUserRepository _userRepository;
		private readonly IRecipeRepository _recipeRepository;
		private readonly IMapper _mapper;

		public SearchService(IUserRepository userRepository, IRecipeRepository recipeRepository, IMapper mapper)
		{
			_userRepository = userRepository;
			_recipeRepository = recipeRepository;
			_mapper = mapper;
		}

		public async Task<SearchResultDTO> GetAllAsync(string criteria, string? userId = null)
		{
			var userList = await _userRepository.GetUsersByCriteriaAsync(criteria);

			var recipesFromDb = await _recipeRepository.GetRecipesByCriteriaAsync(criteria);
			var recipeList = _mapper.Map<IEnumerable<RecipeDTO>>(recipesFromDb.Take(5));
			for (int i = 0; i < recipeList.Count(); i++)
			{
				var user = await _userRepository.GetAsync(u => u.Id == recipesFromDb.ToList()[i].UserId);
				recipeList.ToList()[i].User = _mapper.Map<UserShortInfoDTO>(user);
			}

			SearchResultDTO result = new SearchResultDTO()
			{
				Users = _mapper.Map<IEnumerable<UserShortInfoDTO>>(userList.Take(5)),
				Recipes = _mapper.Map<IEnumerable<RecipeDTO>>(recipeList)
			};
			return result;
		}

		public async Task<SearchResultDTO> GetRecipesAsync(string criteria, string? userId = null)
		{
			IEnumerable<Recipe> recipesFromDb;
			if (userId == null)
			{
				recipesFromDb = await _recipeRepository.GetRecipesByCriteriaAsync(criteria);
			}
			else
			{
				recipesFromDb = await _recipeRepository.GetRecipesByCriteriaAsync(criteria, userId);
			}

			var recipeList = _mapper.Map<IEnumerable<RecipeDTO>>(recipesFromDb);
			for (int i = 0; i < recipeList.Count(); i++)
			{
				var user = await _userRepository.GetAsync(u => u.Id == recipesFromDb.ToList()[i].UserId);
				recipeList.ToList()[i].User = _mapper.Map<UserShortInfoDTO>(user);
			}

			SearchResultDTO result = new SearchResultDTO()
			{
				Recipes = recipeList
			};
			return result;
		}

		public async Task<SearchResultDTO> GetUsersAsync(string criteria, string? userId = null)
		{
			var userList = await _userRepository.GetUsersByCriteriaAsync(criteria);

			SearchResultDTO result = new SearchResultDTO()
			{
				Users = _mapper.Map<IEnumerable<UserShortInfoDTO>>(userList)
			};
			return result;
		}
	}
}
