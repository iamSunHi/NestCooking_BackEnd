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
        public static string RegisterSuccessMessage = "Register successfully. Please check your email";
        public static string RegisterErrorMessage = "Something went wrong when register";
        public static string EmailConfirmationSubjectEmail = "Confirm your email";
        public static string EmailConfirmationContentEmail(string emailConfirmationLink)
        {
            return $"Thank you for your registration. Click here to confirm your email: {emailConfirmationLink}";
        }
        public static string UserNotFoundMessage = "User not found";
        public static string EmailConfirmationSuccessMessage = "Email confirmation successfully";
        public static string SomethingWrongMessage = "Something went wrong";
        public static string InvalidTokenErrorMessage = "Wrong token";
        public static string NotEmailConfirmedErrorMessage = "Sorry, but you are not confirmed your email. Please check your email again";
        public static string LockoutAccountErrorMessage = "Your account has been locked due to multiple failed login attempts. Please try again later.";
        public static string ResendEmailConfirmationSubjectEmail = "Re-send email confirmation";
        public static string ResendEmailConfirmationContentEmail(string emailConfirmationLink)
        {
            return $"Click here to confirm your email: {emailConfirmationLink}";
        }
        public static string InvalidEmailErrorMessage = "Invalid email";
        public static string IncorrectCredentialsLoginErrorMessage = "Username or password is incorrect!";
        public static string AccountLockedOutLoginErrorMessage = "This account is locked out!";
        public static string RequestErrorMessage = "Error in request !";
    }
}
