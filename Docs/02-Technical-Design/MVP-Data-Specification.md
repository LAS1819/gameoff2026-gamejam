# MVP Data Specification

## Overview

This document defines the exact structure of `RoomData`, `InteractableData`, and `InventoryItemData` for the MVP. All MVP rooms and interactables must conform to these specs.

---

## 1. InventoryItemData

### Fields

```csharp
public string id;                    // unique identifier (lowercase, underscore-separated)
public string displayName;           // human-readable name
public string description;           // flavor text / usage description
public bool isKeyItem;              // whether this unlocks progression
```

### MVP Inventory Items

| id | displayName | description | isKeyItem |
|----|-------------|-------------|-----------|
| `staff_key` | Staff Key | An ornate brass key, warm to the touch. | true |

---

## 2. InteractionType Enum

```csharp
public enum InteractionType
{
    Examine,        // look at / read / inspect (no state change)
    Pickup,         // acquire an item and remove from scene
    Use,            // consume an item or interact with a puzzle
    Transition,     // move to another room
    Toggle,         // flip a boolean state (on/off)
    EnterRoom       // enter a room (legacy, use Transition instead)
}
```

**Note:** For MVP, use `Transition` for room changes, not `EnterRoom`.

---

## 3. InteractableData

### Fields

```csharp
public string id;                          // unique identifier (lowercase, underscore-separated)
public string displayName;                 // human-readable name
public string description;                 // flavor text / examination text
public InteractionType interactionType;    // type of interaction (Examine, Pickup, Use, Transition, Toggle)
public bool visibleInNormalReality;        // visible in normal reality?
public bool visibleInOtherReality;         // visible in "OtherSide" reality?
public bool isVisible;                     // runtime: cached visibility state
public string itemId;                      // if Pickup: the item id to acquire
public string requiredItemId;              // required item in inventory to interact
public string resultFlag;                  // flag to set when interaction succeeds
public bool isPickedUp;                    // runtime: has this item been picked up?
public bool isConsumed;                    // runtime: is this interactable consumed/used?
public bool consumeAfterUse;               // should this be marked consumed after use?
public string linkedRoomId;                // if Transition: target room id
```

### Rules

- `id` must be unique within a room
- `itemId` is required if `interactionType == Pickup`
- `itemId` should match an entry in the MVP inventory
- `requiredItemId` is optional; if set, the player must have that item to interact
- `resultFlag` should match an entry in the MVP flags (see Section 4)
- `visibleInNormalReality` and `visibleInOtherReality` determine which reality shows this object
- `linkedRoomId` is required if `interactionType == Transition`
- Runtime fields (`isVisible`, `isPickedUp`, `isConsumed`) are set by managers during play

---

## 4. MVP Global Flags

```csharp
public static class MVPFlags
{
    public const string HAS_STAFF_KEY = "HasStaffKey";        // set by picking up the key
    public const string DOOR_OPENED = "DoorOpened";          // set by using key on door
    public const string REALITY_UNLOCKED = "RealityUnlocked"; // set by examining hallway obstacle
    public const string SHIFT_PERFORMED = "ShiftPerformed";   // set when reality toggle occurs
}
```

---

## 5. RoomData

### Fields

```csharp
public string id;                                    // unique identifier (lowercase)
public string displayName;                          // human-readable room name
public List<InteractableData> interactables;        // all interactable objects in this room
public List<string> connectedRoomIds;               // ids of rooms accessible from here
```

### Rules

- `id` should be short and descriptive (e.g., "vestibule", "hallway")
- `displayName` is for debugging and UI (e.g., "Vestibule", "Main Hallway")
- `interactables` should contain all objects the player can interact with in this room
- `connectedRoomIds` should list rooms accessible via `Transition` interactables (optional, informational)

---

## 6. MVP Room Definitions

### Room: Vestibule

```csharp
RoomData
{
    id = "vestibule"
    displayName = "Vestibule"
    interactables = [
        // Interactable 1: Staff Key
        {
            id = "staff_key",
            displayName = "Staff Key",
            description = "An ornate brass key. It's warm to the touch.",
            interactionType = Pickup,
            visibleInNormalReality = true,
            visibleInOtherReality = false,
            itemId = "staff_key",
            resultFlag = "HasStaffKey",
            consumeAfterUse = true
        },
        // Interactable 2: Front Door
        {
            id = "front_door",
            displayName = "Front Door",
            description = "A heavy wooden door, locked with a brass keyhole.",
            interactionType = Use,
            visibleInNormalReality = true,
            visibleInOtherReality = true,
            requiredItemId = "staff_key",
            resultFlag = "DoorOpened",
            consumeAfterUse = false
        },
        // Interactable 3: Hallway Transition
        {
            id = "hallway_entrance",
            displayName = "Hallway",
            description = "You can see further into the darkness...",
            interactionType = Transition,
            visibleInNormalReality = true,
            visibleInOtherReality = true,
            requiredItemId = null,  // no item required once door is opened
            resultFlag = null,      // no flag set on transition
            linkedRoomId = "hallway"
        }
    ],
    connectedRoomIds = [ "hallway" ]
}
```

