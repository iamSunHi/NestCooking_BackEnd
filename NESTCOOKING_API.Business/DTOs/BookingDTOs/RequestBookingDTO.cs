using NESTCOOKING_API.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
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
        public string TransactionId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime RequiredDate { get; set; }
        public List<string> InfomationDishes { get; set; }
        public double Total { get; set; }
        public DateTime ApprovalStatusDate { get; set; }
       
    }
}
