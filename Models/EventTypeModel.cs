using System.ComponentModel.DataAnnotations;

namespace Eventease.Models
{
    public class EventTypeModel
    {
        [Key]
        public int EventTypeId { get; set; }

        public string EventTypeName { get; set; } = "";

        public ICollection<EventModel>? Events { get; set; }
    }
}