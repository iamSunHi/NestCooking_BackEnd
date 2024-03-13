using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NESTCOOKING_API.Business.DTOs;
using NESTCOOKING_API.Business.DTOs.RecipeDTOs;
using NESTCOOKING_API.Business.Services.IServices;
using System.Security.Claims;

namespace NESTCOOKING_API.Presentation.Controllers
{
	[Route("api/ingredient-tips")]
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

		[HttpGet("page")]
		public async Task<IActionResult> GetIngredientTipsAsync([FromQuery] int pageNumber, [FromQuery] int pageSize)
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
			(int totalItems, int totalPages, IEnumerable<IngredientTipShortInfoDTO> ingredientTipList) result = await _ingredientTipService.GetIngredientTipsAsync(_paginationInfo);

			if (result.ingredientTipList == null)
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
				ingredientTips = result.ingredientTipList
			}));
		}

		[HttpGet("{ingredientTipId}")]
		public async Task<IActionResult> GetIngredientTipAsync([FromRoute] string ingredientTipId)
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
				var userId = HttpContext.User.FindFirst(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;
				await _ingredientTipService.CreateIngredientTipAsync(userId, ingredientTipDTO);
				return Created();
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
				var userId = HttpContext.User.FindFirst(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;
				ingredientTipDTO.UpdatedAt = DateTime.Now;
				await _ingredientTipService.UpdateIngredientTipAsync(userId, ingredientTipDTO);
				return Ok(ResponseDTO.Accept(result: ingredientTipDTO));
			}
			catch (Exception ex)
			{
				return BadRequest(ResponseDTO.BadRequest(message: ex.Message));
			}
		}

		[HttpDelete("delete/{ingredientTipId}")]
		[Authorize]
		public async Task<IActionResult> DeleteIngredientTipAsync([FromRoute] string ingredientTipId)
		{
			try
			{
				var userId = HttpContext.User.FindFirst(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;
				await _ingredientTipService.DeleteIngredientTipAsync(userId, ingredientTipId);
				return NoContent();
			}
			catch (Exception ex)
			{
				return BadRequest(ResponseDTO.BadRequest(message: ex.Message));
			}
		}
	}
}
