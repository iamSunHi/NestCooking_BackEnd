using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NESTCOOKING_API.Business.DTOs.BookingDTOs
{
    public class RequestBookingDTO
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string ChefId { get; set; }
        public string? Status { get; set; }
        public string Address { get; set; }
        public string TransactionRef { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime TimeStart { get; set; }
        public DateTime TimeEnd { get; set; }
        public string Note { get; set; }
        public double Total { get; set; }
        public DateTime ApprovalStatusDate { get; set; }
    }
}
