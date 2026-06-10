using Eventease.Data;
using Eventease.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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


        public async Task<IActionResult> DisplayBookingsView(
            string searchTerm,
            int? eventTypeId,
            int? venueId,
            DateOnly? startDate,
            DateOnly? endDate,
            bool availableOnly = false)
        {
            var query = _context.Bookings
                .Include(b => b.Event)
                    .ThenInclude(e => e.EventType)
                .Include(b => b.Venue)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(b =>
                    b.BookingId.ToString().Contains(searchTerm) ||
                    b.Event.EventName.Contains(searchTerm) ||
                    b.Venue.venueName.Contains(searchTerm));
            }

            if (eventTypeId.HasValue)
            {
                query = query.Where(b =>
                    b.Event.EventTypeId == eventTypeId.Value);
            }

            if (venueId.HasValue)
            {
                query = query.Where(b =>
                    b.VenueId == venueId.Value);
            }

            if (startDate.HasValue)
            {
                query = query.Where(b =>
                    b.BookingDate >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                query = query.Where(b =>
                    b.BookingDate <= endDate.Value);
            }

            if (availableOnly)
            {
                query = query.Where(b => b.Venue.IsAvailable);
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

            ViewBag.SearchTerm = searchTerm;
            ViewBag.StartDate = startDate?.ToString("yyyy-MM-dd");
            ViewBag.EndDate = endDate?.ToString("yyyy-MM-dd");

            ViewBag.EventTypes = new SelectList(
    _context.EventTypes,
    "EventTypeId",
    "EventTypeName",
    eventTypeId);

            ViewBag.Venues = new SelectList(
                _context.Venues,
                "venueId",
                "venueName",
                venueId);

            ViewBag.AvailableOnly = availableOnly;
            return View(bookings);
        }
    }
}
