# MVP Specification - EMPEZAR AQUÍ

**Status:** ✅ Ready for implementation  
**Target Time:** 2.5-3 hours  
**Last Updated:** 2026-11-03

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
Bootstrap Scene (persistent, DontDestroyOnLoad)
└── Systems
    ├── GameManager (flags + initialization)
    ├── RoomManager (load rooms from data)
    ├── InventoryManager (items)
    ├── InteractionManager (click resolution)
    ├── RealityManager (reality toggle + visibility)
    ├── EventManager (stub)
    ├── SaveManager (stub)
    └── UIManager (stub)

Main Scene (all gameplay)
└── Rooms
    ├── Vestibule (3 interactables)
    │  ├── StaffKey (Pickup)
    │  ├── FrontDoor (Use)
    │  └── HallwayEntrance (Transition)
    └── Hallway (3 interactables)
       ├── HallwayObstacle (Examine, visible in Normal only)
       ├── ShiftTrigger (Toggle)
       └── VestibuleExit (Transition)
```

---

## 🚀 Implementation (5 Phases)

### Phase 1: Scenes (30 min)

- [ ] Create `Scenes/Bootstrap.unity` with `Systems` parent
- [ ] Create 8 child GameObjects (one per manager) — no scripts yet
- [ ] Create `Scenes/Main.unity` with Camera, Rooms, FX containers
- [ ] Create Vestibule room container with Visuals, Props, Interactables folders
- [ ] Create Hallway room container with Visuals, Props, Interactables folders
- [ ] Create 6 empty GameObjects for interactables

**Deliverable:** Correct scene hierarchy with all folders

---

### Phase 2: Interactables (45 min)

For each of the 6 interactables below:

- [ ] Add SpriteRenderer (sprite + sorting order)
- [ ] Add BoxCollider2D (IsTriggger = true)
- [ ] Position in scene
- [ ] Create + attach InteractableData (see data table below)

**Vestibule:**

| Name | Type | Position | Visual | Data |
| ------ | ------ | ---------- | -------- | ------ |
| StaffKey | Pickup | (3, 2, 0) | key sprite | itemId: "staff_key", resultFlag: "HasStaffKey", visibleNormal: true, visibleOther: false |
| FrontDoor | Use | (5, 0, 0) | door sprite | requiredItemId: "staff_key", resultFlag: "DoorOpened" |
| HallwayEntrance | Transition | (7, 0, 0) | marker | linkedRoomId: "hallway" |

**Hallway:**

| Name | Type | Position | Visual | Data |
| ------ | ------ | ---------- | -------- | ------ |
| HallwayObstacle | Examine | (5, 0, 0) | stone figure | resultFlag: "RealityUnlocked", visibleNormal: true, visibleOther: false |
| ShiftTrigger | Toggle | (10, 0, 0) | shimmer | resultFlag: "ShiftPerformed" |
| VestibuleExit | Transition | (-5, 0, 0) | marker | linkedRoomId: "vestibule" |

**Deliverable:** All 6 interactables with SpriteRenderer, BoxCollider2D, InteractableData

---

### Phase 3: Scripts (30 min)

**InteractableBehaviour.cs** (attach to all 6 interactables):

```csharp
public class InteractableBehaviour : MonoBehaviour
{
    public InteractableData data;
    
