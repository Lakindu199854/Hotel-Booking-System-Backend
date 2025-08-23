using BookingService.Model;
using BookingService.Service.SpecialRequestService;
using Microsoft.AspNetCore.Mvc;

namespace BookingService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SpecialReqController : ControllerBase
    {
        private readonly ISpecialRequestService _specialReqService;

        public SpecialReqController(ISpecialRequestService specialReqService)
        {
            _specialReqService = specialReqService;
        }

        [HttpGet]
        public IActionResult GetAll() => Ok(_specialReqService.GetAll());

        //[HttpGet("{id}")]
        //public IActionResult GetById(int id)
        //{
        //    var specialRequest = _specialReqService.GetById(id);
        //    return specialRequest == null ? NotFound() : Ok(specialRequest);
        //}

        [HttpPost]
        public IActionResult Add(SpecialRequest specialRequest)
        {
            _specialReqService.Add(specialRequest);
            return Ok(specialRequest);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _specialReqService.Delete(id);
            return NoContent();
        }
    }
}
