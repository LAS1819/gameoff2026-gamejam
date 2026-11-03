# MVP Architecture & Specification — Final Summary

**Status:** Complete and ready for implementation  
**Last Updated:** 2026-11-03  
**Target Implementation Time:** 2.5-3 hours

---

## Executive Summary

We have completed a **comprehensive, data-driven architecture specification** for the Game Off 2026 MVP. The specification is executable: a developer can follow the provided documentation and implement the MVP without ambiguity.

### What Was Delivered

A complete package of **5 documentation pieces** that work together:

1. **MVP-Quick-Reference.md** — Printable 1-page overview
2. **MVP-Implementation-Quickstart.md** — 6-phase checklist
3. **MVP-Data-Specification.md** — Exact field values
4. **MVP-Concrete-Hierarchy.md** — Exact GameObject structure
5. **MVP-Flag-Sequence.md** — Complete state machine and validation flow

**Central Index:** [02-Technical-Design/README.md](./02-Technical-Design/README.md)

---

## The MVP Scope

### Vertical Slice Gameplay

**Sequence (8 steps):**

```
1. Game starts → Vestibule loads with StaffKey visible
2. Click StaffKey → Pick up, flag HasStaffKey = true
3. Click FrontDoor → Open with key, flag DoorOpened = true
4. Click HallwayEntrance → Transition to Hallway
5. Click HallwayObstacle → Examine, flag RealityUnlocked = true
6. Click ShiftTrigger → Toggle reality, flag ShiftPerformed = true
7. HallwayObstacle disappears (not visible in OtherSide reality)
8. Player can navigate freely; loop can repeat
```

### Architecture Philosophy

**Core Principle:** Everything is data. Managers consume configuration, not hardcoded logic.

```
┌─────────────────────────────────────────┐
│ GameManager                             │
│ • Stores global flags                   │
│ • Initializes MVP rooms and data        │
│ • Bootstraps persistent systems         │
└────────────┬────────────────────────────┘
             │
             ├─→ RoomManager
             │   • Loads rooms from data
             │   • Refreshes interactable visibility
             │
             ├─→ InventoryManager
             │   • Tracks item possession
             │   • Syncs with flags
             │
             ├─→ InteractionManager
             │   • Resolves player clicks
             │   • Executes interaction logic
             │   • Updates flags
             │
             ├─→ RealityManager
             │   • Stores current reality state
             │   • Toggles reality
             │   • Triggers visibility refresh
             │
             └─→ EventManager, SaveManager, UIManager
                 (stubs for MVP, reserved for future)
```

**Result:** No room-specific code. No hardcoded scene logic. All logic lives in managers; all content lives in data.

---

## The 4 MVP Flags

| Flag | Set By | Effect |
|------|--------|--------|
| **HasStaffKey** | Picking up StaffKey | Unlocks FrontDoor interaction |
| **DoorOpened** | Using key on FrontDoor | Unlocks HallwayEntrance transition |
| **RealityUnlocked** | Examining HallwayObstacle | Allows ShiftTrigger interaction (optional gating) |
| **ShiftPerformed** | Toggling ShiftTrigger | Marks that shift has occurred; used for progression tracking |

**All flags initialize to false.**  
**All flags are set via InteractionManager based on InteractableData.resultFlag.**

---

## The 6 Interactables

| # | Name | Room | Type | Input | Result |
|---|------|------|------|-------|--------|
| 1 | StaffKey | Vestibule | Pickup | (none) | HasStaffKey = true; item added to inventory; object removed from scene |
| 2 | FrontDoor | Vestibule | Use | staff_key (required) | DoorOpened = true; door visual changes to "open" state |
| 3 | HallwayEntrance | Vestibule | Transition | (none) | Load "hallway" room |
| 4 | HallwayObstacle | Hallway | Examine | (none) | RealityUnlocked = true; visible only in Normal reality |
| 5 | ShiftTrigger | Hallway | Toggle | (none) | ShiftPerformed = true; toggle CurrentReality; refresh visibility |
| 6 | VestibuleExit | Hallway | Transition | (none) | Load "vestibule" room |

