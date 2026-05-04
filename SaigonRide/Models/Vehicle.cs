namespace SaigonRide.Models
{
    public class Vehicle
    {
        public int VehicleId { get; set; }
        public string Type { get; set; } // "StandardBike", "EBike", "Scooter"
        public double FarePerMin { get; set; }
        public int State { get; set; } // 0: Available, 1: InTransit, 2: Maintenance
        public string Code { get; set; }
        public string CurrentPos { get; set; } // Current station position

        // Navigation property
        public ICollection<Rental> Rentals { get; set; } = new List<Rental>();

        public string GetType()
        {
            return Type;
        }

        public List<Vehicle> ListVehicle()
        {
            return new List<Vehicle> { this };
        }
    }
}
