using Milestone3.Models;

namespace Milestone3.ViewModel
{
    public class CarOwnershipMakeViewModel
    {
        public string Vin { get;set;}
        public string Year { get;set;}
        public int MakeId { get;set;}
        public string Model { get;set;}
        public string Color { get;set;}
        public string Type { get;set;}
        public int OwnershipId { get;set;}
        public int OwnerId { get;set;}
        public DateTime PurchaseDate { get;set;}
        public DateTime? SaleDate { get;set;}
        public string BrandName { get;set;}

        public string CurrentOwner { get; set; }
        public Dictionary<Owner,List<string>> PreviousOwner { get;set;}

    }
}
