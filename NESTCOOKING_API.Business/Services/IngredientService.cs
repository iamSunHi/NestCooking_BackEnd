using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using NESTCOOKING_API.Business.DTOs;
using NESTCOOKING_API.Business.DTOs.RecipeDTOs;
using NESTCOOKING_API.Business.Services.IServices;
using NESTCOOKING_API.DataAccess.Models;
using NESTCOOKING_API.DataAccess.Repositories;
using NESTCOOKING_API.DataAccess.Repositories.IRepositories;

namespace NESTCOOKING_API.Business.Services
{
	public class IngredientService : IIngredientService
	{
		private readonly IMapper _mapper;
		private readonly IIngredientTipService _ingredientTipService;
		private readonly IIngredientRepository _ingredientRepository;

		public IngredientService(IMapper mapper, IIngredientTipService ingredientTipService, IIngredientRepository ingredientRepository)
		{
			_mapper = mapper;
			_ingredientTipService = ingredientTipService;
			_ingredientRepository = ingredientRepository;
		}

		public async Task<IEnumerable<IngredientDTO>> GetAllIngredientsAsync()
		{
			var ingredientsFromDb = (await _ingredientRepository.GetAllAsync()).ToList();
			var ingredients = _mapper.Map<List<IngredientDTO>>(ingredientsFromDb);
			for (int i = 0; i < ingredientsFromDb.Count(); i++)
			{
				if (!string.IsNullOrEmpty(ingredientsFromDb[i].IngredientTipId))
					ingredients[i].IngredientTip = await _ingredientTipService.GetIngredientTipShortInfoByIdAsync(ingredientsFromDb[i].IngredientTipId);
			}
			return ingredients;
		}

		public async Task<IngredientDTO> GetIngredientByIdAsync(int id)
		{
			var ingredientsFromDb = await _ingredientRepository.GetAsync(i => i.Id == id);
			var ingredient = _mapper.Map<IngredientDTO>(ingredientsFromDb);
			if (!string.IsNullOrEmpty(ingredientsFromDb.IngredientTipId))
				ingredient.IngredientTip = await _ingredientTipService.GetIngredientTipShortInfoByIdAsync(ingredientsFromDb.IngredientTipId);
			return ingredient;
		}

		public async Task CreateIngredientAsync(IngredientDTO ingredientDTO)
		{
			var ingredient = _mapper.Map<Ingredient>(ingredientDTO);
			if (ingredientDTO.IngredientTip != null)
			{
				ingredient.IngredientTipId = ingredientDTO.IngredientTip.Id;
			}
			await _ingredientRepository.CreateAsync(ingredient);
		}

		public async Task UpdateIngredientAsync(IngredientDTO ingredientDTO)
		{
			var ingredient = _mapper.Map<Ingredient>(ingredientDTO);
			if (ingredientDTO.IngredientTip != null)
			{
				ingredient.IngredientTipId = ingredientDTO.IngredientTip.Id;
			}
			await _ingredientRepository.UpdateAsync(ingredient);
			await _ingredientRepository.SaveAsync();
		}

		public async Task DeleteIngredientAsync(int id)
		{
			var ingredientFromDb = await _ingredientRepository.GetAsync(i => i.Id == id);
			if (ingredientFromDb != null)
			{
				await _ingredientRepository.RemoveAsync(ingredientFromDb);
			}
		}
	}
}
