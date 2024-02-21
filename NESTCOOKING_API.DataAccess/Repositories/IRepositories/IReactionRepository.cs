using NESTCOOKING_API.DataAccess.Models;
using NESTCOOKING_API.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NESTCOOKING_API.DataAccess.Repositories.IRepositories
{
    public interface IReactionRepository 
    {
        Task AddAsync(string targetID,StaticDetails.ReactionType reactionType,string userId);
        Task DeleteAsync(string targetId, string userId);
        Task UpdateReactionAsync(string targetID, StaticDetails.ReactionType reactionType, string userId);
        Task<int> GetTotalReactionsByIdAsync(string targetId);
        Task<Dictionary<string, int>> GetReactionsByIdAsync(string targetId);
    }
}
