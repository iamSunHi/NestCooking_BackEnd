﻿using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
public class CloudinaryService : ICloudinaryService
{
    private readonly Cloudinary _cloudinary;
    public CloudinaryService(IConfiguration configuration)
    {
        Account account;

		var isAzureEnvironment = Environment.GetEnvironmentVariable("AZURE_WEBAPP_NAME") != null;
		if (isAzureEnvironment)
		{
			account = new Account(
				cloud: Environment.GetEnvironmentVariable("CLOUDINARY_CLOUD_NAME"),
				apiKey: Environment.GetEnvironmentVariable("CLOUDINARY_API_KEY"),
				apiSecret: Environment.GetEnvironmentVariable("CLOUDINARY_API_SECRET")
			);
		}
		else
		{
			account = new Account(
				configuration["Cloudinary:CloudName"],
				configuration["Cloudinary:ApiKey"],
				configuration["Cloudinary:ApiSecret"]
			);
		}
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