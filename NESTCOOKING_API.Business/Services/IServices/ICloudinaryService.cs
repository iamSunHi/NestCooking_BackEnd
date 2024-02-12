using Microsoft.AspNetCore.Http;

public interface ICloudinaryService
{
    Task<string> UploadImageAsync(IFormFile file, string path);
    Task<string> UploadVideoAsync(IFormFile file, string path);
}