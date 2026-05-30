namespace Eventease.Models
{
    public class BookingDisplayViewModel
    {
        public int BookingId { get; set; }

        public string? EventName { get; set; }

        public string? VenueName { get; set; }

        public string? CustomerName { get; set; }

        public DateOnly BookingDate { get; set; }
    }
}
