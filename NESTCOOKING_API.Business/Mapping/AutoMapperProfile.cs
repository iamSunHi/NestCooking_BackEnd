using AutoMapper;
using NESTCOOKING_API.Business.DTOs;
using NESTCOOKING_API.Business.DTOs.AuthDTOs;
using NESTCOOKING_API.Business.DTOs.ReportDTOs;
using NESTCOOKING_API.Business.DTOs.ResponseDTOs;
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
			CreateMap<Report, ReportResponsDTO>().ReverseMap()
			.ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User));	
			CreateMap<Response, AdminResponseDTO>().ReverseMap()
				.ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User));
        }
	}
}
