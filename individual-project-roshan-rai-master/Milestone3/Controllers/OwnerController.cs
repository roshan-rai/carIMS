using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Differencing;
using Microsoft.EntityFrameworkCore;
using Milestone3.Models;
using Milestone3.ViewModel;
using System.Runtime.CompilerServices;

namespace Milestone3.Controllers
{
    public class OwnerController : Controller
    {
        private Milestone3DbContext _context;
        public OwnerController(Milestone3DbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> GetAllOwners(string nameToSearch)
        {
            var owners = await _context.Owners.ToListAsync();

            if (nameToSearch != null)
            {
                 owners = await _context.Owners.Where(o=>o.OwnerName.Contains(nameToSearch)).ToListAsync();

            }
            return View(owners);
        }
     
        public async Task<IActionResult> GetCars(int id)
        {
            if (id == 0 || _context.Cars == null)
            {
                return NotFound();
            }
            var vins = await _context.CarOwnerships.Where(co => co.OwnerId == id).Select(co => co.Vin).ToListAsync();

            string carOwnerName = _context.Owners.Find(id).OwnerName;//id pk ho

            List<CarOwnershipMakeViewModel> allInfoOwner= new List<CarOwnershipMakeViewModel>();
            foreach (var v in vins)
            {
                CarOwnershipMakeViewModel allInfo= new CarOwnershipMakeViewModel();
                allInfo.Vin = v;
                //purchase date, all past owner name, make, 
                allInfo.OwnerId= id;
                allInfo.CurrentOwner= carOwnerName;
              
                var makeID = _context.Cars.Where(a => a.Vin == v).FirstOrDefault().MakeId;
                var brandName = _context.Makes.Where(a => a.MakeId == makeID).FirstOrDefault().BrandName;
                allInfo.BrandName = brandName;

                var model = _context.Cars.Where(a => a.Vin == v).FirstOrDefault().Model;
                allInfo.Model = model;

                var type = _context.Cars.Where(a => a.Vin == v).FirstOrDefault().Type;
                allInfo.Type = type;

                var color = _context.Cars.Where(a => a.Vin == v).FirstOrDefault().Color;
                allInfo.Color = color;

                DateTime purchaseDate = _context.CarOwnerships.Where(x => x.OwnerId == id && x.Vin == v).FirstOrDefault().PurchaseDate;
                DateTime? saleDate = _context.CarOwnerships.Where(x => x.OwnerId == id && x.Vin == v).FirstOrDefault().SaleDate;

                string year = _context.Cars.Where(y=>y.Vin==v).FirstOrDefault().Year;
                allInfo.Year = year;

                List<CarOwnership> previousOwners = _context.CarOwnerships.Where(x => (x.Vin == v)).ToList();
                Dictionary <Owner,List<string>> previousOwnersTBR = new Dictionary<Owner, List<string>>();
                if (previousOwners.Count > 0)
                {
                    foreach (CarOwnership previousOwner in previousOwners)
                    {
                        Owner pOwner = _context.Owners.Find(previousOwner.OwnerId);//key of the dictionary
                        string purchaseDatePreviousOwner = _context.CarOwnerships.Where(x => x.OwnerId == previousOwner.OwnerId && x.Vin == v).FirstOrDefault().PurchaseDate.ToString();
                        string saleDatePreviousOwner = _context.CarOwnerships.Where(x => x.OwnerId == previousOwner.OwnerId && x.Vin == v).FirstOrDefault().SaleDate.ToString();

                        List<string> previousOwnerDateTimes = new List<string>();//to assign the value of of the dictionary
                        previousOwnerDateTimes.Add(purchaseDatePreviousOwner);
                        previousOwnerDateTimes.Add(saleDatePreviousOwner);
                        previousOwnersTBR[pOwner]=previousOwnerDateTimes;
                    }

                }
                else
                {
                    Owner currentOwner = _context.Owners.Find(id);//id pk ho dictionary ho
                    List<string> currentOwnerDateTimes = new List<string>();
                    currentOwnerDateTimes.Add(purchaseDate.ToString("yyyy-MM-dd").ToString());
                    currentOwnerDateTimes.Add(saleDate.ToString());
                    previousOwnersTBR[currentOwner]=currentOwnerDateTimes;
                }
                
                allInfo.PreviousOwner= previousOwnersTBR;
                allInfoOwner.Add(allInfo);
            }


            return View(allInfoOwner);
        }


    }
}
