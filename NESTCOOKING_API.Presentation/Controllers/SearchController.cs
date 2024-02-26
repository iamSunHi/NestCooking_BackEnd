using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NESTCOOKING_API.Business.DTOs;
using NESTCOOKING_API.Business.Services.IServices;
using NESTCOOKING_API.Utility;
using System.Security.Claims;

namespace NESTCOOKING_API.Presentation.Controllers
{
	[Route("api/search")]
	[ApiController]
	public class SearchController : ControllerBase
	{
		private readonly ISearchService _searchService;

		public SearchController(ISearchService searchService)
		{
			_searchService = searchService;
		}

		[HttpGet("all/{criteria}")]
		public async Task<IActionResult> GetAllAsync([FromRoute] string criteria)
		{
			try
			{
				criteria = criteria.Trim().Replace('+', ' ');
				var userId = HttpContext.User.FindFirst(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;
				var result = new SearchResultDTO();
				if (userId == null)
				{
					result = await _searchService.GetAllAsync(criteria);
				}
				else
				{
					result = await _searchService.GetAllAsync(criteria, userId: userId);
				}
				if (result.Users.Count() == 0 && result.Recipes.Count() == 0)
				{
					return BadRequest(ResponseDTO.BadRequest(message: AppString.NoResultsFoundErrorMessage));
				}
				return Ok(ResponseDTO.Accept(result: result));
			}
			catch (Exception ex)
			{
				return BadRequest(ResponseDTO.BadRequest(ex.Message));
			}
		}

		[HttpGet("recipes/{criteria}")]
		public async Task<IActionResult> GetRecipesAsync([FromRoute] string criteria)
		{
			try
			{
				criteria = criteria.Trim().Replace('+', ' ');
				var userId = HttpContext.User.FindFirst(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;
				var result = new SearchResultDTO();
				if (userId == null)
				{
					result = await _searchService.GetRecipesAsync(criteria);
				}
				else
				{
					result = await _searchService.GetRecipesAsync(criteria, userId: userId);
				}
				if (result.Recipes.Count() == 0)
				{
					return BadRequest(ResponseDTO.BadRequest(message: AppString.NoResultsFoundErrorMessage));
				}
				return Ok(ResponseDTO.Accept(result: result));
			}
			catch (Exception ex)
			{
				return BadRequest(ResponseDTO.BadRequest(ex.Message));
			}
		}

		[HttpGet("users/{criteria}")]
		public async Task<IActionResult> GetUsersAsync([FromRoute] string criteria)
		{
			try
			{
				criteria = criteria.Trim().Replace('+', ' ');
				var userId = HttpContext.User.FindFirst(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;
				var result = new SearchResultDTO();
				if (userId == null)
				{
					result = await _searchService.GetUsersAsync(criteria);
				}
				else
				{
					result = await _searchService.GetUsersAsync(criteria, userId: userId);
				}
				if (result.Users.Count() == 0)
				{
					return BadRequest(ResponseDTO.BadRequest(message: AppString.NoResultsFoundErrorMessage));
				}
				return Ok(ResponseDTO.Accept(result: result));
			}
			catch (Exception ex)
			{
				return BadRequest(ResponseDTO.BadRequest(ex.Message));
			}
		}
	}
}
