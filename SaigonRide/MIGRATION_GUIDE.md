# Database Migration Guide

## Problem
The previous migrations were conflicting with the database, causing the error:
> "Cannot find the object 'Admins' because it does not exist or you do not have permissions"

## Solution
All old migrations have been removed and replaced with a clean, fresh migration setup.

## New Migrations

### 1. **20260507_InitialCreate.cs**
- Creates all required database tables from scratch
- Tables: Admins, Stations, Users, Vehicles, Rentals, RentalTransactions
- Seeds initial data:
  - 3 Stations (Ben Thanh Market, District 1 Hub, Saigon Center)
  - 8 Vehicles (2 StandardBikes, 2 EBikes, 4 Scooters)
  - 4 Users (2 LocalCommuters, 2 ForeignTourists)

### 2. **20260507_AddVehiclesToMatchCapacity.cs**
- Adds 117 more vehicles to reach 125 total
- Updates station capacities:
  - Ben Thanh Market: 110
  - District 1 Hub: 10
  - Saigon Center: 5
- Vehicle distribution:
  - StandardBikes (SB): 45 total @ 500 VND/min
  - EBikes (EB): 44 total @ 1,000 VND/min
  - Scooters (ES): 36 total @ 1,500 VND/min

## Steps to Apply

### Option 1: Using Visual Studio Package Manager Console
```powershell
# Delete the old database (if it exists)
# Drop the entire database from SQL Server Management Studio, or run:
# DROP DATABASE [YourDatabaseName]

# Apply migrations
Update-Database
```

### Option 2: Using Command Line (PowerShell)
```powershell
cd "C:\Users\mnmnm\OneDrive\Desktop\c#\SaigonRide\SaigonRide"
dotnet ef database drop --force
dotnet ef database update
```

### Option 3: Manual SQL
1. Drop the existing SaigonRide database entirely
2. Run the application - EF Core will create a new database with the migrations applied

## Expected Result
- Fresh database with correct schema
- 125 vehicles seeded across 3 stations
- Station capacities match vehicle counts (110 + 10 + 5 = 125)
- All relationships properly configured (no referential integrity issues)

## Verification

After applying migrations, verify:
1. Database `SaigonRide` exists with all 6 tables
2. Total vehicles = 125
3. Station capacities = 110, 10, 5 (total 125)
4. No error messages when running the application
