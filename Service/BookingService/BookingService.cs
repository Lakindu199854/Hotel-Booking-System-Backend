using Hotel_Booking_App.Dto;
using HotelBookingAPI.model;

namespace HotelBookingAPI.Service.BookingService
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
