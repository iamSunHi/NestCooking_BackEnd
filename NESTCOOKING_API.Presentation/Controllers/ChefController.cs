using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NESTCOOKING_API.Business.Services.IServices;

namespace NESTCOOKING_API.Presentation.Controllers
{
	[Route("api/chef")]
	[ApiController]
	[Authorize]
	public class ChefController : ControllerBase
	{
		private readonly IChefService _chefService;

		public ChefController(IChefService chefService)
		{
			_chefService = chefService;
		}
	}
}
