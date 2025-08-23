using HotelBookingAPI.model;
namespace HotelBookingAPI.Service.RoomService;

public interface IRoomService
{
    List<Room> GetAll();
    Room? GetById(int id);
    void Add(Room room);
    void Delete(int id);

    List<Room> GetAvailableRoomsByDateRange(DateTime checkIn, DateTime checkOut); // ✅ Add this
}
