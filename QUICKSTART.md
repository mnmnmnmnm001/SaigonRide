# Quick Start Guide - Error Handling & Theme Features

## What's New?

### 1. Enhanced Error Handling
Your rent and return forms now have comprehensive validation that catches user errors **before** they cause problems:

**Rent Form Validation:**
- Bank number required
- Payment method must be selected
- Rental duration: 1-1440 minutes
- Passport required for foreign tourists
- Station must have available capacity
- Vehicle must be available for rent

**Return Form Validation:**
- Vehicle must be in transit
- Return station must have available capacity
- Vehicle and transaction must exist

### 2. Dark/Light Theme
Click the theme toggle button in the top-right corner of the navbar to switch between light and dark modes.

**Features:**
- ✅ Theme preference is saved in your browser
- ✅ Works across entire application
- ✅ Smooth transitions between themes
- ✅ High contrast for accessibility

---

## Testing the Features

### Test Error Handling

**Test 1: Missing Bank Number**
1. Go to Rent > Select a Vehicle > Click "Rent"
2. Leave "Bank Number" field empty
3. Click "Confirm Rent"
4. → Error: "Bank number is required."

**Test 2: Foreign Tourist without Passport**
1. Go to Rent > Select a Vehicle > Click "Rent"
2. Select "Foreign Tourist"
3. Leave "Passport Number" empty
4. Click "Confirm Rent"
5. → Error: "Passport number is required for foreign tourists."

**Test 3: Invalid Duration**
1. Go to Rent > Select a Vehicle > Click "Rent"
2. Enter "0" or "2000" in Minutes field
3. Click "Confirm Rent"
4. → Error: "Rental duration must be between 1 and 1440 minutes (24 hours)."

**Test 4: Return to Full Station**
1. Rent a vehicle
2. Go to Return
3. Try to return to a station that's at full capacity
4. → Error: "Return station is at full capacity. Please choose another station."

### Test Theme Switching

1. Look at top-right of navbar
2. Click the theme toggle button (shows current theme)
3. Watch all colors change instantly
4. Refresh the page
5. → Your theme preference is remembered!

---

## Code Changes Summary

### Modified Files:
1. **MainController.cs** - Added comprehensive input validation
2. **_Layout.cshtml** - Added theme CSS/JS and toggle button
3. **RentDetails.cshtml** - Enhanced form with validation
4. **Return.cshtml** - Enhanced form with validation
5. **site.css** - Improved styling and visual design

### New Files:
1. **css/theme.css** - Light/dark theme definitions
2. **js/theme.js** - Theme switching functionality

---

## Architecture

### Error Handling Flow
```
User Submit Form
    ↓
Client-side Validation (HTML5 + JS)
    ↓
Server-side Validation (C# in Controller)
    ↓
If invalid → Redirect with TempData["Error"]
    ↓
Display Error Alert in View
```

### Theme System
```
Page Load
    ↓
theme.js initializes
    ↓
Check LocalStorage for saved theme
    ↓
Apply CSS variables via html[data-theme]
    ↓
All components use CSS variables
```

---

## Browser Console

No errors or warnings should appear in browser console. If you see any:
1. Press F12 to open Developer Tools
2. Go to Console tab
3. Let us know what errors you see

---

## Customization

### Change Theme Colors
Edit `wwwroot/css/theme.css` and modify the color values in `:root` or `html[data-theme="dark"]` sections.

### Add More Validations
Edit `Controllers/MainController.cs` in `ProcessRent()` or `ProcessReturn()` methods and add new `if` checks with `TempData["Error"]`.

### Customize Error Messages
All error messages are in `MainController.cs` - simply change the text in quotes for `TempData["Error"]`.

---

## Troubleshooting

### Theme not switching?
- Clear browser cache and refresh (Ctrl+Shift+Delete)
- Check that `theme.js` is loading (F12 → Network tab)
- Verify `theme.css` is linked in `_Layout.cshtml`

### Error messages not showing?
- Check browser console for JavaScript errors (F12 → Console)
- Verify TempData is being set in controller
- Make sure view has `@if (TempData["Error"] != null)` check

### Form validation not working?
- Check that form has `novalidate` attribute (for custom validation)
- Verify JavaScript is enabled in browser
- Check that form IDs match in JavaScript selectors

---

## Support

For detailed information, see `IMPLEMENTATION_GUIDE.md` in the project root.
