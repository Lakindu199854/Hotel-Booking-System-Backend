//using HotelBookingAPI.model;
//using HotelBookingAPI.Service.RoomService;

//namespace HotelBookingAPI.Service.BookingService.HotelBookingAPI.Service.BookingRoomCoordinatorService;
//public class BookingRoomCoordinatorService
//{
//    private readonly IRoomService _roomService;
//    private readonly IBookingService _bookingService;
//    private readonly BookingServiceImpl _bookingServiceImpl;
//    private readonly RoomServiceImpl _roomServiceImpl;


//    public BookingRoomCoordinatorService(IRoomService roomService, IBookingService bookingService, BookingServiceImpl bookingServiceImpl, RoomServiceImpl roomServiceImpl)
//    {
//        _roomService = roomService;
//        _bookingService = bookingService;
//        _bookingServiceImpl = bookingServiceImpl;
//        _roomServiceImpl = roomServiceImpl;
//    }

//    public List<Room> GetAllRooms() => _roomServiceImpl.Rooms; 
//    public List<Booking> GetAllBookings() => _bookingServiceImpl.Bookings;


//}
