using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NESTCOOKING_API.Business.Authentication;
using NESTCOOKING_API.Business.DTOs;
using NESTCOOKING_API.Business.DTOs.RecipeDTOs;
using NESTCOOKING_API.Business.Services.IServices;
using NESTCOOKING_API.Utility;
using System.Net;

namespace NESTCOOKING_API.Presentation.Controllers
{
	[Route("api/recipes")]
	[ApiController]
	public class RecipeController : ControllerBase
	{
		private PaginationInfoDTO _paginationInfo = new PaginationInfoDTO();
		private readonly IRecipeService _recipeService;

		public RecipeController(IRecipeService recipeService)
		{
			_recipeService = recipeService;
		}

		[HttpGet]
		[ResponseCache(Duration = 30)]
		public async Task<IActionResult> GetAllRecipesAsync()
		{
			var recipes = await _recipeService.GetAllRecipesAsync();
			return Ok(ResponseDTO.Accept(result: recipes));
		}

		[HttpGet("{id}")]
		[ResponseCache(Duration = 30)]
		public async Task<IActionResult> GetRecipeByIdAsync([FromRoute] string id)
		{
			try
			{
				var userId = AuthenticationHelper.GetUserIdFromContext(HttpContext);
				var recipe = await _recipeService.GetRecipeByIdAsync(id, userId);
				if (recipe == null)
				{
					return BadRequest(ResponseDTO.BadRequest(message: $"Recipe with id {id} not found."));
				}
				return Ok(ResponseDTO.Accept(result: recipe));
			}
			catch (Exception ex)
			{
				return BadRequest(ResponseDTO.BadRequest(message: ex.Message));
			}
		}

		[HttpGet("page")]
		public async Task<IActionResult> GetRecipesAsync([FromQuery] int pageNumber, [FromQuery] int pageSize)
		{
			if (pageNumber != 0)
			{
				_paginationInfo.PageNumber = pageNumber;
			}
			if (pageSize != 0)
			{
				_paginationInfo.PageSize = pageSize;
			}
			else if (pageSize > 100)
			{
				_paginationInfo.PageSize = 100;
			}
			(int totalItems, int totalPages, IEnumerable<RecipeDTO> recipeList) result = await _recipeService.GetRecipesAsync(_paginationInfo);

			if (result.recipeList == null)
			{
				return BadRequest(ResponseDTO.BadRequest(message: "Page number is not valid!"));
			}
			return Ok(ResponseDTO.Accept(result: new
			{
				metadata = new
				{
					result.totalItems,
					result.totalPages,
					_paginationInfo.PageNumber,
					_paginationInfo.PageSize
				},
				recipes = result.recipeList
			}));
		}

		[HttpGet("categories/{categoryId}")]
		public async Task<IActionResult> GetRecipesByCategoryIdAsync([FromRoute] int categoryId)
		{
			var recipe = await _recipeService.GetRecipesByCategoryIdAsync(categoryId);
			if (recipe == null)
			{
				return BadRequest(ResponseDTO.BadRequest(message: $"Recipe with categoryId {categoryId} not found."));
			}
			return Ok(ResponseDTO.Accept(result: recipe));
		}

		[HttpGet("user/{userId}")]
		public async Task<IActionResult> GetRecipesByUserIdAsync([FromRoute] string userId)
		{
			if (userId == null)
			{
				return BadRequest(ResponseDTO.BadRequest(message: $"User id is required."));
			}
			var recipe = await _recipeService.GetRecipesByUserIdAsync(userId);
			if (recipe == null)
			{
				return BadRequest(ResponseDTO.BadRequest(message: $"You don't have any recipe."));
			}
			return Ok(ResponseDTO.Accept(result: recipe));
		}

		[HttpPost]
		[Authorize]
		public async Task<IActionResult> CreateRecipeAsync([FromBody] CreateRecipeDTO createRecipeDTO)
		{
			try
			{
				var userId = AuthenticationHelper.GetUserIdFromContext(HttpContext);
				if (userId == null)
				{
					return BadRequest(ResponseDTO.BadRequest(message: AppString.UserNotFoundMessage));
				}
				var result = await _recipeService.CreateRecipeAsync(userId, createRecipeDTO);
				return Ok(ResponseDTO.Create(HttpStatusCode.Created, result: result));
			}
			catch (Exception ex)
			{
				return BadRequest(ResponseDTO.BadRequest(message: ex.Message));
			}
		}

