# MVP Flag Sequence

## Overview

This document defines the exact sequence of flags, interactions, and state transitions for the MVP vertical slice. It is the definitive specification of what should happen when the player completes the core loop.

---

## MVP Global Flags

All progression is driven by these four flags:

```csharp
public const string HAS_STAFF_KEY = "HasStaffKey";              // true when key is picked up
public const string DOOR_OPENED = "DoorOpened";                // true when door is unlocked with key
public const string REALITY_UNLOCKED = "RealityUnlocked";      // true when hallway obstacle is examined
public const string SHIFT_PERFORMED = "ShiftPerformed";        // true when reality toggle occurs
```

**None of these flags should be manually set in the editor.** They are all set via interactions and managers.

---

## Execution Flow

### State: Game Start

**Location:** Vestibule  
**Active Room:** Vestibule  
**Current Reality:** Normal  
**Flags:**
- `HasStaffKey = false`
- `DoorOpened = false`
- `RealityUnlocked = false`
- `ShiftPerformed = false`

**Visible Interactables:**
- ✅ StaffKey (normal reality only, visible)
- ✅ FrontDoor (visible in both realities, locked until key acquired)
- ✅ HallwayEntrance (visible in both realities, locked until door opened)

**Player Actions Available:**
- Click StaffKey

---

### Step 1: Pick Up Staff Key

**Player Action:** Click `StaffKey` interactable

**Sequence:**
1. `InteractionManager.TryInteract("staff_key")` is called
2. Validates: interactable exists, is not already picked up, has no required item
3. Validates: `InteractableData.interactionType == Pickup`
4. Execution:
   - Sets `InteractableData.isPickedUp = true`
   - Sets flag: `GameManager.SetFlag("HasStaffKey", true)`
   - Creates InventoryItemData: `{ id: "staff_key", displayName: "Staff Key", isKeyItem: true }`
   - Calls `InventoryManager.AddItem(staffKeyData)`
   - Calls `RoomManager.RefreshInteractables()`
   - Broadcasts: `InteractionManager.InteractionResolved?.Invoke("staff_key", interactableData)`
5. Unity/Runtime:
   - StaffKey SpriteRenderer.enabled = false (picked up, removed from scene)
   - FrontDoor becomes interactable (has required item now)
   - HallwayEntrance remains locked until door is opened

**New State:**
- `HasStaffKey = true`
- `DoorOpened = false`
- `RealityUnlocked = false`
- `ShiftPerformed = false`

**Visible Interactables:**
- ❌ StaffKey (picked up, removed)
- ✅ FrontDoor (now interactable, has key)
- ❌ HallwayEntrance (still locked until door opened)

**Player Actions Available:**
- Click FrontDoor

---

### Step 2: Use Staff Key on Front Door

**Player Action:** Click `FrontDoor` interactable

**Sequence:**
1. `InteractionManager.TryInteract("front_door")` is called
2. Validates: interactable exists, is not consumed, has required item
   - Checks: `InventoryManager.HasItem("staff_key") == true` ✅
3. Validates: `InteractableData.interactionType == Use`
4. Execution:
   - Sets `InteractableData.isConsumed = true`
   - Sets flag: `GameManager.SetFlag("DoorOpened", true)`
   - Calls `RoomManager.RefreshInteractables()`
   - Broadcasts: `InteractionManager.InteractionResolved?.Invoke("front_door", interactableData)`
5. Unity/Runtime:
   - FrontDoor SpriteRenderer changes (e.g., opens, or becomes "Opened Door" sprite)
   - HallwayEntrance becomes interactable (door is now open)

**New State:**
- `HasStaffKey = true`
- `DoorOpened = true`
- `RealityUnlocked = false`
- `ShiftPerformed = false`

**Visible Interactables:**
- ❌ StaffKey (picked up)
- ✅ FrontDoor (used, shows open state)
- ✅ HallwayEntrance (now interactable, door is open)

**Player Actions Available:**
- Click HallwayEntrance (to enter hallway)

---

### Step 3: Enter Hallway

**Player Action:** Click `HallwayEntrance` interactable

**Sequence:**
1. `InteractionManager.TryInteract("hallway_entrance")` is called
2. Validates: interactable exists, is not consumed, no required item
3. Validates: `InteractableData.interactionType == Transition`
4. Execution:
   - Calls `RoomManager.LoadRoom("hallway")`
   - Calls `RoomManager.RefreshInteractables()`
   - Broadcasts: `InteractionManager.InteractionResolved?.Invoke("hallway_entrance", interactableData)`