**All interactables are GameObject + SpriteRenderer + BoxCollider2D + InteractableBehaviour + InteractableData.**

---

## The 2 Rooms

### Vestibule

```
Vestibule
├── Background (SpriteRenderer, Sorting Order 0)
├── Props/DoorFrame (SpriteRenderer, Sorting Order 1, optional)
└── Interactables
    ├── StaffKey (visible, pickable)
    ├── FrontDoor (visible, requires key)
    └── HallwayEntrance (visible, requires door open)
```

### Hallway

```
Hallway
├── Background (SpriteRenderer, Sorting Order 0)
├── Props/Walls (SpriteRenderer, Sorting Order 1, optional)
└── Interactables
    ├── HallwayObstacle (visible in Normal, hidden in OtherSide)
    ├── ShiftTrigger (always visible)
    └── VestibuleExit (always visible)
```

---

## Scene Structure

### Bootstrap Scene

**Purpose:** Load once, persist forever.

```
Bootstrap
└── Systems (DontDestroyOnLoad = true)
    ├── GameManager (initializes MVP data)
    ├── RoomManager (loads rooms)
    ├── InventoryManager (tracks items)
    ├── InteractionManager (resolves clicks)
    ├── RealityManager (toggles reality)
    ├── EventManager (stub)
    ├── SaveManager (stub)
    └── UIManager (stub)
```

### Main Scene

**Purpose:** Contains all gameplay rooms and interactables.

```
Main
├── Camera (static, orthographic)
└── Rooms
    ├── Vestibule
    │   ├── Visuals/Background
    │   ├── Props/DoorFrame (optional)
    │   └── Interactables/[3 objects]
    └── Hallway
        ├── Visuals/Background
        ├── Props/Walls (optional)
        └── Interactables/[3 objects]
```

**Key principle:** No Player object. No movement controller. No Rigidbody2D. Entirely point-and-click.

---

## Data Model: InteractableData

```csharp
public class InteractableData
{
    // Identification
    public string id;                           // unique within room
    public string displayName;                  // human-readable
    public string description;                  // examination text

    // Interaction Type
    public InteractionType interactionType;    // Examine, Pickup, Use, Transition, Toggle

    // Reality Visibility
    public bool visibleInNormalReality = true;
    public bool visibleInOtherReality = true;

    // Interaction Requirements
    public string itemId;                      // for Pickup: the item to acquire
    public string requiredItemId;              // required item to interact
    public string resultFlag;                  // flag to set on success

    // Room Transition
    public string linkedRoomId;                // for Transition: target room

    // Runtime State
    public bool isVisible;                     // cached visibility
    public bool isPickedUp;                    // has this been picked up?
    public bool isConsumed;                    // is this consumed/used?
    public bool consumeAfterUse;               // should it be consumed after first use?
}
```

---

## Data Model: RoomData

```csharp
public class RoomData
{
    public string id;                                   // "vestibule" or "hallway"
    public string displayName;                         // "Vestibule" or "Main Hallway"
    public List<InteractableData> interactables;       // all clickable objects
    public List<string> connectedRoomIds;              // accessible rooms (informational)
}
```

---

## Implementation Phases (2.5 Hours Total)

### Phase 1: Scenes & GameObjects (30 min)

**Deliverable:** Bootstrap scene + Main scene with hierarchy

- [ ] Create Bootstrap scene, Systems parent, 8 manager GameObjects
- [ ] Create Main scene with Camera, Rooms, FX containers
- [ ] Create Vestibule room container with Visuals, Props, Interactables
- [ ] Create Hallway room container with Visuals, Props, Interactables
- [ ] Create all 6 interactable GameObjects (no scripts yet)

**Reference:** [MVP-Implementation-Quickstart.md § Phase 1](./02-Technical-Design/MVP-Implementation-Quickstart.md)

### Phase 2: Interactable Configuration (45 min)

**Deliverable:** All 6 interactables with SpriteRenderer, BoxCollider2D, InteractableData

