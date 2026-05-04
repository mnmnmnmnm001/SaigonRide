using SaigonRide.Data;
using SaigonRide.Models;
using System.Security.Cryptography;
using System.Text;

namespace SaigonRide.Services
{
    public interface IAdminAuthService
    {
        Task<bool> AuthenticateAsync(string username, string password);
        Task<Admin> GetAdminByUsernameAsync(string username);
        string HashPassword(string password);
        bool VerifyPassword(string password, string hash);
    }

    public class AdminAuthService : IAdminAuthService
    {
        private readonly SaigonRideContext _context;

        public AdminAuthService(SaigonRideContext context)
        {
            _context = context;
        }

        public async Task<bool> AuthenticateAsync(string username, string password)
        {
            var admin = await GetAdminByUsernameAsync(username);
            if (admin == null)
                return false;

            return VerifyPassword(password, admin.PasswordHash);
        }

        public async Task<Admin> GetAdminByUsernameAsync(string username)
        {
            return await _context.Admins.FirstOrDefaultAsync(a => a.Username == username);
        }

        public string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }

        public bool VerifyPassword(string password, string hash)
        {
            var hashOfInput = HashPassword(password);
            return hashOfInput == hash;
        }
    }
}
