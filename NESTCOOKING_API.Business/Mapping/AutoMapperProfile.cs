using AutoMapper;
using NESTCOOKING_API.Business.DTOs;
using NESTCOOKING_API.DataAccess.Models;

namespace NESTCOOKING_API.Business.Mapping
{
	public class AutoMapperProfile : Profile
	{
		public AutoMapperProfile()
		{
			CreateMap<User, UserDTO>().ReverseMap();
			CreateMap<User, RegistrationRequestDTO>().ReverseMap();
            CreateMap<User, GoogleRequestDTO>().ReverseMap();

        }
    }
}
