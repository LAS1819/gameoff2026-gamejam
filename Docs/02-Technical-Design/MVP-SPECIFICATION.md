# MVP Specification - EMPEZAR AQUÍ

**Status:** 🟡 Lobby Complete, Hallway In Progress  
**Target Time:** 2.5-3 hours  
**Last Updated:** 2026-11-04

---

## 🎯 The MVP in 2 Minutes

A point-and-click horror game where you:

1. Pick up a key → Flag: `HasStaffKey`
2. Unlock a door → Flag: `DoorOpened`
3. Enter a hallway → Room transition
4. Examine an obstacle → Flag: `RealityUnlocked`
5. Shift reality → Flag: `ShiftPerformed`
6. Obstacle disappears (not visible in OtherSide reality)
7. Path is clear

**That's it. That's the MVP.**

---

## 🏗️ Architecture (One Rule)

**Everything is data. Managers consume data. No room-specific code.**

```plaintext
Main Scene (single gameplay scene)
├── GameManager (flags + initialization)
├── RoomManager (load rooms from data)
├── InventoryManager (items)
├── InteractionManager (click resolution)
├── RealityManager (reality toggle + visibility)
├── EventManager (stub)
├── SaveManager (stub)
├── UIManager (stub)
└── Rooms
    ├── Lobby ✅ COMPLETE
    │  ├── Visuals
    │  ├── Props
    │  └── Interactables
    │     ├── StaffKey (Pickup) ✅
    │     ├── FrontDoor (Use) ✅
    │     └── HallwayEntrance (Transition) ✅
    └── Hallway 🟡 IN PROGRESS
       ├── Visuals
       ├── Props
       └── Interactables
           ├── HallwayObstacle (Examine)
           ├── ShiftTrigger (Toggle)
           └── LobbyExit (Transition)
```

---

## 🚀 Implementation Status

### Phase 1: Scenes ✅ COMPLETE (30 min)

- [x] Create `Scenes/Main.unity` with Camera, Rooms, FX containers
- [x] Create Lobby room container with Visuals, Props, Interactables folders
- [x] Create Hallway room container with Visuals, Props, Interactables folders
- [x] All 6 empty GameObjects for interactables created

**Deliverable:** ✅ Correct scene hierarchy with all folders

---

### Phase 2: Interactables - Lobby ✅ COMPLETE (45 min)

For Lobby (3/3 complete):

| Name | Type | Position | Status |
| ------ | ------ | ---------- | ------ |
| StaffKey | Pickup | (3, 2, 0) | ✅ Complete |
| FrontDoor | Use | (5, 0, 0) | ✅ Complete |
| HallwayEntrance | Transition | (7, 0, 0) | ✅ Complete |

**Deliverable:** ✅ All 3 Lobby interactables with SpriteRenderer, BoxCollider2D, InteractableData

---

### Phase 2b: Interactables - Hallway 🟡 IN PROGRESS

For Hallway (0/3 complete):

| Name | Type | Position | Status |
| ------ | ------ | ---------- | ------ |
| HallwayObstacle | Examine | (5, 0, 0) | 🟡 Pending |
| ShiftTrigger | Toggle | (10, 0, 0) | 🟡 Pending |
| LobbyExit | Transition | (-5, 0, 0) | 🟡 Pending |

**Next Task:** Configure Hallway interactables

---

### Phase 3: Scripts ✅ COMPLETE

**All core scripts implemented and working:**

- [x] InteractableBehaviour (attach to all interactables)
- [x] GameManager (flags, initialization, single-scene architecture)
- [x] RoomManager (room loading, visibility toggling)
- [x] InteractionManager (click resolution)
- [x] InventoryManager (inventory tracking)
- [x] RealityManager (reality toggle, visibility)
- [x] EventManager, SaveManager, UIManager (stubs)

### Phase 4: Data Initialization (20 min)

Update `GameManager.BuildDefaultRooms()` to create both rooms:

