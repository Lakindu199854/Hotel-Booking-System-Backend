using HotelBookingAPI.model;
using HotelBookingAPI.Service.BookingService;
using Microsoft.AspNetCore.Mvc;

namespace HotelBookingAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _bookingService;

        public BookingController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        [HttpGet]
        public IActionResult GetAll() => Ok(_bookingService.GetAll());

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var booking = _bookingService.GetById(id);
            return booking == null ? NotFound() : Ok(booking);
        }

        [HttpPost]
        public IActionResult Add(Booking booking)
        {
            _bookingService.Add(booking);
            return Ok(booking);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _bookingService.Delete(id);
            return NoContent();
        }


        [HttpGet("filter")]
        public IActionResult FilterByDates(DateTime? startDate, DateTime? endDate)
        {
            var results = _bookingService.GetByCheckInDateRange(startDate, endDate);
            return Ok(results);
        }

    }
}
