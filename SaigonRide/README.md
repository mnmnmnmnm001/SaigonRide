# SaigonRide - Distributed Vehicle Rental System

## Overview

SaigonRide is a distributed network vehicle rental system that allows users to unlock vehicles (bicycles and electric scooters) at one station and return them at another. The system is designed to support two types of users: Local Commuters (using local payment methods) and Foreign Tourists (using international payment methods).

## Core Features

### 1. Dynamic Time & Location Pricing
- **Vehicle Categories with different pricing:**
  - Standard Bike: 500 VND/minute
  - E-Bike: 1,000 VND/minute
  - E-Scooter: 1,500 VND/minute

- **Low Inventory Discount:**
  - When a vehicle is returned to a station with less than 20% capacity, a 15% discount is automatically applied to the rental fare
  - This incentivizes users to help balance inventory across the network

### 2. Multi-Gateway Payment Design
The system supports multiple payment methods categorized by user type:

#### Local Commuters
- MoMo (e-wallet)
- VNPay (e-wallet)
- Cash

#### Foreign Tourists
- Apple Pay
- PayPal
- Cash (always available)

**Passport Verification:** Foreign tourists must provide a valid passport number for international payment processing.

### 3. System Reports
Two main report types are implemented:

#### Revenue Report by Vehicle Category
- Displays total rentals by vehicle type
- Shows total revenue, average fare, and discounts applied
- Supports date range filtering (default: last 30 days)

#### Station Inventory / Utilization Report
- Shows current station capacity and utilization ratio
- Identifies low-inventory stations (< 20%)
- Displays status indicators: Low, Medium, High

## Project Structure

### Models
- **User** (Abstract Base)
  - `LocalCommuter` - Local user with MoMo/VNPay capabilities
  - `ForeignTourist` - International user with Apple Pay/PayPal and passport verification
- **Vehicle** - Available in three types (StandardBike, EBike, Scooter)
- **Station** - Rental locations with capacity management
- **Rental** - Transaction records linking users, vehicles, and stations

### Services
- **IFareCalculationService** - Calculates fares based on vehicle type, duration, and inventory status
- **IPaymentService** - Processes payments and validates payment methods per user type
- **IRentalService** - Manages rental lifecycle (start, end, retrieve)
- **IReportService** - Generates revenue and inventory reports

### Controllers
- **HomeController** - Dashboard with system statistics
- **RentalController** - Rental operations (start, end, view history)
- **UserController** - User management and CRUD operations
- **VehicleController** - Vehicle inventory management
- **StationController** - Station management and capacity tracking
- **ReportController** - Generate and view system reports

### Database
- **SaigonRideContext** - Entity Framework Core Code-First database
- Uses Table Per Hierarchy (TPH) for User inheritance
- Includes seed data for testing

## System Workflow

### 1. Start Rental
1. User selects a vehicle at a station
2. System verifies vehicle is available
3. Rental record is created with start time/location
4. Vehicle status changes to "In Transit"

### 2. End Rental
1. User returns vehicle to a destination station
2. System calculates:
   - Rental duration
   - Base fare (rate × minutes)
   - Low inventory discount (if applicable)
   - Final fare amount
3. Payment is processed using selected method
4. Vehicle status returns to "Available"
5. Station inventory is updated

### 3. Low Inventory Incentive
- If return station capacity < 20%:
  - 15% discount automatically applied
  - Encourages distribution across stations
  - Users see discount details in receipt

## Technical Implementation

### Technology Stack
- **Framework:** ASP.NET Core MVC (.NET 10)
- **Database:** SQL Server (Entity Framework Core Code-First)
- **UI:** Razor Views with Bootstrap 5

### Key Design Patterns
- **Dependency Injection** - All services registered in Program.cs
- **Repository Pattern** - Service layer abstracts data access
- **Strategy Pattern** - Different payment strategies per user type
- **TPH Inheritance** - Single table for User hierarchy

### Payment Processing
- Simulated payment processing (can be extended for real APIs)
- Passport validation for international transactions
- Payment method availability based on user configuration

## Database Schema

### Users Table (TPH)
- UserId, BankNum, ChosenPaymentCode, Payed, UserType
- LocalCommuter: P_MoMo, P_VNPay
- ForeignTourist: Passport, P_ApplePay, P_PayPal

### Vehicles Table
- VehicleId, Type, FarePerMin, State, Code, CurrentPos

