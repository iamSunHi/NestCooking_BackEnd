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
                await _reactionRepository.AddAsync(reactionDTO.TargetID, reactionDTO.Reaction, reactionDTO.Type, userId);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<bool> DeleteReactionAsync(string targetId, string userId, string type)
        {
            try
            {
                await _reactionRepository.DeleteAsync(targetId, userId, type);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<bool> UpdateReactionAsync(ReactionDTO reactionDTO, string userId)
        {
            try
            {
                await _reactionRepository.UpdateReactionAsync(reactionDTO.TargetID, reactionDTO.Reaction, reactionDTO.Type, userId);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<Dictionary<string, List<string>>> GetReactionsByIdAsync(string targetId, string type)
        {
            try
            {
                return await _reactionRepository.GetReactionsByIdAsync(targetId, type);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Dictionary<string, List<string>>> GetUserReaction(string userId, string type)
        {
            try
            {
                var userIdList = await _reactionRepository.GetUserReaction(userId, type);
                return userIdList;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
