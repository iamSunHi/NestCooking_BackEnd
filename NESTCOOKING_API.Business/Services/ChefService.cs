using AutoMapper;
using NESTCOOKING_API.Business.DTOs.BookingDTOs;
using NESTCOOKING_API.Business.Services.IServices;
using NESTCOOKING_API.DataAccess.Models;
using NESTCOOKING_API.DataAccess.Repositories.IRepositories;
using NESTCOOKING_API.Utility;

namespace NESTCOOKING_API.Business.Services
{
	public class ChefService : IChefService
	{
		private readonly IMapper _mapper;
		private readonly IChefDishRepository _chefDishRepository;

		public ChefService(IMapper mapper,
			IChefDishRepository chefDishRepository)
		{
			_mapper = mapper;
			_chefDishRepository = chefDishRepository;
		}

		public async Task<IEnumerable<ChefDishDTO>> GetAllChefDishesAsync()
		{
			var chefDishListFromDb = await _chefDishRepository.GetAllAsync();
			return _mapper.Map<IEnumerable<ChefDishDTO>>(chefDishListFromDb);
		}

		public async Task<ChefDishDTO> GetChefDishByIdAsync(string id)
		{
			var chefDishFromDb = await _chefDishRepository.GetAsync(cd => cd.Id == id);
			return _mapper.Map<ChefDishDTO>(chefDishFromDb);
		}

		public async Task<IEnumerable<ChefDishDTO>> GetChefDishByChefIdAsync(string chefId)
		{
			var chefDishListFromDb = await _chefDishRepository.GetAllAsync(cd => cd.ChefId == chefId);
			return _mapper.Map<IEnumerable<ChefDishDTO>>(chefDishListFromDb);
		}

		public async Task CreateChefDishAsync(ChefDishDTO chefDishDTO)
		{
			if (await this.IsDishExist(chefDishDTO.Name))
			{
				throw new Exception(AppString.DishExisted);
			}

			chefDishDTO.Id = Guid.NewGuid().ToString();
			await _chefDishRepository.CreateAsync(_mapper.Map<ChefDish>(chefDishDTO));
		}

		public async Task<ChefDishDTO> UpdateChefDishAsync(ChefDishDTO chefDishDTO)
		{
			if (await this.IsDishExist(chefDishDTO.Name))
			{
				throw new Exception(AppString.DishExisted);
			}

			await _chefDishRepository.UpdateAsync(_mapper.Map<ChefDish>(chefDishDTO));

			var updatedChefDish = await _chefDishRepository.GetAsync(cd => cd.Id == chefDishDTO.Id);
			return _mapper.Map<ChefDishDTO>(updatedChefDish);
		}

		public async Task RemoveChefDishAsync(string id)
		{
			var chefDishFromDb = await _chefDishRepository.GetAsync(cd => cd.Id == id);
			if (chefDishFromDb == null)
			{
				throw new Exception(AppString.DishNotExisted);
			}

			await _chefDishRepository.RemoveAsync(chefDishFromDb);
		}

		private async Task<bool> IsDishExist(string dishName)
		{
			return (await _chefDishRepository.GetAsync(cd => cd.Name.ToLower() == dishName.ToLower())) != null;
		}
	}
}
