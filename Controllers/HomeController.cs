using Eventease.Data;
using Eventease.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;
using System.Linq.Expressions;

namespace Eventease.Controllers
{
    public class HomeController(ILogger<HomeController> logger, EventEaseDbContext context) : Controller
    {
        private readonly ILogger<HomeController> _logger = logger;
        private readonly EventEaseDbContext _context = context;

       

        public IActionResult Index()
        {
            return View();
        }

        

        [HttpPost]

        public IActionResult Index(UploadModel model)
        {
            if (ModelState.IsValid)
            {
                // Process the uploaded file here
                // For example, you can save the file to the server or perform any necessary operations
                // After processing, you can redirect to a success page or return a view with a success message
                return RedirectToAction("Index");
            }
            // If the model state is not valid, return the view with validation errors
            return View(model);
        }


        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Booking()
        {
            return View("Booking");
        }




        public IActionResult Bookingview()
            { 
               return View("CreateBooking");
        }

        public IActionResult EditView()
        {
            return View("EditBooking");
        }


        public IActionResult DisplayBookings()
        {
            var Allbookings = _context.Bookings.ToList();
            return View(Allbookings);
        }

        /*public IActionResult EditBooking(int? BookingId)
        {
            if(BookingId != null) 
            {
                //find the booking in the database using the booking id
                var bookingInDb = _context.Bookings.SingleOrDefault(b => b.BookingId == BookingId);
                //pass the booking to the view
                return View(bookingInDb);
            }
            return View();
        }*/


        
        public IActionResult DeleteBooking(int BookingId)
        {
            //find the booking in the database using the booking id
            var booking = _context.Bookings.Find(BookingId);
            //remove the booking from the database
           if (booking == null)
            {
               return NotFound();
            }

            _context.Bookings.Remove(booking);
            _context.SaveChanges();

            return RedirectToAction("DisplayBookings");

        }

        
        public IActionResult CreateBooking(BookingModel model)
        {
            
          
                if (ModelState.IsValid)
                {
                    if(model.BookingId == 0)
                    {
                      _context.Bookings.Add(model);

                      _context.SaveChanges();
                      //return RedirectToAction(nameof(Index));
                      return View(model);

                    } 
                }
            
                _context.SaveChanges();
            //return RedirectToAction("Success");
            return RedirectToAction("Bookingview");

        }

        public IActionResult EditBooking(int BookingId)
        {
            
            var booking = _context.Bookings.Find(BookingId);
            
            
                if (booking == null)
                {
                return NotFound();
                    
                }
            

            return View(booking);

        }

        

        public IActionResult UpdateBooking(BookingModel model)
        {
           // if (!ModelState.IsValid)
          //  { 
                _context.Bookings.Update(model);
                _context.SaveChanges();
                return View("Booking");
            
            
            //}
            //return View(model);
        
        
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
