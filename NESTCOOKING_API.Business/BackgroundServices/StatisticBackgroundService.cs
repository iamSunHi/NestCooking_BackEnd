using Microsoft.Extensions.Hosting;
using NESTCOOKING_API.DataAccess.Repositories.IRepositories;

namespace NESTCOOKING_API.Business.BackgroundServices
{
	public class StatisticBackgroundService : BackgroundService
	{
		private readonly IStatisticRepository _statisticRepository;

		public StatisticBackgroundService(IStatisticRepository statisticRepository)
		{
			_statisticRepository = statisticRepository;
		}

		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			while (!stoppingToken.IsCancellationRequested)
			{
				var currentDateOnly = DateOnly.FromDateTime(DateTime.UtcNow.AddHours(7));

				await _statisticRepository.CreateNewDataIfNewDate(currentDateOnly);

				await _statisticRepository.UpdateAllStatistics();

				await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken); // Wait 5 minutes before update again
			}
		}
	}
}
