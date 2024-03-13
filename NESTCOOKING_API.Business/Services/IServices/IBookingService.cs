using NESTCOOKING_API.Business.DTOs.BookingDTOs;
using NESTCOOKING_API.Business.DTOs.UserDTOs;

namespace NESTCOOKING_API.Business.Services.IServices
{
    public interface IBookingService
    {
        Task<List<UserShortInfoDTO>> GetAllChefsAsync(string? city = null);
        Task<IEnumerable<BookingShortInfoDTO>> GetAllBookingsByUserIdAsync(string userId);
        Task<IEnumerable<BookingShortInfoDTO>> GetAllBookingsByChefIdAsync(string chefId);
        Task<BookingDetailDTO> GetBookingByIdAsync(string bookingId);
        Task<BookingDetailDTO> CreateBooking(string userId, CreateBookingDTO createBooking);
        Task<BookingDetailDTO> UpdateBookingStatus(string userId, BookingStatusDTO status);
    }
}
