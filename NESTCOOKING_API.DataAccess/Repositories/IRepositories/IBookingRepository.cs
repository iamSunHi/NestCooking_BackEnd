using NESTCOOKING_API.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NESTCOOKING_API.DataAccess.Repositories.IRepositories
{
    public interface IBookingRepository : IRepository<Booking> 
    {
        Task<IEnumerable<Booking>> GetBookingsByChefId(string chefId);
        Task<User> GetChefByBookingId(string id);
        Task<Booking> UpdateBookingStatus(Booking status);
    }
}
