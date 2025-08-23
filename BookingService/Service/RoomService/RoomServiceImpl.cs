using System;
using System.Collections.Generic;
using System.Linq;
using BookingService.Model;
using BookingService.Data;
using Microsoft.EntityFrameworkCore;

namespace BookingService.Service.RoomService
{
    public class RoomServiceImpl : IRoomService
    {
        private readonly BookingDbContext _context;

        public RoomServiceImpl(BookingDbContext context)
        {
            _context = context;
        }

        public List<Room> GetAll() => _context.Rooms.ToList();

        public Room? GetById(int id) => _context.Rooms.FirstOrDefault(r => r.RoomId == id);

        public void Add(Room room)
        {
            _context.Rooms.Add(room);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var room = _context.Rooms.FirstOrDefault(r => r.RoomId == id);
            if (room != null)
            {
                _context.Rooms.Remove(room);
                _context.SaveChanges();
            }
        }

        public List<Room> GetAvailableRoomsByDateRange(DateTime checkIn, DateTime checkOut)
        {
            var bookedRoomIds = _context.Bookings
                .Where(b => !(b.CheckOutDate <= checkIn || b.CheckInDate >= checkOut))
                .Select(b => b.RoomId)
                .ToHashSet();

            return _context.Rooms
                .Where(r => !bookedRoomIds.Contains(r.RoomId))
                .ToList();
        }
    }
}
