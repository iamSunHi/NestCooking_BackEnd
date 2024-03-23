using NESTCOOKING_API.DataAccess.Models;

namespace NESTCOOKING_API.DataAccess.Repositories.IRepositories
{
    public interface IBookingRepository : IRepository<Booking>
    {
        Task<Booking> UpdateBookingStatus(Booking status);
    }
}
