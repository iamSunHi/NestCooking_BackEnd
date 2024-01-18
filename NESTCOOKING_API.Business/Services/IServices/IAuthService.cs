using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using NESTCOOKING_API.Business.DTOs;
using NESTCOOKING_API.Business.DTOs.EmailDTO;
using NESTCOOKING_API.DataAccess.Models;

namespace NESTCOOKING_API.Business.Services.IServices
{
    public interface IAuthService
    {
        Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO);
        Task<string> Register(RegistrationRequestDTO registrationRequestDTO);
        Task<string> LoginByGoogle(ProviderRequestDTO info);
        Task<string> LoginByFacebook(ProviderRequestDTO info);
        Task<string> GenerateResetPasswordToken(User user);
        Task<IdentityResult> ResetPassword(User user, string resetPasswordToken, string resetPassword);
        Task<bool> VerifyResetPasswordToken(User user, string token);
    }
}
