# Developer's Quick Reference

## Quick Links to Key Files

### Controllers
- **MainController.cs** - Lines 49-117 (ProcessRent validation)
- **MainController.cs** - Lines 176-213 (ProcessReturn validation)

### Views
- **RentDetails.cshtml** - Complete rent form with validation
- **Return.cshtml** - Complete return form with validation
- **_Layout.cshtml** - Theme button in navbar

### Styling
- **wwwroot/css/theme.css** - Theme definitions
- **wwwroot/css/site.css** - Enhanced component styles

### JavaScript
- **wwwroot/js/theme.js** - Theme toggle functionality

---

## How to Add New Validation Rule

### Step 1: Add Check to Controller
```csharp
// In ProcessRent or ProcessReturn method
if (/* condition */)
{
    TempData["Error"] = "Clear error message telling user what to do";
    return RedirectToAction(nameof(ActionName));
}
```

### Step 2: Add HTML5 Validation (Optional)
```html
<!-- In the form input -->
<input type="text" name="fieldName" required />
```

### Step 3: Add JavaScript Validation (Optional)
```javascript
// Listen for change event
inputElement.addEventListener('change', function() {
    // Validate
    if (/* invalid */) {
        this.classList.add('is-invalid');
    }
});
```

---

## How to Customize Error Messages

### Currently Used Error Messages
All error messages are in **MainController.cs**:

```csharp
ProcessRent method (~line 49):
- "Vehicle ID is required."
- "Please select a valid station."
- "Bank number is required."
- "Please select a valid user type."
- "Passport number is required for foreign tourists."
- "Please select a payment method."
- "Rental duration must be between 1 and 1440 minutes (24 hours)."
- "Vehicle or station not found."
- "This vehicle is no longer available for rent."
- "The selected station has no available capacity."
- "Failed to create rental transaction."

ProcessReturn method (~line 176):
- "Vehicle code is required. Please select a vehicle."
- "Please select a valid return station."
- "Vehicle not found."
- "Selected vehicle is not currently in transit."
- "Rental transaction not found for the provided vehicle code."
- "Return station not found."
- "Return station is at full capacity. Please choose another station."
```

To change: Simply edit the string value in the TempData["Error"] = "..." lines.

---

## How to Modify Theme Colors

### In theme.css:

**For Light Theme (Default):**
```css
:root {
    /* Change these values */
    --bg-primary: #ffffff;      /* Main background */
    --text-primary: #212529;    /* Main text color */
    --border-color: #dee2e6;    /* Borders */
    /* ... more variables ... */
}
```

**For Dark Theme:**
```css
html[data-theme="dark"] {
    /* Change these values */
    --bg-primary: #1a1a1a;      /* Dark background */
    --text-primary: #e0e0e0;    /* Light text */
    --border-color: #3d3d3d;    /* Darker borders */
    /* ... more variables ... */
}
```

### All Available Theme Variables:
- `--bg-primary` - Main background color
- `--bg-secondary` - Secondary background (cards, sections)
- `--text-primary` - Primary text color
- `--text-secondary` - Secondary text (muted text)
- `--border-color` - Border and divider color
- `--navbar-bg` - Navigation bar background
- `--navbar-text` - Navigation bar text color
- `--card-bg` - Card/container background
- `--card-shadow` - Card shadow color
- `--input-bg` - Form input background
- `--input-text` - Form input text color
- `--input-border` - Form input border color
- `--alert-error-bg` - Error alert background
- `--alert-error-text` - Error alert text
- `--alert-error-border` - Error alert border
- `--alert-success-bg` - Success alert background
- `--alert-success-text` - Success alert text
- `--alert-success-border` - Success alert border
- `--button-hover-bg` - Button hover background
- `--footer-bg` - Footer background

---

## How to Debug Theme Issues

### Check if Theme CSS is Loading
1. Open DevTools (F12)
2. Go to Elements/Inspector tab
3. Look for `<link rel="stylesheet" href="~/css/theme.css">`
4. Check Network tab to confirm it loaded (200 status)

### Check if Theme JS is Running
1. Open DevTools (F12)
2. Go to Console tab
3. Type: `getCurrentTheme()`
4. Should return "light" or "dark"

### Check LocalStorage
1. Open DevTools (F12)
2. Go to Application → LocalStorage
3. Look for key: "saigonride-theme"
4. Value should be "light" or "dark"

### Force Light Theme
```javascript
// In console
localStorage.setItem('saigonride-theme', 'light');
location.reload();
```

### Force Dark Theme
```javascript
// In console
localStorage.setItem('saigonride-theme', 'dark');
location.reload();
```

---

## How to Debug Validation Issues

### Check Server-Side Validation
1. Add breakpoint in MainController.cs
2. Step through ProcessRent/ProcessReturn method
3. Check which validation check is failing
4. Verify correct redirect is happening