		[HttpPatch("{recipeId}")]
		[Authorize]
		public async Task<IActionResult> UpdateRecipeAsync([FromRoute] string recipeId, [FromBody] UpdateRecipeDTO updateRecipeDTO)
		{
			try
			{
				var userId = AuthenticationHelper.GetUserIdFromContext(HttpContext);
				var result = await _recipeService.UpdateRecipeAsync(userId, recipeId, updateRecipeDTO);
				if (result == null)
				{
					return StatusCode((int)HttpStatusCode.InternalServerError, ResponseDTO.Create(statusCode: HttpStatusCode.InternalServerError, message: "Couldn't update recipe."));
				}
				return Ok(ResponseDTO.Accept(result: result));
			}
			catch (Exception ex)
			{
				return BadRequest(ResponseDTO.BadRequest(message: ex.Message));
			}
		}

		[HttpDelete("delete/{id}")]
		[Authorize]
		public async Task<IActionResult> DeleteRecipeAsync([FromRoute] string id)
		{
			try
			{
				var userId = AuthenticationHelper.GetUserIdFromContext(HttpContext);
				await _recipeService.DeleteRecipeAsync(userId, id);
				return NoContent();
			}
			catch (Exception ex)
			{
				return BadRequest(ResponseDTO.BadRequest(message: ex.Message));
			}
		}

		[HttpGet("favorites")]
		[Authorize]
		public async Task<IActionResult> GetAllFavoriteRecipeAsync()
		{
			try
			{
				var userId = AuthenticationHelper.GetUserIdFromContext(HttpContext);
				var recipes = await _recipeService.GetAllFavoriteRecipeAsync(userId);
				return Ok(ResponseDTO.Accept(result: recipes));
			}
			catch (Exception ex)
			{
				return BadRequest(ResponseDTO.BadRequest(message: ex.Message));
			}
		}

		[HttpGet("favorites/user/{userId}")]
		[Authorize]
		public async Task<IActionResult> GetAllFavoriteRecipeOfOtherAsync([FromRoute] string userId)
		{
			try
			{
				var recipes = await _recipeService.GetAllFavoriteRecipeAsync(userId);
				return Ok(ResponseDTO.Accept(result: recipes));
			}
			catch (Exception ex)
			{
				return BadRequest(ResponseDTO.BadRequest(message: ex.Message));
			}
		}

		[HttpPost("favorites/save/{recipeId}")]
		[Authorize]
		public async Task<IActionResult> SaveFavoriteRecipeAsync([FromRoute] string recipeId)
		{
			try
			{
				var userId = AuthenticationHelper.GetUserIdFromContext(HttpContext);
				await _recipeService.SaveFavoriteRecipeAsync(userId, recipeId);
				return Ok(ResponseDTO.Create(HttpStatusCode.Created, message: "Save favorite recipe successfully"));
			}
			catch (Exception ex)
			{
				return BadRequest(ResponseDTO.BadRequest(message: ex.Message));
			}
		}

		[HttpDelete("favorites/remove/{recipeId}")]
		[Authorize]
		public async Task<IActionResult> RemoveFavoriteRecipeAsync([FromRoute] string recipeId)
		{
			try
			{
				var userId = AuthenticationHelper.GetUserIdFromContext(HttpContext);
				await _recipeService.RemoveFavoriteRecipeAsync(userId, recipeId);
				return Ok(ResponseDTO.Create(HttpStatusCode.NoContent, message: "Remove successfully"));
			}
			catch (Exception ex)
			{
				return BadRequest(ResponseDTO.BadRequest(message: ex.Message));
			}
		}

		[HttpGet("booking")]
		public async Task<IActionResult> GetRecipesForBookingByChefIdAsync([FromQuery] string chefId)
		{
			try
			{
				var recipeList = await _recipeService.GetAllRecipesForBookingByChefIdAsync(chefId);
				return Ok(ResponseDTO.Accept(result: recipeList));
			}
			catch (Exception ex)
			{
				return BadRequest(ResponseDTO.BadRequest(ex.Message));
			}
		}

		[HttpPatch("booking/update")]
		[Authorize(Roles = StaticDetails.Role_Chef)]
		public async Task<IActionResult> UpdateRecipeForBookingAsync([FromBody] RecipeForBookingDTO recipeForBookingDTO)
		{
			try
			{
				var userId = AuthenticationHelper.GetUserIdFromContext(HttpContext);
				var result = await _recipeService.UpdateRecipeForBookingAsync(userId, recipeForBookingDTO);
				if (result == null)
				{
					return StatusCode((int)HttpStatusCode.InternalServerError, ResponseDTO.Create(statusCode: HttpStatusCode.InternalServerError, message: "Couldn't update recipe."));
				}
				return Ok(ResponseDTO.Accept(result: result));
			}
			catch (Exception ex)
			{
				return BadRequest(ResponseDTO.BadRequest(message: ex.Message));
			}
		}
	}
}
