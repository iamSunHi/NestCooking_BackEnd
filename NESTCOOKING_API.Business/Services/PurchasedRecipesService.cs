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
        private readonly ITransactionRepository _transactionRepository;
        public PurchasedRecipesService(IPurchasedRecipesRepository purchasedRecipesRepository, ITransactionRepository transactionRepository)
        {
            _purchasedRecipesRepository = purchasedRecipesRepository;
            _transactionRepository = transactionRepository;
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

        public async Task DeletePurchaseByTransactionId(string transactionId)
        {
            try
            {
                var purchaseRecipe = await _purchasedRecipesRepository.GetAsync(p => p.TransactionId == transactionId);
                await _purchasedRecipesRepository.RemoveAsync(purchaseRecipe);
                await _purchasedRecipesRepository.SaveAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<string> FindIdRecipeByTransactionId(string transactionId)
        {
            try
            {
                var purchaseRecipe = await _purchasedRecipesRepository.GetAsync(p => p.TransactionId == transactionId);
                return purchaseRecipe.RecipeId;
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
                var listRecipe = await _purchasedRecipesRepository.GetAllAsync(r => r.UserId == userId);

                var listRecipeId = new List<string>();

                foreach (var recipe in listRecipe)
                {
                    var transactionId = recipe.TransactionId;
                    var transaction = await _transactionRepository.GetAsync(t => t.Id == transactionId);
                    if (transaction.IsSuccess)
                    {
                        listRecipeId.Add(recipe.RecipeId);
                    }
                }

                return listRecipeId;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
