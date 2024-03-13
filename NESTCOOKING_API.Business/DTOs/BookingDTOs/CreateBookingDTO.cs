namespace NESTCOOKING_API.Business.DTOs.BookingDTOs
{
    public class CreateBookingDTO
    {
        public string ChefId { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string? Note { get; set; }
        public required List<BookingDishDTO> BookingDishes { get; set; }
        public DateTime TimeStart { get; set; }
        public DateTime TimeEnd { get; set; }
        public double Total { get; set; }
    }
}
