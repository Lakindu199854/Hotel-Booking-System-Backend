using HotelBookingAPI.model;

namespace HotelBookingAPI.Service.SpecialRequestService;

public interface ISpecialRequestService
{
    List<SpecialRequest> GetAll();
    void Add(SpecialRequest req);
    void Delete(int id);
}
