using Microsoft.AspNetCore.Mvc;
using SaigonRide.Data;
using SaigonRide.Models;
using Microsoft.EntityFrameworkCore;

namespace SaigonRide.Controllers
{
    public class StationController : Controller
    {
        private readonly SaigonRideContext _context;

        public StationController(SaigonRideContext context)
        {
            _context = context;
        }

        // GET: Station
        public async Task<IActionResult> Index()
        {
            var stations = await _context.Stations.ToListAsync();
            return View(stations);
        }

        // GET: Station/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var station = await _context.Stations.FirstOrDefaultAsync(m => m.StationId == id);
            if (station == null)
                return NotFound();

            return View(station);
        }

        // GET: Station/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Station/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,CurrentCapacity,MaxCapacity,Latitude,Longitude")] Station station)
        {
            if (ModelState.IsValid)
            {
                station.Ratio = station.GetRatio();
                _context.Add(station);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(station);
        }

        // GET: Station/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var station = await _context.Stations.FindAsync(id);
            if (station == null)
                return NotFound();

            return View(station);
        }

        // POST: Station/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("StationId,Name,CurrentCapacity,MaxCapacity,Latitude,Longitude,Ratio")] Station station)
        {
            if (id != station.StationId)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    station.Ratio = station.GetRatio();
                    _context.Update(station);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StationExists(station.StationId))
                        return NotFound();
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(station);
        }

        // GET: Station/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var station = await _context.Stations.FirstOrDefaultAsync(m => m.StationId == id);
            if (station == null)
                return NotFound();

            return View(station);
        }

        // POST: Station/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var station = await _context.Stations.FindAsync(id);
            if (station != null)
            {
                _context.Stations.Remove(station);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool StationExists(int id)
        {
            return _context.Stations.Any(e => e.StationId == id);
        }
    }
}
