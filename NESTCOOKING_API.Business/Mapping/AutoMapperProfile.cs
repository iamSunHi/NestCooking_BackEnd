﻿using AutoMapper;
using NESTCOOKING_API.Business.DTOs;
using NESTCOOKING_API.Business.DTOs.AuthDTOs;
using NESTCOOKING_API.Business.DTOs.ChefRequestDTOs;
using NESTCOOKING_API.Business.DTOs.CommentDTOs;
using NESTCOOKING_API.Business.DTOs.RecipeDTOs;
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
			// User
			CreateMap<User, RegistrationRequestDTO>().ReverseMap();
			CreateMap<User, LoginWithThirdPartyRequestDTO>().ReverseMap();
			CreateMap<User, UserInfoDTO>().ReverseMap();
			CreateMap<User, UserDTO>().ReverseMap();
			CreateMap<User, UserInfoDTO>().ReverseMap();
			CreateMap<User, UpdateUserDTO>().ReverseMap();
			CreateMap<User, UserShortInfoDTO>().ReverseMap();

			// Request to become chef
			CreateMap<RequestToBecomeChef, CreatedRequestToBecomeChefDTO>().ReverseMap();
			CreateMap<RequestToBecomeChef, RequestToBecomeChefDTO>().ReverseMap();
			// Comment
			CreateMap<Comment, CreatedCommentDTO>().ReverseMap();
			CreateMap<Comment, RequestCommentDTO>().ReverseMap();
			// Report
			CreateMap<Report, ReportResponseDTO>().ReverseMap().ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User));
			CreateMap<Report, CreateReportDTO>().ReverseMap();
			CreateMap<Report, UpdateReportDTO>().ReverseMap();
			CreateMap<Response, AdminResponseDTO>().ReverseMap().ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User));

			// Recipe
			CreateMap<Category, CategoryDTO>().ReverseMap();
			CreateMap<IngredientTipContent, IngredientTipContentDTO>().ReverseMap();
			CreateMap<IngredientTip, IngredientTipDTO>().ReverseMap();
			CreateMap<IngredientTip, IngredientTipShortInfoDTO>().ReverseMap();
			CreateMap<Ingredient, IngredientDTO>().ReverseMap();
			CreateMap<Recipe, RecipeDTO>().ReverseMap();
			CreateMap<Instructor, InstructorDTO>().ReverseMap();
			CreateMap<Recipe, RecipeDetailDTO>().ReverseMap();
		}
	}
}
