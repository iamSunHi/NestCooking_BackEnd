using AutoMapper;
using Microsoft.AspNetCore.Identity;
using NESTCOOKING_API.Business.DTOs.BookingDTOs;
using NESTCOOKING_API.Business.Services.IServices;
using NESTCOOKING_API.DataAccess.Models;
using NESTCOOKING_API.DataAccess.Repositories.IRepositories;
using NESTCOOKING_API.Utility;

namespace NESTCOOKING_API.Business.Services
{
	public class BookingService : IBookingService
	{
		private readonly IBookingRepository _bookingRepository;
		private readonly IBookingLineRepository _bookingLineRepository;
		private readonly UserManager<User> _userManager;
		private readonly IMapper _mapper;

		public BookingService(IBookingRepository bookingRepository, IBookingLineRepository bookingLineRepository, UserManager<User> userManager, IMapper mapper)
		{
			_bookingRepository = bookingRepository;
			_bookingLineRepository = bookingLineRepository;
			_userManager = userManager;
			_mapper = mapper;
		}

		public async Task<RequestBookingDTO> ApprovalBooking(ChefApprovalProcessDTO chefApprovalProcessDTO)
		{
			var exitsBookingRequest = await _bookingRepository.GetAsync(b => b.Id == chefApprovalProcessDTO.BookingId);
			if (exitsBookingRequest == null)
			{
				throw new Exception(AppString.RequestBookingNotFound);
			}
			var user = await _userManager.FindByIdAsync(exitsBookingRequest.UserId);
			if (exitsBookingRequest == null || user == null)
			{
				throw new Exception(AppString.RequestBookingNotFound);
			}
			if (chefApprovalProcessDTO.Status != StaticDetails.ActionStatus_ACCEPTED &&
				chefApprovalProcessDTO.Status != StaticDetails.ActionStatus_REJECTED &&
				chefApprovalProcessDTO.Status != StaticDetails.ActionStatus_COMPLETED)
			{
				throw new Exception(AppString.InvalidApprovalTypeErrorMessage);
			}
			exitsBookingRequest.ApprovalStatusDate = DateTime.UtcNow;
			_mapper.Map(chefApprovalProcessDTO, exitsBookingRequest);
			await _bookingRepository.ApprovalBooking(exitsBookingRequest);
			var result = _mapper.Map<RequestBookingDTO>(exitsBookingRequest);
			return result;
		}

		public Task<bool> CheckTypeStatus(string status)
		{
			if (status == null)
			{
				throw new ArgumentNullException(nameof(status), AppString.StatusBookingNull);
			}
			try
			{
				bool isValidType = string.Equals(status, StaticDetails.ActionStatus_ACCEPTED, StringComparison.OrdinalIgnoreCase) ||
								string.Equals(status, StaticDetails.ActionStatus_REJECTED, StringComparison.OrdinalIgnoreCase) ||
								string.Equals(status, StaticDetails.ActionStatus_COMPLETED, StringComparison.OrdinalIgnoreCase);
				return Task.FromResult(isValidType);
			}
			catch (Exception ex)
			{
				throw new Exception(AppString.SomethingWrongMessage, ex);
			}
		}

		public async Task<RequestBookingDTO> CreateBooking(string userId, CreateBookingDTO createBookingDTO)
		{
			try
			{
				var user = await _userManager.FindByIdAsync(userId);

				if (user == null)
				{
					throw new Exception(AppString.SomethingWrongMessage);
				}

				if (user.Balance < createBookingDTO.Total)
				{
					throw new Exception(AppString.InsufficientBalance);
				}

				if (!(await this.CheckIfChefIsAvailable(createBookingDTO.ChefId, createBookingDTO.RequiredDate)))
				{
					throw new Exception(AppString.ChefIsNotAvailable);
				}

				var requestBooking = _mapper.Map<Booking>(createBookingDTO);
				requestBooking.Id = Guid.NewGuid().ToString();
				requestBooking.UserId = userId;
				requestBooking.Status = StaticDetails.ActionStatus_PENDING;
				requestBooking.CreatedAt = DateTime.UtcNow;
				requestBooking.ApprovalStatusDate = DateTime.UtcNow;
				requestBooking.TransactionId = null;

				var result = _mapper.Map<RequestBookingDTO>(_bookingRepository.CreateAsync(requestBooking));

				var firstDish = createBookingDTO.InfomationDishes.FirstOrDefault();
				var recipeId = firstDish?.RecipeId;
				var price = firstDish?.Price;
				var portion = firstDish?.Portion ?? 1;
				var BookingLine = new BookingLine
				{
					BookingId = requestBooking.Id,
					RecipeId = recipeId,
					Quantity = portion,
					TotalPriceOfDishes = createBookingDTO.Total,
				};

				await _bookingLineRepository.CreateAsync(BookingLine);
				return result;
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		public async Task DeleteBookingRequest(string userId, string bookingId)
		{
			try
			{
				var book = await _bookingRepository.GetAsync(b => b.Id == bookingId);
				if (book == null)
				{
					throw new Exception(AppString.RequestBookingNotFound);
				}
				if (book.UserId == userId)
				{
					await _bookingRepository.RemoveAsync(book);
				}
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		public async Task<IEnumerable<RequestBookingDTO>> GetAllBookingRequests()
		{
			var result = _mapper.Map<IEnumerable<RequestBookingDTO>>(await _bookingRepository.GetAllAsync());
			return result;
		}

		public async Task<RequestBookingDTO> GetBookingRequest(string bookingId)
		{
			var result = _mapper.Map<RequestBookingDTO>(await _bookingRepository.GetAsync(book => book.Id == bookingId));
			return result;
		}

		private async Task<bool> CheckIfChefIsAvailable(string chefId, DateOnly requiredDate)
		{
			var bookingFromDb = await _bookingRepository.GetAsync(b => b.ChefId == chefId && b.RequiredDate == requiredDate);
			return bookingFromDb == null;
		}
	}
}
