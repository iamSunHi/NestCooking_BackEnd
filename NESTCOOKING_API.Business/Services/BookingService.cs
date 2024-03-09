using AutoMapper;
using Microsoft.AspNetCore.Identity;
using NESTCOOKING_API.Business.DTOs.BookingDTOs;
using NESTCOOKING_API.Business.Services.IServices;
using NESTCOOKING_API.DataAccess.Models;
using NESTCOOKING_API.DataAccess.Repositories.IRepositories;
using NESTCOOKING_API.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NESTCOOKING_API.Business.Services
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IBookingLineRepository _bookingLineRepository;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        public BookingService(IBookingRepository bookingRepository, UserManager<User> userManager, IMapper mapper, IBookingLineRepository lineRepository)
        {
            _bookingRepository = bookingRepository;
            _userManager = userManager;
            _mapper = mapper;
            _bookingLineRepository = lineRepository;

        }
        public async Task<RequestBookingDTO> CreateBooking(string userId, CreateBookingDTO createBooking)
        {
            try
            {
                var existedUser = await _userManager.FindByIdAsync(userId);
                if (existedUser == null)
                {
                    throw new Exception(message: " User Not Found");
                }
                if (createBooking.TimeEnd <= createBooking.TimeStart || createBooking.TimeStart <= DateTime.UtcNow)
                {
                    throw new Exception(message: "TimeEnd must be greater than TimeStart");
                }
                var lastBooking = await _bookingRepository.GetBookingsByChefId(createBooking.ChefId);
                ValidateBookingTime(createBooking, lastBooking);
                if (existedUser.Balance < createBooking.Total)
                {
                    throw new Exception(message: "Insufficient balance to create booking");
                }
                var requestBooking = _mapper.Map<Booking>(createBooking);
                requestBooking.Id = Guid.NewGuid().ToString();
                requestBooking.Status = StaticDetails.ActionStatus_PENDING;
                requestBooking.CreatedAt = DateTime.UtcNow;
                requestBooking.TransactionRef = "Loading...";
                requestBooking.ApprovalStatusDate = DateTime.UtcNow;
                requestBooking.UserId = userId;
                var newBooking = _bookingRepository.CreateAsync(requestBooking);

                // Add Recipe Infomation Into BookingLine

                foreach (var dish in createBooking.InfomationDishes)
                {
                    var bookingLine = new BookingLine
                    {
                        BookingId = requestBooking.Id,
                        RecipeId = dish.RecipeId,
                        Quantity = dish.Quantity,
                    };
                    await _bookingLineRepository.CreateAsync(bookingLine);

                }
                //var result = _mapper.Map<RequestBookingDTO>(newBooking); => missing mapping
                var result = new RequestBookingDTO
                {
                    Id = requestBooking.Id,
                    UserId = userId,
                    ChefId = createBooking.ChefId,
                    Status = requestBooking.Status,
                    Address = createBooking.Address,
                    TransactionRef = requestBooking.TransactionRef,
                    Note = createBooking.Note,
                    CreatedAt = requestBooking.CreatedAt,
                    TimeStart = createBooking.TimeStart,
                    TimeEnd = createBooking.TimeEnd,
                    Total = createBooking.Total,
                    ApprovalStatusDate = requestBooking.ApprovalStatusDate,
                };
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        private void ValidateBookingTime(CreateBookingDTO createBooking, IEnumerable<Booking> lastBookings)
        {
            if (lastBookings != null && lastBookings.Any())
            {
                foreach (var lastBooking in lastBookings)
                {
                    if (createBooking.TimeStart < lastBooking.TimeEnd.AddHours(1))
                    {
                        throw new Exception($"Chef is currently working. The new booking must start at least 1 hour after {lastBooking.TimeEnd}");
                    }
                }
            }
        }

    }
}