- [ ] Attach SpriteRenderer to each interactable
- [ ] Attach BoxCollider2D (IsTriggger = true)
- [ ] Create and attach InteractableData with exact field values from spec
- [ ] Position each object appropriately

**Reference:** [MVP-Implementation-Quickstart.md § Phase 2](./02-Technical-Design/MVP-Implementation-Quickstart.md)

### Phase 3: Script Configuration (30 min)

**Deliverable:** All scripts attached and configured

- [ ] Attach manager scripts to Bootstrap GameObjects
- [ ] Create InteractableBehaviour script (OnMouseDown → TryInteract)
- [ ] Attach InteractableBehaviour to all 6 interactables
- [ ] Create RealityVisibilityController script (updates SpriteRenderer.enabled)
- [ ] Attach RealityVisibilityController to HallwayObstacle

**Reference:** [MVP-Implementation-Quickstart.md § Phase 3](./02-Technical-Design/MVP-Implementation-Quickstart.md)

### Phase 4: Data Initialization (20 min)

**Deliverable:** GameManager.BuildDefaultRooms() creates both rooms

- [ ] Update GameManager.BuildDefaultRooms() to create Vestibule RoomData
- [ ] Add Hallway RoomData
- [ ] Register both in room dictionary
- [ ] Call RoomManager.SetRooms()
- [ ] Verify all 4 flags initialize to false

**Reference:** [MVP-Implementation-Quickstart.md § Phase 4](./02-Technical-Design/MVP-Implementation-Quickstart.md)

### Phase 5: Play Mode Validation (30 min)

**Deliverable:** Complete MVP sequence works without errors

**Validation Sequence (8 steps):**

1. Start game → Vestibule loads with StaffKey visible
2. Click StaffKey → StaffKey disappears, HasStaffKey = true
3. Click FrontDoor → FrontDoor opens, DoorOpened = true
4. Click HallwayEntrance → Load Hallway
5. Click HallwayObstacle → RealityUnlocked = true (console output)
6. Click ShiftTrigger → Reality toggles, HallwayObstacle disappears
7. Click ShiftTrigger → Reality toggles back, HallwayObstacle reappears
8. Click VestibuleExit → Return to Vestibule, state preserved

**Reference:** [MVP-Flag-Sequence.md § Play Mode Validation Sequence](./02-Technical-Design/MVP-Flag-Sequence.md)

---

## Key Design Decisions

### 1. Data-Driven Everything

**Decision:** All content is configuration in data, not code.

**Why:** Allows non-programmers to modify puzzles, add rooms, and change progression without touching code. Easy to maintain and iterate.

**Implication:** No room-specific logic. All logic is generic and reusable.

### 2. Flag-Based Progression

**Decision:** Game state is driven by boolean flags, not object queries.

**Why:** Decouples progression from object state. Flags are explicit, debuggable, and persist correctly across scene transitions.

**Implication:** Every progression check uses `HasFlag()`, never `ifObjectState.property`.

### 3. Generic Visibility System

**Decision:** Objects control their own visibility via `visibleInNormalReality` and `visibleInOtherReality`.

**Why:** No need to maintain duplicate room hierarchies. Single object, multiple visibility states.

**Implication:** RealityManager simply toggles visibility; RoomManager refreshes. Simple and scalable.

### 4. No Player Avatar

**Decision:** Protagonist is never visible; no free movement.

**Why:** Point-and-click horror, not exploration game. Reduces scope dramatically.

**Implication:** No PlayerController, no Rigidbody2D, no animation system for player movement. Only interactions and transitions.

### 5. Persistent Managers

**Decision:** All managers are global and marked `DontDestroyOnLoad`.

**Why:** Game state must survive room transitions. No re-initialization.

**Implication:** GameManager is truly global. Flags, inventory, and room registry persist forever.

---

## Validation Checklist (Before Declaring MVP Complete)