    private void OnMouseDown()
    {
        if (InteractionManager.Instance != null)
            InteractionManager.Instance.TryInteract(data.id);
        else
            Debug.LogWarning("InteractionManager instance not found.");
    }
}
```

**RealityVisibilityController.cs** (attach to HallwayObstacle):

```csharp
public class RealityVisibilityController : MonoBehaviour
{
    public InteractableData data;
    private SpriteRenderer spriteRenderer;
    
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (RealityManager.Instance != null)
            RealityManager.Instance.RealityChanged += OnRealityChanged;
        Refresh();
        if (RealityManager.Instance == null)
            Debug.LogWarning("RealityManager instance not found.");
    }
    
    void OnRealityChanged(RealityType reality) => Refresh();
    
    void Refresh()
    {
        bool visible = RealityManager.Instance.CurrentReality == RealityType.Normal
            ? data.visibleInNormalReality
            : data.visibleInOtherReality;
        spriteRenderer.enabled = visible;
    }
}
```

**Bootstrap:**

- Attach existing manager scripts to 8 GameObjects
- Verify `DontDestroyOnLoad` is called in `GameManager.Awake()`

**Deliverable:** All scripts attached and compiling

---

### Phase 4: Data Initialization (20 min)

Update `GameManager.BuildDefaultRooms()` to create both rooms:

```csharp
private void BuildDefaultRooms()
{
    var vestibule = new RoomData
    {
        id = "vestibule",
        displayName = "Vestibule",
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
            // VestibuleExit
            new InteractableData
            {
                id = "vestibule_exit",
                displayName = "Back to Vestibule",
                description = "The path back.",
                interactionType = InteractionType.Transition,
                visibleInNormalReality = true,
                visibleInOtherReality = true,
                linkedRoomId = "vestibule"
            }
        }
    };
    
    var roomRegistry = new Dictionary<string, RoomData>
    {
        { "vestibule", vestibule },
        { "hallway", hallway }
    };
    
    RoomManager.Instance.SetRooms(roomRegistry);
    RoomManager.Instance.LoadRoom("vestibule");
}
```

**Deliverable:** GameManager initializes rooms on startup

---

### Phase 5: Validation (30 min)

**Play Mode Checklist (8 Steps):**

1. [ ] Game starts, Vestibule loads with StaffKey visible
2. [ ] Click StaffKey → disappears, HasStaffKey = true
3. [ ] Click FrontDoor → opens, DoorOpened = true
4. [ ] Click HallwayEntrance → loads Hallway
5. [ ] Click HallwayObstacle → console shows examine, RealityUnlocked = true
6. [ ] Click ShiftTrigger → HallwayObstacle disappears
7. [ ] Click ShiftTrigger again → HallwayObstacle reappears
8. [ ] Click VestibuleExit → returns to Vestibule, state preserved

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
string id;                          // unique within room
string displayName;                 // human-readable
string description;                 // examination text
InteractionType interactionType;    // Examine, Pickup, Use, Transition, Toggle
bool visibleInNormalReality;       // visible in Normal reality?
bool visibleInOtherReality;        // visible in OtherSide reality?
string itemId;                      // (Pickup) item to acquire
string requiredItemId;              // item required to interact
string resultFlag;                  // flag to set on success
string linkedRoomId;                // (Transition) target room
bool consumeAfterUse;              // mark consumed after use?
```

### RoomData Fields

```csharp
string id;                              // "vestibule" or "hallway"
string displayName;                     // "Vestibule" or "Main Hallway"
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
| Visibility not changing | Attach RealityVisibilityController to object |
| Systems reinit between rooms | Verify DontDestroyOnLoad called in GameManager |

---

## 📚 Quick Reference Data Tables

### All Interactables at a Glance

| ID | Room | Type | Input | Output | Visibility |
| ---- | ------ | ------ | ------- | -------- | ----------- |
| staff_key | Vestibule | Pickup | — | HasStaffKey=true | Normal only |
| front_door | Vestibule | Use | staff_key | DoorOpened=true | Both |
| hallway_entrance | Vestibule | Transition | — | Load hallway | Both |
| hallway_obstacle | Hallway | Examine | — | RealityUnlocked=true | Normal only |
| shift_trigger | Hallway | Toggle | — | ShiftPerformed=true | Both |
| vestibule_exit | Hallway | Transition | — | Load vestibule | Both |

---

## 🏁 That's It

No other documents. No redundancy. Just these 5 phases.

Follow them in order, validate, done.

**Start with Phase 1. Estimated time: 2.5-3 hours to working MVP.**
