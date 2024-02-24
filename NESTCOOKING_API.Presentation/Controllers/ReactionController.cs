using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NESTCOOKING_API.Business.DTOs;
using NESTCOOKING_API.Business.DTOs.ReactionDTOs;
using NESTCOOKING_API.Business.Services.IServices;
using NESTCOOKING_API.DataAccess.Models;
using System.Security.Claims;

namespace NESTCOOKING_API.Presentation.Controllers
{
    [Route("api/reaction")]
    [ApiController]
    [Authorize]
    public class ReactionController : ControllerBase
    {
        private readonly IReactionService _reactionService;

        public ReactionController(IReactionService reactionService)
        {
            _reactionService = reactionService;
        }

        [HttpPost]
        public async Task<IActionResult> AddReaction([FromBody] ReactionDTO reactionDTO)
        {
            try
            {
                var userId = HttpContext.User.FindFirst(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;
                if (userId == null)
                {
                    throw new UnauthorizedAccessException();
                }
                var result = await _reactionService.AddReactionAsync(reactionDTO, userId);
                if (result)
                {
                    return Ok(ResponseDTO.Accept("Add Reaction Success"));
                }
                else
                {
                    return BadRequest(ResponseDTO.BadRequest("Add Reaction Fail"));
                }

            }
            catch (Exception ex)
            {
                return BadRequest(ResponseDTO.BadRequest(message: ex.Message));
            }

        }
        [HttpDelete("{targetId}")]
        public async Task<IActionResult> Delete(string targetId)
        {
            var userId = HttpContext.User.FindFirst(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;
            try
            {
                if (userId == null)
                {
                    throw new UnauthorizedAccessException();
                }
                var result = await _reactionService.DeleteReactionAsync(targetId, userId);
                if (result)
                {
                    return Ok(ResponseDTO.Accept("Delete Reaction Success"));
                }

                else
                {
                    return BadRequest(ResponseDTO.BadRequest("Delete Reaction Fail"));
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseDTO.BadRequest(message: ex.Message));
            }
        }
        [HttpPut]
        public async Task<IActionResult> UpdateReaction([FromBody] ReactionDTO reactionDTO)
        {
            var userId = HttpContext.User.FindFirst(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;
            try
            {
                if (userId == null)
                {
                    throw new UnauthorizedAccessException();
                }
                var result = await _reactionService.UpdateReactionAsync(reactionDTO, userId);
                if (result)
                {
                    return Ok(ResponseDTO.Accept(message:"Update Reaction Success"));
                }
                else
                {
                    return BadRequest(ResponseDTO.BadRequest(message:"Update Reaction Fail"));
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseDTO.BadRequest(message: ex.Message));
            }
        }
        [HttpGet("{type}")]
        public async Task<IActionResult> GetReactionsById(string type, [FromQuery] string targetId)
        {
            try
            {
                if (string.IsNullOrEmpty(type))
                {
                    return BadRequest(ResponseDTO.BadRequest(message: "Type is required"));
                }
                if (string.IsNullOrEmpty(targetId))
                {
                    return BadRequest(ResponseDTO.BadRequest(message: "Target is required"));
                }
                var result = await _reactionService.GetReactionsByIdAsync(targetId,type);
                return Ok(ResponseDTO.Accept(result: result));
            }

            catch (Exception ex)
            {
                return BadRequest(ResponseDTO.BadRequest(message: ex.Message));
            }
        }

    }
}
