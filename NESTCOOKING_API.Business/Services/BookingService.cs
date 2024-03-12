using AutoMapper;
using Microsoft.AspNetCore.Identity;
using NESTCOOKING_API.Business.DTOs.BookingDTOs;
using NESTCOOKING_API.Business.DTOs.RecipeDTOs;
using NESTCOOKING_API.Business.DTOs.UserDTOs;
using NESTCOOKING_API.Business.Services.IServices;
using NESTCOOKING_API.DataAccess.Models;
using NESTCOOKING_API.DataAccess.Repositories.IRepositories;
using NESTCOOKING_API.Utility;
using System.Collections.Generic;

namespace NESTCOOKING_API.Business.Services
{
    public class BookingService : IBookingService
    {
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly IBookingRepository _bookingRepository;
        private readonly IBookingLineRepository _bookingLineRepository;
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IRecipeRepository _recipeRepository;

        public BookingService(IMapper mapper, UserManager<User> userManager,
            IBookingRepository bookingRepository, IUserRepository userRepository, IBookingLineRepository bookingLineRepository, IRoleRepository roleRepository, ITransactionRepository transactionRepository, IRecipeRepository recipeRepository)
        {
            _mapper = mapper;
            _userManager = userManager;
            _bookingRepository = bookingRepository;
            _bookingLineRepository = bookingLineRepository;
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _transactionRepository = transactionRepository;
            _recipeRepository = recipeRepository;
        }

        public async Task<IEnumerable<BookingShortInfoDTO>> GetAllBookingsByChefIdAsync(string chefId)
        {
            var bookingListFromDb = (await _bookingRepository.GetAllAsync(b => b.ChefId == chefId)).ToList();
            var result = _mapper.Map<List<BookingShortInfoDTO>>(bookingListFromDb);
            return result;
        }

        public async Task<IEnumerable<BookingShortInfoDTO>> GetAllBookingsByUserIdAsync(string userId)
        {
            var bookingListFromDb = (await _bookingRepository.GetAllAsync(b => b.UserId == userId)).ToList();
            var result = _mapper.Map<List<BookingShortInfoDTO>>(bookingListFromDb);

            for (int i = 0; i < bookingListFromDb.Count; i++)
            {
                var chef = await _userRepository.GetAsync(u => u.Id == bookingListFromDb[i].ChefId);
                result[i].Chef = _mapper.Map<UserShortInfoDTO>(chef);
            }

            return result;
        }

        public async Task<List<UserShortInfoDTO>> GetAllChefsAsync(string? city = null)
        {
            var roleChefId = await _roleRepository.GetRoleIdByNameAsync(StaticDetails.Role_Chef);
            var chefList = await _userRepository.GetAllAsync(u => u.RoleId == roleChefId);
            return _mapper.Map<List<UserShortInfoDTO>>(chefList);
        }

        public async Task<BookingDetailDTO> GetBookingByIdAsync(string bookingId)
        {
            var bookingFromDb = await _bookingRepository.GetAsync(b => b.Id == bookingId);
            var booking = _mapper.Map<BookingDetailDTO>(bookingFromDb);

            var user = await _userRepository.GetAsync(u => u.Id == bookingFromDb.UserId);
            booking.User = _mapper.Map<UserShortInfoDTO>(user);
            var chef = await _userRepository.GetAsync(u => u.Id == bookingFromDb.ChefId);
            booking.Chef = _mapper.Map<UserShortInfoDTO>(chef);

            var bookingLines = await _bookingLineRepository.GetAllAsync(bl => bl.BookingId == booking.Id);
            foreach (var bookingLine in bookingLines)
            {
                var recipeForBooking = await _recipeRepository.GetAsync(r => r.Id == bookingLine.RecipeId);
                booking.BookingDishes.Add(_mapper.Map<RecipeForBookingDTO>(recipeForBooking));
            }

            List<BookingTransactionDTO> deposit = new List<BookingTransactionDTO>();
            List<BookingTransactionDTO> withdraw = new List<BookingTransactionDTO>();
            foreach (var transactionId in bookingFromDb.TransactionIdList)
            {
                var transactionFromDb = await _transactionRepository.GetAsync(t => t.Id == transactionId, includeProperties: "User");
                var transaction = _mapper.Map<BookingTransactionDTO>(transactionFromDb);
                transaction.UserFullName = transactionFromDb.User.FirstName + " " + transactionFromDb.User.LastName;
                if (transaction.Amount >= 0)
                    withdraw.Add(transaction);
                else
                    deposit.Add(transaction);
            }
            booking.Transaction = new { deposit, withdraw };

            return booking;
        }

