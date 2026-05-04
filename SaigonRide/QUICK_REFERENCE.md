# SaigonRide - Quick Reference Card

## System Overview
**SaigonRide** is an ASP.NET Core MVC distributed vehicle rental system supporting multiple user types, payment methods, and dynamic pricing with smart discounts.

---

## 🎯 Quick Navigation

```
HOME (Dashboard)
    ↓
├── MANAGEMENT
│   ├── Users (Create/View/Edit/Delete)
│   ├── Vehicles (Create/View/Edit/Delete)
│   ├── Stations (Create/View/Edit/Delete)
│   └── Rentals (Start/View/End/Receipt)
├── REPORTS
│   ├── Revenue Report (by vehicle, date range)
│   └── Inventory Report (station utilization)
└── PRIVACY
```

---

## 👥 User Types

### LocalCommuter
```
Payment Methods:
├─ MoMo (configurable)
├─ VNPay (configurable)
└─ Cash (always available)

Requirements:
└─ Bank account number
```

### ForeignTourist
```
Payment Methods:
├─ Apple Pay (requires passport)
├─ PayPal (requires passport)
└─ Cash (always available)

Requirements:
├─ Passport number (MANDATORY)
└─ Bank account number
```

---

## 🚴 Vehicle Types & Pricing

| Type | Code | Rate | 10 min |
|------|------|------|--------|
| Standard Bike | SB### | 500/min | 5,000 |
| E-Bike | EB### | 1,000/min | 10,000 |
| E-Scooter | ES### | 1,500/min | 15,000 |

**All prices in Vietnamese Dong (VND)**

---

## 🏪 Station Status

| Capacity | Ratio | Status | Discount |
|----------|-------|--------|----------|
| < 20% | 🔴 Low | Low Inventory | ✅ 15% OFF |
| 20-50% | 🟡 Medium | Medium | ❌ No |
| > 50% | 🟢 High | High | ❌ No |

---

## 💰 Fare Calculation

### Base Formula
```
Base Fare = Vehicle Rate × Duration (minutes)
Discount = Base Fare × 0.15 (if returning to low-inventory station)
Final Fare = Base Fare - Discount
```

### Example 1: Standard Bike (30 min)
```
Base Fare = 500 × 30 = 15,000 VND
No Discount = 15,000 VND
```

### Example 2: E-Scooter (20 min) to Low Inventory Station
```
Base Fare = 1,500 × 20 = 30,000 VND
Discount = 30,000 × 0.15 = 4,500 VND
Final Fare = 25,500 VND ✅
```

---

## 📋 Rental Workflow

### Step 1: Start Rental
1. Go to **Management → Rentals**
2. Click **"Start New Rental"**
3. Select:
   - User
   - Vehicle (must be Available)
   - Start Station
4. Click **"Start Rental"**
→ Vehicle state changes to In-Transit

### Step 2: End Rental
1. From rental details, click **"End Rental"**
2. Select:
   - End Station
   - Payment Method (must be enabled for user)
3. Click **"Complete Rental"**
→ System calculates fare + discount
→ Payment processes
→ Receipt displays

### Step 3: View Receipt
- Journey details (time, locations)
- Fare breakdown (base, discount, final)
- Payment confirmation
- User statistics update

---

## 📊 Report Features

### Revenue Report
**Access:** Reports → Revenue Report

**Filters:**
- Start Date (default: 30 days ago)
- End Date (default: today)

**Displays:**
- Total rentals (by vehicle type)
- Total revenue
- Average fare per rental
- Total discount applied

**Use For:**
- Performance analysis
- Revenue trends
- Discount impact

### Inventory Report
**Access:** Reports → Inventory Report

**Displays (Current Snapshot):**
- Station name and ID
- Current capacity (e.g., 15/20)
- Utilization ratio (e.g., 75%)
- Status badge (Low/Medium/High)
- Capacity visualization

**Use For:**
- Identify rebalancing needs
- Find discount opportunities
- Monitor congestion

---

## 🔑 Keyboard Shortcuts

| Action | URL |
|--------|-----|
| Home/Dashboard | `/` or `/Home` |
| User Management | `/User` |
| Vehicle Management | `/Vehicle` |
| Station Management | `/Station` |
| Rental Management | `/Rental` |
| Revenue Report | `/Report/RevenueReport` |
| Inventory Report | `/Report/InventoryReport` |

