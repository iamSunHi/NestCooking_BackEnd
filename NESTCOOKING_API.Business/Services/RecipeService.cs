﻿using AutoMapper;
using Microsoft.AspNetCore.Identity;
using NESTCOOKING_API.Business.DTOs;
using NESTCOOKING_API.Business.DTOs.CommentDTOs;
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
		private readonly IFavoriteRecipeRepository _favoriteRecipeRepository;
		private readonly ICategoryRepository _categoryRepository;
		private readonly UserManager<User> _userManager;
		private readonly IIngredientTipService _ingredientTipService;
		private readonly IReactionRepository _reactionRepository;
		private readonly ICommentRepository _commentRepository;
		private readonly ITransactionRepository _transactionRepository;
		private readonly IPurchasedRecipesRepository _purchasedRecipesRepositoryService;

		public RecipeService(IMapper mapper,
			IUserRepository userRepository,
			IRecipeRepository recipeRepository,
			ICategoryRecipeRepository categoryRecipeRepository,
			IIngredientRepository ingredientRepository,
			IInstructorRepository instructorRepository,
		 	IFavoriteRecipeRepository favoriteRecipeRepository,
			UserManager<User> userManager,
			IIngredientTipService ingredientTipService,
			ICategoryRepository categoryRepository,
			IReactionRepository reactionRepository,
			ICommentRepository commentRepository,
			ITransactionRepository transactionRepository,
			IPurchasedRecipesRepository purchasedRecipesRepositoryService)
		{
			_mapper = mapper;
			_userRepository = userRepository;
			_recipeRepository = recipeRepository;
			_categoryRecipeRepository = categoryRecipeRepository;
			_ingredientRepository = ingredientRepository;
			_instructorRepository = instructorRepository;
			_favoriteRecipeRepository = favoriteRecipeRepository;
			_userManager = userManager;
			_ingredientTipService = ingredientTipService;
			_categoryRepository = categoryRepository;
			_reactionRepository = reactionRepository;
			_commentRepository = commentRepository;
			_transactionRepository = transactionRepository;
			_purchasedRecipesRepositoryService = purchasedRecipesRepositoryService;
		}

		public async Task<IEnumerable<RecipeDTO>> GetAllRecipesAsync()
		{
			var recipesFromDb = await _recipeRepository.GetAllAsync();
			return await ConvertRecipesToListRecipeDTO(recipesFromDb);
		}

		public async Task<(int, int, IEnumerable<RecipeDTO>)> GetRecipesAsync(PaginationInfoDTO paginationInfo)
		{
			var totalItems = (await _recipeRepository.GetAllAsync()).Count();
			var totalPages = (int)Math.Ceiling((double)totalItems / paginationInfo.PageSize);
			var recipesFromDb = await _recipeRepository.GetRecipesWithPaginationAsync(paginationInfo.PageNumber, paginationInfo.PageSize);
			if (recipesFromDb == null)
			{
				return (totalItems, totalPages, null);
			}

			var mappedListRecipe = await ConvertRecipesToListRecipeDTO(recipesFromDb);
			return (totalItems, totalPages, mappedListRecipe);
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
			
			if (recipe.IsPrivate)
			{
				var purchaseData = await _purchasedRecipesRepositoryService.GetAsync(p => p.RecipeId == id && p.UserId == recipeFromDb.UserId);

				if (purchaseData == null)
				{
					return recipe;
				}

				var transactionData = await _transactionRepository.GetAsync(t => t.Id == purchaseData.TransactionId);

				if (!transactionData.IsSuccess)
				{
					return recipe;
				}
			}


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
			instructorList = instructorList.OrderBy(i => i.InstructorOrder);
			recipe.Instructors = _mapper.Map<IEnumerable<InstructorDTO>>(instructorList);

			var reactionList = await _reactionRepository.GetReactionsByIdAsync(id, "recipe");
			recipe.Reactions = reactionList;
			var commentList = await _commentRepository.GetAllCommentsOfRecipeByRecipeId(id);
			recipe.Comments = _mapper.Map<IEnumerable<RequestCommentDTO>>(commentList);

			return recipe;
		}

		public async Task<IEnumerable<RecipeDTO>> GetRecipesByCategoryIdAsync(int categoryId)
		{
			var recipesFromDb = await _categoryRecipeRepository.GetRecipesByCategoryIdAsync(categoryId);
			return await ConvertRecipesToListRecipeDTO(recipesFromDb);

		}

		public async Task<IEnumerable<RecipeDTO>> GetRecipesByUserIdAsync(string userId)
		{
			var recipesFromDb = await _recipeRepository.GetAllAsync(r => r.UserId == userId);
			return await ConvertRecipesToListRecipeDTO(recipesFromDb);

		}

		public async Task<RecipeDetailDTO> CreateRecipeAsync(string userId, CreateRecipeDTO createRecipeDTO)
		{
			var recipe = _mapper.Map<Recipe>(createRecipeDTO);
			var user = await _userManager.FindByIdAsync(userId);
			if (user == null)
			{
				throw new Exception("User not found!");
			}

			var newRecipeId = Guid.NewGuid().ToString();
			recipe.Id = newRecipeId;
			recipe.UserId = userId;
			recipe.CreatedAt = DateTime.UtcNow;
			recipe.UpdatedAt = DateTime.UtcNow;
			await _recipeRepository.CreateAsync(recipe);

			await AddCategories(recipe.Id, createRecipeDTO.Categories);
			await AddIngredients(recipe.Id, createRecipeDTO.Ingredients);
			await AddOrUpdateInstructors(recipe.Id, createRecipeDTO.Instructors);

			var newRecipe = await this.GetRecipeByIdAsync(newRecipeId);
			return newRecipe;
		}

		public async Task<RecipeDetailDTO> UpdateRecipeAsync(string userId, string recipeId, UpdateRecipeDTO updateRecipeDTO)
		{
			var recipeFromDb = await _recipeRepository.GetAsync(i => i.Id == recipeId);
			if (recipeFromDb == null)
			{
				throw new Exception(AppString.RecipeNotFoundErrorMessage);
			}
			if (recipeFromDb.UserId != userId)
			{
				throw new Exception("You don't have permission to update this.");
			}

			var recipe = _mapper.Map<Recipe>(updateRecipeDTO);
			recipe.Id = recipeId;
			recipe.UserId = userId;
			recipe.UpdatedAt = DateTime.UtcNow;
			await _recipeRepository.UpdateAsync(recipe);

			await UpdateCategories(recipeFromDb.Id, updateRecipeDTO.Categories);
			await UpdateIngredients(recipeFromDb.Id, updateRecipeDTO.Ingredients);
			await AddOrUpdateInstructors(recipeFromDb.Id, updateRecipeDTO.Instructors.ToList());

			return await this.GetRecipeByIdAsync(recipeId);
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
				var reactionList = await _reactionRepository.GetReactionsByIdAsync(id, "recipe");
				foreach (var reaction in reactionList)
				{
					await _reactionRepository.DeleteAsync(id, "recipe");
				}
				var commentList = await _commentRepository.GetAllCommentsOfRecipeByRecipeId(id);
				foreach (var comment in commentList)
				{
					await _reactionRepository.DeleteAsync(comment.CommentId, "comment");
					await _commentRepository.RemoveAsync(comment);
				}
				var favoriteRecipeList = await _favoriteRecipeRepository.GetAllAsync(fr => fr.RecipeId == id);
				foreach (var favoriteRecipe in favoriteRecipeList)
				{
					await _favoriteRecipeRepository.RemoveAsync(favoriteRecipe);
				}

				await _recipeRepository.RemoveAsync(recipeFromDb);
			}
			else
			{
				throw new Exception(AppString.RecipeNotFoundErrorMessage);
			}
		}

		public async Task<IEnumerable<RecipeDTO>> GetAllFavoriteRecipeAsync(string userId)
		{
			var favoriteRecipesFromDb = await _favoriteRecipeRepository.GetAllAsync(fr => fr.UserId == userId, includeProperties: "Recipe");
			var recipeList = new List<RecipeDTO>();
			foreach (var favoriteRecipe in favoriteRecipesFromDb)
			{
				var recipe = _mapper.Map<RecipeDTO>(favoriteRecipe.Recipe);
				var userFromDb = await _userRepository.GetAsync(u => u.Id == favoriteRecipe.Recipe.UserId);
				recipe.User = _mapper.Map<UserShortInfoDTO>(userFromDb);
				recipeList.Add(recipe);
			}
			return recipeList;
		}

		public async Task SaveFavoriteRecipeAsync(string userId, string recipeId)
		{
			if (await CheckIfRecipeIsFavorite(userId, recipeId))
				throw new Exception(message: "This recipe is already in your favorite list.");

			FavoriteRecipe favoriteRecipe = new()
			{
				UserId = userId,
				RecipeId = recipeId
			};
			await _favoriteRecipeRepository.CreateAsync(favoriteRecipe);
		}

		public async Task RemoveFavoriteRecipeAsync(string userId, string recipeId)
		{
			if (!(await CheckIfRecipeIsFavorite(userId, recipeId)))
				throw new Exception(message: "This recipe is not exist in your favorite list.");

			var favoriteRecipeFromDb = await _favoriteRecipeRepository.GetAsync(fr => fr.UserId == userId && fr.RecipeId == recipeId);
			await _favoriteRecipeRepository.RemoveAsync(favoriteRecipeFromDb);
		}

		private async Task<bool> CheckIfRecipeIsFavorite(string userId, string recipeId)
		{
			var favoriteRecipeFromDb = await _favoriteRecipeRepository.GetAsync(fr => fr.UserId == userId && fr.RecipeId == recipeId);
			if (favoriteRecipeFromDb == null)
			{
				return false;
			}
			return true;
		}

		private async Task<IEnumerable<RecipeDTO>> ConvertRecipesToListRecipeDTO(IEnumerable<Recipe> recipesFromDb)
		{
			var recipeList = new List<RecipeDTO>();

			foreach (var recipe in recipesFromDb)
			{
				var mappedResult = _mapper.Map<RecipeDTO>(recipe);
				mappedResult.CreatedAt = recipe.CreatedAt;
				mappedResult.UpdatedAt = recipe.UpdatedAt;
				var user = await _userRepository.GetAsync(u => u.Id == recipe.UserId);
				mappedResult.User = _mapper.Map<UserShortInfoDTO>(user);
				recipeList.Add(mappedResult);
			}

			return recipeList;
		}
		private async Task AddCategories(string recipeId, IEnumerable<int> categories)
		{
			foreach (var category in categories)
			{
				var existedCategory = await this._categoryRepository.GetAsync(r => r.Id == category);
				if (existedCategory == null)
				{
					throw new Exception(AppString.CategoryNotFoundErrorMessage);
				}
				await _categoryRecipeRepository.CreateAsync(new CategoryRecipe
				{
					RecipeId = recipeId,
					CategoryId = category,
				});
			}
		}

		private async Task UpdateCategories(string recipeId, IEnumerable<int> categories)
		{
			var relatedCategoryList = await _categoryRecipeRepository.GetAllAsync(cr => cr.RecipeId == recipeId);
			foreach (var categoryRecipe in relatedCategoryList)
			{
				await _categoryRecipeRepository.RemoveAsync(categoryRecipe);
			}
			await AddCategories(recipeId, categories);
		}

		private async Task AddIngredients(string recipeId, IEnumerable<CreateIngredientDTO> ingredients)
		{
			foreach (var ingredient in ingredients)
			{
				var ingredientModel = _mapper.Map<Ingredient>(ingredient);
				ingredientModel.RecipeId = recipeId;
				if (ingredient.IngredientTipId != null)
				{
					var existedIngredientTip = await this._ingredientTipService.GetIngredientTipByIdAsync(ingredient.IngredientTipId);
					if (existedIngredientTip == null)
					{
						throw new Exception(AppString.IngredientTipNotFoundErrorMessage);
					}
					ingredientModel.IngredientTipId = ingredient.IngredientTipId;
				}
				await _ingredientRepository.CreateAsync(ingredientModel);
			}
		}

		private async Task UpdateIngredients(string recipeId, IEnumerable<CreateIngredientDTO> ingredients)
		{
			var relatedIngredientList = await _ingredientRepository.GetAllAsync(i => i.RecipeId == recipeId);
			foreach (var ingredient in relatedIngredientList)
			{
				await _ingredientRepository.RemoveAsync(ingredient);
			}
			await AddIngredients(recipeId, ingredients);
		}

		private async Task AddOrUpdateInstructors(string recipeId, IEnumerable<CreateInstructorDTO> instructors)
		{
			var orderedOrders = instructors.Select(i => i.InstructorOrder).OrderBy(order => order).ToList();

			var duplicateOrder = orderedOrders.GroupBy(order => order).FirstOrDefault(g => g.Count() > 1)?.Key;
			if (duplicateOrder.HasValue)
			{
				throw new Exception($"Duplicate Order value found: {duplicateOrder}");
			}

			var missingOrder = Enumerable.Range(0, orderedOrders.Count).Except(orderedOrders).FirstOrDefault();
			if (missingOrder != default)
			{
				throw new Exception($"Missing Order value: {missingOrder}");
			}

			var sortedInstructors = instructors.OrderBy(i => i.InstructorOrder).ToList();
			var instructorsFromDb = await _instructorRepository.GetAllAsync(i => i.RecipeId == recipeId);
			for (int i = 0; i < sortedInstructors.Count; i++)
			{
				var instructor = _mapper.Map<Instructor>(sortedInstructors[i]);
				instructor.RecipeId = recipeId;
				if (i < instructorsFromDb.Count())
				{
					instructor.Id = instructorsFromDb.ToList()[i].Id;
					await _instructorRepository.UpdateAsync(instructor);
				}
				else
				{
					await _instructorRepository.CreateAsync(instructor);
				}
			}
			for (int i = sortedInstructors.Count; i < instructorsFromDb.Count(); i++)
			{
				var instructor = instructorsFromDb.ToList()[i];
				await _instructorRepository.RemoveAsync(instructor);
			}
		}

		public async Task<IEnumerable<RecipeForBookingDTO>> GetRecipesForBookingByChefIdAsync(string chefId)
		{
			if (await this.ChefIfUserIsChef(chefId))
			{
				var recipeListFromDb = await _recipeRepository.GetAllAsync(r => r.IsAvailableForBooking && r.UserId == chefId);
				return _mapper.Map<IEnumerable<RecipeForBookingDTO>>(recipeListFromDb);
			}
			else
			{
				throw new Exception("This user is not a chef. Please try again.");
			}
		}

		private async Task<bool> ChefIfUserIsChef(string userId)
		{
			var userRole = await _userRepository.GetRoleAsync(userId);
			return userRole == StaticDetails.Role_Chef;
		}

		public async Task<RecipeForBookingDTO> UpdateRecipeForBookingAsync(string userId, RecipeForBookingDTO recipeForBookingDTO)
		{
			var recipeFromDb = await _recipeRepository.GetAsync(i => i.Id == recipeForBookingDTO.Id);
			if (recipeFromDb == null)
			{
				throw new Exception(AppString.RecipeNotFoundErrorMessage);
			}
			if (recipeFromDb.UserId != userId)
			{
				throw new Exception("You don't have permission to update this.");
			}

			var recipe = _mapper.Map<Recipe>(recipeForBookingDTO);
			await _recipeRepository.UpdateRecipeForBookingAsync(recipe);

			recipeFromDb = await _recipeRepository.GetAsync(i => i.Id == recipeForBookingDTO.Id);
			return _mapper.Map<RecipeForBookingDTO>(recipeFromDb);
		}
	}
}
