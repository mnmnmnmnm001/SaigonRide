using Microsoft.AspNetCore.Mvc;
using SaigonRide.Models;
using SaigonRide.Data;
using SaigonRide.Attributes;
using System.Diagnostics;

namespace SaigonRide.Controllers
{
    [AdminAuthentication]
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
            ViewData["IsAdmin"] = !string.IsNullOrEmpty(HttpContext.Session.GetString("AdminUsername"));

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
