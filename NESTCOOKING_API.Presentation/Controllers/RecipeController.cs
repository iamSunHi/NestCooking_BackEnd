using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NESTCOOKING_API.Business.DTOs;
using NESTCOOKING_API.Business.DTOs.RecipeDTOs;
using NESTCOOKING_API.Business.Services.IServices;
using System.Security.Claims;

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
		public async Task<IActionResult> GetAllRecipesAsync()
		{
			var recipes = await _recipeService.GetAllRecipesAsync();
			return Ok(ResponseDTO.Accept(result: recipes));
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetRecipeByIdAsync([FromRoute] string id)
		{
			var recipe = await _recipeService.GetRecipeByIdAsync(id);
			if (recipe == null)
			{
				return BadRequest(ResponseDTO.BadRequest(message: $"Recipe with id {id} not found."));
			}
			return Ok(ResponseDTO.Accept(result: recipe));
		}

		[HttpGet("page/{pageNumber}")]
		public async Task<IActionResult> GetRecipesAsync([FromRoute] int pageNumber)
		{
			if (pageNumber != 0)
			{
				_paginationInfo.PageNumber = pageNumber;
			}
			var recipes = await _recipeService.GetRecipesAsync(_paginationInfo);

			if (recipes == null)
			{
				return BadRequest(ResponseDTO.BadRequest());
			}
			return Ok(ResponseDTO.Accept(result: recipes));
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
		public async Task<IActionResult> CreateRecipeAsync([FromBody] RecipeDetailDTO recipeDetailDTO)
		{
			try
			{
				var userId = HttpContext.User.FindFirst(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;
				await _recipeService.CreateRecipeAsync(userId, recipeDetailDTO);
				return Created();
			}
			catch (Exception ex)
			{
				return BadRequest(ResponseDTO.BadRequest(message: ex.Message));
			}
		}

		[HttpPatch]
		[Authorize]
		public async Task<IActionResult> UpdateRecipeAsync([FromBody] RecipeDetailDTO recipeDetailDTO)
		{
			try
			{
				var userId = HttpContext.User.FindFirst(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;
				recipeDetailDTO.UpdatedAt = DateTime.UtcNow;
				await _recipeService.UpdateRecipeAsync(userId, recipeDetailDTO);
				return Ok(ResponseDTO.Accept(result: recipeDetailDTO));
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
				var userId = HttpContext.User.FindFirst(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;
				await _recipeService.DeleteRecipeAsync(userId, id);
				return NoContent();
			}
			catch (Exception ex)
			{
				return BadRequest(ResponseDTO.BadRequest(message: ex.Message));
			}
		}
	}
}
