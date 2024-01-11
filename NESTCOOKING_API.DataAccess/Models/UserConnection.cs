using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NESTCOOKING_API.DataAccess.Models
{
	public class UserConnection
	{
		[Key]
		public int Id { get; set; }
		public User User { get; set; }
		public User Follower { get; set; }
	}
}
