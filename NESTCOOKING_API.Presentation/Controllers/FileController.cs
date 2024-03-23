using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NESTCOOKING_API.Business.DTOs;
using NESTCOOKING_API.Utility;

namespace NESTCOOKING_API.Presentation.Controllers
{
	[Route("api/upload")]
    [ApiController]
    [Authorize]
    public class FileController : ControllerBase
    {
        private ICloudinaryService _cloudinaryService;

        public FileController(ICloudinaryService cloudinaryService)
        {
            _cloudinaryService = cloudinaryService;
        }

        [HttpPost("image")]
        public async Task<IActionResult> UploadImage([FromForm] Business.DTOs.UserDTOs.UploadImageDTO uploadImageDTO)
        {
            if (string.IsNullOrEmpty(uploadImageDTO.Path))
            {
                return BadRequest(ResponseDTO.BadRequest(message: "Path is required!"));

            }
            if (uploadImageDTO.File == null || !Validation.IsValidImageFileExtension(uploadImageDTO.File))
            {
                return BadRequest(ResponseDTO.BadRequest(message: AppString.InvalidImageErrorMessage));
            }

            var result = await _cloudinaryService.UploadImageAsync(uploadImageDTO.File, uploadImageDTO.Path);

            if (result == null)
            {
                return BadRequest(ResponseDTO.BadRequest(message: "Failed to upload image."));
            }

            return Ok(ResponseDTO.Accept(result: result));
        }
    }
}
