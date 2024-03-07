using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NESTCOOKING_API.Business.DTOs.BookingDTOs
{
    public class Item
    {
        public string RecipeId  { get; set; }
        public double Price { get; set; }
        public int Portion { get; set; }

    }
}
