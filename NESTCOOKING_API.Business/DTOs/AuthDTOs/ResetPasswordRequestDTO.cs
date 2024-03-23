namespace NESTCOOKING_API.Business.DTOs.AuthDTOs
{
    public class ResetPasswordRequestDTO
    {
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
        public string Token { get; set; }
        public string Email { get; set; }
    }
}
