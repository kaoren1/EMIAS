using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMIAS.Model
{
    public partial class Speciality
    {
        public int? IdSpeciality { get; set; }

        public string Name { get; set; } = string.Empty;

        public string ImagePath { get; set; }

    }
}
