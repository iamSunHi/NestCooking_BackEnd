using NESTCOOKING_API.Business.DTOs.BookingDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NESTCOOKING_API.Business.Services.IServices
{
    public interface IBookingService
    {
        Task<RequestBookingDTO> CreateBooking(string userId, CreateBookingDTO createBookingDTO);
        Task<IEnumerable<RequestBookingDTO>> GetAllBookingRequests();
        Task<RequestBookingDTO> ApprovalBooking(string bookingId,ChefApprovalProcessDTO chefApprovalProcessDTO);
        Task DeleteBookingRequest(string userId, string bookingId);
        Task<RequestBookingDTO> GetBookingRequest(string bookingId);
        Task<bool> CheckTypeStatus(string status);
    }
}
