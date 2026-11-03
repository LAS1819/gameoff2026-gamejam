# MVP Technical Documentation Index

**Last updated:** 2026-11-03

---

## Quick Navigation

### For Implementation (Start Here)

| Document | Purpose | Audience |
|----------|---------|----------|
| **[MVP-Implementation-Quickstart.md](./MVP-Implementation-Quickstart.md)** | Step-by-step checklist for building the MVP | Developers implementing the MVP |
| **[MVP-Concrete-Hierarchy.md](./MVP-Concrete-Hierarchy.md)** | Exact GameObject structure and naming | Scene builders, architects |
| **[MVP-Data-Specification.md](./MVP-Data-Specification.md)** | Exact field values for all data objects | Data modelers, scripters |
| **[MVP-Flag-Sequence.md](./MVP-Flag-Sequence.md)** | Exact execution flow and state transitions | QA, testers, designers validating flow |

### High-Level Reference

| Document | Purpose | Audience |
|----------|---------|----------|
| **[MVP-Sprint-1.md](./MVP-Sprint-1.md)** | Sprint goals and acceptance criteria | Project managers, designers, QA |
| **[Technical-Design.md](./Technical-Design.md)** | Architecture overview and philosophy | Architects, senior developers |
| **[GameObject-Hierarchy.md](./GameObject-Hierarchy.md)** | General hierarchy patterns (legacy, see MVP-Concrete-Hierarchy.md) | Reference only |

---

## Document Relationships

```
MVP-Sprint-1.md (What are we building?)
    ↓
    ├─→ MVP-Implementation-Quickstart.md (How do we build it?)
    │   ├─→ MVP-Concrete-Hierarchy.md (What GameObjects do we create?)
    │   ├─→ MVP-Data-Specification.md (What data do we configure?)
    │   └─→ MVP-Flag-Sequence.md (What is the execution flow?)
    │
    └─→ Technical-Design.md (Why are we building it this way?)
```

---

## MVP Scope Summary

### What We're Building

**Vertical Slice:** A minimal but complete gameplay loop validating core mechanics.

**Sequence:**
1. Pick up staff key (Vestibule)
2. Use key to open door (Vestibule)
3. Transition to hallway (Room change)
4. Examine hallway obstacle (Interaction)
5. Shift reality (Toggle)
6. Obstacle disappears in alternate reality (Visibility logic)

### What We're NOT Building

- Multiple endings or branching narrative
- Full art pipeline or sound design
- Advanced puzzle chains beyond the MVP loop
- Persistent save/load system (stub only)
- Event system beyond basic progression
- UI beyond minimal feedback
- Any content beyond Vestibule and Hallway

---

## Core Concepts

### Architecture Philosophy

- **Data-driven:** All game logic is configuration, not code
- **Manager-based:** Global systems persist across scenes
- **Flag-driven:** Progression is controlled by boolean flags
- **Reality-based:** Visibility is determined by reality state and object properties
- **Generic:** All systems are reusable; no room-specific code

### Key Abstractions

| Concept | Definition | Example |
|---------|-----------|---------|
| **RoomData** | Configuration container for a playable space | Vestibule, Hallway |
| **InteractableData** | Configuration for a clickable object | Staff Key, Front Door |
| **InteractionType** | Type of action the player can perform | Pickup, Use, Examine, Transition |
| **GameFlag** | Boolean state controlling progression | HasStaffKey, DoorOpened |
| **RealityType** | World state affecting object visibility | Normal, OtherSide |

### Manager Responsibilities

| Manager | Purpose | Key Methods |
|---------|---------|-------------|
| **GameManager** | Global state, flags, initialization | SetFlag, HasFlag, InitializeMVP |
| **RoomManager** | Room registry, room transitions | LoadRoom, RefreshInteractables |
| **InventoryManager** | Item inventory and validation | AddItem, HasItem, RemoveItem |
| **InteractionManager** | Interaction resolution and logic | TryInteract |
| **RealityManager** | Reality state and visibility refresh | ToggleReality, SetReality |

