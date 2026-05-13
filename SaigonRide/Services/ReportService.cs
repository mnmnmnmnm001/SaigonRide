using SaigonRide.Data;
using SaigonRide.Models;
using Microsoft.EntityFrameworkCore;

namespace SaigonRide.Services
{
    public interface IReportService
    {
        Task<RevenueByVehicleReport> GetRevenueByVehicleReport(DateTime startDate, DateTime endDate);
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
