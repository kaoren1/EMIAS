using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMIAS.Model
{
    public partial class Doctor1
    {
        public int? IdDoctor { get; set; }

        public string Surname { get; set; }

        public string Name { get; set; }

        public string Patronymic { get; set; }

        public int SpecialityId { get; set; }

        public string EnterPassword { get; set; }

        public string WorkAddress { get; set; }

        public Doctor1()
        {
            Surname = string.Empty;
            Name = string.Empty;
            Patronymic = string.Empty;
            EnterPassword = string.Empty;
            WorkAddress = string.Empty;
        }
    }
}
