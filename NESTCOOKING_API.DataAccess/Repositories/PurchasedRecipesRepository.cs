using NESTCOOKING_API.DataAccess.Data;
using NESTCOOKING_API.DataAccess.Models;
using NESTCOOKING_API.DataAccess.Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NESTCOOKING_API.DataAccess.Repositories
{
    public class PurchasedRecipesRepository : Repository<PurchasedRecipe>, IPurchasedRecipesRepository
    {
        public PurchasedRecipesRepository(ApplicationDbContext context) : base(context)
        {
        }

        public List<string> GetPurchasedRecipesByUserId(string userId)
        {
            try
            {
                return  _context.PurchasedRecipes
              .Where(pr => pr.UserId == userId)
              .Select(pr => pr.RecipeId)
              .ToList();
            }catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }          
        }
    }
}
