using Microsoft.AspNetCore.Http;
using System.IO;

namespace NESTCOOKING_API.Utility;
public class Validation
{
    public static bool CheckEmailValid(string email)
    {
        try
        {
            var add = new System.Net.Mail.MailAddress(email);
            return add.Address == email;
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