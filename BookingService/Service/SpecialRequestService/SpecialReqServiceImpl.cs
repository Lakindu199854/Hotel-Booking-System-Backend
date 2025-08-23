using System.Collections.Generic;
using System.Linq;
using BookingService.Model;
using BookingService.Data;
namespace BookingService.Service.SpecialRequestService
{
    public class SpecialReqServiceImpl : ISpecialRequestService
    {
        private readonly BookingDbContext _context;

        public SpecialReqServiceImpl(BookingDbContext context)
        {
            _context = context;
        }

        public List<SpecialRequest> GetAll() => _context.SpecialRequests.ToList();

        public SpecialRequest? GetById(int id) => _context.SpecialRequests.FirstOrDefault(r => r.RequestId == id);

        public void Add(SpecialRequest req)
        {
            _context.SpecialRequests.Add(req);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var req = _context.SpecialRequests.FirstOrDefault(r => r.RequestId == id);
            if (req != null)
            {
                _context.SpecialRequests.Remove(req);
                _context.SaveChanges();
            }
        }
    }
}
