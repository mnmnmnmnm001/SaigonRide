using SaigonRide.Data;
using SaigonRide.Models;
using Microsoft.EntityFrameworkCore;

namespace SaigonRide.Services
{
    public interface IReportService
    {
        Task<List<RevenueByVehicleReport>> GetRevenueByVehicleReport(DateTime startDate, DateTime endDate);
        Task<List<StationInventoryReport>> GetStationInventoryReport();
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

        public async Task<List<RevenueByVehicleReport>> GetRevenueByVehicleReport(DateTime startDate, DateTime endDate)
        {
            // Ensure we include the entire day range
            var adjustedStartDate = startDate.Date;
            var adjustedEndDate = endDate.Date.AddDays(1);

            var completedRentals = await _context.Rentals
                .Include(r => r.Vehicle)
                .Where(r => r.Status == 1 && r.TimeStart >= adjustedStartDate && r.TimeStart < adjustedEndDate)
                .ToListAsync();

            if (!completedRentals.Any())
            {
                return new List<RevenueByVehicleReport>();
            }

            // Group by vehicle type and calculate stats for each group
            var results = completedRentals
                .Where(r => r.Vehicle != null && !string.IsNullOrEmpty(r.Vehicle.Type))
                .GroupBy(r => r.Vehicle.Type)
                .Select(g => new RevenueByVehicleReport
                {
                    VehicleType = g.Key,
                    RentalCount = g.Count(),
                    TotalRevenue = g.Sum(r => r.FinalFare),
                    AverageFare = g.Count() > 0 ? g.Sum(r => r.FinalFare) / g.Count() : 0,
                    TotalDiscount = g.Sum(r => r.DiscountApplied)
                })
                .OrderByDescending(r => r.TotalRevenue)
                .ToList();

            return results;
        }

        public async Task<List<StationInventoryReport>> GetStationInventoryReport()
        {
            var stations = await _context.Stations.ToListAsync();

            var result = new List<StationInventoryReport>();
            foreach (var s in stations)
            {
                var utilizationRatio = s.GetRatio();
                // Status: Low inventory when < 20% capacity, Medium when 20-80%, High when > 80%
                var status = utilizationRatio < 0.2 ? "Low" : (utilizationRatio <= 0.8 ? "Medium" : "High");

                result.Add(new StationInventoryReport
                {
                    StationId = s.StationId,
                    StationName = s.Name,
                    CurrentCapacity = s.CurrentCapacity,
                    MaxCapacity = s.MaxCapacity,
                    UtilizationRatio = utilizationRatio,
                    Status = status
                });
            }

            return result;
        }
    }
}