---

## 🚨 Common Actions

### Create User
1. Management → Users → "Add New User"
2. Select type (LocalCommuter or ForeignTourist)
3. Fill bank info, payment preferences
4. Click Create ✓

### Start Rental
1. Management → Rentals → "Start New Rental"
2. Pick user, vehicle, station
3. Click "Start Rental"
4. System auto-redirects to end rental page ✓

### View Rental Receipt
1. Management → Rentals
2. Find completed rental (Status = Completed)
3. Click "View" for receipt details ✓

### Generate Report
1. Reports → Revenue Report (or Inventory)
2. Set date range if needed
3. Click "Filter" to update
4. View metrics and export if needed ✓

---

## ✅ Validation Rules

| Entity | Rule |
|--------|------|
| Vehicle | Must be Available (State=0) to rent |
| Station | Must exist before use |
| User | Payment method must be enabled |
| Tourist | Passport required for intl. payment |
| Rental | Duration must be > 0 |
| Capacity | Never exceeds max limit |

---

## 💡 Pro Tips

1. **Quick Discount:** Return to stations with red "Low" badges = 15% off
2. **Check Status:** Vehicle badges show real-time availability
3. **Payment First:** Enable all needed payment methods before renting
4. **Station Balance:** Monitor inventory reports to rebalance fleet
5. **Revenue Trends:** Check reports regularly to identify patterns

---

## 🆘 Quick Troubleshooting

| Issue | Solution |
|-------|----------|
| "Vehicle not available" | Select different vehicle or update status |
| "Payment failed" | Enable payment method for user |
| "Station not found" | Create station in Station Management |
| "No report data" | Complete a rental first, check date range |
| "Passport error" | Add passport number to ForeignTourist user |

---

## 📊 Dashboard Metrics

| Metric | Updates | Shows |
|--------|---------|-------|
| Total Users | Real-time | All users in system |
| Total Vehicles | Real-time | All vehicles (all statuses) |
| Total Stations | Real-time | All stations created |
| Total Rentals | Real-time | Active + Completed + Cancelled |
| Total Revenue | Real-time | Sum of all completed rentals |

---

## 🗄️ Seed Data (Ready to Test)

**Stations:** 4 pre-configured locations
**Vehicles:** 6 vehicles (2 per type)
**Users:** 4 test accounts (2 local, 2 tourist)

---

## 🔐 Security Notes

- Passport verification required for international payments
- Payment method validation per user type
- User account separation (no unauthorized rentals)
- Database integrity constraints enforced
- Ready for authentication layer (if needed)

---

## 📱 UI Sections

```
HEADER: Logo + Navigation
├── Home Link
├── Management Dropdown
│   ├── Users
│   ├── Vehicles
│   ├── Stations
│   └── Rentals
├── Reports Dropdown
│   ├── Revenue Report
│   └── Inventory Report
└── Privacy Link

MAIN CONTENT: Page-specific forms/tables/reports

FOOTER: Copyright + Privacy link
```

---

## 🎓 Learning Path

1. **Beginner:** Create users, view dashboard stats
2. **Intermediate:** Add vehicles/stations, start rental
3. **Advanced:** Complete rental cycle with payment
4. **Expert:** Analyze reports, optimize routing

---

## 📞 Getting Help

1. **Setup Issues?** → Check README.md
2. **How to use?** → Read USAGE_GUIDE.md
3. **Technical details?** → See ARCHITECTURE.md
4. **Test scenarios?** → Find in USAGE_GUIDE.md

---

## ✨ Key Features at a Glance

✅ Multi-user support (Locals + Tourists)
✅ Multiple payment methods (MoMo, VNPay, Apple Pay, PayPal)
✅ Dynamic pricing by vehicle type
✅ Smart 15% discount for low-inventory returns
✅ Complete CRUD for all entities
✅ Real-time vehicle status tracking
✅ Automatic fare calculation with discounts
✅ Receipt generation
✅ Revenue analysis reports
✅ Inventory utilization reports
✅ Professional responsive UI
✅ Production-ready database

---

**Version:** 1.0 | **Status:** ✅ Ready | **Last Updated:** Jan 2026

**For detailed information, see:**
- README.md (Overview)
- USAGE_GUIDE.md (Complete guide)
- ARCHITECTURE.md (Technical design)
