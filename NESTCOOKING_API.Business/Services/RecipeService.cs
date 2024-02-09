using AutoMapper;
using Microsoft.AspNetCore.Identity;
using NESTCOOKING_API.Business.DTOs;
using NESTCOOKING_API.Business.DTOs.RecipeDTOs;
using NESTCOOKING_API.Business.DTOs.UserDTOs;
using NESTCOOKING_API.Business.Services.IServices;
using NESTCOOKING_API.DataAccess.Models;
using NESTCOOKING_API.DataAccess.Repositories.IRepositories;
using NESTCOOKING_API.Utility;

namespace NESTCOOKING_API.Business.Services
{
	public class RecipeService : IRecipeService
	{
		private readonly IMapper _mapper;
		private readonly IUserRepository _userRepository;
		private readonly IRecipeRepository _recipeRepository;
		private readonly ICategoryRecipeRepository _categoryRecipeRepository;
		private readonly IIngredientRepository _ingredientRepository;
		private readonly IInstructorRepository _instructorRepository;
		private readonly UserManager<User> _userManager;

		private readonly IIngredientTipService _ingredientTipService;

		public RecipeService(IMapper mapper,
			IUserRepository userRepository, IRecipeRepository recipeRepository, ICategoryRecipeRepository categoryRecipeRepository, IIngredientRepository ingredientRepository, IInstructorRepository instructorRepository,
			UserManager<User> userManager,
			IIngredientTipService ingredientTipService)
		{
			_mapper = mapper;
			_userRepository = userRepository;
			_recipeRepository = recipeRepository;
			_categoryRecipeRepository = categoryRecipeRepository;
			_ingredientRepository = ingredientRepository;
			_instructorRepository = instructorRepository;
			_userManager = userManager;
			_ingredientTipService = ingredientTipService;
		}

		public async Task<IEnumerable<RecipeDTO>> GetAllRecipesAsync()
		{
			var recipesFromDb = await _recipeRepository.GetAllAsync();
			var recipeList = _mapper.Map<IEnumerable<RecipeDTO>>(recipesFromDb);

			for (int i = 0; i < recipesFromDb.Count(); i++)
			{
				var user = await _userRepository.GetAsync(u => u.Id == recipesFromDb.ToList()[i].UserId);
				recipeList.ToList()[i].User = _mapper.Map<UserShortInfoDTO>(user);
			}

			return recipeList;
		}

		public async Task<IEnumerable<RecipeDTO>> GetRecipesAsync(PaginationInfoDTO paginationInfo)
		{
			var recipesFromDb = await _recipeRepository.GetRecipesWithPaginationAsync(paginationInfo.PageNumber, paginationInfo.PageSize);
			return _mapper.Map<IEnumerable<RecipeDTO>>(recipesFromDb);
		}

		public async Task<RecipeDetailDTO> GetRecipeByIdAsync(string id)
		{
			var recipeFromDb = await _recipeRepository.GetAsync(recipe => recipe.Id == id);
			if (recipeFromDb == null)
			{
				return null;
			}

			var recipe = _mapper.Map<RecipeDetailDTO>(recipeFromDb);
			recipe.User = _mapper.Map<UserShortInfoDTO>(await _userRepository.GetAsync(u => u.Id == recipeFromDb.UserId));
			var categoryList = await _categoryRecipeRepository.GetCategoriesByRecipeIdAsync(recipeFromDb.Id);
			recipe.Categories = _mapper.Map<IEnumerable<CategoryDTO>>(categoryList);
			var ingredientList = await _ingredientRepository.GetAllAsync(i => i.RecipeId == id);
			recipe.Ingredients = _mapper.Map<IEnumerable<IngredientDTO>>(ingredientList);
			for (int i = 0; i < recipe.Ingredients.Count(); i++)
			{
				if (!string.IsNullOrEmpty(ingredientList.ToList()[i].IngredientTipId))
				{
					var tip = await _ingredientTipService.GetIngredientTipShortInfoByIdAsync(ingredientList.ToList()[i].IngredientTipId);
					recipe.Ingredients.ToList()[i].IngredientTip = tip;
				}
			}
			var instructorList = await _instructorRepository.GetAllAsync(i => i.RecipeId == id);
			instructorList = instructorList.OrderBy(i => i.StepNumber);
			recipe.Instructors = _mapper.Map<IEnumerable<InstructorDTO>>(instructorList);

			return recipe;
		}

		public async Task<IEnumerable<RecipeDTO>> GetRecipesByCategoryIdAsync(int categoryId)
		{
			var recipesFromDb = await _categoryRecipeRepository.GetRecipesByCategoryIdAsync(categoryId);
			var recipeList = _mapper.Map<IEnumerable<RecipeDTO>>(recipesFromDb);

			for (int i = 0; i < recipesFromDb.Count(); i++)
			{
				var user = await _userRepository.GetAsync(u => u.Id == recipesFromDb.ToList()[i].UserId);
				recipeList.ToList()[i].User = _mapper.Map<UserShortInfoDTO>(user);
			}
			return recipeList;
		}

