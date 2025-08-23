using HotelBookingAPI.model;

namespace Hotel_Booking_App.Dto;
public class CreateBookingDTO
{
    public int RoomId { get; set; }
    public int CustomerId { get; set; }
    public DateTime CheckInDate { get; set; }
    public DateTime CheckOutDate { get; set; }

    public bool IsRecurring { get; set; }
    public int? RecurrenceCount { get; set; }
    public string? RecurrenceType { get; set; }

    public List<SpecialRequest> SpecialRequests { get; set; } = new();
}
