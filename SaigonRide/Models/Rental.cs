namespace SaigonRide.Models
{
    public class Rental
    {
        public int RentalId { get; set; }
        public string UserId { get; set; } // Foreign key to BankNum
        public string VehicleId { get; set; } // Foreign key to Vehicle.Code
        public int StartStationId { get; set; }
        public int EndStationId { get; set; }
        public DateTime TimeStart { get; set; }
        public DateTime TimeEnd { get; set; }
        public double CalculatedFare { get; set; }
        public double DiscountApplied { get; set; }
        public double FinalFare { get; set; }
        public int Status { get; set; } // 0: Active, 1: Completed, 2: Cancelled

        // Navigation properties
        public User User { get; set; }
        public Vehicle Vehicle { get; set; }
        public Station StartStation { get; set; }
        public Station EndStation { get; set; }

        public string Encode(string data)
        {
            return System.Text.Encoding.UTF8.GetString(System.Text.Encoding.UTF8.GetBytes(data));
        }

        public string Decode(string data)
        {
            return System.Text.Encoding.UTF8.GetString(System.Text.Encoding.UTF8.GetBytes(data));
        }
    }
}
