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
        Task<Booking> ApprovalBooking (Booking booking);
    }

}
