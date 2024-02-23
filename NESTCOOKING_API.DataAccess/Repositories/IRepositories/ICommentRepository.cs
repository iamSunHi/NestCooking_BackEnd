using NESTCOOKING_API.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NESTCOOKING_API.DataAccess.Repositories.IRepositories
{
	public interface ICommentRepository : IRepository<Comment>
	{
		Task<Comment> CreateComment(Comment createComment);
		Task<Comment> GetCommentById(string commentId);
		Task<IEnumerable<Comment>> GetAllComments();
		Task<Comment> UpdateComment(Comment updateComment);
		Task<bool> DeleteComment(string commentId);
	}
}
