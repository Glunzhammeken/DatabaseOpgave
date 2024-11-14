using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class facility
    {
        public int Facility_No { get; set; }
        public string Name { get; set; }

        

        public override string ToString()
        {
            return $"ID: {Facility_No}, Name: {Name}";
        }
    }
}
