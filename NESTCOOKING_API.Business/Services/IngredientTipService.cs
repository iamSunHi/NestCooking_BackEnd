using AutoMapper;
using NESTCOOKING_API.Business.DTOs;
using NESTCOOKING_API.Business.DTOs.RecipeDTOs;
using NESTCOOKING_API.Business.Services.IServices;
using NESTCOOKING_API.DataAccess.Models;
using NESTCOOKING_API.DataAccess.Repositories.IRepositories;
using NESTCOOKING_API.Utility;

namespace NESTCOOKING_API.Business.Services
{
	public class IngredientTipService : IIngredientTipService
	{
		private readonly IMapper _mapper;
		private readonly IUserRepository _userRepository;
		private readonly IIngredientTipContentRepository _ingredientTipContentRepository;
		private readonly IIngredientTipRepository _ingredientTipRepository;

		public IngredientTipService(IMapper mapper,
			IUserRepository userRepository, IIngredientTipContentRepository ingredientTipContentRepository, IIngredientTipRepository ingredientTipRepository)
		{
			_mapper = mapper;
			_userRepository = userRepository;
			_ingredientTipContentRepository = ingredientTipContentRepository;
			_ingredientTipRepository = ingredientTipRepository;
		}

		public async Task<IEnumerable<IngredientTipDTO>> GetAllIngredientTipsAsync()
		{
			var ingredientTips = await _ingredientTipRepository.GetAllAsync(includeProperties: "User, Contents");
			return _mapper.Map<IEnumerable<IngredientTipDTO>>(ingredientTips);
		}

		public async Task<IEnumerable<IngredientTipDTO>> GetIngredientTipsAsync(PaginationInfoDTO paginationInfo)
		{
			var ingredientTips = await _ingredientTipRepository.GetIngredientTipsWithPaginationAsync(paginationInfo.PageNumber, paginationInfo.PageSize, includeProperties: "User, Contents");
			return _mapper.Map<IEnumerable<IngredientTipDTO>>(ingredientTips);
		}

		public async Task<IngredientTipDTO> GetIngredientTipByIdAsync(int id)
		{
			var ingredientTip = await _ingredientTipRepository.GetAsync(i => i.Id == id, includeProperties: "User, Contents");
			return _mapper.Map<IngredientTipDTO>(ingredientTip);
		}

		public async Task<IngredientTipShortInfoDTO> GetIngredientTipShortInfoByIdAsync(int id)
		{
			var ingredientTip = await _ingredientTipRepository.GetAsync(i => i.Id == id, includeProperties: "User");
			return _mapper.Map<IngredientTipShortInfoDTO>(ingredientTip);
		}

		public async Task CreateIngredientTipAsync(IngredientTipDTO ingredientTipDTO)
		{
			ingredientTipDTO.CreatedAt = DateTime.Now;
			var ingredientTip = _mapper.Map<IngredientTip>(ingredientTipDTO);
			var userFromDb = await _userRepository.GetAsync(u => u.Id == ingredientTipDTO.User.Id);
			ingredientTip.User = userFromDb;

			await _ingredientTipRepository.CreateAsync(ingredientTip);
		}

		public async Task UpdateIngredientTipAsync(IngredientTipDTO ingredientTipDTO)
		{
			var ingredientTipFromDb = await _ingredientTipRepository.GetAsync(i => i.Id == ingredientTipDTO.Id, includeProperties: "User");
			if (ingredientTipFromDb.User.Id != ingredientTipDTO.User.Id && await _userRepository.GetRoleAsync(ingredientTipDTO.User.Id) != StaticDetails.Role_Admin)
			{
				throw new Exception("You don't have permission to update this.");
			}

			ingredientTipDTO.UpdatedAt = DateTime.Now;
			var ingredientTip = _mapper.Map<IngredientTip>(ingredientTipDTO);
			foreach (var ingredientTipContent in ingredientTip.Contents)
			{
				await _ingredientTipContentRepository.UpdateAsync(ingredientTipContent);
			}

			await _ingredientTipRepository.UpdateAsync(ingredientTip);
		}

		public async Task DeleteIngredientTipAsync(int id)
		{
			var ingredientTip = await _ingredientTipRepository.GetAsync(i => i.Id == id, includeProperties: "Contents");
			if (ingredientTip != null)
			{
				foreach (var ingredientTipContent in ingredientTip.Contents)
				{
					await _ingredientTipContentRepository.RemoveAsync(ingredientTipContent);
				}

				await _ingredientTipRepository.RemoveAsync(ingredientTip);
			}
		}
	}
}
