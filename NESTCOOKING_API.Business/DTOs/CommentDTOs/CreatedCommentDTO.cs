using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NESTCOOKING_API.Business.DTOs.CommentDTOs
{
	public class CreatedCommentDTO
	{
		public string RecipeId { get; set; } = null!;
        public string Content { get; set; } = null!;
		public string Type { get; set; } = null!;
		public string? ParentCommentId { get; set; }
	}
}