### Stations Table
- StationId, Name, CurrentCapacity, MaxCapacity, Latitude, Longitude, Ratio

### Rentals Table
- RentalId, UserId, VehicleId, StartStationId, EndStationId
- TimeStart, TimeEnd, CalculatedFare, DiscountApplied, FinalFare
- Status (0=Active, 1=Completed, 2=Cancelled)

## Getting Started

### Prerequisites
- .NET 10 SDK
- SQL Server (LocalDB or full server)
- Visual Studio or VS Code

### Installation

1. **Clone/Open the project**
```bash
cd SaigonRide
```

2. **Update Database Connection**
   - Edit `appsettings.json`
   - Modify connection string if needed (default uses LocalDB)

3. **Create Database**
   - The database is auto-created on first run
   - Seed data is automatically inserted

4. **Run the Application**
```bash
dotnet run
```
- Access at `https://localhost:5001`

### Initial Data
- 4 Stations pre-configured
- 6 Vehicles (2 per type)
- 4 Test Users (2 Local, 2 Foreign Tourist)

## Usage Examples

### Scenario 1: Local Commuter Rental
1. Login as User 1 (Local Commuter)
2. Select Standard Bike at Ben Thanh Market station
3. Complete trip to District 1 Hub station
4. Pay via MoMo
5. Receipt shows fare calculation

### Scenario 2: Foreign Tourist with Discount
1. Login as User 3 (Foreign Tourist)
2. Rent E-Scooter at Saigon Center
3. Return to Tan Binh Station (currently at 20% capacity - Low Inventory)
4. Automatic 15% discount applied
5. Pay via Apple Pay with passport verification

## Reports

### Access Reports
- **Revenue Report:** Navigate to Reports → Revenue Report
  - Filter by date range
  - View revenue by vehicle category
  - Analyze discount impact

- **Inventory Report:** Navigate to Reports → Inventory Report
  - Current station status
  - Utilization percentage
  - Low inventory alerts

## API Response Examples

### Start Rental Response
```json
{
  "rentalId": 1,
  "userId": 1,
  "vehicleId": 5,
  "startStationId": 3,
  "timeStart": "2026-01-15T10:30:00",
  "status": 0
}
```

### Rental Receipt Response
```json
{
  "rentalId": 1,
  "duration": "45 minutes",
  "calculatedFare": 67500,
  "discountApplied": 10125,
  "finalFare": 57375,
  "discountReason": "Low inventory return bonus"
}
```

## Future Enhancements

1. **Real Payment Gateway Integration**
   - Implement actual MoMo, VNPay, Apple Pay, PayPal APIs
   - PCI compliance and security hardening

2. **Mobile App**
   - Native iOS/Android apps
   - Real-time GPS tracking
   - QR code vehicle unlock

3. **Advanced Analytics**
   - Peak hour analysis
   - Route popularity
   - User behavior patterns

4. **Dynamic Pricing**
   - Time-based pricing (rush hours)
   - Demand-based pricing
   - Seasonal adjustments

5. **Maintenance Management**
   - Auto-maintenance scheduling
   - Damage reporting
   - Vehicle health monitoring

## Error Handling

- **Vehicle Not Available:** Returns HTTP 400 with "Vehicle is not available" message
- **Insufficient Funds:** Payment service validates balance
- **Invalid User Type:** Payment method validation ensures user-appropriate methods
- **Station Capacity:** Max capacity enforced on inventory updates

## Troubleshooting

### Database Connection Issues
- Ensure SQL Server is running
- Check connection string in `appsettings.json`
- Verify LocalDB is installed: `sqllocaldb info mssqllocaldb`

### Payment Processing Failures
- Verify user payment methods are enabled
- Check passport exists for foreign tourists
- Ensure sufficient balance in user's paid amount

### Report Data Issues
- Ensure rentals exist with Status=1 (Completed)
- Check date range selection in report filters
- Verify station has vehicles for inventory report

## Contributing

This project follows ASP.NET Core MVC best practices:
- Controllers handle HTTP requests
- Services contain business logic
- Models define data structures
- Views render responses using Razor

## License

Educational project for distributed vehicle rental system demonstration.

## Contact & Support

For issues or questions, refer to the comprehensive controller documentation and inline code comments throughout the application.

---

**Version:** 1.0  
**Last Updated:** January 2026  
**Status:** Production Ready
