using Eventease.Data;
using Eventease.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Eventease.Controllers
{
    public class BookingDisplay : Controller
    {
        private readonly EventEaseDbContext _context;

        public BookingDisplay(EventEaseDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
       /*
        public async Task<IActionResult> DisplayBookingsView()
        {
            var bookings = await _context.Bookings
                .Include(b => b.Event)
                .Include(b => b.Venue)
                .Select(b => new BookingDisplayViewModel
                {
                    BookingId = b.BookingId,
                    EventName = b.Event!.EventName,
                    VenueName = b.Venue!.venueName,
                    BookingDate = b.BookingDate
                })
                .ToListAsync();

            return View(bookings);
        }*/

        public async Task<IActionResult> DisplayByString(string searchTerm)
        {
            var query = _context.Bookings
                .Include(b => b.Event)
                .Include(b => b.Venue)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(b =>
                    b.Event.EventName.Contains(searchTerm) ||
                    b.Venue.venueName.Contains(searchTerm));
            }

            var bookings = await query
                .Select(b => new BookingDisplayViewModel
                {
                    BookingId = b.BookingId,
                    EventName = b.Event!.EventName,
                    VenueName = b.Venue!.venueName,
                    BookingDate = b.BookingDate
                })
                .ToListAsync();

            return View(bookings);
           //return View("DisplayBookingsView");
        }

        public async Task<IActionResult>
DisplayBookingsView(string searchTerm, DateOnly? bookingDate)
        {
            var query = _context.Bookings
                .Include(b => b.Event)
                .Include(b => b.Venue)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(b =>
                    b.BookingId.ToString().Contains(searchTerm) ||
                    b.Event.EventName.Contains(searchTerm) ||
                    b.Venue.venueName.Contains(searchTerm));
            }

            if (bookingDate.HasValue)
            {
                query = query.Where(b =>
                    b.BookingDate == bookingDate.Value);
            }

            var bookings = await query
                .Select(b => new BookingDisplayViewModel
                {
                    BookingId = b.BookingId,
                    EventName = b.Event.EventName,
                    VenueName = b.Venue.venueName,
                    BookingDate = b.BookingDate
                })
                .ToListAsync();

            return View(bookings);
        }
    }
}
