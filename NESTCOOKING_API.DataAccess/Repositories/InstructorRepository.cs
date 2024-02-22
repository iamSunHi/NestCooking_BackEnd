using NESTCOOKING_API.DataAccess.Data;
using NESTCOOKING_API.DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using NESTCOOKING_API.DataAccess.Repositories.IRepositories;

namespace NESTCOOKING_API.DataAccess.Repositories
{
	public class InstructorRepository : Repository<Instructor>, IInstructorRepository
	{
		public InstructorRepository(ApplicationDbContext context) : base(context)
		{
		}

		public async Task UpdateAsync(Instructor instructor)
		{
			var instructorFromDb = await _context.Instructors.FindAsync(instructor.Id);

			if (instructorFromDb != null)
			{
				if (_context.Entry(instructorFromDb).State == EntityState.Detached)
				{
					_context.Attach(instructorFromDb);
				}
				instructorFromDb.Description = instructor.Description;
				instructorFromDb.ImageUrls = instructor.ImageUrls;
				instructorFromDb.InstructorOrder = instructor.InstructorOrder;

				await _context.SaveChangesAsync();
			}
		}

	}
}
