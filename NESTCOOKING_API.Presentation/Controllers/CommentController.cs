using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NESTCOOKING_API.Business.DTOs.CommentDTOs;
using NESTCOOKING_API.Business.DTOs;
using NESTCOOKING_API.Business.Services.IServices;
using NESTCOOKING_API.Utility;
using System.Security.Claims;

namespace NESTCOOKING_API.Presentation.Controllers
{
	[Route("api/comments")]
	[ApiController]
	[Authorize]
	public class CommentController : ControllerBase
	{
		private readonly ICommentService _commentService;

		public CommentController(ICommentService commentService)
		{
			_commentService = commentService;
		}
		[HttpGet]
		public async Task<IActionResult> GetAllComments()
		{
			try
			{
				var result = await _commentService.GetAllComments();
				return Ok(ResponseDTO.Accept(result: result));
			}
			catch (Exception ex)
			{
				return BadRequest(ResponseDTO.BadRequest(ex.Message));
			}
		}
		[HttpGet("recipe/{recipeId}")]
		public async Task<IActionResult> GetAllCommentsByRecipeId(string recipeId)
		{
			try
			{
				var result = await _commentService.GetCommentsOfRecipeByRecipeId(recipeId);
				return Ok(ResponseDTO.Accept(result: result));
			}
			catch (Exception ex)
			{
				return BadRequest(ResponseDTO.BadRequest(ex.Message));
			}
		}
		[HttpGet("parent/{parentCommentId}")]
		public async Task<IActionResult> GetAllCommentsByParentCommentId(string parentCommentId)
		{
			try
			{
				var result = await _commentService.GetChildCommentsByParentCommentId(parentCommentId);
				return Ok(ResponseDTO.Accept(result: result));
			}
			catch (Exception ex)
			{
				return BadRequest(ResponseDTO.BadRequest(ex.Message));
			}
		}
		[HttpGet("{commentId}")]
		public async Task<IActionResult> GetCommentById(string commentId)
		{
			try
			{
				var result = await _commentService.GetCommentById(commentId);
				return result != null ? Ok(ResponseDTO.Accept(result: result)) : NotFound(AppString.RequestCommentNotFound);
			}
			catch (Exception ex)
			{
				return BadRequest(ResponseDTO.BadRequest(message: ex.Message));
			}
		}
		[HttpDelete("{commentId}")]
		[Authorize]
		public async Task<IActionResult> DeleteComment(string commentId)
		{
			try
			{
				var userId = GetUserIdFromContext(HttpContext);
				await _commentService.DeleteComment(userId, commentId);
				return Ok(ResponseDTO.Accept(result: AppString.DeleteCommentSuccessMessage));
			}
			catch (Exception ex)
			{
				return BadRequest(ResponseDTO.BadRequest(message: ex.Message));
			}
		}
		[HttpPut("{commentId}")]
		[Authorize]
		public async Task<IActionResult> UpdateComment(string commentId, [FromBody] UpdateCommentDTO createCommentDTO)
		{
			try
			{
				var userId = GetUserIdFromContext(HttpContext);
				var updatedComment = await _commentService.UpdateComment(commentId, createCommentDTO);
				if (updatedComment != null)
				{
					return Ok(ResponseDTO.Accept(AppString.UpdateCommentSuccessMassage, result: updatedComment));
				}
				else
				{
					return NotFound(AppString.RequestCommentNotFound);
				}
			}
			catch (Exception ex)
			{
				return BadRequest(ResponseDTO.BadRequest(message: ex.Message));
			}
		}
		[HttpPost]
		[Authorize]
		public async Task<IActionResult> CreateComment([FromBody] CreatedCommentDTO createCommentDTO)
		{
			try
			{
				var userId = GetUserIdFromContext(HttpContext);

				var validType = await _commentService.ValidType(createCommentDTO.Type);
				if (validType)
				{
					var result = await _commentService.CreateComment(userId, createCommentDTO);
					if (result != null)
					{
						return Ok(ResponseDTO.Accept(result: result));
					}
					else
					{
						return StatusCode(500, AppString.CreateCommentInternalServerErrorMessage);
					}
				}
				return StatusCode(500, AppString.CreateCommentInValidType);
			}
			catch (Exception ex)
			{
				return BadRequest(ResponseDTO.BadRequest(message: ex.Message));
			}

		}
		private string GetUserIdFromContext(HttpContext context)
		{
			return context.User.FindFirst(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;
		}

	}
}
