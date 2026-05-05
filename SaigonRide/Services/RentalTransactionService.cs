using SaigonRide.Data;
using SaigonRide.Models;
using Microsoft.EntityFrameworkCore;

namespace SaigonRide.Services
{
    public interface IRentalTransactionService
    {
        Task<bool> CreateRentalTransactionAsync(string bankNumber, string vehicleCode, int minutes, double moneyRented, string userType, string passport, int stationId);
        Task<RentalTransaction> GetRentalTransactionByVehicleCodeAsync(string encryptedVehicleCode);
        Task<bool> CompleteRentalAsync(int transactionId, int returnStationId, double additionalCharge, double refund);
    }

    public class RentalTransactionService : IRentalTransactionService
    {
        private readonly SaigonRideContext _context;
        private readonly IEncryptionService _encryptionService;
        private readonly ILogger<RentalTransactionService> _logger;

        public RentalTransactionService(SaigonRideContext context, IEncryptionService encryptionService, ILogger<RentalTransactionService> logger)
        {
            _context = context;
            _encryptionService = encryptionService;
            _logger = logger;
        }

        public async Task<bool> CreateRentalTransactionAsync(string bankNumber, string vehicleCode, int minutes, double moneyRented, string userType, string passport, int stationId)
        {
            try
            {
                var transaction = new RentalTransaction
                {
                    EncryptedBankNumber = _encryptionService.Encrypt(bankNumber),
                    EncryptedVehicleCode = _encryptionService.Encrypt(vehicleCode),
                    MinutesBorrowed = minutes,
                    MoneyRented = moneyRented,
                    EncryptedPassport = string.IsNullOrEmpty(passport) ? string.Empty : _encryptionService.Encrypt(passport),
                    UserType = userType ?? string.Empty,
                    RentalStartTime = DateTime.Now,
                    StationId = stationId
                };

                _context.RentalTransactions.Add(transaction);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error creating rental transaction for vehicle {VehicleCode}", vehicleCode);
                return false;
            }
        }

        public async Task<RentalTransaction> GetRentalTransactionByVehicleCodeAsync(string vehicleCode)
        {
            // Because encryption uses a random IV the ciphertext will be different each time.
            // To find the transaction, decrypt stored values and compare to the provided vehicleCode.
            try
            {
                var all = await _context.RentalTransactions.ToListAsync();
                foreach (var rt in all)
                {
                    try
                    {
                        var decrypted = _encryptionService.Decrypt(rt.EncryptedVehicleCode);
                        if (string.Equals(decrypted, vehicleCode, StringComparison.OrdinalIgnoreCase))
                            return rt;
                    }
                    catch (Exception inner)
                    {
                        _logger?.LogDebug(inner, "Failed to decrypt vehicle code for transaction {TransactionId}", rt.TransactionId);
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error while searching for rental transaction by vehicle code");
                return null;
            }
        }

        public async Task<bool> CompleteRentalAsync(int transactionId, int returnStationId, double additionalCharge, double refund)
        {
            try
            {
                var transaction = await _context.RentalTransactions.FindAsync(transactionId);
                if (transaction == null)
                    return false;

                _context.RentalTransactions.Remove(transaction);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
