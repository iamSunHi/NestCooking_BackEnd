
using System.Net.Mail;

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

}