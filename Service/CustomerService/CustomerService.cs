using HotelBookingAPI.model;

namespace HotelBookingAPI.Service.CustomerService;

public interface ICustomerService
{
    List<Customer> GetAll();
    Customer? GetById(int id);
    void Add(Customer customer);
}
