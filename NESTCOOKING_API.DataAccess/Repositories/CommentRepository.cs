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
		public CommentRepository(ApplicationDbContext context) : base(context) { }
		public async Task<Comment> UpdateComment(Comment updateComment)
		{
			_context.Update(updateComment);
			await _context.SaveChangesAsync();
			return updateComment;
		}
	}

}
