// IBookingReaderService.cs
using HotelBookingAPI.model;
namespace HotelBookingAPI.model;
public interface IBookingReaderService
{
    List<Booking> GetAll();
}
