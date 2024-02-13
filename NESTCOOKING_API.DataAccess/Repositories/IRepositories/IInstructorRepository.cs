using NESTCOOKING_API.DataAccess.Models;

namespace NESTCOOKING_API.DataAccess.Repositories.IRepositories
{
	public interface IInstructorRepository : IRepository<Instructor>
	{
		Task UpdateAsync(Instructor instructor);
	}
}
