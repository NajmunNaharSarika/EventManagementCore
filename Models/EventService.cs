using EventManagementCore.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventManagementCore.Models
{
    public class EventService
    {

        public int EventServiceId { get; set; }
        [ForeignKey("Event")]
        public int EventId { get; set; }
        [ForeignKey("Client")]
        public int ClientId { get; set; }
        public virtual Event? Event { get; set; }
        public virtual Client? Client { get; set; }


       
    }
}