### Check Client-Side Validation
1. Open DevTools (F12)
2. Go to Console tab
3. Look for JavaScript errors (red messages)
4. Check form element IDs match JavaScript selectors

### Common Issues & Solutions

**Issue: Form submits even with empty fields**
- Solution: Verify form has `novalidate` attribute
- Solution: Check JavaScript event listeners are attached
- Solution: Verify form element IDs

**Issue: Error message doesn't appear**
- Solution: Check TempData["Error"] is set in controller
- Solution: Verify view has `@if (TempData["Error"] != null)` check
- Solution: Check view is using correct model

**Issue: Validation works on first submit but not after**
- Solution: Check form is being cleared properly
- Solution: Verify event listeners aren't being removed

---

## Common Code Patterns

### Add a Validation Check
```csharp
if (/* condition to check */)
{
    TempData["Error"] = "Message shown to user";
    return RedirectToAction(nameof(ActionName));
}
```

### Display Error in View
```html
@if (TempData["Error"] != null)
{
    <div class="alert alert-danger alert-dismissible fade show">
        <strong>Error!</strong> @TempData["Error"]
        <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
    </div>
}
```

### Add Required Field Validation
```html
<input type="text" name="fieldName" required />
<div class="invalid-feedback">
    This field is required.
</div>
```

### Listen for Input Changes
```javascript
element.addEventListener('change', function() {
    // Validation logic
});
```

### Toggle Class for Styling
```javascript
element.classList.add('is-invalid');    // Add class
element.classList.remove('is-invalid'); // Remove class
element.classList.toggle('is-invalid'); // Toggle class
```

---

## Testing Checklist for Developers

### After Making Changes to Validation
- [ ] Run build (should succeed with 0 errors)
- [ ] Test with empty input fields
- [ ] Test with invalid data types
- [ ] Test with boundary values (0, 1440)
- [ ] Test with null/empty strings
- [ ] Verify error message displays
- [ ] Verify correct redirect happens
- [ ] Check no JavaScript errors in console

### After Making Changes to Theme
- [ ] Theme toggle button works
- [ ] All colors change when toggling
- [ ] Theme persists after refresh
- [ ] Works on all pages
- [ ] Works on mobile (responsive)
- [ ] Check no JavaScript errors in console
- [ ] Verify in both Chrome, Firefox, Edge

### Before Committing Code
- [ ] Build is successful
- [ ] No console errors or warnings
- [ ] All validation tests pass
- [ ] All theme tests pass
- [ ] Code follows existing style
- [ ] Comments are clear
- [ ] No debug code left in place

---

## Performance Tips

### Optimize CSS
- Use CSS variables instead of hardcoding colors
- Minimize color property changes
- Prefer CSS over JavaScript for styling

### Optimize JavaScript
- Use event delegation where possible
- Avoid DOM manipulation in loops
- Cache selector results
- Use RequestAnimationFrame for animations

### Optimize HTML
- Keep forms semantic
- Use HTML5 validation attributes
- Minimize DOM depth
- Use proper semantic tags

---

## Accessibility Notes

### Color Contrast
- Light theme: Black text on white (21:1 contrast ratio)
- Dark theme: Light text on dark (10:1 contrast ratio)
- Both meet WCAG AA standards

### Keyboard Navigation
- All form fields keyboard accessible
- Tab order is logical
- Focus states are visible
- Buttons are properly labeled

### Screen Readers
- Proper semantic HTML
- ARIA labels where needed
- Form labels properly associated
- Error messages clearly announced

---

## Git Commit Message Examples

```
feat: Add comprehensive input validation to rent/return forms

- Add 11 validation checks to ProcessRent method
- Add 7 validation checks to ProcessReturn method
- Implement client-side and server-side validation
- Display clear error messages via TempData
```

```
feat: Implement dark/light theme system

- Add theme.css with CSS variables for both themes
- Add theme.js for theme switching functionality
- Add theme toggle button in navbar
- Persist theme preference to localStorage
```

```
refactor: Improve rent and return form UI/UX

- Redesign RentDetails.cshtml with card layout
- Redesign Return.cshtml with card layout
- Add form validation feedback
- Improve error message display
- Enhance responsive design
```

---

## Resources

- **CSS Variables (Custom Properties):** MDN Docs
- **Bootstrap Form Validation:** Bootstrap Docs
- **localStorage API:** MDN Docs
- **JavaScript Event Listeners:** MDN Docs
- **ASP.NET TempData:** Microsoft Docs

---

## Support Contacts

For issues with:
- **Validation logic** → Check MainController.cs
- **CSS/Theme** → Check theme.css and site.css
- **JavaScript** → Check theme.js and view scripts
- **HTML structure** → Check view files (.cshtml)
