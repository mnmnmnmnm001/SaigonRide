using SaigonRide.Data;
using SaigonRide.Models;
using Microsoft.EntityFrameworkCore;

namespace SaigonRide.Services
{
    public interface IRentalService
    {
        Task<Rental> StartRental(int userId, int vehicleId, int startStationId);
        Task<Rental> EndRental(int rentalId, int endStationId, string paymentMethod);
        Task<Rental> GetRentalById(int rentalId);
        Task<List<Rental>> GetUserRentals(int userId);
        Task<List<Rental>> GetAllRentals();
    }

    public class RentalService : IRentalService
    {
        private readonly SaigonRideContext _context;
        private readonly IFareCalculationService _fareCalculationService;
        private readonly IPaymentService _paymentService;

        public RentalService(SaigonRideContext context, IFareCalculationService fareCalculationService, IPaymentService paymentService)
        {
            _context = context;
            _fareCalculationService = fareCalculationService;
            _paymentService = paymentService;
        }

        public async Task<Rental> StartRental(int userId, int vehicleId, int startStationId)
        {
            try
            {
                var user = await _context.Users.FindAsync(userId);
                if (user == null)
                    throw new Exception("User not found");

                var vehicle = await _context.Vehicles.FindAsync(vehicleId);
                if (vehicle == null)
                    throw new Exception("Vehicle not found");

                if (vehicle.State != 0) // Check if available
                    throw new Exception("Vehicle is not available");

                var station = await _context.Stations.FindAsync(startStationId);
                if (station == null)
                    throw new Exception("Station not found");

                // Create rental
                var rental = new Rental
                {
                    UserId = userId,
                    VehicleId = vehicleId,
                    StartStationId = startStationId,
                    TimeStart = DateTime.Now,
                    Status = 0 // Active
                };

                // Update vehicle state
                vehicle.State = 1; // In-Transit

                _context.Rentals.Add(rental);
                _context.Vehicles.Update(vehicle);
                await _context.SaveChangesAsync();

                return rental;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error starting rental: {ex.Message}");
            }
        }

        public async Task<Rental> EndRental(int rentalId, int endStationId, string paymentMethod)
        {
            try
            {
                var rental = await _context.Rentals
                    .Include(r => r.User)
                    .Include(r => r.Vehicle)
                    .Include(r => r.EndStation)
                    .FirstOrDefaultAsync(r => r.RentalId == rentalId);

                if (rental == null)
                    throw new Exception("Rental not found");

                var endStation = await _context.Stations.FindAsync(endStationId);
                if (endStation == null)
                    throw new Exception("End station not found");

                // Calculate rental duration in minutes
                rental.TimeEnd = DateTime.Now;
                int rentalMinutes = (int)(rental.TimeEnd - rental.TimeStart).TotalMinutes;
                if (rentalMinutes <= 0)
                    rentalMinutes = 1;

                // Check if return station has low inventory
                bool isLowInventory = _fareCalculationService.IsLowInventory(endStation);

                // Calculate fare
                rental.CalculatedFare = _fareCalculationService.CalculateFare(rental.Vehicle, rentalMinutes, false);
                rental.DiscountApplied = isLowInventory ? rental.CalculatedFare * 0.15 : 0;
                rental.FinalFare = rental.CalculatedFare - rental.DiscountApplied;

                // Process payment
                bool paymentSuccess = _paymentService.ProcessPayment(rental.User, rental.FinalFare, paymentMethod);
                if (!paymentSuccess)
                    throw new Exception("Payment processing failed");

                // Update rental status
                rental.Status = 1; // Completed
                rental.EndStationId = endStationId;

                // Update vehicle state
                rental.Vehicle.State = 0; // Available
                rental.Vehicle.CurrentPos = $"Station {endStationId}";

                // Update station inventory
                endStation.CurrentCapacity = Math.Min(endStation.CurrentCapacity + 1, endStation.MaxCapacity);
                endStation.Ratio = endStation.GetRatio();

                _context.Rentals.Update(rental);
                _context.Vehicles.Update(rental.Vehicle);
                _context.Stations.Update(endStation);
                await _context.SaveChangesAsync();

                return rental;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error ending rental: {ex.Message}");
            }
        }

        public async Task<Rental> GetRentalById(int rentalId)
        {
            return await _context.Rentals
                .Include(r => r.User)
                .Include(r => r.Vehicle)
                .Include(r => r.StartStation)
                .Include(r => r.EndStation)
                .FirstOrDefaultAsync(r => r.RentalId == rentalId);
        }

        public async Task<List<Rental>> GetUserRentals(int userId)
        {
            return await _context.Rentals
                .Where(r => r.UserId == userId)
                .Include(r => r.Vehicle)
                .Include(r => r.StartStation)
                .Include(r => r.EndStation)
                .OrderByDescending(r => r.TimeStart)
                .ToListAsync();
        }

        public async Task<List<Rental>> GetAllRentals()
        {
            return await _context.Rentals
                .Include(r => r.User)
                .Include(r => r.Vehicle)
                .Include(r => r.StartStation)
                .Include(r => r.EndStation)
                .OrderByDescending(r => r.TimeStart)
                .ToListAsync();
        }
    }
}