```csharp
private void BuildDefaultRooms()
{
    var lobby = new RoomData
    {
        id = "lobby",
        displayName = "Lobby",
        interactables = new List<InteractableData>
        {
            // StaffKey
            new InteractableData
            {
                id = "staff_key",
                displayName = "Staff Key",
                description = "An ornate brass key. It's warm to the touch.",
                interactionType = InteractionType.Pickup,
                visibleInNormalReality = true,
                visibleInOtherReality = false,
                itemId = "staff_key",
                resultFlag = "HasStaffKey",
                consumeAfterUse = true
            },
            // FrontDoor
            new InteractableData
            {
                id = "front_door",
                displayName = "Front Door",
                description = "A heavy wooden door, locked with a brass keyhole.",
                interactionType = InteractionType.Use,
                visibleInNormalReality = true,
                visibleInOtherReality = true,
                requiredItemId = "staff_key",
                resultFlag = "DoorOpened"
            },
            // HallwayEntrance
            new InteractableData
            {
                id = "hallway_entrance",
                displayName = "Hallway",
                description = "You can see further into the darkness...",
                interactionType = InteractionType.Transition,
                visibleInNormalReality = true,
                visibleInOtherReality = true,
                linkedRoomId = "hallway"
            }
        }
    };
    
    var hallway = new RoomData
    {
        id = "hallway",
        displayName = "Main Hallway",
        interactables = new List<InteractableData>
        {
            // HallwayObstacle
            new InteractableData
            {
                id = "hallway_obstacle",
                displayName = "Massive Stone Figure",
                description = "A towering stone figure blocks your path. It's watching you.",
                interactionType = InteractionType.Examine,
                visibleInNormalReality = true,
                visibleInOtherReality = false,
                resultFlag = "RealityUnlocked"
            },
            // ShiftTrigger
            new InteractableData
            {
                id = "shift_trigger",
                displayName = "The Other Side",
                description = "Reality feels thin here...",
                interactionType = InteractionType.Toggle,
                visibleInNormalReality = true,
                visibleInOtherReality = true,
                resultFlag = "ShiftPerformed"
            },
            // LobbyExit
            new InteractableData
            {
                id = "lobby_exit",
                displayName = "Back to Lobby",
                description = "The path back.",
                interactionType = InteractionType.Transition,
                visibleInNormalReality = true,
                visibleInOtherReality = true,
                linkedRoomId = "lobby"
            }
        }
    };
    
    RoomManager.Instance.SetRooms(new List<RoomData> { lobby, hallway });
    RoomManager.Instance.LoadRoom("lobby");
}
```

**Deliverable:** GameManager initializes rooms on startup

---

### Phase 5: Validation (30 min)

**Play Mode Checklist (8 Steps):**

1. [ ] Game starts, Lobby loads with StaffKey visible
2. [ ] Click StaffKey → disappears, HasStaffKey = true
3. [ ] Click FrontDoor → opens, DoorOpened = true
4. [ ] Click HallwayEntrance → loads Hallway
5. [ ] Click HallwayObstacle → console shows examine, RealityUnlocked = true
6. [ ] Click ShiftTrigger → HallwayObstacle disappears
7. [ ] Click ShiftTrigger again → HallwayObstacle reappears
8. [ ] Click LobbyExit → returns to Lobby, state preserved

**If all 8 pass: MVP is complete ✅**

---

## 📊 The 4 MVP Flags

```csharp
HasStaffKey = false         // Set by picking up key
DoorOpened = false          // Set by using key on door
RealityUnlocked = false     // Set by examining obstacle
ShiftPerformed = false      // Set by toggling reality
```

---

## 📋 Data Model

### InteractableData Fields

```csharp
string id;                          // unique interactable id within room (world object id)
string displayName;                 // human-readable
string description;                 // examination text
InteractionType interactionType;    // Examine, Pickup, Use, Transition, Toggle
bool visibleInNormalReality;       // visible in Normal reality?
bool visibleInOtherReality;        // visible in OtherSide reality?
string itemId;                      // (Pickup) inventory item id granted on pickup
string requiredItemId;              // item required to interact
string resultFlag;                  // flag to set on success
string linkedRoomId;                // (Transition) target room
bool consumeAfterUse;              // mark consumed after use?
```

### RoomData Fields

```csharp
string id;                              // "lobby" or "hallway"
string displayName;                     // "Lobby" or "Main Hallway"
List<InteractableData> interactables;   // all clickables in this room
List<string> connectedRoomIds;          // accessible rooms (optional)
```

---

## 🎯 Definition of "Done"

MVP is complete when:

- ✅ All 8 steps in Phase 5 validation pass
- ✅ Flags set at correct times
- ✅ HallwayObstacle visibility toggles correctly
- ✅ Room transitions preserve state
- ✅ Console is clean (no errors)
- ✅ Loop repeats indefinitely

---

## ❓ Troubleshooting

| Problem | Solution |
| --------- | ---------- |
| "Interactable not found" | Verify interactable id matches exactly in data |
| Flag not updating | Check resultFlag field is set in InteractableData |
| Room doesn't load | Verify linkedRoomId exists in BuildDefaultRooms() |
| Visibility not changing | Verify InteractableData visibility fields + current room id |
| Room GameObject not found | Verify "Rooms" parent GameObject exists in Main scene and contains room children |

---

## 📚 Quick Reference Data Tables

### All Interactables at a Glance

| ID | Room | Type | Input | Output | Visibility |
| ---- | ------ | ------ | ------- | -------- | ----------- |
| staff_key | Lobby | Pickup | — | HasStaffKey=true | Normal only |
| front_door | Lobby | Use | staff_key | DoorOpened=true | Both |
| hallway_entrance | Lobby | Transition | — | Load hallway | Both |
| hallway_obstacle | Hallway | Examine | — | RealityUnlocked=true | Normal only |
| shift_trigger | Hallway | Toggle | — | ShiftPerformed=true | Both |
| lobby_exit | Hallway | Transition | — | Load lobby | Both |

---

## 🏁 That's It

No other documents. No redundancy. Just these 5 phases.

Follow them in order, validate, done.

**Start with Phase 1. Estimated time: 2.5-3 hours to working MVP.**
