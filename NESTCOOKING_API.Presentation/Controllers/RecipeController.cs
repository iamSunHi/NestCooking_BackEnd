using Microsoft.AspNetCore.Mvc;
using NESTCOOKING_API.Business.DTOs;
using NESTCOOKING_API.Business.DTOs.RecipeDTOs;
using NESTCOOKING_API.Business.Services.IServices;

namespace NESTCOOKING_API.Presentation.Controllers
{
	[Route("api/recipes")]
	[ApiController]
	public class RecipeController : ControllerBase
	{
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
				return BadRequest(ResponseDTO.BadRequest(message: $"Recipe with id {id} not found"));
			}
			return Ok(ResponseDTO.Accept(result: recipe));
		}

		[HttpGet("categories/{categoryId}")]
		public async Task<IActionResult> GetRecipesByCategoryIdAsync([FromRoute] int categoryId)
		{
			var recipe = await _recipeService.GetRecipesByCategoryIdAsync(categoryId);
			if (recipe == null)
			{
				return BadRequest(ResponseDTO.BadRequest(message: $"Recipe with categoryId {categoryId} not found"));
			}
			return Ok(ResponseDTO.Accept(result: recipe));
		}

		[HttpPost]
		public async Task<IActionResult> CreateRecipeAsync([FromBody] RecipeDetailDTO recipeDetailDTO)
		{
			try
			{
				await _recipeService.CreateRecipeAsync(recipeDetailDTO);
				return Created();
			}
			catch (Exception ex)
			{
				return BadRequest(ResponseDTO.BadRequest(message: ex.Message));
			}
		}

		[HttpPatch]
		public async Task<IActionResult> UpdateRecipeAsync([FromBody] RecipeDetailDTO recipeDetailDTO)
		{
			try
			{
				await _recipeService.UpdateRecipeAsync(recipeDetailDTO);
				return NoContent();
			}
			catch (Exception ex)
			{
				return BadRequest(ResponseDTO.BadRequest(message: ex.Message));
			}
		}

		[HttpDelete("delete/{id}")]
		public async Task<IActionResult> DeleteRecipeAsync([FromRoute] string id)
		{
			try
			{
				await _recipeService.DeleteRecipeAsync(id);
				return NoContent();
			}
			catch (Exception ex)
			{
				return BadRequest(ResponseDTO.BadRequest(message: ex.Message));
			}
		}
	}
}
