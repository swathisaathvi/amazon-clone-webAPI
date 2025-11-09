using Microsoft.Identity.Client;
using Microsoft.VisualBasic;

namespace amazonCloneWebAPI.Models
{
    public class Appointments
    {
        public int AppointmentId { get; set; }
        public DateAndTime? AppointmentDate { get; set; }
        public string? PhysicianName { get; set;}
    }
}
