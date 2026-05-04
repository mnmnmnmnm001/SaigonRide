using SaigonRide.Data;
using SaigonRide.Models;
using Microsoft.EntityFrameworkCore;

namespace SaigonRide.Services
{
    public interface IReportService
    {
        Task<RevenueByVehicleReport> GetRevenueByVehicleReport(DateTime startDate, DateTime endDate);
        Task<StationInventoryReport> GetStationInventoryReport();
    }

    public class RevenueByVehicleReport
    {
        public string VehicleType { get; set; }
        public int RentalCount { get; set; }
        public double TotalRevenue { get; set; }
        public double AverageFare { get; set; }
        public double TotalDiscount { get; set; }
    }

    public class StationInventoryReport
    {
        public int StationId { get; set; }
        public string StationName { get; set; }
        public int CurrentCapacity { get; set; }
        public int MaxCapacity { get; set; }
        public double UtilizationRatio { get; set; }
        public string Status { get; set; } // "Low", "Medium", "High"
    }

    public class ReportService : IReportService
    {
        private readonly SaigonRideContext _context;

        public ReportService(SaigonRideContext context)
        {
            _context = context;
        }

        public async Task<RevenueByVehicleReport> GetRevenueByVehicleReport(DateTime startDate, DateTime endDate)
        {
            var completedRentals = await _context.Rentals
                .Include(r => r.Vehicle)
                .Where(r => r.Status == 1 && r.TimeStart >= startDate && r.TimeStart <= endDate)
                .ToListAsync();

            if (!completedRentals.Any())
            {
                return new RevenueByVehicleReport
                {
                    VehicleType = "No Data",
                    RentalCount = 0,
                    TotalRevenue = 0,
                    AverageFare = 0,
                    TotalDiscount = 0
                };
            }

            var groupedByType = completedRentals.GroupBy(r => r.Vehicle.Type).FirstOrDefault();
            if (groupedByType == null)
                return new RevenueByVehicleReport();

            var totalRevenue = groupedByType.Sum(r => r.FinalFare);
            var totalDiscount = groupedByType.Sum(r => r.DiscountApplied);
            var rentalCount = groupedByType.Count();

            return new RevenueByVehicleReport
            {
                VehicleType = groupedByType.Key,
                RentalCount = rentalCount,
                TotalRevenue = totalRevenue,
                AverageFare = rentalCount > 0 ? totalRevenue / rentalCount : 0,
                TotalDiscount = totalDiscount
            };
        }

        public async Task<StationInventoryReport> GetStationInventoryReport()
        {
            var stations = await _context.Stations.ToListAsync();

            if (!stations.Any())
                return new StationInventoryReport();

            var firstStation = stations.First();
            var utilizationRatio = firstStation.GetRatio();
            var status = utilizationRatio >= 0.8 ? "High" : (utilizationRatio >= 0.5 ? "Medium" : "Low");

            return new StationInventoryReport
            {
                StationId = firstStation.StationId,
                StationName = firstStation.Name,
                CurrentCapacity = firstStation.CurrentCapacity,
                MaxCapacity = firstStation.MaxCapacity,
                UtilizationRatio = utilizationRatio,
                Status = status
            };
        }
    }
}
