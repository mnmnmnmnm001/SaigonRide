using Microsoft.AspNetCore.Mvc;
using SaigonRide.Data;
using SaigonRide.Models;
using SaigonRide.Services;
using Microsoft.EntityFrameworkCore;

namespace SaigonRide.Controllers
{
    public class MainController : Controller
    {
        private readonly SaigonRideContext _context;
        private readonly IFareCalculationService _fareService;
        private readonly IRentalTransactionService _transactionService;
        private readonly IPaymentService _paymentService;
        private readonly IEncryptionService _encryptionService;

        public MainController(SaigonRideContext context, IFareCalculationService fareService, IRentalTransactionService transactionService, IPaymentService paymentService, IEncryptionService encryptionService)
        {
            _context = context;
            _fareService = fareService;
            _transactionService = transactionService;
            _paymentService = paymentService;
            _encryptionService = encryptionService;
        }

        // Main choice page
        public IActionResult Index()
        {
            return View();
        }

        // Show available vehicles
        public async Task<IActionResult> Rent()
        {
            var vehicles = await _context.Vehicles.Where(v => v.State == 0).ToListAsync();
            ViewData["Stations"] = await _context.Stations.ToListAsync();
            return View(vehicles);
        }

        // Details and input for renting a specific vehicle
        public async Task<IActionResult> RentDetails(string id)
        {
            var vehicle = await _context.Vehicles.FindAsync(id);
            if (vehicle == null) return NotFound();
            ViewData["Stations"] = await _context.Stations.ToListAsync();
            return View(vehicle);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProcessRent(string vehicleId, int stationId, string userType, string bankNum, string passport, int minutes, string paymentMethod)
        {
            // Input validation
            if (string.IsNullOrWhiteSpace(vehicleId))
            {
                TempData["Error"] = "Vehicle ID is required.";
                return RedirectToAction(nameof(Rent));
            }

            if (stationId <= 0)
            {
                TempData["Error"] = "Please select a valid station.";
                return RedirectToAction(nameof(RentDetails), new { id = vehicleId });
            }

            if (string.IsNullOrWhiteSpace(bankNum))
            {
                TempData["Error"] = "Bank number is required.";
                return RedirectToAction(nameof(RentDetails), new { id = vehicleId });
            }

            if (string.IsNullOrWhiteSpace(userType) || (userType != "LocalCommuter" && userType != "ForeignTourist"))
            {
                TempData["Error"] = "Please select a valid user type.";
                return RedirectToAction(nameof(RentDetails), new { id = vehicleId });
            }

            if (userType == "ForeignTourist" && string.IsNullOrWhiteSpace(passport))
            {
                TempData["Error"] = "Passport number is required for foreign tourists.";
                return RedirectToAction(nameof(RentDetails), new { id = vehicleId });
            }

            if (string.IsNullOrWhiteSpace(paymentMethod))
            {
                TempData["Error"] = "Please select a payment method.";
                return RedirectToAction(nameof(RentDetails), new { id = vehicleId });
            }

            if (minutes <= 0 || minutes > 1440)
            {
                TempData["Error"] = "Rental duration must be between 1 and 1440 minutes (24 hours).";
                return RedirectToAction(nameof(RentDetails), new { id = vehicleId });
            }

            var vehicle = await _context.Vehicles.FindAsync(vehicleId);
            var station = await _context.Stations.FindAsync(stationId);
            if (vehicle == null || station == null)
            {
                TempData["Error"] = "Vehicle or station not found.";
                return RedirectToAction(nameof(Rent));
            }

            if (vehicle.State != 0)
            {
                TempData["Error"] = "This vehicle is no longer available for rent.";
                return RedirectToAction(nameof(Rent));
            }

            if (station.CurrentCapacity <= 0)
            {
                TempData["Error"] = "The selected station has no available capacity.";
                return RedirectToAction(nameof(RentDetails), new { id = vehicleId });
            }

            double moneyRented = _fareService.CalculateFare(vehicle, minutes, false);

            // Create or find user
            User user = await _context.Users.FirstOrDefaultAsync(u => u.BankNum == bankNum);
            if (user == null)
            {
                if (userType == "LocalCommuter")
                {
                    user = new LocalCommuter { BankNum = bankNum, ChosenPaymentCode = "Cash", Payed = 0, P_MoMo = false, P_VNPay = false };
                }
                else
                {
                    user = new ForeignTourist { BankNum = bankNum, ChosenPaymentCode = "Cash", Payed = 0, Passport = passport ?? "", P_ApplePay = false, P_PayPal = false };
                }
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
            }

            // Charge user from their account balance.
            /* here, we havw no api to connect to bank, so we will just skip
             * get userbankbalance (authorize bank to connect api and get their userbankbalance)
            if (userbankbalance < moneyRented)
            {
                TempData["Error"] = "Insufficient funds in user account.";
                return RedirectToAction(nameof(RentDetails), new { id = vehicleId });
            }*/
            user.Payed = moneyRented;
            _context.Users.Update(user);

            // Create transaction
            bool created = await _transactionService.CreateRentalTransactionAsync(bankNum, vehicle.Code, minutes, moneyRented, user.UserType, passport, stationId);
            if (!created)
            {
                TempData["Error"] = "Failed to create rental transaction.";
                return RedirectToAction(nameof(RentDetails), new { id = vehicleId });
            }

            // Update vehicle and station
            vehicle.State = 1; // In-Transit
            vehicle.CurrentPos = "In-Transit";
            station.CurrentCapacity = Math.Max(0, station.CurrentCapacity - 1);

            _context.Vehicles.Update(vehicle);
            _context.Stations.Update(station);
            await _context.SaveChangesAsync();

            ViewData["VehicleCode"] = vehicle.Code;
            ViewData["Minutes"] = minutes;
            ViewData["MoneyRented"] = moneyRented;
            return View("RentResult");
        }

        public async Task<IActionResult> Return()
        {
            ViewBag.Stations = await _context.Stations.ToListAsync();
            // Load vehicles that are in-transition (State == 1)
            ViewBag.Vehicles = await _context.Vehicles.Where(v => v.State == 1).ToListAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProcessReturn(string vehicleCode, int returnStationId)
        {
            // Input validation
            if (string.IsNullOrWhiteSpace(vehicleCode))
            {
                TempData["Error"] = "Vehicle code is required. Please select a vehicle.";
                return RedirectToAction(nameof(Return));
            }

            if (returnStationId <= 0)
            {
                TempData["Error"] = "Please select a valid return station.";
                return RedirectToAction(nameof(Return));
            }

            var vehicle = await _context.Vehicles.FirstOrDefaultAsync(v => v.Code == vehicleCode);
            if (vehicle == null)
            {
                TempData["Error"] = "Vehicle not found.";
                return RedirectToAction(nameof(Return));
            }

            if (vehicle.State != 1)
            {
                TempData["Error"] = "Selected vehicle is not currently in transit.";
                return RedirectToAction(nameof(Return));
            }

            var transaction = await _transactionService.GetRentalTransactionByVehicleCodeAsync(vehicleCode);
            if (transaction == null)
            {
                TempData["Error"] = "Rental transaction not found for the provided vehicle code.";
                return RedirectToAction(nameof(Return));
            }

            var returnStation = await _context.Stations.FindAsync(returnStationId);
            if (returnStation == null)
            {
                TempData["Error"] = "Return station not found.";
                return RedirectToAction(nameof(Return));
            }

            if (returnStation.CurrentCapacity >= returnStation.MaxCapacity)
            {
                TempData["Error"] = "Return station is at full capacity. Please choose another station.";
                return RedirectToAction(nameof(Return));
            }

            bool isLowInventory = _fareService.IsLowInventory(returnStation);
            double finalFare = _fareService.ApplyDiscount(transaction.MoneyRented, isLowInventory);
            double difference = finalFare - transaction.MoneyRented;

            // Update vehicle and station
            if (vehicle != null)
            {
                vehicle.State = 0;
                vehicle.CurrentPos = returnStation.Name;
                _context.Vehicles.Update(vehicle);
            }

            returnStation.CurrentCapacity = Math.Min(returnStation.CurrentCapacity + 1, returnStation.MaxCapacity);
            _context.Stations.Update(returnStation);

            // Complete transaction (remove transaction record)
            await transaction_complete(transaction, returnStationId, difference);

            await _context.SaveChangesAsync();

            ViewData["FinalFare"] = finalFare;
            ViewData["MoneyRented"] = transaction.MoneyRented;
            ViewData["Difference"] = difference;
            ViewData["VehicleCode"] = vehicleCode;
            return View("ReturnResult");
        }

        // helper to call complete
        private async Task transaction_complete(RentalTransaction transaction, int returnStationId, double difference)
        {
            //call CompleteRentalAsync to remove transaction
            await _transactionService.CompleteRentalAsync(transaction.TransactionId, returnStationId, Math.Max(0, difference), Math.Max(0, -difference));
        }
    }
}
