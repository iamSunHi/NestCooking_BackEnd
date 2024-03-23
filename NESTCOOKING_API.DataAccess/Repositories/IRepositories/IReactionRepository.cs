using NESTCOOKING_API.DataAccess.Models;
using NESTCOOKING_API.Utility;

namespace NESTCOOKING_API.DataAccess.Repositories.IRepositories
{
    public interface IReactionRepository
    {
        Task AddAsync(string targetID, StaticDetails.ReactionType reaction, string type, string userId);
        Task DeleteAsync(string targetId, string userId, string type);
        Task DeleteAsync(string targetId, string type);
        Task UpdateReactionAsync(string targetID, StaticDetails.ReactionType reaction, string type, string userId);
        Task<Dictionary<string, List<string>>> GetReactionsByIdAsync(string targetId, string type);
        Task<Dictionary<string, List<string>>> GetUserReaction(string userId, string type);
    }
}
