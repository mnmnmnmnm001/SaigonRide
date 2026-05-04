namespace SaigonRide.Models
{
    public class Station
    {
        public int StationId { get; set; }
        public string Name { get; set; }
        public int CurrentCapacity { get; set; }
        public int MaxCapacity { get; set; } // Constant

        // Navigation property
        public ICollection<Rental> StartStations { get; set; } = new List<Rental>();
        public ICollection<Rental> EndStations { get; set; } = new List<Rental>();

        public double GetRatio()
        {
            return MaxCapacity > 0 ? (double)CurrentCapacity / MaxCapacity : 0;
        }
    }
}
