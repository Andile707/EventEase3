using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eventease.Models
{
    public class VenueModel
    {
        [Key]
        public int venueId { get; set; } = 0;
        public string venueName { get; set; } = "";
        public string? venueLocation { get; set; }
        public int venueCapacity { get; set; }

        public string? venueImage { get; set; }

        [Display(Name = "Select Venue Image")]
        [NotMapped]
        public IFormFile? venueImageFile { get; set; }

        public ICollection<BookingModel>? Bookings { get; set; }
    }
}
