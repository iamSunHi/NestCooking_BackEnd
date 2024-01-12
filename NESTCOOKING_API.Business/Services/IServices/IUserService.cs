using NESTCOOKING_API.Business.DTOs;

namespace NESTCOOKING_API.Business.Services.IServices
{
	public interface IUserService
	{
		Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO);
		Task<string> Register(RegistrationRequestDTO registrationRequestDTO);
	}
}
