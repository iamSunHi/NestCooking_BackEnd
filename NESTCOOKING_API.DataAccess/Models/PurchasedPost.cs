using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NESTCOOKING_API.DataAccess.Models
{
	public class PurchasedPost
	{
		[Key]
		public int Id { get; set; }
		public User User { get; set; }
		public Post Post { get; set; }
	}
}
