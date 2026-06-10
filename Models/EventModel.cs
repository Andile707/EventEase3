using System.ComponentModel.DataAnnotations;

namespace Eventease.Models
{
    public class EventModel
    {

        [Key]
        public int EventId { get; set; }

        public int VenueId { get; set; }


        public string EventName { get; set; } = "";

        public string? EventDescription { get; set; }

        public DateOnly EventDate { get; set; }

        public ICollection<BookingModel>? Bookings { get; set; }

        public int EventTypeId { get; set; }

        public EventTypeModel? EventType { get; set; }

    }
}