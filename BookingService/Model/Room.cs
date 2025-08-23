namespace HotelBookingAPI.model
{
    public class Room
    {
        public Room()
        {
            IsOccupied = false;
        }
        public int RoomId { get; set; }
        public string RoomNumber { get; set; } = string.Empty;
        public string RoomType { get; set; } = string.Empty;
        public Boolean IsOccupied { get; set; } = false;
    }
}