5. Unity/Runtime:
   - Vestibule interactables are deactivated (room switched)
   - Hallway room becomes active
   - Camera is repositioned (if using per-room camera logic, or stays static)
   - Hallway background displays
   - HallwayObstacle is visible (normal reality)
   - ShiftTrigger is visible
   - VestibuleExit is visible

**New State:**
- Active Room: Hallway
- Current Reality: Normal
- `HasStaffKey = true`
- `DoorOpened = true`
- `RealityUnlocked = false`
- `ShiftPerformed = false`

**Visible Interactables (Hallway):**
- ✅ HallwayObstacle (visible in normal reality, requires interaction)
- ✅ ShiftTrigger (visible in both realities)
- ✅ VestibuleExit (visible in both realities)

**Player Actions Available:**
- Click HallwayObstacle (examine)
- Click ShiftTrigger (perform shift)
- Click VestibuleExit (return to vestibule)

---

### Step 4: Examine Hallway Obstacle

**Player Action:** Click `HallwayObstacle` interactable

**Note:** This step unlocks the ability to shift to the OtherSide reality.

**Sequence:**
1. `InteractionManager.TryInteract("hallway_obstacle")` is called
2. Validates: interactable exists, is not consumed, no required item
3. Validates: `InteractableData.interactionType == Examine`
4. Execution:
   - Sets flag: `GameManager.SetFlag("RealityUnlocked", true)`
   - Calls `RoomManager.RefreshInteractables()`
   - Broadcasts: `InteractionManager.InteractionResolved?.Invoke("hallway_obstacle", interactableData)`
5. UI/Feedback:
   - Display description: "A towering stone figure blocks your path. It's watching you."
   - Optional: play sound effect or visual effect

**New State:**
- `HasStaffKey = true`
- `DoorOpened = true`
- `RealityUnlocked = true`
- `ShiftPerformed = false`

**Visible Interactables (Hallway):**
- ✅ HallwayObstacle (still visible, examined)
- ✅ ShiftTrigger (now interactable, reality unlocked)
- ✅ VestibuleExit (still visible)

**Player Actions Available:**
- Click ShiftTrigger (perform shift)
- Click VestibuleExit (return to vestibule)

---

### Step 5: Perform Reality Shift

**Player Action:** Click `ShiftTrigger` interactable

**Note:** This is the core MVP validation step. After this, the player sees the world change.

**Sequence:**
1. `InteractionManager.TryInteract("shift_trigger")` is called
2. Validates: interactable exists, is not consumed, no required item
3. Validates: `InteractableData.interactionType == Toggle`
4. Execution:
   - Gets current reality: `RealityManager.CurrentReality` (currently Normal)
   - Toggles reality: `RealityManager.ToggleReality()` (switches to OtherSide)
   - Sets flag: `GameManager.SetFlag("ShiftPerformed", true)`
   - Calls `RoomManager.RefreshInteractables()`
   - Broadcasts: `InteractionManager.InteractionResolved?.Invoke("shift_trigger", interactableData)`
5. RealityManager Propagation:
   - Updates internal state: `CurrentReality = OtherSide`
   - Calls `RoomManager.RefreshInteractables()` to update visibility
6. RoomManager Refresh:
   - For each interactable in current room:
     - If `CurrentReality == Normal`: show if `visibleInNormalReality == true`
     - If `CurrentReality == OtherSide`: show if `visibleInOtherReality == true`
   - HallwayObstacle.enabled = false (not visible in OtherSide)
   - HallwayBackground changes (if separate sprite for OtherSide)
7. Unity/Runtime:
   - Visual shift effect (fade, color change, sound, etc.)
   - HallwayObstacle SpriteRenderer.enabled = false
   - Optional visual replacement for "OtherSide" environment

**New State:**
- Active Room: Hallway
- Current Reality: OtherSide
- `HasStaffKey = true`
- `DoorOpened = true`
- `RealityUnlocked = true`
- `ShiftPerformed = true`

**Visible Interactables (Hallway, OtherSide):**
- ❌ HallwayObstacle (hidden in OtherSide reality)
- ✅ ShiftTrigger (visible in both realities)
- ✅ VestibuleExit (visible in both realities)

**Player Actions Available:**
- Path forward is clear; hallway obstacle is gone
- Click ShiftTrigger (shift back to Normal)
- Click VestibuleExit (return to vestibule)

---

## MVP Victory Condition

The MVP is considered **complete and validated** when:

