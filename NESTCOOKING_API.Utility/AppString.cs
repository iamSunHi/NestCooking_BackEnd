using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NESTCOOKING_API.Utility
{
    public class AppString
    {
        public static string NameEmailOwnerDisplay = "Nest Cooking";
        public static string ResetPasswordSubjectEmail = "Verify Your Email";

        public static string ResetPasswordContentEmail(string link)
        {
            return $"Click here to reset your password: {link}";
        }
    }
}
