using NESTCOOKING_API.Business.DTOs.StatisticDTOs;
using NESTCOOKING_API.Business.Services.IServices;
using NESTCOOKING_API.DataAccess.Repositories.IRepositories;
using System.Net.Http.Headers;

namespace NESTCOOKING_API.Business.Services
{
	public class StatisticService : IStatisticService
	{
		private readonly IBookingStatisticRepository _bookingStatisticRepository;
		private readonly IChefStatisticRepository _chefStatisticRepository;
		private readonly IViolationStatisticRepository _violationStatisticRepository;
		private readonly IUserStatisticRepository _userStatisticRepository;
		private readonly IRevenueStatisticRepository _revenueStatisticRepository;

		public StatisticService(
			IBookingStatisticRepository bookingStatisticRepository, IChefStatisticRepository chefStatisticRepository,
			IViolationStatisticRepository violationStatisticRepository, IUserStatisticRepository userStatisticRepository,
			IRevenueStatisticRepository revenueStatisticRepository
			)
		{
			_bookingStatisticRepository = bookingStatisticRepository;
			_chefStatisticRepository = chefStatisticRepository;
			_violationStatisticRepository = violationStatisticRepository;
			_userStatisticRepository = userStatisticRepository;
			_revenueStatisticRepository = revenueStatisticRepository;
		}

		public async Task<object> GetAllStatisticsAsync()
		{
			NumberStatisticDTO numberStatistic = new NumberStatisticDTO();

			var bookingStatisticFromDb = await _bookingStatisticRepository
				.GetAllAsync();
			var totalBooking = bookingStatisticFromDb.Sum(vs => vs.TotalOfBooking);

			var revenueStatisticFromDb = await _revenueStatisticRepository
				.GetAllAsync();
			var totalRevenue = revenueStatisticFromDb.Sum(vs => vs.Revenue);

			var violationStatisticFromDb = await _violationStatisticRepository
				.GetAllAsync();
			var totalViolation = violationStatisticFromDb.Sum(vs => vs.TotalOfViolation);

			numberStatistic = new()
			{
				TotalOfBooking = totalBooking,
				TotalOfRevenue = totalRevenue,
				TotalOfViolation = totalViolation
			};

			var userStatisticFromDb = await _userStatisticRepository
				.GetAllAsync();
			var userStatisticList = new List<ChartStatisticDTO>();
			foreach (var userStatistic in userStatisticFromDb)
			{
				userStatisticList.Add(new()
				{
					Name = userStatistic.Date.ToLongDateString(),
					Uv = userStatistic.NumberOfNewUser,
					Pv = userStatistic.TotalOfUser
				});
			}

			var chefStatisticFromDb = await _chefStatisticRepository
				.GetAllAsync();
			var chefStatisticList = new List<ChartStatisticDTO>();
			foreach (var chefStatistic in chefStatisticFromDb)
			{
				chefStatisticList.Add(new()
				{
					Name = chefStatistic.Date.ToLongDateString(),
					Uv = chefStatistic.NumberOfNewChef,
					Pv = chefStatistic.TotalOfChef
				});
			}

			var chartStatistic = new { userStatisticList, chefStatisticList };

			return new { numberStatistic, chartStatistic };
		}
	}
}
