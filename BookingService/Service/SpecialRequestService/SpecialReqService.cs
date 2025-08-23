using BookingService.Model;

namespace BookingService.Service.SpecialRequestService;

public interface ISpecialRequestService
{
    List<SpecialRequest> GetAll();
    void Add(SpecialRequest req);
    void Delete(int id);
}
