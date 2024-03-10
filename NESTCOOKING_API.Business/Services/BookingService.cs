using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.Identity.Client;
using NESTCOOKING_API.Business.DTOs.BookingDTOs;
using NESTCOOKING_API.Business.DTOs.CommentDTOs;
using NESTCOOKING_API.Business.Services.IServices;
using NESTCOOKING_API.DataAccess.Models;
using NESTCOOKING_API.DataAccess.Repositories;
using NESTCOOKING_API.DataAccess.Repositories.IRepositories;
using NESTCOOKING_API.Utility;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace NESTCOOKING_API.Business.Services
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IBookingLineRepository _bookingLineRepository;
        private readonly UserManager<User> _userManager;
        private readonly IRoleRepository _roleRepository;
        private readonly IMapper _mapper;
        public BookingService(IBookingRepository bookingRepository, UserManager<User> userManager, IMapper mapper, IBookingLineRepository bookingLineRepository, IRoleRepository roleRepository)
        {
            _bookingRepository = bookingRepository;
            _userManager = userManager;
            _mapper = mapper;
            _bookingLineRepository = bookingLineRepository;
            _roleRepository = roleRepository;
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
                if(createBooking.TimeEnd <= createBooking.TimeStart.AddHours(1))
                {
                    throw new Exception("Please ensure that the time interval between 'Time Start' and 'Time End' is at least 1 hour when booking a chef.");
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

                //  minus the total booking amount
                existedUser.Balance -= createBooking.Total;
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

        public async Task<IEnumerable<RequestBookingDTO>> GetAllBookings()
        {
            var result = _mapper.Map<IEnumerable<RequestBookingDTO>>(await _bookingRepository.GetAllAsync());
            return result;
        }

        public async Task<RequestBookingDTO> GetBooking(string bookingId)
        {
            var result = _mapper.Map<RequestBookingDTO>(await _bookingRepository.GetAsync(b => b.Id == bookingId));
            return result;
        }

        public async Task<RequestBookingDTO> UpdateBookingStatus(string userId, BookingStatusDTO status)
        {
            try
            {
                var existUser = await _userManager.FindByIdAsync(userId);
                var existBooking = await _bookingRepository.GetAsync(b => b.Id == status.Id);

                if (existUser == null)
                {
                    throw new Exception("User not found");
                }

                if (existBooking == null)
                {
                    throw new Exception("Booking request not found");
                }

                if (status.Status != StaticDetails.ActionStatus_ACCEPTED && status.Status != StaticDetails.ActionStatus_REJECTED)
                {
                    throw new Exception("Invalid status type");
                }

                if (existUser != null && await _roleRepository.GetRoleNameByIdAsync(userId) == "chef")
                {
                    // Allow chef to approve or reject
                    if (existBooking.Status == StaticDetails.ActionStatus_PENDING)
                    {
                        ProcessBookingStatus(existBooking, status);
                    }
                    throw new Exception("Chef Only Allow Acept or Reject");
                }
                else
                {
                    // Allow user to approve, reject, or complete
                    if (status.Status == StaticDetails.ActionStatus_COMPLETED || status.Status == StaticDetails.ActionStatus_REJECTED)
                    {
                        if (status.Status == StaticDetails.ActionStatus_COMPLETED)
                        {
                            if (existBooking.Status == StaticDetails.ActionStatus_ACCEPTED)
                            {
                                var adminRoleId = await _roleRepository.GetRoleIdByNameAsync("admin");
                                var admin = await _userManager.FindByIdAsync(adminRoleId);
                                var chef = await _bookingRepository.GetChefByBookingId(existBooking.Id);

                                if (admin == null || chef == null)
                                {
                                    throw new Exception("Admin or chef not found");
                                }
                                // add 10% of the total booking amount to admin's wallet
                                admin.Balance += existBooking.Total * 0.1;
                                // add 90% of the total booking amount to chef's wallet
                                chef.Balance += existBooking.Total * 0.9;
                                ProcessBookingStatus(existBooking, status);
                            }
                            throw new Exception("Unavailable Completed Booking Is Pending Status");
                        }
                        if (status.Status == StaticDetails.ActionStatus_REJECTED)
                        {
                            if (existBooking.Status == StaticDetails.ActionStatus_PENDING)
                            {
                                if (existBooking.CreatedAt <= existBooking.CreatedAt.AddMinutes(20))
                                {
                                    // Rejected will minus 20% of total of booking 
                                    existUser.Balance += existBooking.Total * 0.8;
                                }
                                else
                                {
                                    existUser.Balance += existBooking.Total;
                                }
                                ProcessBookingStatus(existBooking, status);
                            }
                            if (existBooking.Status == StaticDetails.ActionStatus_ACCEPTED)
                            {
                                if (existBooking.CreatedAt <= existBooking.ApprovalStatusDate.AddDays(1))
                                {
                                    // if user reject booking after chef accept 1 day ago , the user will lose 30% of the total booking
                                    existUser.Balance += existBooking.Total * 0.4;
                                }
                            }
                            throw new Exception("Unable to cancel booking in progress.");
                        }

                    }
                }
                return _mapper.Map<RequestBookingDTO>(existBooking);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating booking status: {ex.Message}");
            }
        }
        private async void ProcessBookingStatus(Booking existBooking, BookingStatusDTO status)
        {
            existBooking.ApprovalStatusDate = DateTime.Now;
            existBooking.Status = status.Status;
            await _bookingRepository.UpdateBookingStatus(existBooking);
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

