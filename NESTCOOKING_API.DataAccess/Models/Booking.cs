using System.ComponentModel.DataAnnotations;

namespace NESTCOOKING_API.DataAccess.Models
{
    public class Booking
    {
        [Key]
        public string Id { get; set; } = null!;
        public string UserId { get; set; } = null!;
        public string ChefId { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string? Note { get; set; }
        public DateTime TimeStart { get; set; }
        public DateTime TimeEnd { get; set; }
        public double Total { get; set; }
        public string Status { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public DateTime ApprovalStatusDate { get; set; }

        // One booking can have many transactions. Example: take money of user, give chef money,...
        public required List<string> TransactionIdList { get; set; }
    }
}
