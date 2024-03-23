namespace NESTCOOKING_API.DataAccess.Repositories.IRepositories
{
	public interface IStatisticRepository
	{
		Task CreateNewDataIfNewDate(DateOnly date);
		Task UpdateAllStatistics();
	}
}
