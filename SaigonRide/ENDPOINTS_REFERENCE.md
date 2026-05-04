# SaigonRide - Controller Endpoints Reference

## Overview
This document outlines all controller endpoints, their purposes, parameters, and example usage.

---

## 🏠 HomeController

**Base Route:** `/` or `/Home`

### Index (Dashboard)
```
GET /Home/Index
GET / (default)

Purpose: Display system dashboard with statistics
Returns: Dashboard view with:
  - Total Users count
  - Total Vehicles count
  - Total Stations count
  - Total Rentals count
  - Total Revenue (VND)

Example: https://localhost:5001/
```

### Privacy
```
GET /Home/Privacy

Purpose: Display privacy policy
Returns: Privacy view
```

### Error
```
GET /Home/Error (automatic)

Purpose: Handle application errors
Returns: Error view with RequestId
```

---

## 👥 UserController

**Base Route:** `/User`

### Index (List All Users)
```
GET /User
GET /User/Index

Purpose: Display all users in table format
Returns: User list view
Displays: ID, Type, Bank, Payment method, Status
```

### Details
```
GET /User/Details/{id}

Parameters:
  - id (int, required): User ID

Purpose: Display full user profile
Returns: User details view
Displays: All user information including payment methods
Example: /User/Details/1
```

### Create (Form)
```
GET /User/Create

Purpose: Display user creation form
Returns: Create user form view
Fields: User type, bank number, payment preferences
```

### Create (Post)
```
POST /User/Create
Content-Type: application/x-www-form-urlencoded

Parameters:
  - userType (string): "LocalCommuter" or "ForeignTourist"
  - BankNum (string): Bank account number
  - ChosenPaymentCode (string): Initial payment method
  - P_MoMo (bool): Enable MoMo (LocalCommuter)
  - P_VNPay (bool): Enable VNPay (LocalCommuter)
  - Passport (string): Passport number (ForeignTourist)
  - P_ApplePay (bool): Enable Apple Pay (ForeignTourist)
  - P_PayPal (bool): Enable PayPal (ForeignTourist)

Purpose: Create new user
Returns: Redirect to Index on success, Form with errors on failure
Example Form Data:
{
  userType: "ForeignTourist",
  BankNum: "INTL001",
  ChosenPaymentCode: "applepay",
  Passport: "A12345678",
  P_ApplePay: true
}
```

### Edit (Form)
```
GET /User/Edit/{id}

Parameters:
  - id (int, required): User ID

Purpose: Display user edit form
Returns: Edit user form view
```

### Edit (Post)
```
POST /User/Edit/{id}
Content-Type: application/x-www-form-urlencoded

Parameters:
  - UserId (int): User ID
  - BankNum (string): Updated bank number
  - ChosenPaymentCode (string): Updated payment method
  - Payed (double): Updated payment total

Purpose: Update user information
Returns: Redirect to Index on success
```

### Delete (Confirmation)
```
GET /User/Delete/{id}

Parameters:
  - id (int, required): User ID

Purpose: Display delete confirmation
Returns: Delete confirmation view
```

### Delete (Post)
```
POST /User/Delete
(via DeleteConfirmed)

Parameters:
  - id (int): User ID to delete

Purpose: Delete user from system
Returns: Redirect to Index
```

---

## 🚗 VehicleController

**Base Route:** `/Vehicle`

### Index
```
GET /Vehicle
GET /Vehicle/Index

Purpose: Display all vehicles
Returns: Vehicle list with status
Displays: ID, Type, Code, Fare/min, Position, Status
```

### Details
```
GET /Vehicle/Details/{id}

Parameters:
  - id (int): Vehicle ID

Purpose: Show vehicle details
Returns: Vehicle details view
Example: /Vehicle/Details/1
```

### Create (Form)
```
GET /Vehicle/Create

Purpose: Display vehicle creation form
Returns: Create vehicle form
Fields: Type, Code, Fare, Position
```

### Create (Post)
```
POST /Vehicle/Create

Parameters:
  - Type (string): "StandardBike", "EBike", or "Scooter"
  - Code (string): Unique vehicle code
  - FarePerMin (double): Pricing in VND
  - CurrentPos (string): Location

Purpose: Add new vehicle
Returns: Redirect to Index on success
Example:
{
  Type: "EBike",
  Code: "EB003",
  FarePerMin: 1000,
  CurrentPos: "Ben Thanh Market"
}
```

