using System;
namespace HotelBookingAPI.model
{
    public class Booking
    {
        public int BookingId { get; set; }

        // Replace RoomId and CustomerId with object references
        public Room RoomId { get; set; }
        public Customer CustomerId { get; set; }

        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }

        public bool IsRecurring { get; set; }
        public int? RecurrenceCount { get; set; }
        public string? RecurrenceType { get; set; }

        public List<SpecialRequest> SpecialRequests { get; set; } = new();
    }
}
