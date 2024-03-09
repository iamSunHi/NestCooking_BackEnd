using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NESTCOOKING_API.DataAccess.Models
{
	public class Booking
	{
        [Key]
        public string Id { get; set; } = null!;
        public string UserId { get; set; } = null!;
        [ForeignKey("UserId")]
        public User User { get; set; }
        public string ChefId { get; set; } = null!;
        [ForeignKey("ChefId")]
        public User Chef { get; set; }
        public string? Status { get; set; }
        public string Address { get; set; } = null!;
        public string TransactionRef { get; set; }
        //public Transaction Payment { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime TimeStart { get; set; }
        public DateTime TimeEnd { get; set; }
        public string Note { get; set; }
        public double Total { get; set; }
        public DateTime ApprovalStatusDate { get; set; }
        public ICollection<BookingLine> BookingLines { get; set; }
    }
}
