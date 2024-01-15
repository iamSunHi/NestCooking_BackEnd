using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using NESTCOOKING_API.Business.DTOs;

namespace NESTCOOKING_API.Business.Services.IServices
{
    public interface IAuthService
    {
        Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO);
        Task<string> Register(RegistrationRequestDTO registrationRequestDTO);
        Task<string> LoginByGoogle(GoogleRequestDTO info);
        Task<string> LoginByFacebook(FacebookRequestDTO info);
    }
}
