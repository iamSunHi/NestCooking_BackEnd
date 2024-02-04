using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NESTCOOKING_API.Business.DTOs;
using NESTCOOKING_API.Business.DTOs.RecipeDTOs;
using NESTCOOKING_API.Business.Services.IServices;

namespace NESTCOOKING_API.Presentation.Controllers
{
	[Route("api/recipe")]
	[ApiController]
	public class RecipeController : ControllerBase
	{
		private readonly IRecipeService _recipeService;
		private PaginationInfoDTO _paginationInfo = new PaginationInfoDTO();

		public RecipeController(IRecipeService recipeService)
		{
			_recipeService = recipeService;
		}

		[HttpGet("{pageNumber}")]
		public async Task<IActionResult> GetRecipes([FromRoute] int pageNumber)
		{
			if (pageNumber != null)
			{
				_paginationInfo.PageNumber = pageNumber;
			}
			var recipeList = _recipeService.GetRecipesAsync(_paginationInfo);
			if (recipeList == null)
			{
				return BadRequest(ResponseDTO.BadRequest());
			}
			return Ok(ResponseDTO.Accept(result: recipeList));
		}

		[HttpPost]
		[Authorize]
		public async Task<IActionResult> CreateRecipe(RecipeCreationDTO recipeCreationDTO)
		{
			throw new NotImplementedException();
		}
	}
}
