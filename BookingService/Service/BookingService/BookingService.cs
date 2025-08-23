using BookingService.Dto;
using BookingService.Model;

namespace BookingService.Service.BookingService
{
    public interface IBookingService
    {
        List<Booking> GetAll();
        Booking? GetById(int id);
        void Add(CreateBookingDTO booking);
        void Delete(int id);
        List<Booking> GetByCheckInDateRange(DateTime? startDate, DateTime? endDate);
    }
}
