using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMIAS.Model
{
    public class UserChoose
    {     
        public UserChoose(DateTime date)
        {
            this.date = date;
        }

        public DateTime date { get; private set; }
     
    }
}
