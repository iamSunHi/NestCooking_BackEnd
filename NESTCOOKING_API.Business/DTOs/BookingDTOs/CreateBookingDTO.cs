using NESTCOOKING_API.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NESTCOOKING_API.Business.DTOs.BookingDTOs
{
    public class CreateBookingDTO
    {
        public string ChefId { get; set; }
        public List<Item> InfomationDishes { get; set; } 
        public string Address { get; set; }
        public DateTime RequiredDate { get; set; }
        public double Total { get; set; }
    }
}
