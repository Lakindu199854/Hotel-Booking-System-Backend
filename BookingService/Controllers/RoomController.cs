using HotelBookingAPI.model;
using HotelBookingAPI.Service.RoomService;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace HotelBookingAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoomController : ControllerBase
    {
        private readonly IRoomService _roomService;
        private readonly ILogger<RoomController> _logger;

        public RoomController(ILogger<RoomController> logger,IRoomService roomService)
        {
            _logger = logger;
            _roomService = roomService;
        }

        [HttpGet]
        public IActionResult GetAll() => Ok(_roomService.GetAll());

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var room = _roomService.GetById(id);
            return room == null ? NotFound() : Ok(room);
        }

        [HttpPost]
        public IActionResult Add(Room room)
        {
            _roomService.Add(room);
               return Ok(room);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _roomService.Delete(id);
            return NoContent();
        }

        [HttpGet("available")]
        public IActionResult GetAvailableRooms([FromQuery] DateTime checkIn, [FromQuery] DateTime checkOut)
        {
            if (checkIn >= checkOut)
            {
                return BadRequest("Check-out date must be after check-in date.");
            }

            var availableRooms = _roomService.GetAvailableRoomsByDateRange(checkIn, checkOut);
            return Ok(availableRooms);
        }
    }
}
