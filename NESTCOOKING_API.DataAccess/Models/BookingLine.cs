namespace NESTCOOKING_API.DataAccess.Models
{
    public class BookingLine
    {
        public string BookingId { get; set; } = null!;
        public string RecipeId { get; set; } = null!;
        public int Quantity { get; set; }
        public string? Note { get; set; }
    }
}