		public async Task CreateRecipeAsync(string userId, RecipeDetailDTO recipeDetailDTO)
		{
			var recipe = _mapper.Map<Recipe>(recipeDetailDTO);
			var user = await _userManager.FindByIdAsync(userId);
			if (user != null)
			{
				recipe.Id = Guid.NewGuid().ToString();
				recipe.UserId = userId;
				recipe.CreatedAt = DateTime.UtcNow;
				await _recipeRepository.CreateAsync(recipe);

				// Add Categories
				foreach (var category in recipeDetailDTO.Categories)
				{
					await _categoryRecipeRepository.CreateAsync(new()
					{
						RecipeId = recipe.Id, CategoryId = category.Id,
					});
				}
				// Add Ingredients
				foreach (var ingredient in recipeDetailDTO.Ingredients)
				{
					var ingredientModel = _mapper.Map<Ingredient>(ingredient);
					ingredientModel.RecipeId = recipe.Id;
					if (ingredient.IngredientTip != null)
					{
						ingredientModel.IngredientTipId = ingredient.IngredientTip.Id;
					}
					await _ingredientRepository.CreateAsync(ingredientModel);
				}
				// Add Instructors
				foreach (var instructor in recipeDetailDTO.Instructors)
				{
					var instructorModel = _mapper.Map<Instructor>(instructor);
					instructorModel.RecipeId = recipe.Id;
					await _instructorRepository.CreateAsync(instructorModel);
				}
			}
			else
			{
				throw new Exception("User not found!");
			}
		}

		public async Task UpdateRecipeAsync(string userId, RecipeDetailDTO recipeDetailDTO)
		{
			var recipeFromDb = await _recipeRepository.GetAsync(i => i.Id == recipeDetailDTO.Id);
			if (recipeFromDb != null)
			{
				if (recipeFromDb.UserId != userId && await _userRepository.GetRoleAsync(userId) != StaticDetails.Role_Admin)
				{
					throw new Exception("You don't have permission to update this.");
				}

				var recipe = _mapper.Map<Recipe>(recipeDetailDTO);
				recipe.UserId = userId;
				recipe.UpdatedAt = DateTime.UtcNow;
				await _recipeRepository.UpdateAsync(recipe);

				var relatedCategoryList = await _categoryRecipeRepository.GetAllAsync(cr => cr.RecipeId == recipeFromDb.Id);
				foreach (var categoryRecipe in relatedCategoryList)
				{
					await _categoryRecipeRepository.RemoveAsync(categoryRecipe);
				}
				foreach (var category in recipeDetailDTO.Categories)
				{
					await _categoryRecipeRepository.CreateAsync(new()
					{
						RecipeId = recipeFromDb.Id, CategoryId = category.Id
					});
				}

				var relatedIngredientList = await _ingredientRepository.GetAllAsync(i => i.RecipeId == recipeFromDb.Id);
				foreach (var ingredient in relatedIngredientList)
				{
					await _ingredientRepository.RemoveAsync(ingredient);
				}
				foreach (var ingredient in recipeDetailDTO.Ingredients)
				{
					var ingredientModel = _mapper.Map<Ingredient>(ingredient);
					ingredientModel.Id = 0;
					ingredientModel.RecipeId = recipeFromDb.Id;
					if (ingredient.IngredientTip != null)
						ingredientModel.IngredientTipId = ingredient.IngredientTip.Id;
					await _ingredientRepository.CreateAsync(ingredientModel);
				}

				var instructorsFromDb = await _instructorRepository.GetAllAsync(i => i.RecipeId == recipeFromDb.Id);
				if (recipeDetailDTO.Instructors.Count() > instructorsFromDb.Count())
				{
					for (int i = 0; i< instructorsFromDb.Count(); i++)
					{
						var instructor = _mapper.Map<Instructor>(recipeDetailDTO.Instructors.ToList()[i]);
						instructor.Id = instructorsFromDb.ToList()[i].Id;
						await _instructorRepository.UpdateAsync(instructor);
					}
					for (int i = instructorsFromDb.Count(); i < recipeDetailDTO.Instructors.Count(); i++)
					{
						var instructor = _mapper.Map<Instructor>(recipeDetailDTO.Instructors.ToList()[i]);
						instructor.Id = 0;
						instructor.RecipeId = recipeFromDb.Id;
						await _instructorRepository.CreateAsync(instructor);
					}
				}
				else
				{
					for (int i = 0; i < recipeDetailDTO.Instructors.Count(); i++)
					{
						var instructor = _mapper.Map<Instructor>(recipeDetailDTO.Instructors.ToList()[i]);
						instructor.Id = instructorsFromDb.ToList()[i].Id;
						instructor.RecipeId = recipeFromDb.Id;
						await _instructorRepository.UpdateAsync(instructor);
					}
					for (int i = recipeDetailDTO.Instructors.Count(); i < instructorsFromDb.Count(); i++)
					{
						var instructor = instructorsFromDb.ToList()[i];
						await _instructorRepository.RemoveAsync(instructor);
					}
				}
			}
		}

		public async Task DeleteRecipeAsync(string userId, string id)
		{
			var recipeFromDb = await _recipeRepository.GetAsync(r => r.Id == id);
			if (recipeFromDb != null)
			{
				var categoryRecipeList = await _categoryRecipeRepository.GetAllAsync(cr => cr.RecipeId == id);
				foreach (var categoryRecipe in categoryRecipeList)
				{
					await _categoryRecipeRepository.RemoveAsync(categoryRecipe);
				}

				var ingredientsFromDb = await _ingredientRepository.GetAllAsync(i => i.RecipeId == id);
				foreach (var ingredient in ingredientsFromDb)
				{
					await _ingredientRepository.RemoveAsync(ingredient);
				}
				var instructorsFromDb = await _instructorRepository.GetAllAsync(i => i.RecipeId == id);
				foreach (var instructor in instructorsFromDb)
				{
					await _instructorRepository.RemoveAsync(instructor);
				}

				await _recipeRepository.RemoveAsync(recipeFromDb);
			}
		}
	}
}
