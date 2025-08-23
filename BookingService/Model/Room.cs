using System.ComponentModel.DataAnnotations;
namespace BookingService.Model
{
    public class Room
    {
        public Room()
        {
            IsOccupied = false;
        }
    [Key]
    public int RoomId { get; set; }
        public string RoomNumber { get; set; } = string.Empty;
        public string RoomType { get; set; } = string.Empty;
        public Boolean IsOccupied { get; set; } = false;
    }
}
