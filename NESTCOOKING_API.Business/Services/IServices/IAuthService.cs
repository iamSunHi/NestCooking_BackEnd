using NESTCOOKING_API.Business.DTOs;

namespace NESTCOOKING_API.Business.Services.IServices
{
    public interface IAuthService
    {
        Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO);
        Task<bool> Register(RegistrationRequestDTO registrationRequestDTO);
        Task<(string, string, string, string)> GenerateResetPasswordToken(string identifier);
        Task<bool> ResetPassword(ResetPasswordRequestDTO resetPasswordRequestDTO);
        Task<bool> VerifyResetPasswordToken(string email, string token);
        Task<string> LoginWithThirdParty(ProviderRequestDTO info);
        Task<bool> VerifyEmailConfirmation(string email, string token);
        Task<(string, string)> GenerateEmailConfirmationTokenAsync(string email);
    }
}
