using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http;

namespace NESTCOOKING_API.Business.DTOs.UserDTOs
{
    public class UploadImageDTO
    {
        public IFormFile File { get; set; }
        public string Path { get; set; }
    }
}
