namespace SaigonRide.Models
{
    public class Station
    {
        public int StationId { get; set; }
        public string Name { get; set; }
        public int CurrentCapacity { get; set; }
        public int MaxCapacity { get; set; } // Constant
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Ratio { get; set; } // Utilization ratio

        // Navigation property
        public ICollection<Rental> StartStations { get; set; } = new List<Rental>();
        public ICollection<Rental> EndStations { get; set; } = new List<Rental>();

        public double GetRatio()
        {
            return MaxCapacity > 0 ? (double)CurrentCapacity / MaxCapacity : 0;
        }
    }
}
