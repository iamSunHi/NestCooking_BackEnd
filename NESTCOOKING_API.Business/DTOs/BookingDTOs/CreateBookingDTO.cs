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
        public string Address { get; set; }
        public List<ItemDTO> InfomationDishes { get; set; }
        public DateTime TimeStart { get; set; }
        public DateTime TimeEnd { get; set; }
        public string Note { get; set; }
        public double Total { get; set; }
        //public string TransactionRef { get; set; }
    }
}
