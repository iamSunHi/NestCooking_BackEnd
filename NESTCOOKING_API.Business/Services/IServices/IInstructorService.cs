using NESTCOOKING_API.Business.DTOs.RecipeDTOs;

namespace NESTCOOKING_API.Business.Services.IServices
{
	public interface IInstructorService
	{
		Task<IEnumerable<InstructorDTO>> GetAllInstructorsAsync();
		Task<InstructorDTO> GetInstructorByIdAsync(int id);
		Task CreateInstructorAsync(InstructorDTO instructorDTO);
		Task UpdateInstructorAsync(InstructorDTO instructorDTO);
		Task DeleteInstructorAsync(int id);
	}
}
