﻿using AutoMapper;
using Microsoft.AspNetCore.Identity;
using NESTCOOKING_API.Business.DTOs;
using NESTCOOKING_API.Business.DTOs.AdminDTOs;
using NESTCOOKING_API.Business.DTOs.CommentDTOs;
using NESTCOOKING_API.Business.DTOs.NotificationDTOs;
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
		private readonly IRoleRepository _roleRepository;
		private readonly IBookingLineRepository _bookingLineRepository;
		private readonly IBookingRepository _bookingRepository;
		private readonly INotificationService _notificationService;

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
			IPurchasedRecipesRepository purchasedRecipesRepositoryService,
			IRoleRepository roleRepository,
			IBookingLineRepository bookingLineRepository,
			IBookingRepository bookingRepository,
			INotificationService notificationService)
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
			_roleRepository = roleRepository;
			_bookingLineRepository = bookingLineRepository;
			_bookingRepository = bookingRepository;
			_notificationService = notificationService;
		}

		public async Task<IEnumerable<RecipeDTO>> GetAllVerifiedRecipesAsync()
		{
			var recipesFromDb = await _recipeRepository.GetAllAsync(r => r.IsVerified && r.IsPublic);
			recipesFromDb = recipesFromDb.OrderByDescending(r => r.CreatedAt);
			return await ConvertRecipesToListRecipeDTO(recipesFromDb);
		}

		public async Task<IEnumerable<RecipeDTO>> GetAllRecipesAsync()
		{
			var recipesFromDb = await _recipeRepository.GetAllAsync();
			recipesFromDb = recipesFromDb.OrderByDescending(r => r.CreatedAt);
			return await ConvertRecipesToListRecipeDTO(recipesFromDb);
		}

		public async Task<(int, int, IEnumerable<RecipeDTO>)> GetRecipesAsync(PaginationInfoDTO paginationInfo)
		{
			var totalItems = (await _recipeRepository.GetAllAsync(r => r.IsVerified && r.IsPublic)).Count();
			var totalPages = (int)Math.Ceiling((double)totalItems / paginationInfo.PageSize);
			var recipesFromDb = await _recipeRepository.GetRecipesWithPaginationAsync(paginationInfo.PageNumber, paginationInfo.PageSize);
			if (recipesFromDb == null)
			{
				return (totalItems, totalPages, null);
			}

			var mappedListRecipe = await ConvertRecipesToListRecipeDTO(recipesFromDb);
			return (totalItems, totalPages, mappedListRecipe);
		}

		public async Task<RecipeDetailDTO> GetRecipeByIdAsync(string id, string? userId = null)
		{
			var recipeFromDb = await _recipeRepository.GetAsync(recipe => recipe.Id == id);
			if (recipeFromDb == null)
			{
				return null;
			}
			if (!recipeFromDb.IsVerified)
			{
				if (userId == null)
				{
					throw new Exception("This recipe is not verified! You don't have permission to view the details of this recipe!");
				}
				var roleOfUser = await _userRepository.GetRoleAsync(userId);
				if (roleOfUser != StaticDetails.Role_Admin && recipeFromDb.UserId != userId)
					throw new Exception("This recipe is not verified! You don't have permission to view the details of this recipe!");
			}

			var recipe = _mapper.Map<RecipeDetailDTO>(recipeFromDb);
			recipe.User = _mapper.Map<UserShortInfoDTO>(await _userRepository.GetAsync(u => u.Id == recipeFromDb.UserId));

			var categoryList = await _categoryRecipeRepository.GetCategoriesByRecipeIdAsync(recipeFromDb.Id);
			recipe.Categories = _mapper.Map<IEnumerable<CategoryDTO>>(categoryList);

			if (recipe.IsPrivate)
			{
				if (userId == null)
				{
					return recipe;
				}
				else
				{
					var roleAdminId = (await _roleRepository.GetAsync(r => r.Name == StaticDetails.Role_Admin)).Id;
					var userFromDb = await _userRepository.GetAsync(u => u.Id == userId);
					if (recipeFromDb.UserId != userId && userFromDb.RoleId != roleAdminId)
					{
						var purchaseData = await _purchasedRecipesRepositoryService.GetAsync(p => p.RecipeId == id && p.UserId == userId);
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
			recipesFromDb = recipesFromDb.Where(r => r.IsVerified && r.IsPublic).ToList();
			return await ConvertRecipesToListRecipeDTO(recipesFromDb);

		}

		public async Task<IEnumerable<RecipeDTO>> GetRecipesByUserIdAsync(string userId, string? currentUserId)
		{
			var recipesFromDb = (await _recipeRepository.GetAllAsync(r => r.UserId == userId)).ToList();
			if (currentUserId != null && currentUserId != userId)
			{
				recipesFromDb = recipesFromDb.Where(r => r.IsVerified && r.IsPublic).ToList();
			}
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
			recipe.CreatedAt = DateTime.UtcNow.AddHours(7);
			recipe.UpdatedAt = DateTime.UtcNow.AddHours(7);
			await _recipeRepository.CreateAsync(recipe);

			await AddCategories(recipe.Id, createRecipeDTO.Categories);
			await AddIngredients(recipe.Id, createRecipeDTO.Ingredients);
			await AddOrUpdateInstructors(recipe.Id, createRecipeDTO.Instructors);

			var newRecipe = await this.GetRecipeByIdAsync(newRecipeId, userId);
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
			recipe.UpdatedAt = DateTime.UtcNow.AddHours(7);
			if (!recipe.IsPublic)
			{
				await _recipeRepository.VerifyAsync(recipeId, false, false);
			}
			await _recipeRepository.UpdateAsync(recipe);

			await UpdateCategories(recipeFromDb.Id, updateRecipeDTO.Categories);
			await UpdateIngredients(recipeFromDb.Id, updateRecipeDTO.Ingredients);
			await AddOrUpdateInstructors(recipeFromDb.Id, updateRecipeDTO.Instructors.ToList());

			return await this.GetRecipeByIdAsync(recipeId, userId);
		}
		public async Task DeleteRecipeAsync(string userId, string id)
		{
			var recipeFromDb = await _recipeRepository.GetAsync(r => r.Id == id);

			if (recipeFromDb != null)
			{
				if ((await _userRepository.GetRoleAsync(userId)) != StaticDetails.Role_Admin &&
					userId != recipeFromDb.UserId
				)
				{
					throw new Exception("You don't have permission to remove this recipe!");
				}
				if (recipeFromDb.IsPrivate)
				{
					var purchasedRecipeList = await _purchasedRecipesRepositoryService.GetAllAsync(pr => pr.RecipeId == id);
					if (purchasedRecipeList.Any())
					{
						throw new Exception("You cannot remove this recipe because it has already been purchased.");
					}
				}
				if (recipeFromDb.IsAvailableForBooking)
				{
					var bookingLineList = await _bookingLineRepository.GetAllAsync(bl => bl.RecipeId == id);
					if (bookingLineList.Any())
					{
						foreach (var bookingLine in bookingLineList)
						{
							var booking = await _bookingRepository.GetAsync(b => b.Id == bookingLine.BookingId);
							if (booking.Status == StaticDetails.ActionStatus_PENDING)
							{
								throw new Exception("You cannot remove this recipe because it has already been booked.");
							}
						}
						foreach (var bookingLine in bookingLineList)
						{
							await _bookingLineRepository.RemoveAsync(bookingLine);
						}
					}
				}

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

		public async Task<List<RecipeForBookingDTO>> GetAllRecipesForBookingByChefIdAsync(string chefId)
		{
			var roleChefId = await _roleRepository.GetRoleIdByNameAsync(StaticDetails.Role_Chef);
			var isChefExist = (await _userRepository.GetAsync(u => u.Id == chefId && u.RoleId == roleChefId)) != null;
			if (!isChefExist)
			{
				throw new Exception("This user does not exist or is not a chef. Please try again.");
			}

			var recipeFromDb = await _recipeRepository.GetAllAsync(r => r.UserId == chefId && r.IsAvailableForBooking);
			return _mapper.Map<List<RecipeForBookingDTO>>(recipeFromDb);
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

		public async Task VerifyRecipeAsync(AdminVerifyRecipeDTO adminVerifyRecipeDTO)
		{
			switch (adminVerifyRecipeDTO.Status)
			{
				case StaticDetails.ActionStatus_ACCEPTED:
					{
						var recipeFromDb = await _recipeRepository.GetAsync(r => r.Id == adminVerifyRecipeDTO.RecipeId);
						await _recipeRepository.VerifyAsync(adminVerifyRecipeDTO.RecipeId, true, true);
						NotificationCreateDTO notificationCreateDTO = new()
						{
							ReceiverId = recipeFromDb.UserId,
							NotificationType = StaticDetails.NotificationType_ANNOUNCEMENT,
							TargetType = StaticDetails.TargetType_RECIPE,
							Content = AppString.NotificationAcceptedRecipe
						};
						await _notificationService.CreateNotificationAsync(notificationCreateDTO);
						break;
					}
				case StaticDetails.ActionStatus_REJECTED:
					{
						var recipeFromDb = await _recipeRepository.GetAsync(r => r.Id == adminVerifyRecipeDTO.RecipeId);
						await _recipeRepository.VerifyAsync(adminVerifyRecipeDTO.RecipeId, true, false);
						NotificationCreateDTO notificationCreateDTO = new()
						{
							ReceiverId = recipeFromDb.UserId,
							NotificationType = StaticDetails.NotificationType_ANNOUNCEMENT,
							TargetType = StaticDetails.TargetType_RECIPE,
							Content = AppString.NotificationRejectedRecipe
						};
						await _notificationService.CreateNotificationAsync(notificationCreateDTO);
						break;
					}
				default:
					throw new Exception(AppString.InvalidStatusType);
			}
		}
	}
}
