using SaigonRide.Models;
using SaigonRide.Services;
using Xunit;

namespace SaigonRide.Tests
{
    public class FareCalculatorTests
    {
        private readonly FareCalculationService _service = new FareCalculationService();

        // Testing FR2: 15% Discount Trigger (IsLowInventory Boundary Logic)
        // Validates TC01 - TC07 using Boundary Value Analysis (BVA)
        [Theory]
        [InlineData(0, 10, true)]    // TC01: BVA Min (0%) -> Low (Discount)
        [InlineData(1, 10, true)]    // TC02: BVA Below (10%) -> Low (Discount)
        [InlineData(19, 100, true)]  // TC03: BVA Just Below (19%) -> Low (Discount)
        [InlineData(20, 100, false)] // TC04: BVA At Boundary (20%) -> Not Low (No Discount)
        [InlineData(21, 100, false)] // TC05: BVA Just Above (21%) -> Not Low (No Discount)
        [InlineData(5, 10, false)]   // TC06: BVA High (50%) -> Not Low (No Discount)
        [InlineData(10, 10, false)]  // TC07: BVA Max (100%) -> Not Low (No Discount)
        public void IsLowInventory_BoundaryValueAnalysis(int current, int max, bool expected)
        {
            // Arrange
            var station = new Station { CurrentCapacity = current, MaxCapacity = max };

            // Act
            bool result = _service.IsLowInventory(station);

            // Assert
            Assert.Equal(expected, result);
        }

        // Testing FR1: Base Fares & Final Calculation 
        // Validates TC08 - TC11 using Equivalence Partitioning (EP)
        [Theory]
        [InlineData(500, 10, false, 5000)]   // TC08: Bike, 10 min, No Discount (500 * 10 = 5000)
        [InlineData(1500, 10, false, 15000)] // TC09: Scooter, 10 min, No Discount (1500 * 10 = 15000)
        [InlineData(500, 10, true, 4250)]    // TC10: Bike, 10 min, 15% Discount (5000 * 0.85 = 4250)
        [InlineData(1500, 20, true, 25500)]  // TC11: Scooter, 20 min, 15% Discount (30000 * 0.85 = 25500)
        public void CalculateFare_PricingLogic_EP(double rate, int minutes, bool isLow, double expected)
        {
            // Arrange
            var vehicle = new Vehicle { FarePerMin = rate };

            // Act
            double actual = _service.CalculateFare(vehicle, minutes, isLow);

            // Assert
            Assert.Equal(expected, actual);
        }

        // TC12: Edge Case - Instant return
        [Fact]
        public void CalculateFare_ZeroMinutes_ReturnsZero()
        {
            // Arrange
            var vehicle = new Vehicle { FarePerMin = 500 };

            // Act
            double result = _service.CalculateFare(vehicle, 0, false);

            // Assert
            Assert.Equal(0, result);
        }
    }
}