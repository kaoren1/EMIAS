using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMIAS.Model
{
    public partial class AppointmentDocument
    {
        public int? IdAppointmentDocumentt { get; set; }

        public int AppointmentId { get; set; }

        public string Rtf { get; set; } = null!;
    }
}