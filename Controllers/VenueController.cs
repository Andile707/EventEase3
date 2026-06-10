using Eventease.Data;
using Eventease.Models;
using Eventease.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Eventease.Controllers
{
    public class VenueController : Controller
    {
        private readonly EventEaseDbContext _context;
        private readonly IAzureService _azureService;

        public VenueController(EventEaseDbContext context, IAzureService azureService)
        {
            _context = context;
            _azureService = azureService;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Venues.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var venue = await _context.Venues
                .Include(v => v.Bookings)
                .FirstOrDefaultAsync(v => v.venueId == id);

            if (venue == null)
                return NotFound();

            return View(venue);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(VenueModel venue)
        {
            if (venue.venueImageFile != null && venue.venueImageFile.Length > 0)
            {
                venue.venueImage =
                    await _azureService.UploadFiles(venue.venueImageFile);
            }

            ModelState.Remove("venueImage");

            if (ModelState.IsValid)
            {
                _context.Venues.Add(venue);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(venue);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var venue = await _context.Venues.FindAsync(id);

            if (venue == null)
                return NotFound();

            return View(venue);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, VenueModel venue)
        {
            if (id != venue.venueId)
                return NotFound();

            ModelState.Remove("venueImage");

            if (ModelState.IsValid)
            {
                var existingVenue = await _context.Venues.FindAsync(id);

                if (existingVenue == null)
                    return NotFound();

                existingVenue.venueName = venue.venueName;
                existingVenue.venueLocation = venue.venueLocation;
                existingVenue.venueCapacity = venue.venueCapacity;
                existingVenue.IsAvailable = venue.IsAvailable;

                if (venue.venueImageFile != null && venue.venueImageFile.Length > 0)
                {
                    existingVenue.venueImage =
                        await _azureService.UploadFiles(venue.venueImageFile);
                }

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(venue);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var venue = await _context.Venues
                .FirstOrDefaultAsync(v => v.venueId == id);

            if (venue == null)
                return NotFound();

            return View(venue);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var venue = await _context.Venues.FindAsync(id);

            bool hasBookings = await _context.Bookings
                .AnyAsync(b => b.VenueId == id);

            if (hasBookings)
            {
                TempData["ErrorMessage"] =
                    "This venue cannot be deleted because it has active bookings.";

                return RedirectToAction(nameof(Index));
            }

            if (venue != null)
            {
                _context.Venues.Remove(venue);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool VenueExists(int id)
        {
            return _context.Venues.Any(v => v.venueId == id);
        }
    }
}
