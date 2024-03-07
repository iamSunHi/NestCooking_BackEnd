﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NESTCOOKING_API.DataAccess.Data;
using NESTCOOKING_API.DataAccess.Repositories.IRepositories;
using NESTCOOKING_API.Utility;

namespace NESTCOOKING_API.DataAccess.Repositories
{
	public class StatisticRepository : IStatisticRepository
	{
		private readonly IServiceScopeFactory _scopeFactory;

		public StatisticRepository(IServiceScopeFactory scopeFactory)
		{
			_scopeFactory = scopeFactory;
		}

		public async Task CreateNewDataIfNewDate(DateOnly currentDate)
		{
			using var scope = _scopeFactory.CreateScope();
			var _context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
			if (_context.UserStatistics.OrderByDescending(us => us.Date).First().Date != currentDate)
			{
				var totalViolation = _context.Reports.Select(r => r.Status == StaticDetails.ActionStatus_ACCEPTED).Count();
				await _context.ViolationStatistics.AddAsync(new() { Date = currentDate, TotalOfViolation = totalViolation });

				var totalRevenue = 0;
				await _context.RevenueStatistics.AddAsync(new() { Date = currentDate, Revenue = totalRevenue });

				var totalBooking = 0;
				await _context.BookingStatistics.AddAsync(new() { Date = currentDate, TotalOfBooking = totalBooking });

				var totalUser = _context.Users.Count();
				await _context.UserStatistics.AddAsync(new() { Date = currentDate, TotalOfUser = totalUser });

				var roleChefId = (await _context.Roles.FirstAsync(r => r.Name == StaticDetails.Role_Chef)).Id;
				var totalChef = _context.Users.Select(u => u.RoleId == roleChefId).Count();
				await _context.ChefStatistics.AddAsync(new() { Date = currentDate, TotalOfChef = totalChef });

				await _context.SaveChangesAsync();
			}
		}

		public async Task UpdateAllStatistics()
		{
			using var scope = _scopeFactory.CreateScope();
			var _context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
			var currentDate = DateTime.UtcNow.Date;
			var currentDateOnly = DateOnly.FromDateTime(currentDate);
			var userListFromDb = _context.Users.ToList();

			var totalNewUser = userListFromDb.Where(u => u.CreatedAt.Date == currentDate.Date).Count();
			var totalUser = userListFromDb.Count();
			var currentUserStatistic = await _context.UserStatistics.FirstAsync(us => us.Date == currentDateOnly);
			currentUserStatistic.NumberOfNewUser = totalNewUser;
			currentUserStatistic.TotalOfUser = totalUser;
			_context.UserStatistics.Update(currentUserStatistic);

			var roleChef = await _context.Roles.FirstAsync(r => r.Name == StaticDetails.Role_Chef);
			var totalNewChef = userListFromDb.Where(u => u.RoleId == roleChef.Id && u.CreatedAt.Date == currentDate.Date).Count();
			var totalChef = userListFromDb.Where(u => u.RoleId == roleChef.Id).Count();
			var currentChefStatistic = await _context.ChefStatistics.FirstAsync(us => us.Date == currentDateOnly);
			currentChefStatistic.NumberOfNewChef = totalNewChef;
			currentChefStatistic.TotalOfChef = totalChef;
			_context.ChefStatistics.Update(currentChefStatistic);

			var reportListFromDb = _context.Reports.ToList();
			var totalViolation = reportListFromDb.Where(r => r.Status == StaticDetails.ActionStatus_COMPLETED).Count();
			var currentViolationStatistic = await _context.ViolationStatistics.FirstAsync(us => us.Date == currentDateOnly);
			currentViolationStatistic.TotalOfViolation = totalViolation;
			_context.ViolationStatistics.Update(currentViolationStatistic);

			// Revenue
			// Booking

			await _context.SaveChangesAsync();
		}
	}
}
