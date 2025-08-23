using BookingService.Dto;
using BookingService.Model;
using BookingService.Service.CustomerService;
using Microsoft.AspNetCore.Mvc;

namespace BookingService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet]
        public IActionResult GetAll() => Ok(_customerService.GetAll());

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var customer = _customerService.GetById(id);
            return customer == null ? NotFound() : Ok(customer);
        }

        [HttpPost]
        public IActionResult Add(CreateCustomerDto customerDto)
        {
            var newCustomer = new Customer
            {
                Name = customerDto.Name,
                Email = customerDto.Email,
                Phone = customerDto.Phone
            };
            _customerService.Add(newCustomer);
            return Ok(newCustomer);
        }
    }
}
