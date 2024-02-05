using Microsoft.AspNetCore.Mvc;
using NESTCOOKING_API.Business.DTOs;
using NESTCOOKING_API.Business.DTOs.RecipeDTOs;
using NESTCOOKING_API.Business.Services.IServices;

namespace NESTCOOKING_API.Presentation.Controllers
{
	[Route("api/ingredient")]
	[ApiController]
	public class IngredientController : ControllerBase
	{
		private readonly IIngredientService _ingredientService;

		public IngredientController(IIngredientService ingredientService)
		{
			_ingredientService = ingredientService;
		}

		[HttpGet]
		public async Task<IActionResult> GetIngredientsAsync()
		{
			var ingredientList = await _ingredientService.GetAllIngredientsAsync();

			if (ingredientList == null)
			{
				return BadRequest(ResponseDTO.BadRequest());
			}
			return Ok(ResponseDTO.Accept(result: ingredientList));
		}

		[HttpGet("{ingredientId}")]
		public async Task<IActionResult> GetIngredientAsync([FromRoute] int ingredientId)
		{
			var ingredient = await _ingredientService.GetIngredientByIdAsync(ingredientId);

			if (ingredient == null)
			{
				return BadRequest(ResponseDTO.BadRequest(message: $"Not found any ingredient with the id: {ingredientId}"));
			}
			return Ok(ResponseDTO.Accept(result: ingredient));
		}

		[HttpPost]
		public async Task<IActionResult> CreateIngredientAsync([FromBody] IngredientDTO ingredientDTO)
		{
			try
			{
				var createdIngredient = await _ingredientService.CreateIngredientAsync(ingredientDTO);
				if (createdIngredient == null)
				{
					return BadRequest(ResponseDTO.BadRequest(message: "This ingredient already exists!"));
				}
				return Created($"api/ingredient/{createdIngredient.Id}", createdIngredient);
			}
			catch (Exception ex)
			{
				return BadRequest(ResponseDTO.BadRequest(message: ex.Message));
			}
		}

		[HttpPatch]
		public async Task<IActionResult> UpdateIngredientAsync([FromBody] IngredientDTO ingredientDTO)
		{
			try
			{
				await _ingredientService.UpdateIngredientAsync(ingredientDTO);
				return NoContent();
			}
			catch (Exception ex)
			{
				return BadRequest(ResponseDTO.BadRequest(message: ex.Message));
			}
		}

		[HttpDelete("delete/{ingredientId}")]
		public async Task<IActionResult> DeleteIngredientAsync([FromRoute] int ingredientId)
		{
			try
			{
				await _ingredientService.DeleteIngredientAsync(ingredientId);
				return NoContent();
			}
			catch (Exception ex)
			{
				return BadRequest(ResponseDTO.BadRequest(message: ex.Message));
			}
		}
	}
}
