using Microsoft.EntityFrameworkCore;
using NESTCOOKING_API.DataAccess.Data;
using NESTCOOKING_API.DataAccess.Models;
using NESTCOOKING_API.DataAccess.Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NESTCOOKING_API.DataAccess.Repositories
{
    public class BookingRepository : Repository<Booking>, IBookingRepository
    {
        public BookingRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Booking>> GetBookingsByChefId(string chefId)
        {
            var bookings = await _context.Bookings
                .Where(b => b.ChefId == chefId).ToListAsync();
            return bookings;

        }

        public async Task<User> GetChefByBookingId(string id)
        {
            var findChef = await _context.Bookings
                 .Where(b => b.Id == id)
                 .Select(b => b.User) 
                 .FirstOrDefaultAsync();
            return findChef;
        }

        public async Task<Booking> UpdateBookingStatus(Booking status)
        {
            _context.Bookings.Update(status);
            _context.SaveChanges();
            return status;
        }
    }
}
