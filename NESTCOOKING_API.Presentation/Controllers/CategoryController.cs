using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NESTCOOKING_API.Business.DTOs;
using NESTCOOKING_API.Business.DTOs.RecipeDTOs;
using NESTCOOKING_API.Business.Services.IServices;
using NESTCOOKING_API.Utility;

namespace NESTCOOKING_API.Presentation.Controllers
{
    [Route("api/categories")]
	[ApiController]
	public class CategoryController : ControllerBase
	{
		private PaginationInfoDTO _paginationInfo = new PaginationInfoDTO();
		private ICategoryService _categoryService;

		public CategoryController(ICategoryService categoryService)
		{
			_categoryService = categoryService;
		}

		[HttpGet]
		public async Task<IActionResult> GetCategoriesAsync()
		{
			var categoryList = await _categoryService.GetAllCategoriesAsync();

			if (categoryList == null)
			{
				return BadRequest(ResponseDTO.BadRequest());
			}
			return Ok(ResponseDTO.Accept(result: categoryList));
		}

		[HttpGet("page")]
		public async Task<IActionResult> GetCategoriesAsync([FromQuery] int pageNumber, [FromQuery] int pageSize)
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
			(int totalItems, int totalPages, IEnumerable<CategoryDTO> categorieList) result = await _categoryService.GetCategoriesAsync(_paginationInfo);

			if (result.categorieList == null)
			{
				return BadRequest(ResponseDTO.BadRequest(message: "Page number is not valid!"));
			}
			return Ok(ResponseDTO.Accept(result: new
			{
				metadata = new
				{
					result.totalItems,
					result.totalPages,
					pageNumber,
					pageSize
				},
				categories = result.categorieList
			}));
		}

		[HttpGet("{categoryId}")]
		public async Task<IActionResult> GetCategoryAsync([FromRoute] int categoryId)
		{
			var category = await _categoryService.GetCategoryByIdAsync(categoryId);

			if (category == null)
			{
				return BadRequest(ResponseDTO.BadRequest(message: $"Not found any category with the id: {categoryId}"));
			}
			return Ok(ResponseDTO.Accept(result: category));
		}
	}
}
