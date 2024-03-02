using Microsoft.EntityFrameworkCore;
using NESTCOOKING_API.DataAccess.Data;
using NESTCOOKING_API.DataAccess.Models;
using NESTCOOKING_API.DataAccess.Repositories.IRepositories;
using NESTCOOKING_API.Utility;

namespace NESTCOOKING_API.DataAccess.Repositories
{
	public class ReactionRepository : IReactionRepository
	{
		private readonly ApplicationDbContext _context;

		public ReactionRepository(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task AddAsync(string targetID, StaticDetails.ReactionType reaction, string type, string userID)
		{
			try
			{
				if (String.Equals(type, "recipe"))
				{
					var recipe = await _context.Recipes.FirstOrDefaultAsync(r => r.Id == targetID);
					var user = await _context.Users.FirstOrDefaultAsync(r => r.Id == userID);
					var emoji = await _context.Reactions.FirstOrDefaultAsync(r => r.Emoji == reaction.ToString());
					var reactionRecipe = new RecipeReaction
					{
						Recipe = recipe,
						User = user,
						Reaction = emoji,
						CreatedAt = DateTime.UtcNow,
						UpdatedAt = DateTime.UtcNow
					};
					await _context.AddAsync(reactionRecipe);
					await _context.SaveChangesAsync();
				}
				else if (String.Equals(type, "comment"))
				{
					var comment = await _context.Comments.FirstOrDefaultAsync(c => c.CommentId == targetID);
					var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userID);
					var emoji = await _context.Reactions.FirstOrDefaultAsync(r => r.Emoji == reaction.ToString());
					var commentReaction = new CommentReaction
					{
						Comment = comment,
						User = user,
						Reaction = emoji,
						CreatedAt = DateTime.UtcNow,
						UpdatedAt = DateTime.UtcNow
					};
					await _context.AddAsync(commentReaction);
					await _context.SaveChangesAsync();
				}
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		public async Task DeleteAsync(string targetId, string userId, string type)
		{
			if (String.Equals(type, "recipe"))
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
			else if (String.Equals(type, "comment"))
			{
				var reactionsToDelete = await _context.CommentReaction
					.Where(r => r.User.Id == userId && r.Comment.CommentId == targetId)
					.ToListAsync();
				if (reactionsToDelete.Any())
				{
					_context.CommentReaction.RemoveRange(reactionsToDelete);
					await _context.SaveChangesAsync();
				}
			}
		}
		public async Task DeleteAsync(string targetId, string type)
		{
			if (string.Equals(type, "recipe"))
			{
				var reactionsToDelete = await _context.RecipeReaction
					.Where(r => r.Recipe.Id == targetId)
					.ToListAsync();
				if (reactionsToDelete.Any())
				{
					foreach (var reaction in reactionsToDelete)
					{
						_context.RecipeReaction.Remove(reaction);
						await _context.SaveChangesAsync();
					}
				}
			}
			else if (string.Equals(type, "comment"))
			{
				var reactionsToDelete = await _context.CommentReaction
					.Where(r => r.Comment.CommentId == targetId)
					.ToListAsync();
				if (reactionsToDelete.Any())
				{
					foreach (var reaction in reactionsToDelete)
					{
						_context.CommentReaction.Remove(reaction);
						await _context.SaveChangesAsync();
					}
				}
			}
			else
			{
				throw new Exception("Type is not valid!");
			}
		}
		public async Task UpdateReactionAsync(string targetID, StaticDetails.ReactionType reaction, string type, string userId)
		{
			try
			{
				if (String.Equals(type, "recipe"))
				{
					var reactionRecipe = await _context.RecipeReaction
						.FirstOrDefaultAsync(r => r.User.Id == userId && r.Recipe.Id == targetID);
					var emoji = await _context.Reactions.FirstOrDefaultAsync(r => r.Emoji == reaction.ToString());
					if (reactionRecipe != null)
					{
						reactionRecipe.Reaction = emoji;
						reactionRecipe.UpdatedAt = DateTime.UtcNow;
						await _context.SaveChangesAsync();
					}
				}
				else if (String.Equals(type, "comment"))
				{
					var reactionComment = await _context.CommentReaction
						.FirstOrDefaultAsync(r => r.User.Id == userId && r.Comment.CommentId == targetID);
					var emoji = await _context.Reactions.FirstOrDefaultAsync(r => r.Emoji == reaction.ToString());
					if (reactionComment != null)
					{
						reactionComment.Reaction = emoji;
						reactionComment.UpdatedAt = DateTime.UtcNow;
						await _context.SaveChangesAsync();
					}
				}
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}
		public async Task<Dictionary<string, int>> GetReactionsByIdAsync(string targetId, string type)
		{
			Dictionary<string, int>? reactions = null;
			if (String.Equals(type, "recipe"))
			{
				reactions = await _context.RecipeReaction
				.Where(rr => rr.Recipe.Id == targetId)
				.GroupBy(rr => rr.Reaction.Emoji)
				.Select(g => new { Reaction = g.Key, Count = g.Count() })
				.ToDictionaryAsync(x => x.Reaction, x => x.Count);
			}
			else
			{
				reactions = await _context.CommentReaction
				.Where(rr => rr.Comment.CommentId == targetId)
				.GroupBy(rr => rr.Reaction.Emoji)
				.Select(g => new { Reaction = g.Key, Count = g.Count() })
				.ToDictionaryAsync(x => x.Reaction, x => x.Count);
			}
			return reactions;
		}

        public async Task<List<string>> GetReactionUserById(string targetId, string type)
        {
			try
			{
                List<string> userIdList = null;
                if (String.Equals(type, "recipe")) {
                    userIdList = await _context.RecipeReaction
                .Where(rr => rr.RecipeId == targetId)
                .Select(rr => rr.UserId).Distinct()
                .ToListAsync();
                }
                else {
                    userIdList = await _context.CommentReaction
               .Where(rr => rr.CommentId == targetId)
               .Select(rr => rr.UserId).Distinct()
               .ToListAsync();
                }
                return userIdList;
            }catch(Exception ex)
			{
				throw new Exception(ex.Message);
			}
        }
    }
}