1. ✅ Player picks up staff key (HasStaffKey = true)
2. ✅ Player uses key on door (DoorOpened = true)
3. ✅ Player enters hallway (room transitions work)
4. ✅ Player examines obstacle (RealityUnlocked = true)
5. ✅ Player shifts reality (ShiftPerformed = true)
6. ✅ Obstacle becomes invisible in OtherSide
7. ✅ Player can navigate where obstacle was before

**Play Mode Validation Sequence:**

```
1. Start game in Editor
2. Verify Vestibule loads with StaffKey visible
3. Click StaffKey → disappears, flag set
4. Click FrontDoor → opens, becomes interactable
5. Click HallwayEntrance → loads Hallway scene
6. Verify HallwayObstacle visible in Hallway
7. Click HallwayObstacle → flag set
8. Click ShiftTrigger → reality toggles
9. Verify HallwayObstacle disappears
10. Click ShiftTrigger again → back to Normal
11. Verify HallwayObstacle reappears
12. Click VestibuleExit → back to Vestibule
13. Verify state is preserved (no flags reset)
```

---

## Flag Dependencies and Constraints

### HasStaffKey

- **Set By:** Interaction with StaffKey (Pickup)
- **Checked By:** FrontDoor (requires HasStaffKey to use)
- **Blocks:** Cannot open door without this flag
- **Synced With:** InventoryManager (item added to inventory)

### DoorOpened

- **Set By:** Interaction with FrontDoor (Use)
- **Checked By:** RoomManager (to unlock HallwayEntrance transition)
- **Blocks:** Cannot enter hallway without this flag
- **Effect:** FrontDoor visual changes to "open" state

### RealityUnlocked

- **Set By:** Interaction with HallwayObstacle (Examine)
- **Not currently checked by any interactable in MVP (optional design)**
- **Optional:** Could be used to gate ShiftTrigger, but MVP allows shift without examining obstacle
- **Note:** This is a design decision; if you want to require obstacle examination before shift, add `requiredItemId = "reality_unlocked"` to ShiftTrigger

### ShiftPerformed

- **Set By:** Interaction with ShiftTrigger (Toggle)
- **Purpose:** Tracking that shift has occurred (for future narrative or progression)
- **Effect:** Triggers RealityManager visibility refresh

---

## Data-Driven Progression

Every step is defined in data, not code:

| Step | Interactable | InteractionType | Required Item | Result Flag | Effect |
|------|--------------|-----------------|---------------|-------------|--------|
| 1 | staff_key | Pickup | (none) | HasStaffKey | Add item to inventory |
| 2 | front_door | Use | staff_key | DoorOpened | Door opens, unlock hallway |
| 3 | hallway_entrance | Transition | (none) | (none) | Load hallway room |
| 4 | hallway_obstacle | Examine | (none) | RealityUnlocked | Unlock shift ability |
| 5 | shift_trigger | Toggle | (none) | ShiftPerformed | Toggle reality, refresh visibility |

This means:
- No hardcoded scene logic
- No conditional branches per room
- All logic lives in manager scripts
- Content is pure data

---

## Reality Switch Implementation Detail

When `RealityManager.ToggleReality()` is called:

```csharp
public void ToggleReality()
{
    CurrentReality = (CurrentReality == RealityType.Normal) 
        ? RealityType.OtherSide 
        : RealityType.Normal;

    RoomManager.Instance.RefreshInteractables();
}
```

`RoomManager.RefreshInteractables()` then iterates all interactables in the current room:

```csharp
private void RefreshInteractables()
{
    var currentRoom = CurrentRoom;
    if (currentRoom == null) return;

    foreach (var interactable in currentRoom.interactables)
    {
        bool shouldBeVisible = RealityManager.Instance.CurrentReality == RealityType.Normal
            ? interactable.visibleInNormalReality
            : interactable.visibleInOtherReality;

        // Update GameObject visibility
        // (assumes each interactable is a GameObject with a behavior script)
        interactable.isVisible = shouldBeVisible;
        // Propagate to SpriteRenderer or other visual components
    }
}
```

---

## Related Files

- `MVP-Data-Specification.md` — exact values for each interactable
- `MVP-Concrete-Hierarchy.md` — GameObject placement and naming
- `Assets/Scripts/GameManager.cs` — flag storage and retrieval
- `Assets/Scripts/InteractionManager.cs` — interaction resolution
- `Assets/Scripts/RealityManager.cs` — reality toggle logic
- `Assets/Scripts/RoomManager.cs` — room loading and refresh
- `Assets/Scripts/InventoryManager.cs` — item tracking