**Architecture:**
- [ ] All managers are persistent (DontDestroyOnLoad)
- [ ] No character controller or player movement
- [ ] No duplicated room hierarchies
- [ ] All interactables use InteractableData
- [ ] All progression uses flags

**Data:**
- [ ] All 4 flags initialize to false
- [ ] All 6 interactables have exact data from spec
- [ ] All rooms loaded from GameManager.BuildDefaultRooms()
- [ ] No hardcoded scene-specific logic

**Gameplay (8-step sequence):**
- [ ] Pick up key → HasStaffKey = true ✓
- [ ] Use key → DoorOpened = true ✓
- [ ] Transition → Active room changes ✓
- [ ] Examine obstacle → RealityUnlocked = true ✓
- [ ] Shift reality → Current reality toggles ✓
- [ ] Visibility updates → HallwayObstacle disappears ✓
- [ ] Shift back → Obstacle reappears ✓
- [ ] State persists → No flags reset ✓

**No Errors:**
- [ ] Console clean (no exceptions)
- [ ] All manager instances found
- [ ] Room registry complete
- [ ] No missing asset references

---

## Success Metrics

**MVP is "Done" when:**

1. ✅ The 8-step sequence completes without errors
2. ✅ All 4 flags are set correctly at the right times
3. ✅ HallwayObstacle visibility toggles with reality
4. ✅ Room transitions work without state loss
5. ✅ Console has zero critical errors
6. ✅ The loop can repeat indefinitely

**Estimated Time to "Done":** 2.5-3 hours of focused implementation

---

## Next Steps After MVP

Once the MVP is validated and the loop works:

1. **Phase 6 (Week 2):** Expand content
   - Add more rooms (Library, Basement, Ritual Chamber)
   - Add more puzzles and items
   - Expand the flag system

2. **Phase 7 (Week 3):** Polish and narrative
   - Add UI feedback and inventory UI
   - Add dialogue/text fragments
   - Add audio and visual effects

3. **Phase 4 (Week 4):** Endings and refinement
   - Add multiple endings
   - Fine-tune pacing
   - Balance difficulty

---

## Documentation Files Created

```
Docs/02-Technical-Design/
├── README.md                           (← central index)
├── MVP-Quick-Reference.md              (← printable 1-pager)
├── MVP-Implementation-Quickstart.md    (← phase checklist)
├── MVP-Data-Specification.md           (← exact values)
├── MVP-Concrete-Hierarchy.md           (← exact structure)
└── MVP-Flag-Sequence.md                (← state machine)
```

---

## Recommended Reading Order

1. This document (you're reading it)
2. [MVP-Quick-Reference.md](./02-Technical-Design/MVP-Quick-Reference.md) — Print this
3. [MVP-Implementation-Quickstart.md](./02-Technical-Design/MVP-Implementation-Quickstart.md) — Start implementing
4. Refer to spec docs as needed during implementation

---

## Questions?

- **"What's the exact data for [interactable]?"** → See [MVP-Data-Specification.md](./02-Technical-Design/MVP-Data-Specification.md)
- **"Where does [GameObject] go?"** → See [MVP-Concrete-Hierarchy.md](./02-Technical-Design/MVP-Concrete-Hierarchy.md)
- **"What happens when I click [thing]?"** → See [MVP-Flag-Sequence.md](./02-Technical-Design/MVP-Flag-Sequence.md)
- **"How do I validate the MVP?"** → See [MVP-Flag-Sequence.md § Play Mode Validation Sequence](./02-Technical-Design/MVP-Flag-Sequence.md)

---

## Summary

We have transformed an architectural idea into an **executable specification**. Every GameObject, every script, every piece of data is defined. The specification is complete, unambiguous, and ready for implementation.

**The MVP is not built yet, but it is fully designed.**

**Target:** 2.5-3 hours from specification to working game loop.

**Status:** Ready for implementation. ✅

---

**Document Created:** 2026-11-03  
**By:** Jamgent (Architect)  
**Next Step:** Begin [MVP-Implementation-Quickstart.md § Phase 1](./02-Technical-Design/MVP-Implementation-Quickstart.md)
