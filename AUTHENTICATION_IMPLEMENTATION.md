# SaigonRide Authentication & Architecture Changes

## Summary of Implementation

This document outlines all the changes made to implement the authentication system and rename Kiosk to Main.

---

## 1. **Authentication System Implementation**

### 1.1 Session Support (Program.cs)
- Added session services with 30-minute idle timeout
- Added session middleware to the HTTP pipeline
- Changed default controller route from Home to Main

### 1.2 Admin Authentication Attribute
- Created new file: `SaigonRide\Attributes\AdminAuthenticationAttribute.cs`
- Automatically redirects to login if user is not authenticated
- Checks for "AdminUsername" in session

### 1.3 Admin Controller Updates
- Updated authentication logic with hardcoded credentials:
  - **Username**: `qwe`
  - **Password**: `1234560`
- Stores username in session upon successful login
- Added new `Logout` action that clears session

### 1.4 Protected Controllers
Added `[AdminAuthentication]` attribute to:
- HomeController
- RentalController
- StationController
- UserController
- VehicleController
- ReportController

---

## 2. **Kiosk to Main Rename**

### 2.1 Controller Rename
- Renamed `KioskController.cs` to `MainController.cs`
- Updated class name from `KioskController` to `MainController`
- Removed old `KioskController.cs` file

### 2.2 Views Reorganization
- Moved folder `Views/Kiosk/` to `Views/Main/`
- All views now reference Main instead of Kiosk

### 2.3 Route Configuration
- Updated default route in Program.cs:
  - From: `{controller=Home}/{action=Index}`
  - To: `{controller=Main}/{action=Index}`
- Application now starts at Main screen by default

---

## 3. **User Interface Changes**

### 3.1 Main/Index.cshtml (Kiosk Screen)
- Full-screen kiosk interface with two main buttons:
  - "Rent Vehicle"
  - "Return Vehicle"
- **Admin Login Panel** positioned at bottom-right with:
  - Username field
  - Password field
  - Login button
- If logged in, shows:
  - "Go to Dashboard" button (redirects to Home)
  - "Logout" button

### 3.2 Home/Index.cshtml (Admin Dashboard)
- Added logout button in top-right corner
- Displays authentication status:
  - Shows "Logged In" or "Not Logged In"
  - Logout button only visible when logged in
- All admin functionality remains unchanged

---

## 4. **User Flow**

### 4.1 Initial State (Logged Out)
1. User starts application → directed to Main/Index (Kiosk screen)
2. User can:
   - Rent a vehicle
   - Return a vehicle
   - See admin login panel at bottom-right

### 4.2 Admin Login Flow
1. Admin enters credentials at bottom-right of Main screen
2. If credentials match (qwe / 1234560):
   - Session is created
   - Redirected to Home/Index (Admin Dashboard)
3. If credentials invalid:
   - Error message displayed
   - User remains on Main screen

### 4.3 Admin Authenticated State
1. Admin can access all admin functionality:
   - Manage Users (/User)
   - Manage Vehicles (/Vehicle)
   - Manage Stations (/Station)
   - View Rentals (/Rental)
   - View Reports (/Report)
2. Admin can logout by:
   - Clicking "Logout" button in Home/Index top-right
   - This clears session and redirects to Main screen

### 4.4 Access Control
- Any attempt to access protected routes without authentication → redirected to Admin/Login
- Session expires after 30 minutes of inactivity
- Protected routes include all CRUD operations for:
  - Rentals
  - Stations
  - Users
  - Vehicles
  - Reports

---

## 5. **File Changes Summary**

### Modified Files
- `Program.cs` - Added session support, changed default route
- `Controllers/AdminController.cs` - Updated with hardcoded credentials and logout
- `Controllers/HomeController.cs` - Added authentication check
- `Controllers/MainController.cs` (renamed from KioskController.cs)
- `Controllers/RentalController.cs` - Added authentication
- `Controllers/StationController.cs` - Added authentication
- `Controllers/UserController.cs` - Added authentication
- `Controllers/VehicleController.cs` - Added authentication
- `Controllers/ReportController.cs` - Added authentication
- `Views/Home/Index.cshtml` - Updated with logout button and auth status
- `Views/Main/Index.cshtml` (renamed from Views/Kiosk/Index.cshtml) - Enhanced with login panel

### New Files
- `Attributes/AdminAuthenticationAttribute.cs` - Authentication filter

### Deleted Files
- `Controllers/KioskController.cs` - Removed (renamed to MainController.cs)
- `Views/Kiosk/` folder - Removed (moved to Views/Main/)

---

## 6. **Testing Checklist**

- [ ] Application starts at Main screen (kiosk)
- [ ] Login panel visible at bottom-right of Main screen
- [ ] Invalid credentials show error message
- [ ] Valid credentials (qwe / 1234560) redirect to Home
- [ ] Session is created after login
- [ ] Logout button appears in Home after login
- [ ] Logout clears session and redirects to Main
- [ ] All CRUD operations require authentication
- [ ] Unauthenticated access to /User redirects to login
- [ ] Unauthenticated access to /Vehicle redirects to login
- [ ] Unauthenticated access to /Station redirects to login
- [ ] Unauthenticated access to /Rental redirects to login
- [ ] Unauthenticated access to /Report redirects to login
- [ ] Session timeout after 30 minutes of inactivity

---

## 7. **Credentials**

**Admin Account**
- Username: `qwe`
- Password: `1234560`

---

## 8. **Notes**

- All admin functionality is now behind authentication wall
- The kiosk (Main screen) remains publicly accessible
- Session is stored in-process (can be configured for distributed scenarios)
- Session timeout is set to 30 minutes
- All views have been updated to reference the new controller names
