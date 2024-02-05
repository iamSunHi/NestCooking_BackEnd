using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NESTCOOKING_API.Business.DTOs;
using NESTCOOKING_API.Business.DTOs.RecipeDTOs;
using NESTCOOKING_API.Business.Services.IServices;
using NESTCOOKING_API.Utility;

namespace NESTCOOKING_API.Presentation.Controllers
{
    [Route("api/category")]
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

		[HttpGet("page/{pageNumber}")]
		public async Task<IActionResult> GetCategoriesAsync([FromRoute] int pageNumber)
		{
			if (pageNumber != null)
			{
				_paginationInfo.PageNumber = pageNumber;
			}
			var categoryList = await _categoryService.GetCategoriesAsync(_paginationInfo);

			if (categoryList == null)
			{
				return BadRequest(ResponseDTO.BadRequest());
			}
			return Ok(ResponseDTO.Accept(result: categoryList));
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

		[HttpPost]
		[Authorize(Roles = StaticDetails.Role_Admin)]
		public async Task<IActionResult> CreateCategoryAsync([FromBody] CategoryDTO categoryDTO)
		{
			try
			{
				var createdCategory = await _categoryService.CreateCategoryAsync(categoryDTO);
				if (createdCategory == null)
				{
					return BadRequest(ResponseDTO.BadRequest(message: "This category already exists!"));
				}
				return Created($"api/admin/category/{createdCategory.Id}", createdCategory);
			}
			catch (Exception ex)
			{
				return BadRequest(ResponseDTO.BadRequest(message: ex.Message));
			}
		}

		[HttpPatch]
		[Authorize(Roles = StaticDetails.Role_Admin)]
		public async Task<IActionResult> UpdateCategoryAsync([FromBody] CategoryDTO categoryDTO)
		{
			try
			{
				await _categoryService.UpdateCategoryAsync(categoryDTO);
				return NoContent();
			}
			catch (Exception ex)
			{
				return BadRequest(ResponseDTO.BadRequest(message: ex.Message));
			}
		}

		[HttpDelete("delete/{categoryId}")]
		[Authorize(Roles = StaticDetails.Role_Admin)]
		public async Task<IActionResult> DeleteCategoryAsync([FromRoute] int categoryId)
		{
			try
			{
				await _categoryService.DeleteCategoryAsync(categoryId);
				return NoContent();
			}
			catch (Exception ex)
			{
				return BadRequest(ResponseDTO.BadRequest(message: ex.Message));
			}
		}
	}
}
