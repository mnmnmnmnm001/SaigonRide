# SaigonRide - Architecture & System Design

## System Architecture

```
┌─────────────────────────────────────────────────────────────────┐
│                        PRESENTATION LAYER                       │
│  Views (Razor Pages) + Controllers (ASP.NET Core MVC)          │
│  - Home Dashboard                                               │
│  - Rental Management (Create, View, End)                       │
│  - User Management (CRUD)                                       │
│  - Vehicle Management (CRUD)                                    │
│  - Station Management (CRUD)                                    │
│  - Reports (Revenue, Inventory)                                │
└──────────────────────┬──────────────────────────────────────────┘
                       │
┌──────────────────────▼──────────────────────────────────────────┐
│                     BUSINESS LOGIC LAYER                         │
│  Service Interfaces & Implementations                            │
│  ┌──────────────────────────────────────────────────────────┐   │
│  │ IFareCalculationService                                  │   │
│  │ - CalculateFare(vehicle, minutes, isLowInventory)        │   │
│  │ - ApplyDiscount(fare, isLowInventory)                    │   │
│  │ - IsLowInventory(station)                                │   │
│  └──────────────────────────────────────────────────────────┘   │
│  ┌──────────────────────────────────────────────────────────┐   │
│  │ IPaymentService                                          │   │
│  │ - ProcessPayment(user, amount, method)                   │   │
│  │ - GetAvailablePaymentMethods(user)                       │   │
│  └──────────────────────────────────────────────────────────┘   │
│  ┌──────────────────────────────────────────────────────────┐   │
│  │ IRentalService                                           │   │
│  │ - StartRental(userId, vehicleId, stationId)             │   │
│  │ - EndRental(rentalId, endStationId, paymentMethod)      │   │
│  │ - GetRentalById(rentalId)                                │   │
│  │ - GetUserRentals(userId)                                │   │
│  └──────────────────────────────────────────────────────────┘   │
│  ┌──────────────────────────────────────────────────────────┐   │
│  │ IReportService                                           │   │
│  │ - GetRevenueByVehicleReport(startDate, endDate)         │   │
│  │ - GetStationInventoryReport()                            │   │
│  └──────────────────────────────────────────────────────────┘   │
└──────────────────────┬──────────────────────────────────────────┘
                       │
┌──────────────────────▼──────────────────────────────────────────┐
│                       DATA ACCESS LAYER                          │
│  Entity Framework Core (Code-First)                             │
│  SaigonRideContext - DbSets for all entities                    │
└──────────────────────┬──────────────────────────────────────────┘
                       │
┌──────────────────────▼──────────────────────────────────────────┐
│                     DATABASE LAYER                               │
│  SQL Server / LocalDB                                           │
│  Tables: Users, Vehicles, Stations, Rentals                    │
└─────────────────────────────────────────────────────────────────┘
```

## Entity Relationship Diagram

```
┌──────────────┐
│    User      │ (Abstract Base - TPH Inheritance)
├──────────────┤
│ UserId (PK)  │
│ BankNum      │
│ PaymentCode  │
│ Payed        │
│ UserType     │
└────────┬─────┘
         │
    ┌────┴────┐
    │          │
┌───▼────────────────┐    ┌──────────────────┐
│ LocalCommuter      │    │ ForeignTourist   │
├────────────────────┤    ├──────────────────┤
│ P_MoMo: bool       │    │ Passport: string │
│ P_VNPay: bool      │    │ P_ApplePay: bool │
│                    │    │ P_PayPal: bool   │
└────────────────────┘    └──────────────────┘

                 ┌───────────────────┐
                 │    Rental         │
                 ├───────────────────┤
        ┌───────▶│ RentalId (PK)     │
        │        │ UserId (FK)       │◀──────┐
        │        │ VehicleId (FK)    │       │
        │        │ StartStationId FK │       │
        │        │ EndStationId (FK) │       │
        │        │ TimeStart         │       │
        │        │ TimeEnd           │       │
        │        │ CalculatedFare    │       │
        │        │ DiscountApplied   │       │
        │        │ FinalFare         │       │
        │        │ Status            │       │
        │        └─────────┬─────────┘       │
        │                  │                 │
┌───────┴──────┐    ┌──────┴──────┐    ┌────-┴─────────┐
│   Vehicle    │    │   Station   │    │    User       │
├──────────────┤    ├─────────────┤    ├───────────────┤
│VehicleId(PK) │    │StationId(PK)│    │UserId (PK)    │
│Type          │    │Name         │    │(References)   │
│FarePerMin    │    │CurrentCap   │    │               │
│State         │    │MaxCapacity  │    │               │
│Code          │    │Latitude     │    │               │
│CurrentPos    │    │Longitude    │    │               │
│              │    │Ratio        │    │               │
└──────────────┘    └─────────────┘    └───────────────┘
```

