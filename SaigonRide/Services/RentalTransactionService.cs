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

        public RentalTransactionService(SaigonRideContext context, IEncryptionService encryptionService)
        {
            _context = context;
            _encryptionService = encryptionService;
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
                    EncryptedPassport = string.IsNullOrEmpty(passport) ? null : _encryptionService.Encrypt(passport),
                    UserType = userType,
                    RentalStartTime = DateTime.Now,
                    StationId = stationId
                };

                _context.RentalTransactions.Add(transaction);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<RentalTransaction> GetRentalTransactionByVehicleCodeAsync(string vehicleCode)
        {
            var encryptedCode = _encryptionService.Encrypt(vehicleCode);
            //crash if user enter wrong code, use ai fix it
            return await _context.RentalTransactions.FirstOrDefaultAsync(rt => rt.EncryptedVehicleCode == encryptedCode);
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
