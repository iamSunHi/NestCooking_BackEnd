using AutoMapper;
using NESTCOOKING_API.Business.DTOs;
using NESTCOOKING_API.Business.DTOs.AuthDTOs;
using NESTCOOKING_API.Business.DTOs.ChefRequestDTOs;
using NESTCOOKING_API.Business.DTOs.UserDTOs;
using NESTCOOKING_API.DataAccess.Models;

namespace NESTCOOKING_API.Business.Mapping
{
	public class AutoMapperProfile : Profile
	{
		public AutoMapperProfile()
		{
			CreateMap<User, UserDTO>().ReverseMap();
			CreateMap<User, RegistrationRequestDTO>().ReverseMap();
			CreateMap<User, LoginWithThirdPartyRequestDTO>().ReverseMap();
			CreateMap<User, UserInfoDTO>().ReverseMap();
			CreateMap<RequestToBecomeChef, RequestToBecomeChefDTO>().ReverseMap();
		}
	}
}
