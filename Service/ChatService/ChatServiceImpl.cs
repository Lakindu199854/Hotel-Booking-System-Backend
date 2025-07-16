namespace Hotel_Booking_App.Service.ChatService
{
    using HotelBookingAPI.model;
    using HotelBookingAPI.Service.BookingService;
    using HotelBookingAPI.Service.RoomService;
    using System.Text.RegularExpressions;

    public class ChatServiceImpl : ChatService
    {
        private readonly IBookingService _bookingService;
        private readonly IRoomService _roomService;

        public ChatServiceImpl(IBookingService bookingService, IRoomService roomService)
        {
            _bookingService = bookingService;
            _roomService = roomService;
        }

        public string GetResponse(string userMessage)
        {
            var input = userMessage.ToLower();

            // Option: Predict Availability
            if (input.Contains("predict") && input.Contains("availability"))
            {
                string extractedDate = ExtractDateFromInput(userMessage);
                if (DateTime.TryParse(extractedDate, out var date))
                {
                    var prediction = PredictAvailabilityAndPrice(date);
                    return $"For {date:yyyy-MM-dd}, expected availability is **{prediction.Availability}** and price trend is **{prediction.PriceTrend}**.";
                }

                return "Please enter a valid date in the format YYYY-MM-DD to predict availability.";
            }

            // Option: Check Available Rooms
            if (input.Contains("available rooms"))
            {
                string extractedDate = ExtractDateFromInput(userMessage);
                if (DateTime.TryParse(extractedDate, out var date))
                {
                    return GetRoomAvailabilityByType(date);
                }

                return "Please enter a valid date in the format YYYY-MM-DD to check available rooms.";
            }

            // Option: Check-in time
            if (input.Contains("check-in"))
            {
                return "Check-in time is from 2 PM onward. Check-out is before 12 PM.";
            }

            return "I'm not sure how to help with that. You can ask me things like";
                   
        }

        // -----------------------------
        // Helper Methods
        // -----------------------------

        private string ExtractDateFromInput(string input)
        {
            var match = Regex.Match(input, @"\d{4}-\d{2}-\d{2}");
            return match.Success ? match.Value : null;
        }

        private (string Availability, string PriceTrend) PredictAvailabilityAndPrice(DateTime date)
        {
            if (IsHoliday(date))
                return ("Very Low", "Very High");

            if (IsWeekend(date))
                return ("Low", "High");

            if (IsOffSeason(date))
                return ("High", "Low");

            return ("Moderate", "Stable");
        }

        private bool IsWeekend(DateTime date) =>
            date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday;

        private bool IsHoliday(DateTime date)
        {
            var holidays = new List<DateTime>
            {
                new DateTime(2025, 12, 25),
                new DateTime(2025, 12, 31),
                new DateTime(2025, 4, 14)
            };

            return holidays.Contains(date.Date);
        }

        private bool IsOffSeason(DateTime date) =>
            date.Month == 6 || date.Month == 7;

        private string GetRoomAvailabilityByType(DateTime date)
        {
            List<Room> allRooms = _roomService.GetAll();
            var bookings = _bookingService.GetAll();

            var bookedRoomIds = bookings
                .Where(b => b.CheckInDate < date && b.CheckOutDate > date)
                .Select(b => b.RoomId)
                .ToList();

            var availableRooms = allRooms
                .Where(room => !bookedRoomIds.Contains(room.RoomId))
                .GroupBy(r => r.RoomType)
                .Select(g => new
                {
                    RoomType = g.Key,
                    Count = g.Count()
                });

            if (!availableRooms.Any())
                return $"No rooms are available on {date:yyyy-MM-dd}.";

            var resultLines = availableRooms.Select(r => $"{r.RoomType}: {r.Count}");
            return $"Available rooms for {date:yyyy-MM-dd}:\n" + string.Join("\n", resultLines);
        }
    }
}
