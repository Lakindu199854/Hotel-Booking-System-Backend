using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace BookingService.Model
{
    public class Booking
    {
    [Key]
    public int BookingId { get; set; }

    [ForeignKey("Room")]
    public int RoomId { get; set; }
    public Room Room { get; set; }
    [ForeignKey("Customer")]
    public int CustomerId { get; set; }
    public Customer Customer { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }

        public bool IsRecurring { get; set; }
        public int? RecurrenceCount { get; set; }
        public string? RecurrenceType { get; set; }

    public List<SpecialRequest> SpecialRequests { get; set; } = new();
    }
}