        public async Task<BookingDetailDTO> CreateBooking(string userId, CreateBookingDTO createBooking)
        {
            try
            {
                if (DateTime.UtcNow.AddDays(2) >= createBooking.TimeStart)
                {
                    throw new Exception(message: "Booking must be placed 2 days or more from now.");
                }
                if (createBooking.TimeEnd <= createBooking.TimeStart || createBooking.TimeStart <= DateTime.UtcNow)
                {
                    throw new Exception(message: "'Time End' must be greater than 'Time Start'.");
                }
                if (createBooking.TimeEnd < createBooking.TimeStart.AddHours(1))
                {
                    throw new Exception("Please ensure that the time interval between 'Time Start' and 'Time End' is at least 1 hour.");
                }

                await this.ValidateBookingTime(createBooking);

                var userFromDb = await _userRepository.GetAsync(u => u.Id == userId);
                if (userFromDb.Balance < createBooking.Total)
                {
                    throw new Exception(message: "You don't have enough balance to create this booking. You must have 100% of the total booking amount to create this.");
                }

                await _userRepository.IncreaseUserBalanceAsync(userFromDb.Id, -createBooking.Total);
                var transactionId = await this.CreateTransactionAsync(
                    userId, StaticDetails.PaymentType_BOOKING,
                    -createBooking.Total, AppString.PaymentSendDepositForBookingOfUser
                );

                var newBooking = _mapper.Map<Booking>(createBooking);
                newBooking.Id = Guid.NewGuid().ToString();
                newBooking.Status = StaticDetails.ActionStatus_PENDING;
                newBooking.CreatedAt = DateTime.UtcNow;
                newBooking.ApprovalStatusDate = DateTime.UtcNow;
                newBooking.UserId = userId;
                newBooking.TransactionIdList =
                [
                    transactionId
                ];

                var admin = await _userRepository.GetAsync(u => u.Email == AppString.MailEmail);
                await _userRepository.IncreaseUserBalanceAsync(admin.Id, createBooking.Total);
                transactionId = await this.CreateTransactionAsync(
                    admin.Id, StaticDetails.PaymentType_BOOKING,
                    createBooking.Total, AppString.PaymentReceiveDepositForBookingOfUser
                );
                newBooking.TransactionIdList.Add(transactionId);

                await _bookingRepository.CreateAsync(newBooking);

                // Add Recipes infomation into BookingLine
                foreach (var dish in createBooking.BookingDishes)
                {
                    var bookingLine = new BookingLine
                    {
                        BookingId = newBooking.Id,
                        RecipeId = dish.RecipeId,
                        Quantity = dish.Quantity,
                    };
                    await _bookingLineRepository.CreateAsync(bookingLine);
                }

                var createdBooking = await this.GetBookingByIdAsync(newBooking.Id);
                return createdBooking;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<BookingDetailDTO> UpdateBookingStatus(string userId, BookingStatusDTO bookingStatusDTO)
        {
            var bookingFromDb = await _bookingRepository.GetAsync(b => b.Id == bookingStatusDTO.BookingId);
            if (bookingFromDb == null)
            {
                throw new Exception("Booking not found.");
            }

            var user = await _userRepository.GetAsync(u => u.Id == bookingFromDb.UserId);
            var admin = await _userRepository.GetAsync(u => u.Email == AppString.MailEmail);
            var chef = await _userRepository.GetAsync(u => u.Id == bookingFromDb.ChefId);

            var bookingDepositMoneyOfChef = bookingFromDb.Total * 0.2;

            bookingStatusDTO.Status = bookingStatusDTO.Status.ToUpper();
            if (bookingStatusDTO.Status is not StaticDetails.ActionStatus_ACCEPTED &&
                bookingStatusDTO.Status is not StaticDetails.ActionStatus_REJECTED &&
                bookingStatusDTO.Status is not StaticDetails.ActionStatus_COMPLETED &&
                bookingStatusDTO.Status is not StaticDetails.ActionStatus_CANCELED)
            {
                throw new Exception("Invalid status type.");
            }

            if ((await _userRepository.GetRoleAsync(userId)) is StaticDetails.Role_Chef)
            {
                if (bookingFromDb.Status == StaticDetails.ActionStatus_PENDING)
                {
                    switch (bookingStatusDTO.Status)
                    {
                        case StaticDetails.ActionStatus_ACCEPTED:
                            {
                                if (chef.Balance < bookingDepositMoneyOfChef)
                                {
                                    throw new Exception(message: "You don't have enough balance to accept this booking. You must have 20% of the total booking amount to accept this.");
                                }
                                await this.ValidateBookingTime(_mapper.Map<CreateBookingDTO>(bookingFromDb));

                                await _userRepository.IncreaseUserBalanceAsync(admin.Id, bookingDepositMoneyOfChef);
                                var transactionId = await this.CreateTransactionAsync(
                                    admin.Id, StaticDetails.PaymentType_BOOKING,
                                    bookingDepositMoneyOfChef, AppString.PaymentReceiveDepositForBookingOfChef
                                );
                                bookingFromDb.TransactionIdList.Add(transactionId);

                                await _userRepository.IncreaseUserBalanceAsync(chef.Id, -bookingDepositMoneyOfChef);
                                transactionId = await this.CreateTransactionAsync(
                                    chef.Id, StaticDetails.PaymentType_BOOKING,
                                    -bookingDepositMoneyOfChef, AppString.PaymentSendDepositForBookingOfChef
                                );
                                bookingFromDb.TransactionIdList.Add(transactionId);
                                break;
                            }
                        case StaticDetails.ActionStatus_REJECTED:
                            {
                                await _userRepository.IncreaseUserBalanceAsync(user.Id, bookingFromDb.Total);
                                var transactionId = await this.CreateTransactionAsync(
                                    user.Id, StaticDetails.PaymentType_BOOKING,
                                    bookingFromDb.Total, AppString.PaymentDepositBackForBooking
                                );
                                bookingFromDb.TransactionIdList.Add(transactionId);

                                await _userRepository.IncreaseUserBalanceAsync(admin.Id, -bookingFromDb.Total);
                                transactionId = await this.CreateTransactionAsync(
                                    admin.Id, StaticDetails.PaymentType_BOOKING,
                                    -bookingFromDb.Total, AppString.PaymentDepositBackForBooking
                                );
                                bookingFromDb.TransactionIdList.Add(transactionId);
                                break;
                            }
                        default:
                            throw new Exception(AppString.SomethingWrongMessage);
                    }
                }
                else if (bookingFromDb.Status == StaticDetails.ActionStatus_ACCEPTED)
                {
                    if (bookingStatusDTO.Status == StaticDetails.ActionStatus_CANCELED)
                    {
                        // if chef cancel a accepted booking, he/she will lose deposit money
                        await _userRepository.IncreaseUserBalanceAsync(user.Id, bookingFromDb.Total + bookingDepositMoneyOfChef);
                        var transactionId = await this.CreateTransactionAsync(
                            user.Id, StaticDetails.PaymentType_BOOKING,
                            bookingFromDb.Total + bookingDepositMoneyOfChef, AppString.PaymentCancelAcceptedBookingByChef
                        );
                        bookingFromDb.TransactionIdList.Add(transactionId);

                        await _userRepository.IncreaseUserBalanceAsync(admin.Id, -(bookingFromDb.Total + bookingDepositMoneyOfChef));
                        transactionId = await this.CreateTransactionAsync(
                            admin.Id, StaticDetails.PaymentType_BOOKING,
                            -(bookingFromDb.Total + bookingDepositMoneyOfChef), AppString.PaymentCancelAcceptedBookingByChef
                        );
                        bookingFromDb.TransactionIdList.Add(transactionId);
                    }
                    else
                    {
                        throw new Exception(AppString.SomethingWrongMessage);
                    }
                }
            }
            else
            {
                // allow user to approve, cancel, or complete
                switch (bookingStatusDTO.Status)
                {
                    case StaticDetails.ActionStatus_COMPLETED:
                        {
                            if (bookingFromDb.Status == StaticDetails.ActionStatus_ACCEPTED)
                            {
                                // decrease 90% of the total booking amount and keep 10% as fee to admin's balance
                                await _userRepository.IncreaseUserBalanceAsync(admin.Id, -(bookingFromDb.Total * 0.9 + bookingDepositMoneyOfChef));
                                var transactionId = await this.CreateTransactionAsync(
                                    admin.Id, StaticDetails.PaymentType_BOOKING,
                                    -(bookingFromDb.Total * 0.9 + bookingDepositMoneyOfChef), AppString.PaymentCompleteBooking
                                );
                                bookingFromDb.TransactionIdList.Add(transactionId);

                                // increase 90% of the total booking amount and booking deposit money of chef to chef's balance
                                await _userRepository.IncreaseUserBalanceAsync(chef.Id, bookingFromDb.Total * 0.9 + bookingDepositMoneyOfChef);
                                transactionId = await this.CreateTransactionAsync(
                                    user.Id, StaticDetails.PaymentType_BOOKING,
                                    bookingFromDb.Total * 0.9 + bookingDepositMoneyOfChef, AppString.PaymentCompleteBooking
                                );
                                bookingFromDb.TransactionIdList.Add(transactionId);
                            }
                            else
                            {
                                throw new Exception(AppString.SomethingWrongMessage);
                            }
                            break;
                        }
                    case StaticDetails.ActionStatus_CANCELED:
                        {
                            switch (bookingFromDb.Status)
                            {
                                case StaticDetails.ActionStatus_PENDING:
                                    {
                                        // if the user cancel a pending booking, they will not lose money
                                        await _userRepository.IncreaseUserBalanceAsync(user.Id, bookingFromDb.Total);
                                        var transactionId = await this.CreateTransactionAsync(
                                            user.Id, StaticDetails.PaymentType_BOOKING,
                                            bookingFromDb.Total, AppString.PaymentDepositBackForBooking
                                        );
                                        bookingFromDb.TransactionIdList.Add(transactionId);

                                        await _userRepository.IncreaseUserBalanceAsync(admin.Id, -bookingFromDb.Total);
                                        transactionId = await this.CreateTransactionAsync(
                                            admin.Id, StaticDetails.PaymentType_BOOKING,
                                            -bookingFromDb.Total, AppString.PaymentDepositBackForBooking
                                        );
                                        bookingFromDb.TransactionIdList.Add(transactionId);
                                        break;
                                    }
                                case StaticDetails.ActionStatus_ACCEPTED:
                                    {
                                        await _userRepository.IncreaseUserBalanceAsync(admin.Id, -(bookingFromDb.Total * 0.95));
                                        var transactionId = await this.CreateTransactionAsync(
                                            admin.Id, StaticDetails.PaymentType_BOOKING,
                                            -(bookingFromDb.Total * 0.95), AppString.PaymentCancelAcceptedBookingByUser
                                        );
                                        bookingFromDb.TransactionIdList.Add(transactionId);

                                        if (DateTime.UtcNow >= bookingFromDb.TimeStart.AddHours(-2))
                                        {
                                            // if the user cancel accepted booking less than 2 hours, they will lose 75% of the total (5% to admin, 70% to chef)
                                            await _userRepository.IncreaseUserBalanceAsync(user.Id, bookingFromDb.Total * 0.25);
                                            transactionId = await this.CreateTransactionAsync(
                                                user.Id, StaticDetails.PaymentType_BOOKING,
                                                bookingFromDb.Total * 0.25, AppString.PaymentCancelAcceptedBookingByUser
                                            );
                                            bookingFromDb.TransactionIdList.Add(transactionId);

                                            await _userRepository.IncreaseUserBalanceAsync(chef.Id, bookingFromDb.Total * 0.7 + bookingDepositMoneyOfChef);
                                            transactionId = await this.CreateTransactionAsync(
                                                chef.Id, StaticDetails.PaymentType_BOOKING,
                                                bookingFromDb.Total * 0.7 + bookingDepositMoneyOfChef, AppString.PaymentCancelAcceptedBookingByUser
                                            );
                                            bookingFromDb.TransactionIdList.Add(transactionId);
                                        }
                                        else if (DateTime.UtcNow >= bookingFromDb.TimeStart.AddDays(-1))
                                        {
                                            // if the user cancel accepted booking more than 2 hours to 1 day, they will lose 45% of the total (5% to admin, 40% to chef)
                                            await _userRepository.IncreaseUserBalanceAsync(user.Id, bookingFromDb.Total * 0.55);
                                            transactionId = await this.CreateTransactionAsync(
                                                user.Id, StaticDetails.PaymentType_BOOKING,
                                                bookingFromDb.Total * 0.5, AppString.PaymentCancelAcceptedBookingByUser
                                            );
                                            bookingFromDb.TransactionIdList.Add(transactionId);

                                            await _userRepository.IncreaseUserBalanceAsync(chef.Id, bookingFromDb.Total * 0.4 + bookingDepositMoneyOfChef);
                                            transactionId = await this.CreateTransactionAsync(
                                                chef.Id, StaticDetails.PaymentType_BOOKING,
                                                bookingFromDb.Total * 0.4 + bookingDepositMoneyOfChef, AppString.PaymentCancelAcceptedBookingByUser
                                            );
                                            bookingFromDb.TransactionIdList.Add(transactionId);
                                        }
                                        else //if (DateTime.UtcNow < bookingFromDb.TimeStart.AddDays(-1))
                                        {
                                            // if the user cancel accepted booking more than 1 day, they will lose 25% of the total (5% to admin, 20% to chef)
                                            await _userRepository.IncreaseUserBalanceAsync(user.Id, bookingFromDb.Total * 0.75);
                                            transactionId = await this.CreateTransactionAsync(
                                                user.Id, StaticDetails.PaymentType_BOOKING,
                                                bookingFromDb.Total * 0.75, AppString.PaymentCancelAcceptedBookingByUser
                                            );
                                            bookingFromDb.TransactionIdList.Add(transactionId);

                                            await _userRepository.IncreaseUserBalanceAsync(chef.Id, bookingFromDb.Total * 0.2 + bookingDepositMoneyOfChef);
                                            transactionId = await this.CreateTransactionAsync(
                                                user.Id, StaticDetails.PaymentType_BOOKING,
                                                bookingFromDb.Total * 0.2 + bookingDepositMoneyOfChef, AppString.PaymentCancelAcceptedBookingByUser
                                            );
                                            bookingFromDb.TransactionIdList.Add(transactionId);
                                        }
                                        break;
                                    }
                                default:
                                    {
                                        throw new Exception(AppString.SomethingWrongMessage);
                                    }
                            }
                            break;
                        }
                    default:
                        {
                            throw new Exception("Invalid status type.");
                        }
                }
            }

            await this.ProcessBookingStatus(bookingFromDb, bookingStatusDTO);
            return await this.GetBookingByIdAsync(bookingStatusDTO.BookingId);
        }

        private async Task ProcessBookingStatus(Booking existBooking, BookingStatusDTO status)
        {
            existBooking.ApprovalStatusDate = DateTime.UtcNow;
            existBooking.Status = status.Status;
            await _bookingRepository.UpdateBookingStatus(existBooking);
        }

        private async Task ValidateBookingTime(CreateBookingDTO createBooking)
        {
            var bookingsOfChefFromDb = await _bookingRepository.GetAllAsync(b => b.ChefId == createBooking.ChefId && b.Status == StaticDetails.ActionStatus_ACCEPTED);
            if (bookingsOfChefFromDb != null && bookingsOfChefFromDb.Any())
            {
                foreach (var booking in bookingsOfChefFromDb.OrderByDescending(b => b.CreatedAt))
                {
                    if (createBooking.TimeStart < booking.TimeEnd.AddHours(1))
                    {
                        throw new Exception($"The chef has been booked for this time. Please select another time period.");
                    }
                }
            }
        }

        private async Task<string> CreateTransactionAsync(
            string userId, string type, double amount, string description
        )
        {
            Transaction transaction = new()
            {
                Id = Guid.NewGuid().ToString(),
                UserId = userId,
                Type = type,
                Amount = amount,
                Description = description,
                Currency = StaticDetails.Currency_VND,
                Payment = StaticDetails.Payment_Wallet,
                IsSuccess = true,
                CreatedAt = DateTime.UtcNow
            };
            await _transactionRepository.CreateAsync(transaction);

            return transaction.Id;
        }
    }
}

