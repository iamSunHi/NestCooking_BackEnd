using NESTCOOKING_API.Business.DTOs.ReactionDTOs;
using NESTCOOKING_API.Business.Services.IServices;
using NESTCOOKING_API.DataAccess.Models;
using NESTCOOKING_API.DataAccess.Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NESTCOOKING_API.Business.Services
{
    public class ReactionService : IReactionService
    {
        private readonly IReactionRepository _reactionRepository;

        public ReactionService(IReactionRepository reactionRepository)
        {
            _reactionRepository = reactionRepository;
        }
        public async Task<bool> AddReactionAsync(ReactionDTO reactionDTO, string userId)
        {
            try
            {
                await _reactionRepository.AddAsync(reactionDTO.TargetID, reactionDTO.ReactionType, userId);
                return true;
            } catch(Exception ex)
            {
                return false;
            }
           
        }

        public async Task<bool> DeleteReactionAsync(string targetId, string userId)
        {
            try
            {
                await _reactionRepository.DeleteAsync(targetId, userId);
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }
        public async Task<bool> UpdateReactionAsync(ReactionDTO reactionDTO, string userId)
        {
            try {
                await _reactionRepository.UpdateReactionAsync(reactionDTO.TargetID, reactionDTO.ReactionType, userId);
                return true;
            }catch(Exception ex)
            {
                return false;
            }
            
        }
        public async Task<int> GetTotalReactionsByIdAsync(string targetId)
        {
            return await _reactionRepository.GetTotalReactionsByIdAsync(targetId);
        }
        public async Task<Dictionary<string, int>> GetReactionsByIdAsync(string targetId)
        {
            return await _reactionRepository.GetReactionsByIdAsync(targetId);
        }
    }
}
