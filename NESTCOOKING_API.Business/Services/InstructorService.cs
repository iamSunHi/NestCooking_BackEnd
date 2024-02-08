using AutoMapper;
using NESTCOOKING_API.Business.DTOs.RecipeDTOs;
using NESTCOOKING_API.Business.Services.IServices;
using NESTCOOKING_API.DataAccess.Models;
using NESTCOOKING_API.DataAccess.Repositories.IRepositories;

namespace NESTCOOKING_API.Business.Services
{
	public class InstructorService : IInstructorService
	{
		private readonly IMapper _mapper;
		private readonly IInstructorRepository _instructorRepository;

		public InstructorService(IMapper mapper, IInstructorRepository instructorRepository)
		{
			_mapper = mapper;
			_instructorRepository = instructorRepository;
		}

		public async Task<IEnumerable<InstructorDTO>> GetAllInstructorsAsync()
		{
			var instructorsFromDb = await _instructorRepository.GetAllAsync();
			return _mapper.Map<IEnumerable<InstructorDTO>>(instructorsFromDb);
		}

		public async Task<InstructorDTO> GetInstructorByIdAsync(int id)
		{
			var instructorFromDb = await _instructorRepository.GetAsync(i => i.Id == id);
			return _mapper.Map<InstructorDTO>(instructorFromDb);
		}

		public async Task CreateInstructorAsync(InstructorDTO instructorDTO)
		{
			var instructor = _mapper.Map<Instructor>(instructorDTO);
			await _instructorRepository.CreateAsync(instructor);
		}

		public async Task UpdateInstructorAsync(InstructorDTO instructorDTO)
		{
			var instructor = _mapper.Map<Instructor>(instructorDTO);
			await _instructorRepository.UpdateAsync(instructor);
			await _instructorRepository.SaveAsync();
		}

		public async Task DeleteInstructorAsync(int id)
		{
			var instructorFromDb = await _instructorRepository.GetAsync(i => i.Id == id);
			if (instructorFromDb != null)
			{
				await _instructorRepository.RemoveAsync(instructorFromDb);
			}
		}
	}
}
