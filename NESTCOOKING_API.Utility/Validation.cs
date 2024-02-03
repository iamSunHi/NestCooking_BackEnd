using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace NESTCOOKING_API.Utility;
public class Validation
{
    public static bool CheckEmailValid(string email)
    {
        try
        {
            var emailValidation = new EmailAddressAttribute();
            return emailValidation.IsValid(email);
        }
        catch
        {
            return false;
        }
    }

    public static bool IsValidImageFileExtension(IFormFile file)
    {
        // Limit the size of the file to
        long maxSizeInBytes = 10 * 1024 * 1024;

        if (file.Length > maxSizeInBytes)
        {
            return false;
        }

        // Validate the file extension
        string[] validImageFileExtension = [".png", ".jpg", ".jpeg", ".webp", ".gif"];
        var fileExtension = Path.GetExtension(file.FileName).ToLower();
        if (validImageFileExtension.Contains(fileExtension))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

}