## Data Flow Diagram

### Rental Start Flow
```
User Interface (Create Rental)
    ↓
RentalController.Create()
    ↓
RentalService.StartRental()
    ├─ Validate User exists
    ├─ Validate Vehicle available (State == 0)
    ├─ Validate Station exists
    ├─ Create Rental record
    ├─ Update Vehicle State to 1 (In-Transit)
    └─ Save to Database
    ↓
Redirect to EndRental View
```

### Rental End Flow
```
User Interface (End Rental with Payment)
    ↓
RentalController.EndRental()
    ↓
RentalService.EndRental()
    ├─ Retrieve Rental record
    ├─ Calculate duration (TimeEnd - TimeStart)
    │
    ├─ FareCalculationService
    │  ├─ Calculate base fare = Vehicle.FarePerMin × minutes
    │  ├─ Check IsLowInventory(EndStation)
    │  │  └─ If capacity < 20%: Apply 15% discount
    │  └─ Return FinalFare = BaseFare - Discount
    │
    ├─ PaymentService.ProcessPayment()
    │  ├─ Validate payment method available for user
    │  ├─ Validate passport for ForeignTourist
    │  ├─ Process payment (simulate)
    │  └─ Update User.Payed
    │
    ├─ Update Rental:
    │  ├─ TimeEnd = Now
    │  ├─ EndStationId = provided
    │  ├─ CalculatedFare = baseFare
    │  ├─ DiscountApplied = discount
    │  ├─ FinalFare = final amount
    │  └─ Status = 1 (Completed)
    │
    ├─ Update Vehicle
    │  ├─ State = 0 (Available)
    │  └─ CurrentPos = EndStation location
    │
    ├─ Update Station
    │  ├─ CurrentCapacity += 1
    │  └─ Ratio = Capacity / MaxCapacity
    │
    └─ Save all changes
    ↓
Display Receipt with details
```

### Payment Processing Decision Tree
```
ProcessPayment(user, amount, method)
    │
    ├─ Is method in GetAvailablePaymentMethods()?
    │  └─ NO → Return false
    │
    ├─ Is user LocalCommuter?
    │  ├─ YES
    │  │  ├─ method == "MoMo" && P_MoMo?
    │  │  │  └─ YES → Process, Update Payed, Return true
    │  │  ├─ method == "VNPay" && P_VNPay?
    │  │  │  └─ YES → Process, Update Payed, Return true
    │  │  ├─ method == "Cash"?
    │  │  │  └─ YES → Process, Update Payed, Return true
    │  │  └─ ELSE → Return false
    │  │
    │  └─ Is user ForeignTourist?
    │     ├─ Passport is null/empty?
    │     │  └─ YES → Return false (Validation required)
    │     ├─ method == "ApplePay" && P_ApplePay?
    │     │  └─ YES → Process, Update Payed, Return true
    │     ├─ method == "PayPal" && P_PayPal?
    │     │  └─ YES → Process, Update Payed, Return true
    │     ├─ method == "Cash"?
    │     │  └─ YES → Process, Update Payed, Return true
    │     └─ ELSE → Return false
    │
    └─ ELSE → Return false
```

## Discount Logic

```
IsLowInventory Check:
    │
    └─ Station.GetRatio() = CurrentCapacity / MaxCapacity
       │
       ├─ If Ratio < 0.20 (< 20%):
       │  └─ TRUE: Low Inventory Incentive Active
       │     └─ Discount = CalculatedFare × 0.15
       │        FinalFare = CalculatedFare - Discount
       │
       └─ Else (≥ 20%):
          └─ FALSE: No Discount
             └─ FinalFare = CalculatedFare (full price)
```

## Service Dependencies

```
Dependency Injection Chain:

Program.cs registers:
    ├─ IFareCalculationService → FareCalculationService (Scoped)
    ├─ IPaymentService → PaymentService (Scoped)
    │  └─ PaymentService depends on IFareCalculationService
    ├─ IRentalService → RentalService (Scoped)
    │  └─ RentalService depends on:
    │     ├─ SaigonRideContext
    │     ├─ IFareCalculationService
    │     └─ IPaymentService
    └─ IReportService → ReportService (Scoped)
       └─ ReportService depends on SaigonRideContext

Controllers inject dependencies:
    ├─ HomeController: SaigonRideContext
    ├─ RentalController: SaigonRideContext, IRentalService, IPaymentService
    ├─ UserController: SaigonRideContext
    ├─ VehicleController: SaigonRideContext
    ├─ StationController: SaigonRideContext
    └─ ReportController: IReportService
```

