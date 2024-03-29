﻿using NESTCOOKING_API.Business.DTOs.ReactionDTOs;
using NESTCOOKING_API.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NESTCOOKING_API.Business.Services.IServices
{
    public interface IReactionService
    {
        Task<bool> AddReactionAsync(ReactionDTO reactionDTO, string userId);
        Task<bool> DeleteReactionAsync(string targetId, string userId, string type);
        Task<bool> UpdateReactionAsync(ReactionDTO reactionDTO, string userId);
        Task<Dictionary<string, List<string>>> GetUserReaction(string userId, string type);
        Task<Dictionary<string, List<string>>> GetReactionsByIdAsync(string targetId, string type);
    }
}
