using System.ComponentModel.DataAnnotations;

namespace SaigonRide.Models
{
    public class RentalTransaction
    {
        [Key]
        public int TransactionId { get; set; }
        public string EncryptedBankNumber { get; set; }
        public string EncryptedVehicleCode { get; set; }
        public int MinutesBorrowed { get; set; }
        public double MoneyRented { get; set; }
        public string EncryptedPassport { get; set; } // For foreign tourists only
        public string UserType { get; set; } // "LocalCommuter" or "ForeignTourist"
        public DateTime RentalStartTime { get; set; }
        public int StationId { get; set; } // Station where vehicle was rented

        // Navigation property
        public Station Station { get; set; }
    }
}
