using Microsoft.AspNetCore.Mvc;
using SaigonRide.Models;
using SaigonRide.Data;
using System.Diagnostics;

namespace SaigonRide.Controllers
{
    public class HomeController : Controller
    {
        private readonly SaigonRideContext _context;

        public HomeController(SaigonRideContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            ViewData["TotalUsers"] = _context.Users.Count();
            ViewData["TotalVehicles"] = _context.Vehicles.Count();
            ViewData["TotalStations"] = _context.Stations.Count();
            ViewData["TotalRentals"] = _context.Rentals.Count();
            ViewData["TotalRevenue"] = _context.Rentals.Where(r => r.Status == 1).Sum(r => r.FinalFare);

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
