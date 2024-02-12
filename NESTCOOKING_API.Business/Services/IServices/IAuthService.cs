using NESTCOOKING_API.Business.DTOs;
using NESTCOOKING_API.Business.DTOs.AuthDTOs;
using static NESTCOOKING_API.Utility.StaticDetails;

namespace NESTCOOKING_API.Business.Services.IServices
{
    public interface IAuthService
    {
        Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO);
        Task<bool> Register(RegistrationRequestDTO registrationRequestDTO);
        Task<(string, string)> GenerateResetPasswordToken(string identifier);
        Task<bool> ResetPassword(ResetPasswordRequestDTO resetPasswordRequestDTO);
        Task<bool> VerifyResetPasswordToken(string email, string token);
        Task<string> LoginWithThirdParty(OAuth2RequestDTO oAuth2RequestDTO, ProviderLogin providerLogin);
        Task<(string Email, string Username, string AvatarURL)> VerifyIdentifierResetPassword(string identifier);
        Task<bool> VerifyEmailConfirmation(string email, string token);
        Task<bool> VerifyEmailResetPassword(string email, string token);

        Task<(string, string)> GenerateEmailConfirmationTokenAsync(string email);
    }
}
