namespace NESTCOOKING_API.Business.DTOs.BookingDTOs
{
    public class BookingTransactionDTO
    {
        public string UserFullName { get; set; } = null!;
        public double Amount { get; set; }
        public string Description { get; set; } = null!;
    }
}
