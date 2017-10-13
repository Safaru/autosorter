using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostCodeSorter
{
    class Courier
    {
        public int CourierNumber { get; set; }

        public List<string> DaysProcessed { get; set; }

        public  PostcodeMatrix postcodeMatrix { get; set; }

        public Courier(string csvLocation)
        {
             
        }
    }
}
