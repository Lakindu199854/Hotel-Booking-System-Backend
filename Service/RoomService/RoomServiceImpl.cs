using HotelBookingAPI.model;
using HotelBookingAPI.Service.BookingService;

namespace HotelBookingAPI.Service.RoomService;


public class RoomServiceImpl : IRoomService
{
    private readonly IBookingReaderService _bookingService;

    public RoomServiceImpl(IBookingReaderService bookingService)
    {
        _bookingService = bookingService;
    }

    public List<Room> Rooms { get; set; } = new()
    {
        new Room { RoomId = 1, RoomNumber = "101", RoomType = "Single" ,IsOccupied=false},
        new Room { RoomId = 2, RoomNumber = "102", RoomType = "Double" ,IsOccupied=false}
    };

    public List<Room> GetAll() => Rooms;

    public Room? GetById(int id) => Rooms.FirstOrDefault(r => r.RoomId == id);

    public void Add(Room room) => Rooms.Add(room);

    public void Delete(int id) => Rooms.RemoveAll(r => r.RoomId == id);

    public List<Room> GetAvailableRoomsByDateRange(DateTime checkIn, DateTime checkOut)
    {
        var allBookings = _bookingService.GetAll();

        var bookedRoomIds = allBookings
            .Where(b =>
                !(b.CheckOutDate <=checkIn || b.CheckInDate >= checkOut)) // overlaps
            .Select(b => b.RoomId.RoomId)
            .ToHashSet();

        return Rooms.Where(r => !bookedRoomIds.Contains(r.RoomId)).ToList();
    }
}
