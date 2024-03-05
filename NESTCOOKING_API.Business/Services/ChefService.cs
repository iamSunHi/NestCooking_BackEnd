using AutoMapper;
using NESTCOOKING_API.Business.Services.IServices;

namespace NESTCOOKING_API.Business.Services
{
	public class ChefService : IChefService
	{
		private readonly IMapper _mapper;

		public ChefService(IMapper mapper)
		{
			_mapper = mapper;
		}
	}
}
