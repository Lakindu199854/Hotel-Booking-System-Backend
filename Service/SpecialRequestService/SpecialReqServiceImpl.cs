using HotelBookingAPI.model;

namespace HotelBookingAPI.Service.SpecialRequestService;

public class SpecialRequestServiceImpl : ISpecialRequestService
{
    public List<SpecialRequest> specialReqList = new()
    {
        new SpecialRequest { RequestId = 1, Description = "Airport Pick Up" },
        new SpecialRequest { RequestId = 2, Description = "Extra Pillow" }
    };

    public List<SpecialRequest> GetAll() => specialReqList;

    public SpecialRequest? GetById(int id) => specialReqList.FirstOrDefault(r => r.RequestId == id);

    public void Add(SpecialRequest req) => specialReqList.Add(req);

    public void Delete(int id) => specialReqList.RemoveAll(r => r.RequestId == id);
}
