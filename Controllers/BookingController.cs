using Eventease.Data;
using Eventease.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Eventease.Controllers
{
    public class BookingController : Controller
    {
        private readonly EventEaseDbContext _context;

        public BookingController(EventEaseDbContext context)
        {
            _context = context;
        }

        // GET: Booking
        public async Task<IActionResult> Index()
        {
            var bookings = _context.Bookings
                .Include(b => b.Event)
                .Include(b => b.Venue);

            return View(await bookings.ToListAsync());
        }

        // GET: Booking/DisplayBookingsView
        public async Task<IActionResult> DisplayBookingsView(string searchTerm, DateOnly? bookingDate)
        {
            var bookings = _context.Bookings
                .Include(b => b.Event)
                .Include(b => b.Venue)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                bookings = bookings.Where(b =>
                    b.BookingId.ToString().Contains(searchTerm) ||
                    b.BookingName.Contains(searchTerm) ||
                    b.Event.EventName.Contains(searchTerm) ||
                    b.Venue.venueName.Contains(searchTerm));
            }

            if (bookingDate.HasValue)
            {
                bookings = bookings.Where(b =>
                    b.BookingDate == bookingDate.Value);
            }

            ViewBag.SearchTerm = searchTerm;
            ViewBag.BookingDate = bookingDate?.ToString("yyyy-MM-dd");

            return View(await bookings.ToListAsync());
        }

        // GET: Booking/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var booking = await _context.Bookings
                .Include(b => b.Event)
                .Include(b => b.Venue)
                .FirstOrDefaultAsync(b => b.BookingId == id);

            if (booking == null)
                return NotFound();

            return View(booking);
        }

        // GET: Booking/Create
        public IActionResult Create()
        {
            PopulateDropdowns();
            return View();
        }

        // POST: Booking/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("BookingId,BookingName,VenueId,EventId,BookingDate")] BookingModel booking)
        {
            if (ModelState.IsValid)
            {
                bool bookingExists = await _context.Bookings.AnyAsync(b =>
                    b.VenueId == booking.VenueId &&
                    b.BookingDate == booking.BookingDate);

                if (bookingExists)
                {
                    ModelState.AddModelError("",
                        "This venue is already booked on the selected date.");

                    PopulateDropdowns(booking.VenueId, booking.EventId);
                    return View(booking);
                }

                _context.Bookings.Add(booking);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            PopulateDropdowns(booking.VenueId, booking.EventId);
            return View(booking);
        }

        // GET: Booking/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var booking = await _context.Bookings.FindAsync(id);

            if (booking == null)
                return NotFound();

            PopulateDropdowns(booking.VenueId, booking.EventId);
            return View(booking);
        }

        // POST: Booking/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            [Bind("BookingId,BookingName,VenueId,EventId,BookingDate")] BookingModel booking)
        {
            if (id != booking.BookingId)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    bool bookingExists = await _context.Bookings.AnyAsync(b =>
                     b.VenueId == booking.VenueId &&
                     b.BookingDate == booking.BookingDate &&
                     b.BookingId != booking.BookingId);

                    if (bookingExists)
                    {
                        ModelState.AddModelError("",
                            "This venue is already booked on the selected date.");

                        PopulateDropdowns(booking.VenueId, booking.EventId);
                        return View(booking);
                    }
                    _context.Bookings.Update(booking);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookingExists(booking.BookingId))
                        return NotFound();

                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            PopulateDropdowns(booking.VenueId, booking.EventId);
            return View(booking);
        }

        // GET: Booking/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var booking = await _context.Bookings
                .Include(b => b.Event)
                .Include(b => b.Venue)
                .FirstOrDefaultAsync(b => b.BookingId == id);

            if (booking == null)
                return NotFound();

            return View(booking);
        }

        // POST: Booking/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);

            if (booking != null)
            {
                _context.Bookings.Remove(booking);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool BookingExists(int id)
        {
            return _context.Bookings.Any(b => b.BookingId == id);
        }

        private void PopulateDropdowns(int? selectedVenueId = null, int? selectedEventId = null)
        {
            ViewData["VenueId"] = new SelectList(
                _context.Venues,
                "venueId",
                "venueName",
                selectedVenueId
            );

            ViewData["EventId"] = new SelectList(
                _context.Events,
                "EventId",
                "EventName",
                selectedEventId
            );
        }
    }
}
