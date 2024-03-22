using NESTCOOKING_API.Business.DTOs.UserDTOs;

namespace NESTCOOKING_API.Business.DTOs.BookingDTOs
{
    public class BookingShortInfoDTO
    {
        public string Id { get; set; } = null!;
		public UserShortInfoDTO User { get; set; }
		public UserShortInfoDTO Chef { get; set; }
        public DateTime TimeStart { get; set; }
        public DateTime TimeEnd { get; set; }
        public double Total { get; set; }
        public string Status { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public DateTime ApprovalStatusDate { get; set; }
        public string PhoneNumber { get; set; } = null!;
    }
}
