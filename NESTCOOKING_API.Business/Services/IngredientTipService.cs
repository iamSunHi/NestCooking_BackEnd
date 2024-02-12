using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using NESTCOOKING_API.Business.DTOs;
using NESTCOOKING_API.Business.DTOs.RecipeDTOs;
using NESTCOOKING_API.Business.DTOs.UserDTOs;
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

		public async Task<IEnumerable<IngredientTipShortInfoDTO>> GetAllIngredientTipsAsync()
		{
			var ingredientTips = await _ingredientTipRepository.GetAllAsync();
			if (ingredientTips == null)
			{
				return null;
			}
			var tipList = _mapper.Map<IEnumerable<IngredientTipShortInfoDTO>>(ingredientTips);
			for (int i = 0; i < ingredientTips.Count(); i++)
			{
				var userFromDb = await _userRepository.GetAsync(u => u.Id == ingredientTips.ToList()[i].UserId);
				tipList.ToList()[i].User = _mapper.Map<UserShortInfoDTO>(userFromDb);
			}
			return tipList;
		}

		public async Task<IEnumerable<IngredientTipShortInfoDTO>> GetIngredientTipsAsync(PaginationInfoDTO paginationInfo)
		{
			var ingredientTips = await _ingredientTipRepository.GetIngredientTipsWithPaginationAsync(paginationInfo.PageNumber, paginationInfo.PageSize);
			if (ingredientTips == null)
			{
				return null;
			}
			var tipList = _mapper.Map<IEnumerable<IngredientTipShortInfoDTO>>(ingredientTips);
			for (int i = 0; i < ingredientTips.Count(); i++)
			{
				var userFromDb = await _userRepository.GetAsync(u => u.Id == ingredientTips.ToList()[i].UserId);
				tipList.ToList()[i].User = _mapper.Map<UserShortInfoDTO>(userFromDb);
			}
			return tipList;
		}

		public async Task<IngredientTipDTO> GetIngredientTipByIdAsync(string id)
		{
			var ingredientTip = await _ingredientTipRepository.GetAsync(i => i.Id == id);
			if (ingredientTip == null)
			{
				return null;
			}
			var tip = _mapper.Map<IngredientTipDTO>(ingredientTip);
			var userFromDb = await _userRepository.GetAsync(u => u.Id == ingredientTip.UserId);
			tip.User = _mapper.Map<UserShortInfoDTO>(userFromDb);
			var tipContentsFromDb = await _ingredientTipContentRepository.GetAllAsync(c => c.IngredientTipId == ingredientTip.Id);
			tip.Contents = _mapper.Map<IEnumerable<IngredientTipContentDTO>>(tipContentsFromDb);
			return tip;
		}

		public async Task<IngredientTipShortInfoDTO> GetIngredientTipShortInfoByIdAsync(string id)
		{
			var ingredientTip = await _ingredientTipRepository.GetAsync(i => i.Id == id);
			if (ingredientTip == null)
			{
				return null;
			}
			var tip = _mapper.Map<IngredientTipShortInfoDTO>(ingredientTip);
			var userFromDb = await _userRepository.GetAsync(u => u.Id == ingredientTip.UserId);
			tip.User = _mapper.Map<UserShortInfoDTO>(userFromDb);
			return tip;
		}

		public async Task CreateIngredientTipAsync(string userId, IngredientTipDTO ingredientTipDTO)
		{
			var ingredientTip = _mapper.Map<IngredientTip>(ingredientTipDTO);
			ingredientTip.Id = Guid.NewGuid().ToString();
			ingredientTip.UserId = userId;
			ingredientTip.CreatedAt = DateTime.UtcNow;
			await _ingredientTipRepository.CreateAsync(ingredientTip);

			foreach (var content in ingredientTipDTO.Contents)
			{
				var contentForDb = _mapper.Map<IngredientTipContent>(content);
				contentForDb.Id = 0;
				contentForDb.IngredientTipId = ingredientTip.Id;
				await _ingredientTipContentRepository.CreateAsync(contentForDb);
			}
		}

		public async Task UpdateIngredientTipAsync(string userId, IngredientTipDTO ingredientTipDTO)
		{
			var ingredientTipFromDb = await _ingredientTipRepository.GetAsync(i => i.Id == ingredientTipDTO.Id);
			if (ingredientTipFromDb.UserId != userId && await _userRepository.GetRoleAsync(userId) != StaticDetails.Role_Admin)
			{
				throw new Exception("You don't have permission to update this.");
			}

			var ingredientTip = _mapper.Map<IngredientTip>(ingredientTipDTO);
			var contentsFromDb = await _ingredientTipContentRepository.GetAllAsync(c => c.IngredientTipId == ingredientTipDTO.Id);

			if (ingredientTipDTO.Contents.Count() > contentsFromDb.Count())
			{
				for (int i = 0; i < contentsFromDb.Count(); i++)
				{
					var content = _mapper.Map<IngredientTipContent>(ingredientTipDTO.Contents.ToList()[i]);
					content.Id = contentsFromDb.ToList()[i].Id;
					await _ingredientTipContentRepository.UpdateAsync(content);
				}
				for (int i =  contentsFromDb.Count(); i < ingredientTipDTO.Contents.Count(); i++)
				{
					var content = _mapper.Map<IngredientTipContent>(ingredientTipDTO.Contents.ToList()[i]);
					content.Id = 0;
					content.IngredientTipId = ingredientTipFromDb.Id;
					await _ingredientTipContentRepository.CreateAsync(content);
				}
			}
			else
			{
				for (int i = 0; i < ingredientTipDTO.Contents.Count(); i++)
				{
					var content = _mapper.Map<IngredientTipContent>(ingredientTipDTO.Contents.ToList()[i]);
					content.Id = contentsFromDb.ToList()[i].Id;
					await _ingredientTipContentRepository.UpdateAsync(content);
				}
				for (int i = ingredientTipDTO.Contents.Count(); i < contentsFromDb.Count(); i++)
				{
					var content = contentsFromDb.ToList()[i];
					await _ingredientTipContentRepository.RemoveAsync(content);
				}
			}

			await _ingredientTipRepository.UpdateAsync(ingredientTip);
		}

		public async Task DeleteIngredientTipAsync(string userId, string id)
		{
			var ingredientTipFromDb = await _ingredientTipRepository.GetAsync(i => i.Id == id);

			if (ingredientTipFromDb != null)
			{
				if (ingredientTipFromDb.UserId != userId && await _userRepository.GetRoleAsync(userId) != StaticDetails.Role_Admin)
				{
					throw new Exception("You don't have permission to update this.");
				}

				var contentsFromDb = await _ingredientTipContentRepository.GetAllAsync(c => c.IngredientTipId == id);
				foreach (var content in contentsFromDb)
				{
					await _ingredientTipContentRepository.RemoveAsync(content);
				}

				await _ingredientTipRepository.RemoveAsync(ingredientTipFromDb);
			}
		}
	}
}
