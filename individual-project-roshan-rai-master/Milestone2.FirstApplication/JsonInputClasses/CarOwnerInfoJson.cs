using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Milestone2.FirstApplication.JsonInputClasses
{
    public class CarOwnerInfoJson
    {
        public string VIN { get; set; }
        public List<OwnerJson> Owners { get; set; }


    }
}
