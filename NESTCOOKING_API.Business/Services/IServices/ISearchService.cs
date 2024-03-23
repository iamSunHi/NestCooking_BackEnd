using NESTCOOKING_API.Business.DTOs;

namespace NESTCOOKING_API.Business.Services.IServices
{
	public interface ISearchService
	{
		Task<SearchResultDTO> GetAllAsync(string criteria, string? userId = null);
		Task<SearchResultDTO> GetRecipesAsync(string criteria, string? userId = null);
		Task<SearchResultDTO> GetUsersAsync(string criteria, string? userId = null);
	}
}
