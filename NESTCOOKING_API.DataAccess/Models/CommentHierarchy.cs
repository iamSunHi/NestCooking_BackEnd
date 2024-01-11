using System.ComponentModel.DataAnnotations;

namespace NESTCOOKING_API.DataAccess.Models
{
	public class CommentHierarchy
	{
		[Key]
		public string Id { get; set; } = null!;
		public Comment ParrentComment { get; set; }
		public Comment ChildComment { get; set; }
	}
}
