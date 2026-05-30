using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;



namespace Eventease.Models
{
    
    public class BookingModel
    {

        // This class represents a booking in the system. It contains properties for the booking ID, name, venue ID, event ID, and booking date.
        [Key]
        
        public int BookingId { get; set; } 

        [Required(ErrorMessage = "Booking name is required")]
         public string BookingName { get; set; } = string.Empty;


        [Required]
        public int VenueId { get; set; } 


        [Required]
        public int EventId { get; set; } 


        [Required]
        public DateOnly BookingDate { get; set; }

       
     // Navigation Properties

    [ValidateNever]
    public EventModel Event { get; set; } = null!;

    [ValidateNever]
    public VenueModel Venue { get; set; } = null!;



}
}
