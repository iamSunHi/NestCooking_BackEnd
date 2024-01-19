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
        public static string ConfirmPasswordMismatchErrorMessage = "The confirm password does not match the new password";

        public static string SamePasswordErrorMessage = "New password must be different from current password";
        public static string ChangePasswordSuccessMessage = "Password changed successfully.";

        public static string InvalidPasswordErrorMessage = "Invalid password.";
        public static string InvalidImageErrorMessage = "Please choose a valid image file to upload.";
        public static string UpdateAvatarSuccessMessage = "Change avatar successfully";
        public static string UpdateAvatarErrorMessage = "Something went error when update the avatar";
        public static string UpdateInformationSuccessMessage = "Your information changed successfully.";
        public static string UpdateInformationErrorMessage = "Something went wrong when update your information.";

    }
}
