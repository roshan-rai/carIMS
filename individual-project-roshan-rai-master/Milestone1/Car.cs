using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace Milestone1
{
    public class Car
    {
       // VIN Year    Make Model   Owners Color   Type
       public string VIN{get;set;}
       public string Year{get;set;}
       public Make Make{get;set;}
       public string Model{get;set;}
       public List<Owner> Owners {get;set; }
       public string Color{get;set;}
       public string Type {get;set; }

        
        public Car(string vin, string year, Make make, string model, string color, string type)
        {
            VIN = vin;
            Year = year;
            Make = make;
            Model = model;
            Color = color; 
            Type = type;
            Owners = new List<Owner>();


        }
        public override string ToString()
        {
            return $"{VIN} {Color} {Model} {Type}";
        }

    }
}
