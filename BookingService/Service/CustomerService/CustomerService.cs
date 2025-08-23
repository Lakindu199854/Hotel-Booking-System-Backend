using BookingService.Model;

namespace BookingService.Service.CustomerService;

public interface ICustomerService
{
    List<Customer> GetAll();
    Customer? GetById(int id);
    void Add(Customer customer);
}
