using System.Collections.Generic;
using System.Linq;
using BookingService.Model;
using BookingService.Data;
namespace BookingService.Service.CustomerService
{
    public class CustomerServiceImpl : ICustomerService
    {
        private readonly BookingDbContext _context;

        public CustomerServiceImpl(BookingDbContext context)
        {
            _context = context;
        }

        public List<Customer> GetAll() => _context.Customers.ToList();

        public Customer? GetById(int id) => _context.Customers.FirstOrDefault(c => c.CustomerId == id);

        public void Add(Customer customer)
        {
            _context.Customers.Add(customer);
            _context.SaveChanges();
        }
    }
}
