using Microsoft.EntityFrameworkCore;
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
	public class CommentRepository : Repository<Comment>, ICommentRepository
	{
		private readonly ApplicationDbContext _context;

		public CommentRepository(ApplicationDbContext context) : base(context)
		{

			_context = context;
		}

		public async Task<Comment> CreateComment(Comment createComment)
		{
			_context.Comments.Add(createComment);
			await _context.SaveChangesAsync();
			return createComment;
		}

		public async Task<bool> DeleteComment(string commentId)
		{
			var commentIdToDelete = await GetCommentById(commentId);
			if (commentIdToDelete != null)
			{
				_context.Comments.Remove(commentIdToDelete);
				await _context.SaveChangesAsync();
				return true;
			}
			return false;
		}

		public async Task<IEnumerable<Comment>> GetAllComments()
		{
			var comments = await _context.Comments.ToListAsync();
			return comments;
		}

		public async Task<Comment> GetCommentById(string commentId)
		{
			var comment = await _context.Comments.FindAsync(commentId);
			return comment;
		}

		public async Task<Comment> UpdateComment(Comment updateComment)
		{
			_context.Update(updateComment);
			await _context.SaveChangesAsync();
			return updateComment;
		}
	}

}
