using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NESTCOOKING_API.Business.DTOs;
using NESTCOOKING_API.Business.DTOs.BookingDTOs;
using NESTCOOKING_API.Business.Services.IServices;
using NESTCOOKING_API.Utility;
using System.Security.Claims;

namespace NESTCOOKING_API.Presentation.Controllers
{
	[Route("api/chef")]
	[ApiController]
	[Authorize]
	public class ChefController : ControllerBase
	{
		private readonly IChefService _chefService;

		public ChefController(IChefService chefService)
		{
			_chefService = chefService;
		}

		[HttpGet("{chefId}/dishes")]
		public async Task<IActionResult> GetAllChefDishesByChefIdAsync(string chefId)
		{
			try
			{
				var chefDishList = await _chefService.GetChefDishByChefIdAsync(chefId);
				return Ok(ResponseDTO.Accept(result: chefDishList));
			}
			catch (Exception ex)
			{
				if (ex.InnerException != null)
				{
					return BadRequest(ResponseDTO.BadRequest(message: ex.InnerException.Message));
				}
				return BadRequest(ResponseDTO.BadRequest(message: ex.Message));
			}
		}

		[HttpGet("dishes")]
		public async Task<IActionResult> GetAllChefDishesAsync()
		{
			try
			{
				var chefDishList = await _chefService.GetAllChefDishesAsync();
				return Ok(ResponseDTO.Accept(result: chefDishList));
			}
			catch (Exception ex)
			{
				if (ex.InnerException != null)
				{
					return BadRequest(ResponseDTO.BadRequest(message: ex.InnerException.Message));
				}
				return BadRequest(ResponseDTO.BadRequest(message: ex.Message));
			}
		}

		[HttpGet("dishes/{dishId}")]
		public async Task<IActionResult> GetChefDishByChefIdAsync(string dishId)
		{
			try
			{
				var chefDish = await _chefService.GetChefDishByIdAsync(dishId);
				return Ok(ResponseDTO.Accept(result: chefDish));
			}
			catch (Exception ex)
			{
				if (ex.InnerException != null)
				{
					return BadRequest(ResponseDTO.BadRequest(message: ex.InnerException.Message));
				}
				return BadRequest(ResponseDTO.BadRequest(message: ex.Message));
			}
		}

		[HttpPost("dishes/create")]
		[Authorize(Roles = StaticDetails.Role_Chef)]
		public async Task<IActionResult> CreateChefDishAsync(ChefDishDTO chefDishDTO)
		{
			try
			{
				var chefId = this.GetUserIdFromContext(HttpContext);
				chefDishDTO.ChefId = chefId;
				await _chefService.CreateChefDishAsync(chefDishDTO);
				return Created();
			}
			catch (Exception ex)
			{
				if (ex.InnerException != null)
				{
					return BadRequest(ResponseDTO.BadRequest(message: ex.InnerException.Message));
				}
				return BadRequest(ResponseDTO.BadRequest(message: ex.Message));
			}
		}

		[HttpPatch("dishes/update")]
		[Authorize(Roles = StaticDetails.Role_Chef + "," + StaticDetails.Role_Admin)]
		public async Task<IActionResult> UpdateChefDishAsync(ChefDishDTO chefDishDTO)
		{
			try
			{
				var updatedChefDish = await _chefService.UpdateChefDishAsync(chefDishDTO);
				return Ok(ResponseDTO.Accept(result: updatedChefDish));
			}
			catch (Exception ex)
			{
				if (ex.InnerException != null)
				{
					return BadRequest(ResponseDTO.BadRequest(message: ex.InnerException.Message));
				}
				return BadRequest(ResponseDTO.BadRequest(message: ex.Message));
			}
		}

		[HttpDelete("dishes/delete/{chefDishId}")]
		[Authorize(Roles = StaticDetails.Role_Chef + "," + StaticDetails.Role_Admin)]
		public async Task<IActionResult> DeleteChefDishAsync([FromRoute] string chefDishId)
		{
			try
			{
				await _chefService.RemoveChefDishAsync(chefDishId);
				return NoContent();
			}
			catch (Exception ex)
			{
				if (ex.InnerException != null)
				{
					return BadRequest(ResponseDTO.BadRequest(message: ex.InnerException.Message));
				}
				return BadRequest(ResponseDTO.BadRequest(message: ex.Message));
			}
		}

		private string GetUserIdFromContext(HttpContext context)
		{
			return context.User.FindFirst(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;
		}
	}
}
