namespace NESTCOOKING_API.Business.DTOs.BookingDTOs
{
	public class CreateBookingDTO
    {
        public string ChefId { get; set; }
        public List<Item> InfomationDishes { get; set; } 
        public string Address { get; set; }
        public DateOnly RequiredDate { get; set; }
        public double Total { get; set; }
    }
}
