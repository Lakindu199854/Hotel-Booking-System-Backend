
using System;
using System.Collections.Generic;
using System.Linq;
using BookingService.Model;
using BookingService.Data;
using BookingService.Dto;
using BookingService.Service.RoomService;
using BookingService.Service.CustomerService;
using Microsoft.EntityFrameworkCore;

namespace BookingService.Service.BookingService
{
    public class BookingServiceImpl : IBookingService
    {
        private readonly BookingDbContext _context;
        private readonly IRoomService _roomService;
        private readonly ICustomerService _customerService;

        public BookingServiceImpl(BookingDbContext context, IRoomService roomService, ICustomerService customerService)
        {
            _context = context;
            _roomService = roomService;
            _customerService = customerService;
        }


        public List<Booking> GetAll() => _context.Bookings.Include(b => b.Room).Include(b => b.Customer).Include(b => b.SpecialRequests).ToList();

        public Booking? GetById(int id) => _context.Bookings.Include(b => b.Room).Include(b => b.Customer).Include(b => b.SpecialRequests).FirstOrDefault(b => b.BookingId == id);

        public void Add(CreateBookingDTO booking)
        {
            if (booking.IsRecurring && booking.RecurrenceCount.HasValue && !string.IsNullOrEmpty(booking.RecurrenceType))
            {
                var currentCheckIn = booking.CheckInDate;
                var currentCheckOut = booking.CheckOutDate;

                for (int i = 0; i < booking.RecurrenceCount.Value; i++)
                {
                    if (IsOverlappingBooking(booking.RoomId, currentCheckIn, currentCheckOut))
                    {
                        throw new InvalidOperationException($"Room {booking.RoomId} is already booked between {currentCheckIn} and {currentCheckOut}");
                    }

                    var newBooking = new Booking
                    {
                        RoomId = booking.RoomId,
                        CustomerId = booking.CustomerId,
                        CheckInDate = currentCheckIn,
                        CheckOutDate = currentCheckOut,
                        IsRecurring = true,
                        RecurrenceCount = null,
                        RecurrenceType = null,
                        SpecialRequests = booking.SpecialRequests.Select(r => new SpecialRequest
                        {
                            Description = r.Description
                        }).ToList()
                    };

                    _context.Bookings.Add(newBooking);
                    _context.SaveChanges();

                    switch (booking.RecurrenceType.ToLower())
                    {
                        case "daily":
                            currentCheckIn = currentCheckIn.AddDays(1);
                            currentCheckOut = currentCheckOut.AddDays(1);
                            break;
                        case "weekly":
                            currentCheckIn = currentCheckIn.AddDays(7);
                            currentCheckOut = currentCheckOut.AddDays(7);
                            break;
                        case "monthly":
                            currentCheckIn = currentCheckIn.AddMonths(1);
                            currentCheckOut = currentCheckOut.AddMonths(1);
                            break;
                        default:
                            throw new ArgumentException("Invalid recurrence type.");
                    }
                }
            }
            else
            {
                if (IsOverlappingBooking(booking.RoomId, booking.CheckInDate, booking.CheckOutDate))
                {
                    throw new InvalidOperationException($"Room {booking.RoomId} is already booked between {booking.CheckInDate} and {booking.CheckOutDate}");
                }

                var newBooking = new Booking
                {
                    RoomId = booking.RoomId,
                    CustomerId = booking.CustomerId,
                    CheckInDate = booking.CheckInDate,
                    CheckOutDate = booking.CheckOutDate,
                    IsRecurring = false,
                    RecurrenceCount = null,
                    RecurrenceType = null,
                    SpecialRequests = booking.SpecialRequests.Select(r => new SpecialRequest
                    {
                        Description = r.Description
                    }).ToList()
                };

                _context.Bookings.Add(newBooking);
                _context.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            var booking = _context.Bookings.FirstOrDefault(b => b.BookingId == id);
            if (booking != null)
            {
                _context.Bookings.Remove(booking);
                _context.SaveChanges();
            }
        }

        public List<Booking> GetByCheckInDateRange(DateTime? startDate, DateTime? endDate)
        {
            return _context.Bookings
                .Where(b =>
                    (!startDate.HasValue || b.CheckInDate >= startDate.Value) &&
                    (!endDate.HasValue || b.CheckInDate <= endDate.Value))
                .Include(b => b.Room)
                .Include(b => b.Customer)
                .Include(b => b.SpecialRequests)
                .ToList();
        }

        private bool IsOverlappingBooking(int roomId, DateTime checkIn, DateTime checkOut)
        {
            return _context.Bookings.Any(b =>
                b.RoomId == roomId &&
                b.CheckInDate < checkOut &&
                b.CheckOutDate > checkIn
            );
        }

        private Room getRoomById(int roomId)
        {
            return _roomService.GetById(roomId);
        }

        private Customer getCustomerById(int customerId)
        {
            return _customerService.GetById(customerId);
        }
    }
}

