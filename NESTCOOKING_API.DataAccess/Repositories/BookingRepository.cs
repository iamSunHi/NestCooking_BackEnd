using Microsoft.EntityFrameworkCore;
using NESTCOOKING_API.DataAccess.Data;
using NESTCOOKING_API.DataAccess.Models;
using NESTCOOKING_API.DataAccess.Repositories.IRepositories;

namespace NESTCOOKING_API.DataAccess.Repositories
{
    public class BookingRepository : Repository<Booking>, IBookingRepository
    {
        public BookingRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Booking> UpdateBookingStatus(Booking entity)
        {
            var bookingFromDb = await this.GetAsync(b => b.Id == entity.Id);
            if (bookingFromDb != null)
            {
                if (_context.Entry(bookingFromDb).State == EntityState.Detached)
                {
                    _context.Attach(bookingFromDb);
                }

                bookingFromDb.ApprovalStatusDate = entity.ApprovalStatusDate;
                bookingFromDb.Status = entity.Status;
                bookingFromDb.TransactionIdList = entity.TransactionIdList;

                _context.SaveChanges();

                return bookingFromDb;
            }
            else
            {
                throw new Exception("Booking does not exist!");
            }
        }
    }
}
