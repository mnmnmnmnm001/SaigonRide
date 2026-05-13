# 📊 Inventory Status & Threshold Logic

## Overview

The inventory status system categorizes station capacity into three levels: **Low**, **Medium**, and **High**. This guide explains the thresholds and how they work.

---

## Threshold Rules

### Status Calculation Formula

```csharp
double utilizationRatio = currentCapacity / maxCapacity;

string status = utilizationRatio < 0.2 ? "Low" : 
               (utilizationRatio <= 0.8 ? "Medium" : "High");
```

### Visual Representation

```
0% ───── 20% ───── 80% ───── 100%
┌──────┬─────────────┬──────────┐
│ Low  │   Medium    │   High   │
│  🔴  │     🟡      │    🟢    │
└──────┴─────────────┴──────────┘
< 20%  20% to 80%   > 80%
```

## Threshold Details

### 🔴 LOW Inventory
- **Threshold:** < 20% capacity utilized
- **When:** Current vehicles < 20% of max capacity
- **Action:** Admin warning - time to restock
- **Color:** Red
- **Alert Level:** Critical

### 🟡 MEDIUM Inventory
- **Threshold:** 20% to 80% capacity utilized
- **When:** Current vehicles between 20% and 80% of max
- **Action:** Normal operations - monitor but no urgency
- **Color:** Yellow
- **Alert Level:** Normal

### 🟢 HIGH Inventory
- **Threshold:** > 80% capacity utilized
- **When:** Current vehicles > 80% of max capacity
- **Action:** Consider redistribution - station is busy
- **Color:** Green
- **Alert Level:** Informational

---

## Real-World Examples

### Example 1: Station with 100 Max Capacity (Ben Thanh Market)

| Current | Ratio | Percentage | Status | Meaning |
|---------|-------|------------|--------|---------|
| 15 | 0.15 | 15% | LOW 🔴 | Critical - Urgent restock needed |
| 20 | 0.20 | 20% | MEDIUM 🟡 | Threshold - Just at 20% |
| 50 | 0.50 | 50% | MEDIUM 🟡 | Good - Healthy inventory |
| 80 | 0.80 | 80% | MEDIUM 🟡 | Approaching full |
| 81 | 0.81 | 81% | HIGH 🟢 | Station nearly full |
| 100 | 1.00 | 100% | HIGH 🟢 | Full capacity |

### Example 2: Station with 10 Max Capacity (District 1 Hub)

| Current | Ratio | Percentage | Status | Meaning |
|---------|-------|------------|--------|---------|
| 1 | 0.10 | 10% | LOW 🔴 | Only 1 vehicle available |
| 2 | 0.20 | 20% | MEDIUM 🟡 | Minimal but threshold |
| 4 | 0.40 | 40% | MEDIUM 🟡 | Half full |
| 8 | 0.80 | 80% | MEDIUM 🟡 | Almost full |
| 9 | 0.90 | 90% | HIGH 🟢 | Very busy |
| 10 | 1.00 | 100% | HIGH 🟢 | Full |

### Example 3: Station with 5 Max Capacity (Saigon Center)

| Current | Ratio | Percentage | Status | Meaning |
|---------|-------|------------|--------|---------|
| 0 | 0.00 | 0% | LOW 🔴 | No vehicles! |
| 1 | 0.20 | 20% | MEDIUM 🟡 | 1 vehicle available |
| 2 | 0.40 | 40% | MEDIUM 🟡 | 2 vehicles |
| 4 | 0.80 | 80% | MEDIUM 🟡 | 4 vehicles |
| 5 | 1.00 | 100% | HIGH 🟢 | Full |

### Example 4: Station with 33/100 Capacity (User's Scenario)

**Question:** A station has 33 vehicles with a max capacity of 100. What status?

```
Current: 33
Max: 100
Ratio: 33/100 = 0.33 = 33%
33% > 20%, so Status = MEDIUM 🟡

NOT Low ✅ (because > 20% threshold)
```

---

## Status Transitions

### When Status Changes

#### 🔴 → 🟡 Transition
When inventory reaches **exactly 20%** or above:
```
Current: 19/100 = 19% → Status: LOW 🔴
Current: 20/100 = 20% → Status: MEDIUM 🟡 (threshold)
Current: 21/100 = 21% → Status: MEDIUM 🟡
```

#### 🟡 → 🟢 Transition
When inventory exceeds **80%**:
```
Current: 80/100 = 80% → Status: MEDIUM 🟡
Current: 81/100 = 81% → Status: HIGH 🟢
```

---

## Implementation in Code

### ReportService.cs

```csharp
public async Task<List<StationInventoryReport>> GetStationInventoryReport()
{
    var stations = await _context.Stations.ToListAsync();

    var result = new List<StationInventoryReport>();
    foreach (var s in stations)
    {
        // Calculate utilization ratio
        var utilizationRatio = s.GetRatio();

        // Determine status based on thresholds
        var status = utilizationRatio < 0.2 ? "Low" :
                    (utilizationRatio <= 0.8 ? "Medium" : "High");

        result.Add(new StationInventoryReport
        {
            StationId = s.StationId,
            StationName = s.Name,
            CurrentCapacity = s.CurrentCapacity,
            MaxCapacity = s.MaxCapacity,
            UtilizationRatio = utilizationRatio,
            Status = status // "Low", "Medium", or "High"
        });
    }

    return result;
}
```

