using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NESTCOOKING_API.Business.DTOs;
using NESTCOOKING_API.Business.Services.IServices;
using System.Net;

namespace NESTCOOKING_API.Presentation.Controllers
{
	[Route("api/user")]
	[ApiController]
	public class UserController : ControllerBase
	{
		protected ResponseDTO _responseDTO;
		private readonly IUserService _userService;

		public UserController(IUserService userService)
		{
			this._responseDTO = new ResponseDTO();
			_userService = userService;
		}
	}
}
