﻿using AutoMapper;
using NESTCOOKING_API.Business.DTOs;
using NESTCOOKING_API.Business.DTOs.AuthDTOs;
using NESTCOOKING_API.Business.DTOs.CategoryDTOs;
using NESTCOOKING_API.Business.DTOs.RecipeDTOs;
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

			// Category
			CreateMap<Category, DTOs.CategoryDTOs.CategoryDTO>().ReverseMap();
			// Recipe
			CreateMap<Recipe, RecipeCreationDTO>().ReverseMap();
			CreateMap<Recipe, RecipeDTO>().ReverseMap();
			CreateMap<Recipe, RecipeDetailDTO>().ReverseMap();
		}
	}
}
