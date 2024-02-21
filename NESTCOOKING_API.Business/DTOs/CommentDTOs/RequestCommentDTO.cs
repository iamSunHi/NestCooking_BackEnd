using NESTCOOKING_API.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NESTCOOKING_API.Business.DTOs.CommentDTOs
{
	public class RequestCommentDTO
	{
		public string CommentId { get; set; }
		public string UserId { get; set; }
		public string RecipeId { get; set; } 
		public string Type { get; set; }
		public string Content { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime UpdatedAt { get; set; }
		public string? ParentCommentId { get; set; }
	}
}
