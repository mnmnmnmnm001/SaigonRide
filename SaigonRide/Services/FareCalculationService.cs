using SaigonRide.Models;

namespace SaigonRide.Services
{
    public interface IFareCalculationService
    {
        double CalculateFare(Vehicle vehicle, int minutes, bool isLowInventoryReturn);
        bool IsLowInventory(Station station);
        double ApplyDiscount(double fare, bool isLowInventory);
    }

    public class FareCalculationService : IFareCalculationService
    {
        private const double LowInventoryThreshold = 0.20; // 20%
        private const double LowInventoryDiscount = 0.15; // 15% discount

        public double CalculateFare(Vehicle vehicle, int minutes, bool isLowInventoryReturn)
        {
            if (vehicle == null || minutes <= 0)
                return 0;

            double baseFare = vehicle.FarePerMin * minutes;
            return ApplyDiscount(baseFare, isLowInventoryReturn);
        }

        public bool IsLowInventory(Station station)
        {
            if (station == null || station.MaxCapacity == 0)
                return false;

            double capacityRatio = (double)station.CurrentCapacity / station.MaxCapacity;
            return capacityRatio < LowInventoryThreshold;
        }

        public double ApplyDiscount(double fare, bool isLowInventory)
        {
            if (isLowInventory)
            {
                return fare * (1 - LowInventoryDiscount);
            }
            return fare;
        }
    }
}
