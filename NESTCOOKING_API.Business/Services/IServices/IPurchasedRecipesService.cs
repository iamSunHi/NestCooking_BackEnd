using NESTCOOKING_API.Business.DTOs.TransactionDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NESTCOOKING_API.Business.Services.IServices
{
    public interface IPurchasedRecipesService
    {
        public Task CreatePurchasedRecipe(string recipeId, string transactionId, string userId);
        public Task<List<string>> GetPurchasedRecipesByUserId(string userId);
    }
}
