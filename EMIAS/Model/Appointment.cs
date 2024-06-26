using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMIAS.Model
{
    public class Appointment
    {
        public int IdAppointment { get; set; }
        public long Oms { get; set; }
        public int DoctorId { get; set; }
        public DateTime AppointmentDate { get; set; }
        public TimeSpan AppointmentTime { get; set; }
        public int StatusId { get; set; }
        public Doctor Doctor { get; set; }
    }

}