### Station.cs Model

```csharp
public class Station
{
    public int StationId { get; set; }
    public string Name { get; set; }
    public int CurrentCapacity { get; set; }
    public int MaxCapacity { get; set; }

    // Helper method to calculate ratio
    public double GetRatio()
    {
        return MaxCapacity > 0 ? 
            (double)CurrentCapacity / MaxCapacity : 0;
    }
}
```

---

## Admin Report Display

### Inventory Report View

The admin sees inventory status color-coded:

```
┌─────────────────────────────────────────────────┐
│           Inventory Status Report              │
├─────────────────────┬────────┬────────┬────────┤
│ Station Name        │ Current│ Max    │ Status │
├─────────────────────┼────────┼────────┼────────┤
│ Ben Thanh Market    │ 110    │ 110    │ 🟢 HIGH│
│ District 1 Hub      │ 10     │ 10     │ 🟢 HIGH│
│ Saigon Center       │ 5      │ 5      │ 🟢 HIGH│
└─────────────────────┴────────┴────────┴────────┘

Legend:
🔴 LOW     = Red (< 20%)
🟡 MEDIUM  = Yellow (20% - 80%)
🟢 HIGH    = Green (> 80%)
```

---

## Testing the Thresholds

### Test Case 1: Low Inventory Warning
```
Setup: Create a station with 25 max capacity
Action: Set current capacity to 4 vehicles
Expected: Status = LOW 🔴 (4/25 = 16% < 20%)
✓ Pass: Admin report shows red warning
```

### Test Case 2: Medium Inventory (Exact Threshold)
```
Setup: Station with 50 max capacity
Action: Set current capacity to 10 vehicles
Expected: Status = MEDIUM 🟡 (10/50 = 20%, not < 20%)
✓ Pass: Admin report shows yellow status
```

### Test Case 3: High Inventory
```
Setup: Station with 100 max capacity
Action: Set current capacity to 85 vehicles
Expected: Status = HIGH 🟢 (85/100 = 85% > 80%)
✓ Pass: Admin report shows green status
```

### Test Case 4: Edge Case (33/100)
```
Setup: Station with 100 max capacity
Action: Set current capacity to 33 vehicles
Expected: Status = MEDIUM 🟡 (33/100 = 33%, which is > 20%)
✓ Pass: NOT Low, correctly shows Medium
```

---

## Common Questions

### Q: Why is the threshold 20% and not 50%?
**A:** 20% is a common industry standard for "low stock" warnings. It gives management time to restock before critical shortages occur, while avoiding false alerts when inventory is healthy.

### Q: What happens at exactly 20%?
**A:** At exactly 20%, the status is **MEDIUM**, not LOW. The logic is `< 0.2`, meaning 20% and above is Medium.

### Q: Can I change the thresholds?
**A:** Yes! Edit the logic in `ReportService.cs`:
```csharp
// Change 0.2 to different value
var status = utilizationRatio < 0.25 ? "Low" : // 25% threshold
```

### Q: Why is the upper threshold 80%?
**A:** 80% indicates the station is running at high capacity and may need redistribution. It's a common SLA threshold in logistics.

### Q: How often is the status updated?
**A:** Status is calculated on-demand when the admin requests the Inventory Report. It reflects the current state at that moment.

---

## Monitoring & Alerts

### What Admins Should Monitor

**🔴 LOW (< 20%):**
- Check immediately
- Plan restocking
- Monitor demand
- May need temporary closure

**🟡 MEDIUM (20%-80%):**
- Normal operations
- Regular monitoring
- No action needed

**🟢 HIGH (> 80%):**
- Station busy
- Good demand
- Consider moving vehicles if available

---

## API Response Example

When admin requests inventory report:

```json
{
  "stationInventoryReport": [
    {
      "stationId": 1,
      "stationName": "Ben Thanh Market",
      "currentCapacity": 110,
      "maxCapacity": 110,
      "utilizationRatio": 1.0,
      "status": "High"
    },
    {
      "stationId": 2,
      "stationName": "District 1 Hub",
      "currentCapacity": 10,
      "maxCapacity": 10,
      "utilizationRatio": 1.0,
      "status": "High"
    },
    {
      "stationId": 3,
      "stationName": "Saigon Center",
      "currentCapacity": 5,
      "maxCapacity": 5,
      "utilizationRatio": 1.0,
      "status": "High"
    }
  ]
}
```

---

## Summary Table

| Threshold | Range | Status | Color | Admin Action |
|-----------|-------|--------|-------|--------------|
| Low | < 20% | LOW | 🔴 Red | Urgent: Restock immediately |
| Medium | 20%-80% | MEDIUM | 🟡 Yellow | Normal: Monitor regularly |
| High | > 80% | HIGH | 🟢 Green | Info: Station busy, good demand |

---

**Effective Date:** 2025-05-05  
**Last Updated:** 2025-05-05  
**Status:** ✅ Active & Deployed
