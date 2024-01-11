using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NESTCOOKING_API.DataAccess.Models
{
	public class Reaction
	{
		[Key]
		public string Id { get; set; } = null!;
		public string Emoji { get; set; } = null!;
		public DateTime CreatedAt { get; set; }
		public DateTime UpdatedAt { get; set; }

		public IEnumerable<Post> Posts { get; set; }
	}
}
