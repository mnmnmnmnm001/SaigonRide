using Microsoft.AspNetCore.Mvc;
using SaigonRide.Services;
using SaigonRide.Attributes;

namespace SaigonRide.Controllers
{
    [AdminAuthentication]
    public class ReportController : Controller
    {
        private readonly IReportService _reportService;

        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }

        // GET: Report/RevenueReport
        public async Task<IActionResult> RevenueReport(DateTime? startDate, DateTime? endDate)
        {
            if (!startDate.HasValue)
                startDate = DateTime.Now.AddDays(-30);
            if (!endDate.HasValue)
                endDate = DateTime.Now;

            var report = await _reportService.GetRevenueByVehicleReport(startDate.Value, endDate.Value);
            ViewData["StartDate"] = startDate;
            ViewData["EndDate"] = endDate;
            return View(report);
        }

        // GET: Report/InventoryReport
        public async Task<IActionResult> InventoryReport()
        {
            var report = await _reportService.GetStationInventoryReport();
            return View(report);
        }
    }
}
