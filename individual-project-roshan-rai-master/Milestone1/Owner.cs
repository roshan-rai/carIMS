using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Milestone1
{
    public class Owner: IComparable<Owner>
    {
        //Coretta Pomfret;3/4/2004
        public string Name{get;set;}
        public DateTime PurchaseDate{get;set;}
        public DateTime? SaleDate {get;set; }//question says can be null

        //public List<Car> Cars { get;set;}
        public Owner()
        {
            //Cars = new List<Car>();//always initialize the list in all the ctor
            Name = string.Empty;
            PurchaseDate = new DateTime();
            
        }
        public Owner(string name, DateTime purchaseDate, DateTime saleDate)
        {
            //Cars = new List<Car>();//always initialize the list in all the ctor
            Name = name;
            PurchaseDate = purchaseDate;  
            SaleDate = saleDate;                       
        }

        public Owner(string name, DateTime purchaseDate)
        {
            //Cars = new List<Car>();//always initialize the list in all the ctor
            Name = name;
            PurchaseDate = purchaseDate;
        }
        public override string ToString() 
        {
            return $"{Name}";
        }

        public int CompareTo(Owner owner)
        {
            // Compare based on the 'Name' property (ascending order)
            return string.Compare(Name, owner.Name, StringComparison.Ordinal);
        }
    }
}
