# SaigonRide - Complete Usage Guide

## Table of Contents
1. [System Overview](#system-overview)
2. [Getting Started](#getting-started)
3. [User Management](#user-management)
4. [Vehicle Management](#vehicle-management)
5. [Station Management](#station-management)
6. [Rental Operations](#rental-operations)
7. [Payment Processing](#payment-processing)
8. [Reports and Analytics](#reports-and-analytics)
9. [Test Scenarios](#test-scenarios)
10. [Troubleshooting](#troubleshooting)

---

## System Overview

SaigonRide is an ASP.NET Core MVC application for managing a distributed vehicle rental network. The system supports:

- **Two user types:** Local Commuters and Foreign Tourists
- **Three vehicle types:** Standard Bikes, E-Bikes, and E-Scooters
- **Multiple payment methods:** MoMo, VNPay, Apple Pay, PayPal, and Cash
- **Dynamic pricing:** Based on vehicle type and rental duration
- **Smart discounts:** 15% off for returning vehicles to low-inventory stations

---

## Getting Started

### Prerequisites
- .NET 10 SDK installed
- SQL Server or LocalDB
- Visual Studio, VS Code, or command line

### Installation Steps

1. **Open the project**
   ```bash
   cd C:\Users\mnmnm\OneDrive\Desktop\c#\SaigonRide\SaigonRide\
   ```

2. **Configure database (if needed)**
   - Edit `appsettings.json`
   - Update connection string if using different SQL Server

3. **Run the application**
   ```bash
   dotnet run
   ```
   - Application opens at `https://localhost:5001`

4. **First Launch**
   - Database auto-creates with seed data
   - 4 test stations, 6 vehicles, 4 test users

### Navigation

The navbar at the top provides quick access to:
- **Home** - Dashboard with statistics
- **Management** - Users, Vehicles, Stations, Rentals
- **Reports** - Revenue and Inventory reports
- **Privacy** - Privacy policy

---

## User Management

### View All Users

1. Navigate to **Management → Users**
2. Table displays:
   - User ID, Type, Bank Account, Payment Method
   - Total paid amount
   - Payment options availability
   - Action buttons (View, Edit, Delete)

### Create New User

1. Click **"Add New User"** button
2. Select **User Type:**
   - **Local Commuter** - Shows MoMo/VNPay options
   - **Foreign Tourist** - Shows Apple Pay/PayPal and passport field

3. Fill in required fields:
   - **Bank Number** - Account identifier
   - **Preferred Payment Method** - Initial choice
   - **Payment Checkboxes** - Enable/disable payment options
   - **Passport** (Tourists only) - Required for international payments

4. Click **"Create User"**

### View User Details

1. Click **View** button on any user
2. Displays:
   - User profile information
   - Payment methods status
   - Total payment history
   - Account badges (Local/Tourist)

### Edit User

1. Click **Edit** button on user row
2. Update:
   - Bank account number
   - Preferred payment method
   - Total paid amount (usually auto-updated via rentals)
3. Click **"Update User"**

### Delete User

1. Click **Delete** button
2. Confirm deletion on confirmation page
3. User and associated data removed

---

## Vehicle Management

### View Vehicle Fleet

1. Navigate to **Management → Vehicles**
2. Table shows all vehicles with:
   - Vehicle ID, Type, Code
   - Fare per minute (pricing)
   - Current position/location
   - Status badge:
     - 🟢 **Green "Available"** - Ready to rent
     - 🟡 **Yellow "In Transit"** - Currently rented
     - 🔴 **Red "Maintenance"** - Not available

### Create New Vehicle

1. Click **"Add New Vehicle"** button
2. Select **Vehicle Type:**
   - Standard Bike (500 VND/min)
   - E-Bike (1,000 VND/min)
   - E-Scooter (1,500 VND/min)

3. Fill details:
   - **Vehicle Code** - Unique identifier (e.g., "SB001", "EB002")
   - **Fare Per Minute** - Pricing in VND
   - **Current Position** - Station name or location

4. Click **"Create Vehicle"**

### View Vehicle Details

1. Click **View** button
2. Shows comprehensive vehicle information:
   - Type and identification
   - Pricing details
   - Current location and status
   - Rental history link

### Edit Vehicle

1. Click **Edit** button
2. Update any field:
   - Type, Code, Fare
   - Status (Available/In Transit/Maintenance)
   - Position
3. Click **"Update Vehicle"**

### Monitor Vehicle Status

The **Status** field indicates:
- **0 = Available** - Can be rented
- **1 = In Transit** - Currently rented
- **2 = Maintenance** - Under repair, unavailable
- System auto-updates when rentals start/end

---

## Station Management

### View All Stations

1. Navigate to **Management → Stations**
2. Each station shows:
   - Location coordinates (Latitude, Longitude)
   - Current capacity (e.g., "15/20")
   - Utilization ratio percentage
   - Status indicator:
     - 🔴 **Red "Low"** - < 20% capacity (discount eligible)
     - 🟡 **Yellow "Medium"** - 20-50% capacity
     - 🟢 **Green "High"** - > 50% capacity

### Create New Station

1. Click **"Add New Station"** button
2. Enter information:
   - **Station Name** - Display name
   - **Latitude** - Geographic coordinate
   - **Longitude** - Geographic coordinate
   - **Maximum Capacity** - Vehicle limit
   - **Current Capacity** - Starting inventory

3. Click **"Create Station"**

**Example coordinates:**
- Ben Thanh Market: 10.7725, 106.6992
- District 1: 10.7758, 106.7008

### View Station Details

1. Click **View** button
2. Displays:
   - Full location info and capacity
   - Utilization ratio with visual progress bar
   - Current status with color coding
   - Edit/Delete options

### Edit Station

1. Click **Edit** button
2. Update capacity or location information
3. Ratio automatically recalculates
4. Click **"Update Station"**

### Monitor Inventory

**Low Inventory Alert (< 20%):**
- Station appears in red "Low" status
- Vehicles returned here get 15% discount
- Automatically applies during rental completion

**Manual Capacity Updates:**
- Edit station to adjust current capacity
- Use when vehicles transferred manually
- Ratio updates in real-time

---

## Rental Operations

### Start a Rental

1. Navigate to **Management → Rentals**
2. Click **"Start New Rental"** button
3. Select from dropdowns:
   - **User** - Who is renting (ID and type shown)
   - **Vehicle** - Type, code, fare/min
   - **Start Station** - Pickup location with capacity info

4. Click **"Start Rental"**
   - Automatically redirects to "End Rental" page
   - Vehicle status changes to "In Transit"
   - Rental timer starts

### View Active Rentals

1. On **Rentals** page, see table with:
   - Rental ID and associated user
   - Vehicle type and code
   - Start/end stations
   - Timestamps and financial data
   - Status indicator (Active/Completed/Cancelled)

### Complete a Rental

1. Click **"End Rental"** button for active rental
2. Summary shows:
   - Vehicle and stations
   - Start time
   - Current elapsed time

3. Select:
   - **End Station** - Drop-off location (capacity shown)
   - **Payment Method** - Available for this user
     - Available methods vary by user type
     - Payment method must be enabled for user

4. Click **"Complete Rental"**
   - System calculates fare
   - Applies discount if applicable
   - Processes payment
   - Updates vehicle and station status
   - Displays receipt

### View Rental History

1. Click **View** on any completed rental
2. Shows complete rental record:
   - Journey details (time, locations)
   - Fare breakdown
   - Discount applied
   - Final amount paid

### Receipt Details

After rental completion, receipt shows:
- **Base Fare** = Vehicle.FarePerMin × Duration
- **Discount** (if applicable) = -15% for low inventory
- **Final Amount** = Base Fare - Discount
- Discount reason if applied

---

## Payment Processing

### Payment Methods by User Type

#### Local Commuters
| Method | Code | Status |
|--------|------|--------|
| MoMo | momo | Enabled in seed data |
| VNPay | vnpay | Configurable per user |
| Cash | cash | Always available |

#### Foreign Tourists
| Method | Code | Requirements |
|--------|------|--------------|
| Apple Pay | applepay | Valid passport required |
| PayPal | paypal | Valid passport required |
| Cash | cash | Always available |

### Processing Rules

1. **Payment method must be enabled for user**
   - System checks user's payment flags
   - Rejects unavailable methods

2. **Passport validation (Tourists)**
   - ForeignTourist must have passport number
   - Missing passport → Payment fails

3. **Amount processing**
   - Simulated payment (can integrate real APIs)
   - User.Payed amount updated
   - Transaction recorded in Rental

### Viewing Payment History

1. Navigate to **User Details**
2. **Total Paid** field shows cumulative payments
3. User can view individual rental receipts for breakdown

---

## Reports and Analytics

### Revenue Report by Vehicle Category

1. Navigate to **Reports → Revenue Report**
2. Default shows last 30 days
3. Customize with date picker:
   - **Start Date** - Report period beginning
   - **End Date** - Report period ending
4. Click **"Filter"** to update

**Report displays:**
- Vehicle Type category
- Total Rentals count
- Total Revenue in VND
- Average Fare per rental
- Total Discount Applied

**Example Insight:**
- E-Scooter may show higher revenue due to 1500 VND/min rate
- Discount total shows impact of low-inventory incentive
- Average fare helps identify usage patterns

### Station Inventory / Utilization Report

1. Navigate to **Reports → Inventory Report**
2. Shows current snapshot (no date range)
3. Displays:
   - Station Name and ID
   - Current Capacity / Max Capacity
   - Utilization Ratio (percentage)
   - Status: Low/Medium/High
   - Visual capacity bar

**Low Inventory Status:**
- Appears when < 20%
- Shows warning about discount eligibility
- Indicates rebalancing needed

**Use for:**
- Planning vehicle redistribution
- Predicting discount impact
- Identifying congestion
- Capacity forecasting

---

## Test Scenarios

### Scenario 1: Local Commuter Routine Trip

**Setup:**
- User 1 (Local Commuter, MoMo enabled)
- Standard Bike SB001 at Ben Thanh Market
- 15 minute rental

**Steps:**
1. Start Rental
   - Select User 1
   - Select SB001 (500 VND/min)
   - Select "Ben Thanh Market"
   - Click Start

2. End Rental
   - Wait ~1 minute (testing)
   - Select "District 1 Hub" (high capacity)
   - Select "MoMo" payment
   - Click Complete

**Expected Result:**
- Base Fare ≈ 500 VND
- No discount (District 1 has > 20% capacity)
- Receipt shows 500 VND due
- User 1 "Total Paid" increases

---

### Scenario 2: Foreign Tourist with Discount

**Setup:**
- User 3 (Foreign Tourist, Passport: A12345678)
- E-Scooter ES001 at Saigon Center
- 30 minute rental to low-inventory station

**Steps:**
1. Start Rental
   - Select User 3
   - Select ES001 (1500 VND/min)
   - Select "Saigon Center"

2. End Rental
   - Select "Tan Binh Station" (currently at ~20% capacity)
   - Select "Apple Pay" payment
   - Click Complete

**Expected Result:**
- Base Fare = 1500 × 30 = 45,000 VND
- Tan Binh has < 20% → Discount applies
- Discount = 45,000 × 0.15 = 6,750 VND
- Final Fare = 38,250 VND
- Receipt shows discount reason

---

### Scenario 3: Multiple Rentals Report

**Setup:**
- Complete 3-5 rentals over different dates
- Mix of vehicle types and stations
- Include low-inventory returns

**Test Report:**
1. Go to Revenue Report
2. Set date range to include all rentals
3. Observe:
   - Rental count accurate
   - Total revenue sums correctly
   - Average fare calculated
   - Discount total shows cumulative savings

---

### Scenario 4: Station Rebalancing

**Setup:**
- Tan Binh Station at 20% capacity (5/25)
- Track through rental process

**Steps:**
1. View Inventory Report
   - Tan Binh shows "Low" status
2. Complete 3 rentals returning to Tan Binh
   - Each gets 15% discount
   - Station capacity increases to 8/25
3. View Report again
   - Status may change to "Medium" (if > 20%)
   - Utilization ratio increases

**Learning:**
- System incentivizes high-demand stations
- Discounts help redistribute inventory
- Reports track rebalancing effectiveness

---

## Troubleshooting

### Cannot Start Rental

**Problem:** "Vehicle is not available"
- **Cause:** Vehicle state != 0 (Available)
- **Solution:** Check vehicle status in Vehicle Management
  - Change from In Transit/Maintenance to Available
  - Or select different vehicle

**Problem:** "Station not found"
- **Cause:** Station doesn't exist
- **Solution:** Verify station exists in Station Management
  - Create station if needed

**Problem:** "User not found"
- **Cause:** User ID invalid
- **Solution:** Check User Management for available users

---

### Payment Processing Fails

**Problem:** "Payment processing failed"
- **Cause:** Multiple possible reasons

**For Local Commuters:**
1. Check payment method is enabled for user
   - Edit user, enable MoMo/VNPay
2. Verify correct payment method selected at checkout
3. Cash always available as fallback

**For Foreign Tourists:**
1. Verify passport number exists
   - Edit user, add passport if blank
2. Check Apple Pay/PayPal enabled for user
   - Edit user, enable international methods
3. Cash option available

---

### Report Shows No Data

**Problem:** Revenue Report displays "No rental data"
- **Cause:** No completed rentals in date range
- **Solution:**
  1. Complete a rental (status must be 1, not 0)
  2. Adjust date range to include rental
  3. Refresh page

**Problem:** Inventory Report blank
- **Cause:** No stations in system
- **Solution:**
  1. Go to Station Management
  2. Create at least one station
  3. Return to Inventory Report

---

### Database Issues

**Problem:** "Cannot connect to database"
- **Cause:** SQL Server not running or connection string wrong
- **Solution:**
  1. Verify SQL Server is running
     - LocalDB: `sqllocaldb start mssqllocaldb`
  2. Check connection string in appsettings.json
  3. Create LocalDB if needed: `sqllocaldb create mssqllocaldb`

**Problem:** "Database doesn't exist"
- **Cause:** First run or database deleted
- **Solution:**
  1. Restart application
  2. Database auto-creates
  3. Seed data automatically inserts

---

### Performance Issues

**Problem:** Page loads slowly
- **Cause:** Database query inefficiency
- **Solution:**
  1. Check for many rentals/users
  2. Filter reports by date range
  3. Use pagination if available
  4. Index database tables if needed

---

### User Interface Issues

**Problem:** Forms not submitting
- **Cause:** JavaScript error or validation failure
- **Solution:**
  1. Check browser console for errors (F12)
  2. Verify all required fields filled
  3. Check form validation messages in red
  4. Use different browser if issue persists

**Problem:** Navigation menu not working
- **Cause:** Bootstrap JS not loaded
- **Solution:**
  1. Clear browser cache (Ctrl+Shift+Delete)
  2. Hard refresh page (Ctrl+F5)
  3. Restart browser

---

## Tips & Best Practices

### Data Entry
- Use consistent station names for tracking
- Vehicle codes should be unique (e.g., SB001, not SB1)
- Passport numbers for tourists prevent payment errors
- Keep coordinates accurate for location services

### Testing
- Create test users with each payment method
- Test with different date ranges for reports
- Verify discount applies to low-inventory stations
- Check vehicle status transitions during rentals

### Operations
- Monitor Station Inventory Report regularly
- Review Revenue Reports to identify trends
- Ensure payment methods enabled before customer rentals
- Keep vehicle codes descriptive for easy identification

### Maintenance
- Archive old rentals for performance
- Update vehicle status when undergoing maintenance
- Rebalance station inventory during low-traffic periods
- Regular database backups recommended

---

## Quick Reference

### Key URLs
- Dashboard: `https://localhost:5001/`
- Users: `https://localhost:5001/User`
- Vehicles: `https://localhost:5001/Vehicle`
- Stations: `https://localhost:5001/Station`
- Rentals: `https://localhost:5001/Rental`
- Reports: `https://localhost:5001/Report/RevenueReport`

### Vehicle Types & Rates
| Type | Rate | Duration | Cost |
|------|------|----------|------|
| Standard Bike | 500/min | 10 min | 5,000 VND |
| E-Bike | 1,000/min | 10 min | 10,000 VND |
| E-Scooter | 1,500/min | 10 min | 15,000 VND |

### Discount Rules
- **Threshold:** < 20% station capacity
- **Discount:** 15% off final fare
- **Applied:** Automatically at rental completion
- **Benefit:** Encourages inventory rebalancing

---

**Last Updated:** January 2026  
**Version:** 1.0  
**Support:** See README.md for additional documentation
