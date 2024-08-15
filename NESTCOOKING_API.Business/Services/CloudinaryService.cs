using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
public class CloudinaryService : ICloudinaryService
{
	private readonly Cloudinary _cloudinary;
	public CloudinaryService(IConfiguration configuration)
	{
		string CLOUDINARY_CLOUD_NAME = string.IsNullOrEmpty(Environment.GetEnvironmentVariable("AZURE_WEBAPP_NAME")) ? configuration["APPSETTING_CLOUDINARY_CLOUD_NAME"] : Environment.GetEnvironmentVariable("APPSETTING_CLOUDINARY_CLOUD_NAME");
		string CLOUDINARY_API_KEY = string.IsNullOrEmpty(Environment.GetEnvironmentVariable("AZURE_WEBAPP_NAME")) ? configuration["APPSETTING_CLOUDINARY_API_KEY"] : Environment.GetEnvironmentVariable("APPSETTING_CLOUDINARY_API_KEY");
		string CLOUDINARY_API_SECRET = string.IsNullOrEmpty(Environment.GetEnvironmentVariable("AZURE_WEBAPP_NAME")) ? configuration["APPSETTING_CLOUDINARY_API_SECRET"] : Environment.GetEnvironmentVariable("APPSETTING_CLOUDINARY_API_SECRET");

		Account account = new Account(
			CLOUDINARY_CLOUD_NAME,
			CLOUDINARY_API_KEY,
			CLOUDINARY_API_SECRET
		);
		_cloudinary = new Cloudinary(account);
	}
	public async Task<string> UploadImageAsync(IFormFile file, string path)
	{
		if (file.Length > 0)
		{
			using (var stream = file.OpenReadStream())
			{
				var uploadParams = new ImageUploadParams()
				{
					File = new FileDescription(file.FileName, stream),
					Folder = path
				};
				var uploadResult = await _cloudinary.UploadAsync(uploadParams);
				return uploadResult.Url.AbsoluteUri;
			}
		}
		return null;
	}
	public async Task<string> UploadVideoAsync(IFormFile file, string path)
	{
		if (file.Length > 0)
		{
			using (var stream = file.OpenReadStream())
			{
				var uploadParams = new VideoUploadParams()
				{
					File = new FileDescription(file.FileName, stream),
					Folder = path
				};
				var uploadResult = await _cloudinary.UploadAsync(uploadParams);
				return uploadResult.Url.AbsoluteUri;
			}
		}
		return null;
	}
}