using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Milestone3;
using Milestone3.Models;

namespace Milestone3.Controllers
{
    public class CarController : Controller
    {
        private readonly Milestone3DbContext _context;

        public CarController(Milestone3DbContext context)
        {
            _context = context;
        }
        public IActionResult Index(string color, string make, string year, string model, string type)
        {
            var cars = _context.Cars.ToList();

            if (!string.IsNullOrEmpty(color))
            {
                cars = cars.Where(c => c.Color.Contains(color)).ToList();

            }

            if (!string.IsNullOrEmpty(type))
            {
                cars = cars.Where(t => t.Type.Contains(type)).ToList();

            }

            ViewBag.AvailableMakes = new SelectList(_context.Makes, "BrandName", "BrandName");
            if (!string.IsNullOrEmpty(make))
            {
                //get makeid of make with the brandname
                int makeIDTOLOOK= _context.Makes.Where(m=> m.BrandName==make).FirstOrDefault().MakeId;

                cars = cars.Where(c => c.MakeId==makeIDTOLOOK).ToList();
            }

            if (year != null)
            {
                cars = cars.Where(c => c.Year == year).ToList();
            }
            ViewBag.AvailableModels = new SelectList(_context.Cars, "Model","Model");

            if (!string.IsNullOrEmpty(model))
            {
                cars = cars.Where(c => c.Model.Contains(model)).ToList();
            }

            return View(cars);
        }


        // GET: Car/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.Cars == null)
            {
                return NotFound();
            }

            var car = await _context.Cars.FirstOrDefaultAsync(m => m.Vin == id);
            car.Make = _context.Makes.Find(car.MakeId);
            if (car == null)
            {
                return NotFound();
            }

            return View(car);
        }
    }
}
