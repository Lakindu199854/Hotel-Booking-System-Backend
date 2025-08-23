using HotelBookingAPI.model;

namespace HotelBookingAPI.Service.CustomerService;

public class CustomerServiceImpl : ICustomerService
{
    public List<Customer> Customers { get; set; } = new();

    public List<Customer> GetAll() => Customers;

    public Customer? GetById(int id) => Customers.FirstOrDefault(c => c.CustomerId == id);

    public void Add(Customer customer)
    {
        customer.CustomerId = Customers.Count + 1;
        Customers.Add(customer);
    }
}