### Edit (Form)
```
GET /Vehicle/Edit/{id}

Parameters:
  - id (int): Vehicle ID

Purpose: Display edit form
```

### Edit (Post)
```
POST /Vehicle/Edit

Parameters:
  - VehicleId (int): Vehicle ID
  - Type (string): Vehicle type
  - Code (string): Vehicle code
  - FarePerMin (double): Pricing
  - State (int): 0=Available, 1=In-Transit, 2=Maintenance
  - CurrentPos (string): Location

Purpose: Update vehicle
Returns: Redirect to Index
```

### Delete
```
GET /Vehicle/Delete/{id}
POST /Vehicle/Delete

Purpose: Delete vehicle
Returns: Confirmation view → Redirect to Index
```

---

## 🏪 StationController

**Base Route:** `/Station`

### Index
```
GET /Station
GET /Station/Index

Purpose: List all stations with capacity info
Returns: Station table
Displays: ID, Name, Location, Capacity, Ratio, Status
```

### Details
```
GET /Station/Details/{id}

Parameters:
  - id (int): Station ID

Purpose: Show detailed station info
Returns: Station details with capacity visualization
```

### Create (Form)
```
GET /Station/Create

Purpose: Display station creation form
Returns: Create form view
```

### Create (Post)
```
POST /Station/Create

Parameters:
  - Name (string): Station name
  - Latitude (double): GPS latitude
  - Longitude (double): GPS longitude
  - MaxCapacity (int): Max vehicles
  - CurrentCapacity (int): Starting inventory

Purpose: Add new station
Returns: Redirect to Index
Example:
{
  Name: "Airport Station",
  Latitude: 10.8260,
  Longitude: 106.6545,
  MaxCapacity: 40,
  CurrentCapacity: 20
}
```

### Edit
```
GET /Station/Edit/{id}
POST /Station/Edit

Parameters:
  - StationId (int): Station ID
  - Name (string)
  - Latitude (double)
  - Longitude (double)
  - MaxCapacity (int)
  - CurrentCapacity (int)

Purpose: Update station information
```

### Delete
```
GET /Station/Delete/{id}
POST /Station/Delete

Purpose: Delete station
```

---

## 🚴 RentalController

**Base Route:** `/Rental`

### Index
```
GET /Rental
GET /Rental/Index

Purpose: Display all rentals
Returns: Rental table with all transactions
Displays: ID, User, Vehicle, Stations, Times, Fares, Status
```

### Details
```
GET /Rental/Details/{id}

Parameters:
  - id (int): Rental ID

Purpose: Show rental details
Returns: Detailed view with journey info
```

### Create (Form - Start Rental)
```
GET /Rental/Create

Purpose: Display rental start form
Returns: Form with dropdowns for:
  - Users (with type badge)
  - Available vehicles only (State=0)
  - Stations (with capacity info)
```

### Create (Post - Start Rental)
```
POST /Rental/Create

Parameters:
  - UserId (int): Who is renting
  - VehicleId (int): Which vehicle
  - StartStationId (int): Where renting from

Purpose: Start new rental
Returns: Redirect to EndRental form
Side Effects:
  - Creates Rental record with TimeStart=Now, Status=0
  - Updates Vehicle.State to 1 (In-Transit)

Example:
{
  UserId: 1,
  VehicleId: 5,
  StartStationId: 1
}
```

### EndRental (Form)
```
GET /Rental/EndRental/{id}

Parameters:
  - id (int): Rental ID

Purpose: Display rental completion form
Returns: Form showing:
  - Current rental summary
  - End station selector
  - Payment method selector (filtered by user type)
```

### EndRental (Post)
```
POST /Rental/EndRental/{id}

Parameters:
  - id (int, form hidden): Rental ID
  - EndStationId (int): Return station
  - paymentMethod (string): "MoMo", "VNPay", "ApplePay", "PayPal", "Cash"

Purpose: Complete rental with payment
Returns: Redirect to Receipt on success
Side Effects:
  - Calculates duration: TimeEnd = Now
  - Calculates base fare: Vehicle.FarePerMin × minutes
  - Checks if EndStation.GetRatio() < 0.20 for discount
  - Applies 15% discount if low inventory
  - Validates payment method for user
  - Processes payment (simulated)
  - Updates Vehicle.State to 0 (Available)
  - Updates Vehicle.CurrentPos to EndStation
  - Increments EndStation.CurrentCapacity
  - Updates Rental.Status to 1 (Completed)

Example:
{
  id: 1,
  EndStationId: 3,
  paymentMethod: "MoMo"
}

Response (on success):
  Redirect to /Rental/Receipt/1
```