---

## Implementation Workflow

### Phase 1: Scene & GameObject Setup

**Deliverable:** Bootstrap scene and Main scene with hierarchy structure

**Duration:** ~30 min

**Checklist:**
- Create Bootstrap scene with Systems container
- Create 8 manager GameObjects (no scripts yet)
- Create Main scene with Camera, Rooms, FX
- Create Vestibule room container with Visuals, Props, Interactables
- Create Hallway room container with Visuals, Props, Interactables

**Reference:** [MVP-Implementation-Quickstart.md § Phase 1](./MVP-Implementation-Quickstart.md)

---

### Phase 2: Interactable Configuration

**Deliverable:** 6 fully configured interactables with exact data

**Duration:** ~45 min

**Checklist:**
- Create StaffKey (Vestibule) with Pickup data
- Create FrontDoor (Vestibule) with Use data
- Create HallwayEntrance (Vestibule) with Transition data
- Create HallwayObstacle (Hallway) with Examine data
- Create ShiftTrigger (Hallway) with Toggle data
- Create VestibuleExit (Hallway) with Transition data

**Reference:** [MVP-Implementation-Quickstart.md § Phase 2](./MVP-Implementation-Quickstart.md)

---

### Phase 3: Script Configuration

**Deliverable:** All manager scripts attached and initialized

**Duration:** ~30 min

**Checklist:**
- Attach scripts to Bootstrap managers
- Create InteractableBehaviour script
- Create RealityVisibilityController script
- Attach scripts to all 6 interactables
- Configure DontDestroyOnLoad

**Reference:** [MVP-Implementation-Quickstart.md § Phase 3](./MVP-Implementation-Quickstart.md)

---

### Phase 4: Data Initialization

**Deliverable:** GameManager builds and loads rooms at startup

**Duration:** ~20 min

**Checklist:**
- Update GameManager.BuildDefaultRooms()
- Verify all flags initialize to false
- Verify room registry is populated
- Test initial room load in Play Mode

**Reference:** [MVP-Implementation-Quickstart.md § Phase 4](./MVP-Implementation-Quickstart.md)

---

### Phase 5: Play Mode Validation

**Deliverable:** Complete MVP sequence works without errors

**Duration:** ~30 min

**Validation Sequence (8 steps):**
1. Start game → Vestibule loads
2. Pick up key → StaffKey disappears, HasStaffKey = true
3. Use key on door → FrontDoor opens, DoorOpened = true
4. Enter hallway → Hallway loads, HallwayObstacle visible
5. Examine obstacle → RealityUnlocked = true
6. Shift reality → HallwayObstacle disappears
7. Shift back → HallwayObstacle reappears
8. Return to vestibule → State preserved

**Reference:** [MVP-Flag-Sequence.md § Play Mode Validation Sequence](./MVP-Flag-Sequence.md)

---

## Data Reference

### MVP Flags

```csharp
HasStaffKey          → Set by picking up the key
DoorOpened          → Set by using key on door
RealityUnlocked     → Set by examining hallway obstacle
ShiftPerformed      → Set by toggling reality
```

### MVP Inventory Items

```
staff_key (Staff Key)
  ├── displayName: "Staff Key"
  ├── description: "An ornate brass key. It's warm to the touch."
  └── isKeyItem: true
```

### MVP Interactables

| id | Room | Type | Required Item | Result Flag |
|----|------|------|---------------|------------|
| staff_key | Vestibule | Pickup | — | HasStaffKey |
| front_door | Vestibule | Use | staff_key | DoorOpened |
| hallway_entrance | Vestibule | Transition | — | — |
| hallway_obstacle | Hallway | Examine | — | RealityUnlocked |
| shift_trigger | Hallway | Toggle | — | ShiftPerformed |
| vestibule_exit | Hallway | Transition | — | — |

### MVP Rooms

