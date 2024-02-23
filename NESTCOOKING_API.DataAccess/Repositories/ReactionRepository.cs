using Microsoft.EntityFrameworkCore;
using NESTCOOKING_API.DataAccess.Data;
using NESTCOOKING_API.DataAccess.Models;
using NESTCOOKING_API.DataAccess.Repositories.IRepositories;
using NESTCOOKING_API.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NESTCOOKING_API.DataAccess.Repositories
{
    public class ReactionRepository : IReactionRepository
    {
        private readonly ApplicationDbContext _context;

        public ReactionRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(string targetID, StaticDetails.ReactionType reactionType, string userID)
        {
            try
            {
                string checkTarget = "";

                if (await _context.Recipes.AnyAsync(r => r.Id == targetID))
                {
                    checkTarget = "Recipe";
                }
                //else if(await _context.Comments.AnyAsync(r => r.Id == targetID))
                //{
                //    var checkTarget = "Comment";
                //}
                if (String.Equals(checkTarget, "Recipe"))
                {
                    var recipe = await _context.Recipes.FirstOrDefaultAsync(r => r.Id == targetID);
                    var user = await _context.Users.FirstOrDefaultAsync(r => r.Id == userID);
                    var reaction = await _context.Reaction.FirstOrDefaultAsync(r => r.Emoji == reactionType.ToString());
                    var reactionRecipe = new RecipeReaction
                    {
                        Recipe = recipe,
                        User = user,
                        Reaction = reaction,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    };
                    await _context.AddAsync(reactionRecipe);
                    await _context.SaveChangesAsync();
                }
                //else if (String.Equals(checkTarget, "Comment")){
                //    var comment = await _context.Comments.FirstOrDefaultAsync(r => r.Id == targetID);
                //    var user = await _context.Users.FirstOrDefaultAsync(r => r.Id == userID);
                //    var reaction = await _context.Reaction.FirstOrDefaultAsync(r => r.Emoji == reactionType.ToString());
                //    var commentReaction = new CommentReaction
                //    {
                //        Comment = comment,
                //        User=user,  
                //        Reaction = reaction,
                //        CreatedAt = DateTime.UtcNow,
                //        UpdatedAt = DateTime.UtcNow
                //    };
                //    await _context.AddAsync(commentReaction);
                //    await _context.SaveChangesAsync();
                //}
            }catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public async Task DeleteAsync(string targetId, string userId)
        {
            string checkTarget = "";
            if (await _context.Recipes.AnyAsync(r => r.Id == targetId))
            {
                checkTarget = "Recipe";
            }
            //else if(await _context.Comments.AnyAsync(r => r.Id == targetID))
            //{
            //    var checkTarget = "Comment";
            //}
            if (String.Equals(checkTarget, "Recipe"))
            {
                var reactionsToDelete = await _context.RecipeReaction
                    .Where(r => r.User.Id == userId && r.Recipe.Id == targetId)
                    .ToListAsync();
                if (reactionsToDelete.Any())
                {
                    _context.RecipeReaction.RemoveRange(reactionsToDelete);
                    await _context.SaveChangesAsync();
                }
            }

            //else if (String.Equals(checkTarget, "Comment"))
            //{
            //    var reactionsToDelete = await _context.CommentReaction
            //        .Where(r => r.User.Id == userId && r.Comment.Id == targetId)
            //        .ToListAsync();
            //if (reactionsToDelete.Any())
            //{
            //    _context.CommentReaction.RemoveRange(reactionsToDelete);
            //    await _context.SaveChangesAsync();
            //}
            //}
        }
        public async Task UpdateReactionAsync(string targetID, StaticDetails.ReactionType reactionType, string userId)
        {
            try
            {
                string checkTarget = "";

                if (await _context.Recipes.AnyAsync(r => r.Id == targetID))
                {
                    checkTarget = "Recipe";
                }
                //else if(await _context.Comments.AnyAsync(r => r.Id == targetID))
                //{
                //    var checkTarget = "Comment";
                //}
                if (String.Equals(checkTarget, "Recipe"))
                {
                    var reaction = await _context.RecipeReaction
                        .FirstOrDefaultAsync(r => r.User.Id == userId && r.Recipe.Id == targetID);
                    var emoji = await _context.Reaction.FirstOrDefaultAsync(r => r.Emoji == reactionType.ToString());
                    if (reaction != null)
                    {
                        reaction.Reaction = emoji;
                        reaction.UpdatedAt = DateTime.UtcNow;
                        await _context.SaveChangesAsync();
                    }

                }
                //else if (String.Equals(checkTarget, "Comment"))
                //{
                //    var reaction = await _context.CommentReaction
                //        .FirstOrDefaultAsync(r => r.User.Id == userId && r.Comment.Id == targetID);
                //    var emoji = await _context.Reaction.FirstOrDefaultAsync(r => r.Emoji == reactionType.ToString());
                //    if (reaction != null)
                //    {
                //        reaction.Reaction = emoji;
                //        reaction.UpdatedAt = DateTime.UtcNow;
                //        await _context.SaveChangesAsync();
                //    }
                //}
            }catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<Dictionary<string, int>> GetReactionsByIdAsync(string targetId, string type)
        {
            //if (String.Equals(type, "Recipe"))
            //{
            //    var reactions = await _context.RecipeReaction
            //    .Where(rr => rr.Recipe.Id == targetId)
            //    .GroupBy(rr => rr.Reaction.Emoji)
            //    .Select(g => new { Reaction = g.Key, Count = g.Count() })
            //    .ToDictionaryAsync(x => x.Reaction, x => x.Count);
            //    return reactions;
            //}
            //else
            //{
            //    var reactions = await _context.CommentReaction
            //    .Where(rr => rr.Comment.Id == targetId)
            //    .GroupBy(rr => rr.Reaction.Emoji)
            //    .Select(g => new { Reaction = g.Key, Count = g.Count() })
            //    .ToDictionaryAsync(x => x.Reaction, x => x.Count);
            //    return reactions;
            //}

            var reactions = await _context.RecipeReaction
                .Where(rr => rr.Recipe.Id == targetId)
                .GroupBy(rr => rr.Reaction.Emoji)
                .Select(g => new { Reaction = g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.Reaction, x => x.Count);
            return reactions;

        }
    }
}