### Receipt
```
GET /Rental/Receipt/{id}

Parameters:
  - id (int): Rental ID

Purpose: Display completed rental receipt
Returns: Receipt view showing:
  - Journey summary (vehicle, stations, time)
  - Fare breakdown:
    * Base Fare
    * Discount applied (if any)
    * Final amount due
  - Discount reason (if applicable)
  - Payment confirmation

Example Display:
  Base Fare: 15,000 VND
  Low Inventory Discount (15%): -2,250 VND
  Total Amount Due: 12,750 VND ✓
```

### ListVehicles
```
GET /Rental/ListVehicles

Purpose: Display available vehicles for rental
Returns: Available vehicles list
```

### UserRentals
```
GET /Rental/UserRentals/{id}

Parameters:
  - id (int): User ID

Purpose: Show all rentals for specific user
Returns: Rental history for user (ordered by date)
```

---

## 📊 ReportController

**Base Route:** `/Report`

### RevenueReport
```
GET /Report/RevenueReport
GET /Report/RevenueReport?startDate={date}&endDate={date}

Parameters (Optional):
  - startDate (DateTime): Report period start
  - endDate (DateTime): Report period end
  - Default: Last 30 days if not specified

Purpose: Generate revenue report by vehicle category
Returns: RevenueByVehicleReport view
Displays:
  - Vehicle Type
  - Total Rentals (count)
  - Total Revenue (VND)
  - Average Fare per rental
  - Total Discount Applied (VND)

Query Logic:
  1. Fetch all Rentals where:
     - Status == 1 (Completed)
     - TimeStart >= startDate
     - TimeStart <= endDate
  2. Group by Vehicle.Type
  3. Calculate metrics for each type

Example URLs:
  /Report/RevenueReport
  /Report/RevenueReport?startDate=2026-01-01&endDate=2026-01-31
```

### InventoryReport
```
GET /Report/InventoryReport

Purpose: Generate station inventory report
Returns: StationInventoryReport view
Displays:
  - Station ID and Name
  - Current Capacity / Max Capacity
  - Utilization Ratio (percentage)
  - Status: "Low" | "Medium" | "High"
  - Capacity progress bar

Status Calculation:
  - Ratio = CurrentCapacity / MaxCapacity
  - If Ratio < 0.20 → "Low" (Discount eligible)
  - If Ratio < 0.50 → "Medium"
  - Else → "High"

Example Display:
  Station: Tan Binh Station
  Capacity: 5/25
  Utilization: 20%
  Status: Low Inventory ⚠️

Note: Shows current snapshot, not historical
```

---

## 🔄 Data Flow Examples

### Complete Rental Flow

```
1. GET /Rental/Create
   ↓ Display form

2. POST /Rental/Create
   Body: {UserId:1, VehicleId:5, StartStationId:1}
   ↓ Create rental, update vehicle state

3. Redirect → GET /Rental/EndRental/1
   ↓ Display completion form

4. POST /Rental/EndRental/1
   Body: {EndStationId:3, paymentMethod:"MoMo"}
   ↓ Calculate fare, apply discount, process payment

5. Redirect → GET /Rental/Receipt/1
   ↓ Display receipt with details
```

### Payment Processing Flow

```
POST /Rental/EndRental
  ↓
RentalService.EndRental()
  ├─ FareCalculationService.CalculateFare()
  │  ├─ Check IsLowInventory(station)
  │  └─ Apply discount if true
  │
  ├─ PaymentService.ProcessPayment()
  │  ├─ GetAvailablePaymentMethods(user)
  │  ├─ Validate method in list
  │  ├─ Check passport for ForeignTourist
  │  └─ Update User.Payed
  │
  ├─ Update Vehicle state
  ├─ Update Station capacity
  ├─ Save rental record
  │
  └─ Return to Receipt
```

---

## 📝 Response Formats

### Error Responses
```
Form Validation Error:
  Returns: Same form view with ModelState errors
  Display: Red error messages under invalid fields

Application Error:
  Returns: Error view with:
  - RequestId
  - Exception message
  - Stack trace (development only)
```

### Successful Responses
```
POST requests:
  Returns: HTTP 302 Redirect to appropriate view

GET requests:
  Returns: HTML view with data
```

---

