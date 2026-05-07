using System.ComponentModel.DataAnnotations;

namespace EventManagementCore.Models
{
    public class Event
    {

       

        public int EventId { get; set; }
        [Display(Name = "Event Name"), Required]
        public string? EventName { get; set; }
        public bool IsActive { get; set; } = true;
        public virtual ICollection<EventService> EventServices { get; set; } = new List<EventService>();
        


    }
}

