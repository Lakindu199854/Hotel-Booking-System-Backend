using HotelBookingAPI.model;

namespace HotelBookingAPI.Service.BookingService;

public class BookingServiceImpl : IBookingService
{
    private List<Booking> Bookings = new();

    public List<Booking> GetAll() => Bookings;

    public Booking? GetById(int id) => Bookings.FirstOrDefault(b => b.BookingId == id);

    public void Add(Booking booking)
    {
        if (booking.IsRecurring && booking.RecurrenceCount.HasValue && !string.IsNullOrEmpty(booking.RecurrenceType))
        {
            var currentCheckIn = booking.CheckInDate;
            var currentCheckOut = booking.CheckOutDate;

            for (int i = 0; i < booking.RecurrenceCount.Value; i++)
            {
                // Check for overlap before adding
                if (IsOverlappingBooking(booking.RoomId, currentCheckIn, currentCheckOut))
                {
                    throw new InvalidOperationException($"Room {booking.RoomId} is already booked between {currentCheckIn} and {currentCheckOut}");
                }

                var newBooking = new Booking
                {
                    BookingId = Bookings.Count + 1,
                    RoomId = booking.RoomId,
                    CustomerId = booking.CustomerId,
                    CheckInDate = currentCheckIn,
                    CheckOutDate = currentCheckOut,
                    IsRecurring = true,
                    RecurrenceCount = null,
                    RecurrenceType = null,
                    SpecialRequests = booking.SpecialRequests.Select(r => new SpecialRequest
                    {
                        RequestId = r.RequestId,
                        Description = r.Description
                    }).ToList()
                };

                Bookings.Add(newBooking);

                // Move dates forward
                switch (booking.RecurrenceType.ToLower())
                {
                    case "daily":
                        currentCheckIn = currentCheckIn.AddDays(1);
                        currentCheckOut = currentCheckOut.AddDays(1);
                        break;
                    case "weekly":
                        currentCheckIn = currentCheckIn.AddDays(7);
                        currentCheckOut = currentCheckOut.AddDays(7);
                        break;
                    case "monthly":
                        currentCheckIn = currentCheckIn.AddMonths(1);
                        currentCheckOut = currentCheckOut.AddMonths(1);
                        break;
                    default:
                        throw new ArgumentException("Invalid recurrence type.");
                }
            }
        }
        else
        {
            // Single booking
            if (IsOverlappingBooking(booking.RoomId, booking.CheckInDate, booking.CheckOutDate))
            {
                throw new InvalidOperationException($"Room {booking.RoomId} is already booked between {booking.CheckInDate} and {booking.CheckOutDate}");
            }

            booking.BookingId = Bookings.Count + 1;
            Bookings.Add(booking);
        }
    }



    public void Delete(int id) => Bookings.RemoveAll(b => b.BookingId == id);

    public List<Booking> GetByCheckInDateRange(DateTime? startDate, DateTime? endDate)
    {
        return Bookings
            .Where(b =>
                (!startDate.HasValue || b.CheckInDate >= startDate.Value) &&
                (!endDate.HasValue || b.CheckInDate <= endDate.Value))
            .ToList();
    }

    private bool IsOverlappingBooking(int roomId, DateTime checkIn, DateTime checkOut)
    {
        return Bookings.Any(b =>
            b.RoomId == roomId &&
            b.CheckInDate < checkOut &&
            b.CheckOutDate > checkIn
        );
    }

}