## 🔐 Validation Rules by Endpoint

### RentalController.Create (Start)
```
✓ UserId must exist in Users table
✓ VehicleId must exist and State == 0 (Available)
✓ StartStationId must exist
✓ No duplicate active rentals for same vehicle
```

### RentalController.EndRental (Complete)
```
✓ RentalId must exist and Status == 0 (Active)
✓ EndStationId must exist
✓ Payment method must be in user's available methods
✓ For ForeignTourist: Passport must not be empty
✓ Duration must be > 0
```

### UserController.Create
```
✓ BankNum not empty
✓ ChosenPaymentCode not empty
✓ For ForeignTourist: Passport should be provided
✓ At least one payment method must be enabled
```

### VehicleController.Create
```
✓ Type must be: StandardBike, EBike, or Scooter
✓ Code must be unique
✓ FarePerMin > 0
✓ CurrentPos not empty
```

### StationController.Create
```
✓ Name not empty and unique
✓ MaxCapacity > 0
✓ CurrentCapacity <= MaxCapacity
✓ Latitude and Longitude valid ranges
```

---

## 💾 Database Operations

### Rental Lifecycle

**Start (POST /Rental/Create):**
```csharp
INSERT INTO Rentals 
  (UserId, VehicleId, StartStationId, TimeStart, Status)
VALUES 
  (1, 5, 1, DateTime.Now, 0);

UPDATE Vehicles 
SET State = 1 
WHERE VehicleId = 5;
```

**End (POST /Rental/EndRental):**
```csharp
UPDATE Rentals
SET TimeEnd = DateTime.Now,
    EndStationId = 3,
    CalculatedFare = 15000,
    DiscountApplied = 0,
    FinalFare = 15000,
    Status = 1
WHERE RentalId = 1;

UPDATE Vehicles
SET State = 0, CurrentPos = 'Station 3'
WHERE VehicleId = 5;

UPDATE Stations
SET CurrentCapacity = 16
WHERE StationId = 3;

UPDATE Users
SET Payed = 65000
WHERE UserId = 1;
```

---

## 🧪 Example API Calls (for testing)

### Create User (cURL)
```bash
curl -X POST https://localhost:5001/User/Create \
  -H "Content-Type: application/x-www-form-urlencoded" \
  -d "userType=ForeignTourist&BankNum=INTL001&ChosenPaymentCode=applepay&Passport=A12345678&P_ApplePay=true"
```

### Start Rental (Browser Form)
```html
<form method="post" action="/Rental/Create">
  <select name="UserId"><option value="1">User 1</option></select>
  <select name="VehicleId"><option value="5">Scooter ES001</option></select>
  <select name="StartStationId"><option value="1">Ben Thanh Market</option></select>
  <button type="submit">Start Rental</button>
</form>
```

### End Rental (Browser Form)
```html
<form method="post" action="/Rental/EndRental/1">
  <select name="EndStationId"><option value="3">Saigon Center</option></select>
  <select name="paymentMethod"><option value="MoMo">MoMo</option></select>
  <button type="submit">Complete Rental</button>
</form>
```

---

## 📋 Endpoint Summary Table

| Controller | Action | Method | Purpose |
|-----------|--------|--------|---------|
| Home | Index | GET | Dashboard |
| User | Index | GET | List users |
| User | Create | GET/POST | Add user |
| User | Details | GET | View user |
| User | Edit | GET/POST | Update user |
| User | Delete | GET/POST | Remove user |
| Vehicle | Index | GET | List vehicles |
| Vehicle | Create | GET/POST | Add vehicle |
| Vehicle | Details | GET | View vehicle |
| Vehicle | Edit | GET/POST | Update vehicle |
| Vehicle | Delete | GET/POST | Remove vehicle |
| Station | Index | GET | List stations |
| Station | Create | GET/POST | Add station |
| Station | Details | GET | View station |
| Station | Edit | GET/POST | Update station |
| Station | Delete | GET/POST | Remove station |
| Rental | Index | GET | List rentals |
| Rental | Create | GET/POST | Start rental |
| Rental | EndRental | GET/POST | Complete rental |
| Rental | Receipt | GET | Show receipt |
| Rental | Details | GET | View rental |
| Report | RevenueReport | GET | Revenue analysis |
| Report | InventoryReport | GET | Inventory status |

---

**Last Updated:** January 2026  
**Version:** 1.0  
**Framework:** ASP.NET Core MVC (.NET 10)
