using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMIAS.Model
{
    public partial class Patient
    {
        public long Oms { get; set; }

        public string Surname { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public string Patronymic { get; set; } = string.Empty;

        public DateTime BirthDate { get; set; }

        public string Address { get; set; } = string.Empty;

        public string LivingAddress { get; set; } = string.Empty;

        public string Phone { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Nickname { get; set; } = string.Empty;
    }
}
