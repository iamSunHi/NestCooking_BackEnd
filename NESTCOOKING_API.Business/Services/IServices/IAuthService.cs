using NESTCOOKING_API.Business.DTOs;

namespace NESTCOOKING_API.Business.Services.IServices
{
	public interface IAuthService
    {
        Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO);
        Task<string> Register(RegistrationRequestDTO registrationRequestDTO);
		Task<(string, string)> GenerateResetPasswordToken(string userName);
        Task<string> ResetPassword(ResetPasswordRequestDTO resetPasswordRequestDTO);
        Task<bool> VerifyResetPasswordToken(string email, string token);
        Task<string> LoginWithThirdParty(ProviderRequestDTO info);
    }
}
