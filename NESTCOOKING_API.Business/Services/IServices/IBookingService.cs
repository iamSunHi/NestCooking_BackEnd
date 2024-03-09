using NESTCOOKING_API.Business.DTOs.BookingDTOs;
using NESTCOOKING_API.Business.DTOs.CommentDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NESTCOOKING_API.Business.Services.IServices
{
    public interface IBookingService
    {
        Task<RequestBookingDTO> CreateBooking(string userId, CreateBookingDTO createBooking);
    }
}
