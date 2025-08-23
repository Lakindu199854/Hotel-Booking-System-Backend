using System.ComponentModel.DataAnnotations;
namespace BookingService.Model
{
    public class SpecialRequest
    {
    [Key]
    public int RequestId { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}