```
Vestibule
  └── Contains: staff_key, front_door, hallway_entrance

Hallway
  └── Contains: hallway_obstacle, shift_trigger, vestibule_exit
```

---

## Validation Checklist

### Before Declaring MVP Complete

**Architecture:**
- [ ] All managers are persistent (DontDestroyOnLoad)
- [ ] No character controller or player movement
- [ ] No duplicated room hierarchies
- [ ] All interactables use InteractableData
- [ ] All progression uses flags, not object queries

**Data:**
- [ ] All 4 flags initialize to false
- [ ] All 6 interactables have exact data from spec
- [ ] All room data is loaded from GameManager.BuildDefaultRooms()
- [ ] No hardcoded scene-specific logic

**Gameplay:**
- [ ] Pick up key → HasStaffKey = true ✓
- [ ] Use key → DoorOpened = true ✓
- [ ] Transition works → Active room changes ✓
- [ ] Examine obstacle → RealityUnlocked = true ✓
- [ ] Shift reality → Current reality toggles ✓
- [ ] Visibility updates → HallwayObstacle disappears ✓
- [ ] State persists → No flags reset on room change ✓

**No Errors:**
- [ ] Console is clean (no exception logs)
- [ ] All manager instances found correctly
- [ ] Room registry is complete
- [ ] No missing asset references

---

## Common Pitfalls & Solutions

| Problem | Root Cause | Solution |
|---------|-----------|----------|
| "Interactable not found" | ID mismatch in data | Check [MVP-Data-Specification.md](./MVP-Data-Specification.md) for exact IDs |
| Flag not updating | InteractionManager not calling SetFlag() | Verify resultFlag is set in InteractableData |
| Room doesn't load | linkedRoomId doesn't exist in registry | Verify room ID matches BuildDefaultRooms() |
| Object visibility not changing | RealityVisibilityController not attached | Attach script to all reality-dependent objects |
| Systems recreating each scene change | DontDestroyOnLoad not called | Verify GameManager.Awake() calls it |

---

## Performance & Optimization Notes

For MVP scope, no optimization is needed. However, keep these in mind for future:

- **Room loading:** Current approach loads all rooms at startup. Lazy-load for large projects.
- **Visibility refresh:** Called every interaction. Cache results if performance issues arise.
- **Interactable queries:** Linear search through room interactables. Use dictionary for large rooms.
- **Memory:** All systems are persistent. Ensure proper cleanup in OnDestroy.

---

## Related External Documentation

- **[Technical-Design.md](./Technical-Design.md)** — High-level architecture philosophy
- **[GameObject-Hierarchy.md](./GameObject-Hierarchy.md)** — General patterns (reference)
- **[README.md](../../README.md)** — Project overview
- **[Docs/README.md](../README.md)** — Documentation structure

---

## Questions or Issues?

If implementing the MVP and you hit a question:

1. Check **[MVP-Implementation-Quickstart.md](./MVP-Implementation-Quickstart.md)** § Troubleshooting
2. Review the specific phase documentation (Phase 1-5)
3. Cross-reference the exact specification in **[MVP-Data-Specification.md](./MVP-Data-Specification.md)** or **[MVP-Flag-Sequence.md](./MVP-Flag-Sequence.md)**
4. Verify hierarchy against **[MVP-Concrete-Hierarchy.md](./MVP-Concrete-Hierarchy.md)**

---

## Version History

- **v1.0** (2026-11-03) — Initial MVP documentation package
  - Created MVP-Implementation-Quickstart.md (phase-by-phase checklist)
  - Created MVP-Data-Specification.md (exact data values)
  - Created MVP-Concrete-Hierarchy.md (exact GameObject structure)
  - Created MVP-Flag-Sequence.md (exact execution flow)
  - Updated MVP-Sprint-1.md with cross-references
  - Created this index

---

**Last Updated:** 2026-11-03 by Jamgent  
**Status:** Ready for implementation  
**Next Step:** Follow [MVP-Implementation-Quickstart.md](./MVP-Implementation-Quickstart.md) Phase 1
