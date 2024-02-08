using Microsoft.AspNetCore.Mvc;
using NESTCOOKING_API.Business.DTOs;
using NESTCOOKING_API.Business.DTOs.RecipeDTOs;
using NESTCOOKING_API.Business.Services.IServices;

namespace NESTCOOKING_API.Presentation.Controllers
{
	[Route("api/instructors")]
	[ApiController]
	public class InstructorController : ControllerBase
	{
		private readonly IInstructorService _instructorService;

		public InstructorController(IInstructorService instructorService)
		{
			_instructorService = instructorService;
		}

		[HttpGet]
		public async Task<IActionResult> GetInstructorsAsync()
		{
			var instructorList = await _instructorService.GetAllInstructorsAsync();

			if (instructorList == null)
			{
				return BadRequest(ResponseDTO.BadRequest());
			}
			return Ok(ResponseDTO.Accept(result: instructorList));
		}

		[HttpGet("{instructorId}")]
		public async Task<IActionResult> GetInstructorAsync([FromRoute] int instructorId)
		{
			var instructor = await _instructorService.GetInstructorByIdAsync(instructorId);

			if (instructor == null)
			{
				return BadRequest(ResponseDTO.BadRequest(message: $"Not found any instructor with the id: {instructorId}"));
			}
			return Ok(ResponseDTO.Accept(result: instructor));
		}

		[HttpPost]
		public async Task<IActionResult> CreateInstructorAsync([FromBody] InstructorDTO instructorDTO)
		{
			try
			{
				await _instructorService.CreateInstructorAsync(instructorDTO);
				return Created();
			}
			catch (Exception ex)
			{
				return BadRequest(ResponseDTO.BadRequest(message: ex.Message));
			}
		}

		[HttpPatch]
		public async Task<IActionResult> UpdateInstructorAsync([FromBody] InstructorDTO instructorDTO)
		{
			try
			{
				await _instructorService.UpdateInstructorAsync(instructorDTO);
				return NoContent();
			}
			catch (Exception ex)
			{
				return BadRequest(ResponseDTO.BadRequest(message: ex.Message));
			}
		}

		[HttpDelete("delete/{instructorId}")]
		public async Task<IActionResult> DeleteInstructorAsync([FromRoute] int instructorId)
		{
			try
			{
				await _instructorService.DeleteInstructorAsync(instructorId);
				return NoContent();
			}
			catch (Exception ex)
			{
				return BadRequest(ResponseDTO.BadRequest(message: ex.Message));
			}
		}
	}
}