## State Transitions

### Vehicle States
```
┌─────────┐
│ State 0 │ ◀──────────────────────┐
│Available│                        │
└────┬────┘                        │
     │ startRental()               │
     ▼                             │
┌─────────┐      endRental()   ┌───┴────┐
│ State 1 │────────────────────▶│State 0 │
│In-Transit                     │Available
└────┬────┘                     └────────┘
     │ maintenance
     ▼
┌─────────┐
│ State 2 │
│Maintenance
└─────────┘
```

### Rental Status
```
START RENTAL
    ↓
Status = 0 (Active)
    ↓
  [In Progress]
    ↓
END RENTAL (Success)
    ↓
Status = 1 (Completed) ────→ Generate Receipt

OR

CANCEL RENTAL
    ↓
Status = 2 (Cancelled)
```

## Report Structure

### Revenue Report
```
Parameters: startDate, endDate
    │
    ├─ Query all Rentals where:
    │  ├─ Status == 1 (Completed)
    │  ├─ TimeStart >= startDate
    │  └─ TimeStart <= endDate
    │
    ├─ Group by Vehicle.Type
    │
    └─ Calculate for each type:
       ├─ RentalCount = Count()
       ├─ TotalRevenue = Sum(FinalFare)
       ├─ AverageFare = TotalRevenue / RentalCount
       └─ TotalDiscount = Sum(DiscountApplied)
```

### Inventory Report
```
Parameters: None (Current snapshot)
    │
    ├─ Query all Stations
    │
    └─ For first/current station:
       ├─ CurrentCapacity
       ├─ MaxCapacity
       ├─ UtilizationRatio = CurrentCapacity / MaxCapacity
       │
       └─ Determine Status:
          ├─ If Ratio < 0.20 → "Low"
          ├─ If Ratio < 0.50 → "Medium"
          └─ Else → "High"
```

## Class Hierarchy

```
User (Abstract Base Class)
 │
 ├─ LocalCommuter
 │  ├─ Properties:
 │  │  ├─ P_MoMo: bool
 │  │  └─ P_VNPay: bool
 │  │
 │  └─ Inherited from User:
 │     ├─ UserId
 │     ├─ BankNum
 │     ├─ ChosenPaymentCode
 │     ├─ Payed
 │     └─ UserType = "LocalCommuter"
 │
 └─ ForeignTourist
    ├─ Properties:
    │  ├─ Passport: string
    │  ├─ P_ApplePay: bool
    │  └─ P_PayPal: bool
    │
    └─ Inherited from User:
       ├─ UserId
       ├─ BankNum
       ├─ ChosenPaymentCode
       ├─ Payed
       └─ UserType = "ForeignTourist"
```

## Pricing Table

```
┌──────────────┬─────────────────┬──────────────┐
│ Vehicle Type │ Fare Per Minute │ Example (30m)│
├──────────────┼─────────────────┼──────────────┤
│ StandardBike │     500 VND     │  15,000 VND  │
├──────────────┼─────────────────┼──────────────┤
│ E-Bike       │   1,000 VND     │  30,000 VND  │
├──────────────┼─────────────────┼──────────────┤
│ E-Scooter    │   1,500 VND     │  45,000 VND  │
└──────────────┴─────────────────┴──────────────┘

Low Inventory Discount: 15% off final fare
```

## Configuration

### appsettings.json
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=SaigonRide;Trusted_Connection=true;"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information"
    }
  }
}
```

### Program.cs Services
```csharp
// Database Context
builder.Services.AddDbContext<SaigonRideContext>(options =>
    options.UseSqlServer(connectionString));

// Business Services
builder.Services.AddScoped<IFareCalculationService, FareCalculationService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<IRentalService, RentalService>();
builder.Services.AddScoped<IReportService, ReportService>();

// Application Setup
app.UseRouting();
app.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
```

---

This architecture supports:
- ✅ Scalability through service layer abstraction
- ✅ Testability with dependency injection
- ✅ Maintainability through clear separation of concerns
- ✅ Extensibility for future payment gateway integrations
- ✅ Multi-user type support with polymorphism
- ✅ Complex business logic (dynamic pricing, discounts)
