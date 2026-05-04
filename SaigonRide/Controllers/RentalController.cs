using Microsoft.AspNetCore.Mvc;
using SaigonRide.Data;
using SaigonRide.Models;
using Microsoft.EntityFrameworkCore;

namespace SaigonRide.Controllers
{
    public class RentalController : Controller
    {
        private readonly SaigonRideContext _context;
        private readonly Services.IRentalService _rentalService;
        private readonly Services.IPaymentService _paymentService;

        public RentalController(SaigonRideContext context, Services.IRentalService rentalService, Services.IPaymentService paymentService)
        {
            _context = context;
            _rentalService = rentalService;
            _paymentService = paymentService;
        }

        // GET: Rental
        public async Task<IActionResult> Index()
        {
            var rentals = await _rentalService.GetAllRentals();
            return View(rentals);
        }

        // GET: Rental/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var rental = await _rentalService.GetRentalById(id.Value);
            if (rental == null)
                return NotFound();

            return View(rental);
        }

        // GET: Rental/Create
        public async Task<IActionResult> Create()
        {
            ViewData["Users"] = await _context.Users.ToListAsync();
            ViewData["Vehicles"] = await _context.Vehicles.Where(v => v.State == 0).ToListAsync();
            ViewData["Stations"] = await _context.Stations.ToListAsync();
            return View();
        }

        // POST: Rental/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,VehicleId,StartStationId")] Rental rental)
        {
            try
            {
                var createdRental = await _rentalService.StartRental(rental.UserId, rental.VehicleId, rental.StartStationId);
                return RedirectToAction(nameof(EndRental), new { id = createdRental.RentalId });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }

            ViewData["Users"] = await _context.Users.ToListAsync();
            ViewData["Vehicles"] = await _context.Vehicles.Where(v => v.State == 0).ToListAsync();
            ViewData["Stations"] = await _context.Stations.ToListAsync();
            return View(rental);
        }

        // GET: Rental/EndRental/5
        public async Task<IActionResult> EndRental(int? id)
        {
            if (id == null)
                return NotFound();

            var rental = await _rentalService.GetRentalById(id.Value);
            if (rental == null)
                return NotFound();

            ViewData["Stations"] = await _context.Stations.ToListAsync();
            var paymentMethods = _paymentService.GetAvailablePaymentMethods(rental.User);
            ViewData["PaymentMethods"] = paymentMethods;

            return View(rental);
        }

        // POST: Rental/EndRental/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EndRental(int id, [Bind("EndStationId")] int endStationId, string paymentMethod)
        {
            try
            {
                var rental = await _rentalService.EndRental(id, endStationId, paymentMethod);
                return RedirectToAction(nameof(Receipt), new { id = rental.RentalId });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }

            var rental2 = await _rentalService.GetRentalById(id);
            ViewData["Stations"] = await _context.Stations.ToListAsync();
            var paymentMethods = _paymentService.GetAvailablePaymentMethods(rental2.User);
            ViewData["PaymentMethods"] = paymentMethods;

            return View(rental2);
        }

        // GET: Rental/Receipt/5
        public async Task<IActionResult> Receipt(int? id)
        {
            if (id == null)
                return NotFound();

            var rental = await _rentalService.GetRentalById(id.Value);
            if (rental == null)
                return NotFound();

            return View(rental);
        }

        // GET: Rental/UserRentals/5
        public async Task<IActionResult> UserRentals(int? id)
        {
            if (id == null)
                return NotFound();

            var rentals = await _rentalService.GetUserRentals(id.Value);
            return View(rentals);
        }

        // GET: Rental/ListVehicles
        public async Task<IActionResult> ListVehicles()
        {
            var vehicles = await _context.Vehicles.ToListAsync();
            return View(vehicles);
        }
    }
}
