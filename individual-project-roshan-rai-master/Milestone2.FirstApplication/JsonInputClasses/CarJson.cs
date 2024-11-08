using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Milestone2.FirstApplication.JsonInputClasses
{
    public class CarJson
    {
        public string VIN{get;set;}
        public string Year { get;set;}
        public MakeJson Make{get;set;}
        public string Model{get;set;}
        public string Color {get;set;}
        public string Type { get;set; }


    }
}
