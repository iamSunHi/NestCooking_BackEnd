namespace NESTCOOKING_API.Business.DTOs.AuthDTOs
{
    public class VerifyEmailTokenRequestDTO
    {
        public string Token { get; set; }
        public string Email { get; set; }
    }
}