### Room: Hallway

```csharp
RoomData
{
    id = "hallway"
    displayName = "Main Hallway"
    interactables = [
        // Interactable 1: Hallway Obstacle (normal reality only)
        {
            id = "hallway_obstacle",
            displayName = "Massive Stone Figure",
            description = "A towering stone figure blocks your path. It's watching you.",
            interactionType = Examine,
            visibleInNormalReality = true,
            visibleInOtherReality = false,  // hidden in OtherSide
            resultFlag = "RealityUnlocked",
            consumeAfterUse = false
        },
        // Interactable 2: Reality Shift Trigger
        {
            id = "shift_trigger",
            displayName = "The Other Side",
            description = "Reality feels thin here...",
            interactionType = Toggle,
            visibleInNormalReality = true,
            visibleInOtherReality = true,
            resultFlag = "ShiftPerformed",
            consumeAfterUse = false
        },
        // Interactable 3: Return to Vestibule
        {
            id = "vestibule_exit",
            displayName = "Back to Vestibule",
            description = "The path back.",
            interactionType = Transition,
            visibleInNormalReality = true,
            visibleInOtherReality = true,
            linkedRoomId = "vestibule"
        }
    ],
    connectedRoomIds = [ "vestibule" ]
}
```

---

## 7. Data Model Assumptions

1. **No Duplicated Rooms in Hierarchy**
   - RoomData is immutable configuration
   - Reality changes happen through visibility toggles, not by creating duplicate rooms
   - RealityManager updates visibility based on `visibleInNormalReality` and `visibleInOtherReality`

2. **No Player Movement**
   - InteractionType does not include movement or pathfinding
   - Room transitions are instantaneous via Transition interactables
   - No animation or lerp between rooms in MVP

3. **Flags Drive Everything**
   - Progression is checked via `HasFlag()`, not by querying object state
   - Object visibility depends on reality state, not on hardcoded scene logic
   - Interactables are enabled/disabled based on required items and flags

4. **Generic Managers Use This Data**
   - RoomManager loads RoomData and creates a registry
   - InteractionManager resolves clicks using InteractableData
   - RealityManager updates visibility using visibleInNormalReality/visibleInOtherReality
   - InventoryManager checks `HasItem()` for required item validation
   - GameManager stores flags and checks progression

---

## 8. Data Initialization

GameManager.cs should initialize the MVP rooms and load them at startup:

```csharp
private void BuildDefaultRooms()
{
    var vestibule = new RoomData { ... };  // as defined in Section 6
    var hallway = new RoomData { ... };    // as defined in Section 6

    var roomRegistry = new Dictionary<string, RoomData>
    {
        { "vestibule", vestibule },
        { "hallway", hallway }
    };

    RoomManager.Instance.SetRooms(roomRegistry);
    RoomManager.Instance.LoadRoom("vestibule");
}
```

---

## 9. Validation Checklist

Before declaring the MVP data-ready, verify:

- [ ] All flag names match `MVPFlags` constants exactly
- [ ] All `itemId` references match InventoryItemData.id
- [ ] All `requiredItemId` references exist in inventory or are null
- [ ] All `linkedRoomId` references exist in RoomData
- [ ] No duplicate `id` within a room
- [ ] All `interactionType` values are valid enum members
- [ ] Pickup interactables have non-null `itemId`
- [ ] Transition interactables have non-null `linkedRoomId`
- [ ] Use/Examine interactables with results have non-null `resultFlag`
- [ ] visibleInNormalReality and visibleInOtherReality are both set (not null)

---

## Related Files

- `Assets/Scripts/MVPDataModels.cs` — data class definitions
- `Assets/Scripts/GameManager.cs` — flag storage and initialization
- `Assets/Scripts/RoomManager.cs` — room registry and interactable visibility
- `Assets/Scripts/InteractionManager.cs` — interaction resolution
- `Assets/Scripts/RealityManager.cs` — reality toggle and visibility refresh
- `MVP-Concrete-Hierarchy.md` — GameObject placement in scenes
- `MVP-Flag-Sequence.md` — execution flow and flag dependencies
