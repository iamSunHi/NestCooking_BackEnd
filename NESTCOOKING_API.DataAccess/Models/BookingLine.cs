using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NESTCOOKING_API.DataAccess.Models
{
	public class BookingLine
	{
		public string BookingId { get; set; }
		public Booking Booking { get; set; }
		public string RecipeId { get; set; }
		public Recipe Recipe { get; set; }
		public int UnitPrice { get; set; }
		public int Quantity { get; set; }
		public double TotalPriceOfDishes { get; set; }
	}
}
