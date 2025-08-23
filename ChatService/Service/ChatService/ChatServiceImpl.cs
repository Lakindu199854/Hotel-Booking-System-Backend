namespace ChatService.Service.ChatService
{
    using System.Text.RegularExpressions;

    public class ChatServiceImpl : IChatService
    {
        public ChatServiceImpl()
        {
        }

        public string GetResponse(string userMessage)
        {
            var input = userMessage.ToLower();

            // Option: Predict Availability
            if (input.Contains("predict") && input.Contains("availability"))
            {
                 return "Predicting availability is not yet implemented in this service.";
            }

            // Option: Check Available Rooms
            if (input.Contains("available rooms"))
            {
                return "Checking available rooms is not yet implemented in this service.";
            }

            // Option: Check-in time
            if (input.Contains("check-in"))
            {
                return "Standard check-in time is at 2:00 PM.";
            }

            // Option: Check-out time
            if (input.Contains("check-out"))
            {
                return "Standard check-out time is at 12:00 PM.";
            }

            // Option: Contact
            if (input.Contains("contact") || input.Contains("help"))
            {
                return "For assistance, please call our support at +123456789.";
            }

            // Default response
            return "I'm sorry, I don't understand. You can ask about 'available rooms', 'check-in time', or 'contact'.";
        }

        private string ExtractDateFromInput(string input)
        {
            var datePattern = @"\d{4}-\d{2}-\d{2}";
            var match = Regex.Match(input, datePattern);
            return match.Success ? match.Value : string.Empty;
        }

        private string GetRoomAvailabilityByType(DateTime date)
        {
            return $"Checking for rooms on {date:yyyy-MM-dd} is not implemented yet.";
        }

        private (string Availability, string PriceTrend) PredictAvailabilityAndPrice(DateTime date)
        {
            return ("N/A", "N/A");
        }
    }
}
