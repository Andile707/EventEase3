using Eventease.Data;
using Eventease.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Eventease.Controllers
{
    public class EventController : Controller
    {
        private readonly EventEaseDbContext _context;

        public EventController(EventEaseDbContext context)
        {
            _context = context;
        }

        // GET: Event
        public async Task<IActionResult> Index()
        {
            return View(await _context.Events.ToListAsync());
        }

        // GET: Event/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var eventModel = await _context.Events
                .Include(e => e.Bookings)
                .FirstOrDefaultAsync(e => e.EventId == id);

            if (eventModel == null)
                return NotFound();

            return View(eventModel);
        }

        // GET: Event/Create
        public IActionResult Create()
        {
            ViewBag.VenueId = new SelectList(
                _context.Venues,
                "venueId",
                "venueName");

            ViewBag.EventTypeId = new SelectList(
                _context.EventTypes,
                "EventTypeId",
                "EventTypeName");

            return View();
        }

        // POST: Event/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
    [Bind("EventId,VenueId,EventTypeId,EventName,EventDescription,EventDate")]
    EventModel eventModel)
        {
            if (ModelState.IsValid)
            {
                _context.Events.Add(eventModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.VenueId = new SelectList(
                _context.Venues,
                "venueId",
                "venueName",
                eventModel.VenueId);

            ViewBag.EventTypeId = new SelectList(
                _context.EventTypes,
                "EventTypeId",
                "EventTypeName",
                eventModel.EventTypeId);

            return View(eventModel);
        }

        // GET: Event/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var eventModel = await _context.Events.FindAsync(id);

            if (eventModel == null)
                return NotFound();

            return View(eventModel);
        }

        // POST: Event/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            [Bind("EventId,VenueId,EventName,EventDescription,EventDate")] EventModel eventModel)
        {
            if (id != eventModel.EventId)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Events.Update(eventModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventExists(eventModel.EventId))
                        return NotFound();

                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            return View(eventModel);
        }

        // GET: Event/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var eventModel = await _context.Events
                .FirstOrDefaultAsync(e => e.EventId == id);

            if (eventModel == null)
                return NotFound();

            return View(eventModel);
        }

        // POST: Event/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var eventItem = await _context.Events.FindAsync(id);

            bool hasBookings = await _context.Bookings
                .AnyAsync(b => b.EventId == id);

            if (hasBookings)
            {
                TempData["ErrorMessage"] =
                    "This event cannot be deleted because it has active bookings.";

                return RedirectToAction(nameof(Index));
            }

            if (eventItem != null)
            {
                _context.Events.Remove(eventItem);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool EventExists(int id)
        {
            return _context.Events.Any(e => e.EventId == id);
        }
    }
}
