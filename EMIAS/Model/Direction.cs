using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMIAS.Model
{
    public partial class Direction
    {
        public int? IdDirection { get; set; }

        public int SpecialityId { get; set; } 

        public long Oms { get; set; }

        public Speciality Speciality { get; set; }
    }
}
