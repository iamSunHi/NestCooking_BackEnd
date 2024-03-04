using AutoMapper;
using NESTCOOKING_API.Business.DTOs.TransactionDTOs;
using NESTCOOKING_API.Business.Services.IServices;
using NESTCOOKING_API.DataAccess.Models;
using NESTCOOKING_API.DataAccess.Repositories;
using NESTCOOKING_API.DataAccess.Repositories.IRepositories;
using NESTCOOKING_API.Utility;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NESTCOOKING_API.Business.Services
{
    public class PurchasedRecipesService : IPurchasedRecipesService
    {
        private readonly IPurchasedRecipesRepository _purchasedRecipesRepository;
        public PurchasedRecipesService(IPurchasedRecipesRepository purchasedRecipesRepository)
        {
            _purchasedRecipesRepository = purchasedRecipesRepository;
        }
        public async Task CreatePurchasedRecipe(string recipeId, string transactionId, string userId)
        {
            try
            {
                var purchasedRecipe = new PurchasedRecipe
                {
                    Id = Guid.NewGuid().ToString(),
                    UserId = userId,
                    RecipeId = recipeId,
                    TransactionId = transactionId
                };
                await _purchasedRecipesRepository.CreateAsync(purchasedRecipe);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<List<string>> GetPurchasedRecipesByUserId(string userId)
        {
            try
            {
                var recipeIdList =  _purchasedRecipesRepository.GetPurchasedRecipesByUserId(userId);
                return recipeIdList;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
