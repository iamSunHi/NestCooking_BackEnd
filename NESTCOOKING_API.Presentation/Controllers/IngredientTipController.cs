using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NESTCOOKING_API.Business.DTOs;
using NESTCOOKING_API.Business.DTOs.RecipeDTOs;
using NESTCOOKING_API.Business.Services.IServices;
using NESTCOOKING_API.Utility;

namespace NESTCOOKING_API.Presentation.Controllers
{
	[Route("api/ingredient-tip")]
	[ApiController]
	public class IngredientTipController : ControllerBase
	{
		private PaginationInfoDTO _paginationInfo = new PaginationInfoDTO();
		private IIngredientTipService _ingredientTipService;

		public IngredientTipController(IIngredientTipService ingredientTipService)
		{
			_ingredientTipService = ingredientTipService;
		}

		[HttpGet]
		public async Task<IActionResult> GetIngredientTipsAsync()
		{
			var ingredientTipList = await _ingredientTipService.GetAllIngredientTipsAsync();

			if (ingredientTipList == null)
			{
				return BadRequest(ResponseDTO.BadRequest());
			}
			return Ok(ResponseDTO.Accept(result: ingredientTipList));
		}

		[HttpGet("page/{pageNumber}")]
		public async Task<IActionResult> GetIngredientTipsAsync([FromRoute] int pageNumber)
		{
			if (pageNumber != null)
			{
				_paginationInfo.PageNumber = pageNumber;
			}
			var ingredientTips = await _ingredientTipService.GetIngredientTipsAsync(_paginationInfo);

			if (ingredientTips == null)
			{
				return BadRequest(ResponseDTO.BadRequest());
			}
			return Ok(ResponseDTO.Accept(result: ingredientTips));
		}

		[HttpGet("{ingredientTipId}")]
		public async Task<IActionResult> GetIngredientTipAsync([FromRoute] int ingredientTipId)
		{
			var ingredientTip = await _ingredientTipService.GetIngredientTipByIdAsync(ingredientTipId);

			if (ingredientTip == null)
			{
				return BadRequest(ResponseDTO.BadRequest(message: $"Not found any ingredient tip with the id: {ingredientTipId}"));
			}
			return Ok(ResponseDTO.Accept(result: ingredientTip));
		}

		[HttpPost]
		[Authorize]
		public async Task<IActionResult> CreateIngredientTipAsync([FromBody] IngredientTipDTO ingredientTipDTO)
		{
			try
			{
				var createdIngredientTip = await _ingredientTipService.CreateIngredientTipAsync(ingredientTipDTO);
				if (createdIngredientTip == null)
				{
					return BadRequest(ResponseDTO.BadRequest(message: "Failed to create ingredient tip!"));
				}
				return Created($"api/ingredient-tip/{createdIngredientTip.Id}", createdIngredientTip);
			}
			catch (Exception ex)
			{
				return BadRequest(ResponseDTO.BadRequest(message: ex.Message));
			}
		}

		[HttpPatch]
		[Authorize]
		public async Task<IActionResult> UpdateIngredientTipAsync([FromBody] IngredientTipDTO ingredientTipDTO)
		{
			try
			{
				await _ingredientTipService.UpdateIngredientTipAsync(ingredientTipDTO);
				return NoContent();
			}
			catch (Exception ex)
			{
				return BadRequest(ResponseDTO.BadRequest(message: ex.Message));
			}
		}

		[HttpDelete("delete/{ingredientTipId}")]
		[Authorize]
		public async Task<IActionResult> DeleteIngredientTipAsync([FromRoute] int ingredientTipId)
		{
			try
			{
				await _ingredientTipService.DeleteIngredientTipAsync(ingredientTipId);
				return NoContent();
			}
			catch (Exception ex)
			{
				return BadRequest(ResponseDTO.BadRequest(message: ex.Message));
			}
		}
	}
}
