using Microsoft.AspNetCore.Mvc;
using SaigonRide.Services;

namespace SaigonRide.Controllers
{
    public class AdminController : Controller
    {
        private readonly IAdminAuthService _authService;

        public AdminController(IAdminAuthService authService)
        {
            _authService = authService;
        }

        // GET: Admin/Login
        public IActionResult Login()
        {
            return View();
        }

        // POST: Admin/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                ModelState.AddModelError("", "Username and password are required.");
                return View();
            }

            // Hardcoded credentials as specified
            if (username == "qwe" && password == "1234560")
            {
                HttpContext.Session.SetString("AdminUsername", username);
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "Invalid credentials");
            return View();
        }

        // GET: Admin/Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Main");
        }
    }
}
