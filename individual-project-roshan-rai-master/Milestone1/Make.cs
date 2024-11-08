using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Milestone1
{
    public class Make:IComparable<Make>
    {
        //Volkswagen|Oliver Blume|1937|https://www.vw.com/
        public string Name{get;set;}
        public string CEO{get;set;}
        public string FoundingYear{get;set;}
        public string Website {get;set; }
        public Make()
        {
            //Cars = new List<Car>();//always initialize the list in all the ctor
            Name = string.Empty; 
            CEO=string.Empty; 
            FoundingYear = string.Empty;
            Website= string.Empty;
        }
        public Make(string name, string ceo, string foundingYear, string website)
        {
            //Cars = new List<Car>();//always initialize the list in all the ctor
            Name = name;
            CEO = ceo;
            FoundingYear = foundingYear;
            Website = website;
        }
        public override string ToString()
        {
            return $"{Name}|{CEO}|{FoundingYear}|{Website}";
        }
        public int CompareTo(Make make)
        {
            // Compare based on the 'Name' property (ascending order)
            return string.Compare(Name, make.Name, StringComparison.Ordinal);
        }

    }
}
