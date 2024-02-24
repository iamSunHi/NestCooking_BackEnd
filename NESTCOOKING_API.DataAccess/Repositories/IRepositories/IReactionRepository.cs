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
        Task AddAsync(string targetID,StaticDetails.ReactionType reactionType,string type,string userId);
        Task DeleteAsync(string targetId, string userId, string type);
        Task UpdateReactionAsync(string targetID, StaticDetails.ReactionType reactionType,string type, string userId);
        Task<Dictionary<string, int>> GetReactionsByIdAsync(string targetId, string type);
    }
}
