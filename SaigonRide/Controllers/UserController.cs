using Microsoft.AspNetCore.Mvc;
using SaigonRide.Data;
using SaigonRide.Models;
using SaigonRide.Attributes;
using Microsoft.EntityFrameworkCore;

namespace SaigonRide.Controllers
{
    [AdminAuthentication]
    public class UserController : Controller
    {
        private readonly SaigonRideContext _context;

        public UserController(SaigonRideContext context)
        {
            _context = context;
        }

        // GET: User
        public async Task<IActionResult> Index()
        {
            var users = await _context.Users.ToListAsync();
            return View(users);
        }

        // GET: User/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
                return NotFound();

            var user = await _context.Users.FirstOrDefaultAsync(m => m.BankNum == id);
            if (user == null)
                return NotFound();

            return View(user);
        }

        // GET: User/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: User/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string userType, [Bind("BankNum,ChosenPaymentCode")] User user)
        {
            if (ModelState.IsValid)
            {
                if (userType == "LocalCommuter")
                {
                    var localCommuter = new LocalCommuter
                    {
                        BankNum = user.BankNum,
                        ChosenPaymentCode = user.ChosenPaymentCode,
                        Payed = 0,
                        P_MoMo = true,
                        P_VNPay = false
                    };
                    _context.Add(localCommuter);
                }
                else if (userType == "ForeignTourist")
                {
                    var tourist = new ForeignTourist
                    {
                        BankNum = user.BankNum,
                        ChosenPaymentCode = user.ChosenPaymentCode,
                        Payed = 0,
                        Passport = "",
                        P_ApplePay = true,
                        P_PayPal = false
                    };
                    _context.Add(tourist);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: User/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
                return NotFound();

            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return NotFound();

            return View(user);
        }

        // POST: User/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, string chosenPaymentCode, double payed)
        {
            if (id == null)
                return NotFound();

            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    user.ChosenPaymentCode = chosenPaymentCode;
                    user.Payed = payed;
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(id))
                        return NotFound();
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: User/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
                return NotFound();

            var user = await _context.Users.FirstOrDefaultAsync(m => m.BankNum == id);
            if (user == null)
                return NotFound();

            return View(user);
        }

        // POST: User/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (string.IsNullOrEmpty(id))
                return RedirectToAction("Index", "Home");

            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index", "Home");
        }

        private bool UserExists(string id)
        {
            return _context.Users.Any(e => e.BankNum == id);
        }
    }
}
