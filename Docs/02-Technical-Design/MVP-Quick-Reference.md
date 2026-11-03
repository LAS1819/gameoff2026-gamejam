# MVP Quick Reference Card

**Print this. Keep it visible while implementing.**

---

## The 5-Minute Overview

### What We're Building

A point-and-click horror game where:
- You pick up a key
- Use the key to open a door
- Enter a hallway
- Examine an obstacle (reveals you can shift reality)
- Shift to an alternate reality
- The obstacle disappears
- You proceed

### Architecture: One Rule

**Everything is data. Managers consume data. No room-specific code.**

```
GameManager (state + initialization)
  → RoomManager (load rooms from data)
    → InteractionManager (resolve clicks using data)
      → Managers update flags
        → RoomManager refreshes visibility
```

---

## The 4 Flags (All You Really Need to Know)

| Flag | When | Effect |
|------|------|--------|
| **HasStaffKey** | Pick up the key | Unlocks door |
| **DoorOpened** | Use key on door | Unlocks hallway entrance |
| **RealityUnlocked** | Examine obstacle | Allows reality shift |
| **ShiftPerformed** | Toggle reality | Updates visibility |

---

## The 6 Interactables

### Vestibule

```
1. StaffKey
   Type: Pickup
   Sets: HasStaffKey
   Disappears when picked up

2. FrontDoor
   Type: Use
   Requires: staff_key (item)
   Sets: DoorOpened
   Shows "open" state after use

3. HallwayEntrance
   Type: Transition
   Goes to: hallway
   Requires: DoorOpened to be true
```

### Hallway

```
4. HallwayObstacle
   Type: Examine
   Sets: RealityUnlocked
   Visible only in Normal reality
   Disappears in OtherSide reality

5. ShiftTrigger
   Type: Toggle
   Sets: ShiftPerformed
   Switches reality (Normal ↔ OtherSide)

6. VestibuleExit
   Type: Transition
   Goes to: vestibule
```

---

## The Flow (8 Steps)

```
1. Game starts → Vestibule loads
2. Click StaffKey → HasStaffKey = true
3. Click FrontDoor → DoorOpened = true
4. Click HallwayEntrance → Load Hallway
5. Click HallwayObstacle → RealityUnlocked = true
6. Click ShiftTrigger → Current reality toggles
7. Obstacle.visible = (CurrentReality != Normal)
8. Click again to shift back → Obstacle reappears
```

---

## The Hierarchy (Copy This)

```
Bootstrap Scene
└── Systems
    ├── GameManager (script: GameManager.cs)
    ├── RoomManager (script: RoomManager.cs)
    ├── InventoryManager (script: InventoryManager.cs)
    ├── InteractionManager (script: InteractionManager.cs)
    ├── RealityManager (script: RealityManager.cs)
    ├── EventManager (stub)
    ├── SaveManager (stub)
    └── UIManager (stub)

Main Scene
├── Camera (orthographic, static)
└── Rooms
    ├── Vestibule
    │   ├── Visuals/Background (SpriteRenderer)
    │   ├── Props/DoorFrame (SpriteRenderer, optional)
    │   └── Interactables
    │       ├── StaffKey (SpriteRenderer + BoxCollider2D + InteractableBehaviour + data)
    │       ├── FrontDoor (SpriteRenderer + BoxCollider2D + InteractableBehaviour + data)
    │       └── HallwayEntrance (SpriteRenderer + BoxCollider2D + InteractableBehaviour + data)
    └── Hallway
        ├── Visuals/Background (SpriteRenderer)
        ├── Props/Walls (SpriteRenderer, optional)
        └── Interactables
            ├── HallwayObstacle (SpriteRenderer + BoxCollider2D + InteractableBehaviour + data + RealityVisibilityController)
            ├── ShiftTrigger (SpriteRenderer + BoxCollider2D + InteractableBehaviour + data)
            └── VestibuleExit (SpriteRenderer + BoxCollider2D + InteractableBehaviour + data)
```

---

## Every Interactable Needs

- [ ] SpriteRenderer (visual)
- [ ] BoxCollider2D (trigger, IsTriggger = true)
- [ ] InteractableBehaviour script
- [ ] InteractableData (serialized in inspector)
- [ ] RealityVisibilityController (if visibility changes with reality)

---

## Every Room Needs

- [ ] Visuals/Background (SpriteRenderer)
- [ ] Props/ (decorative, non-interactive)
- [ ] Interactables/ (all clickables)

---

## Every Scene Needs

- [ ] **Bootstrap** loads first
- [ ] **Main** loads after
- [ ] Bootstrap.Systems is `DontDestroyOnLoad`
- [ ] GameManager.InitializeMVP() builds rooms
- [ ] RoomManager.LoadRoom("vestibule") starts the game

---

## InteractableData Template

```csharp
id: "unique_name"
displayName: "Human Readable Name"
description: "Flavor text / examination text"
interactionType: Pickup | Use | Examine | Transition | Toggle
visibleInNormalReality: true/false
visibleInOtherReality: true/false
itemId: "item_id" (if Pickup)
requiredItemId: "item_id" (if requires item)
resultFlag: "FlagName" (if sets a flag)
linkedRoomId: "room_id" (if Transition)
```

---

## RoomData Template

```csharp
id: "vestibule" or "hallway"
displayName: "Vestibule" or "Main Hallway"
interactables: [list of InteractableData]
connectedRoomIds: [list of room ids accessible from here]
```

---

## RealityVisibilityController Pseudocode

```csharp
public class RealityVisibilityController : MonoBehaviour
{
    public InteractableData data;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        RealityManager.RealityChanged += OnRealityChanged;
        Refresh();
    }

    void OnRealityChanged(RealityType newReality)
    {
        Refresh();
    }

    void Refresh()
    {
        bool visible = RealityManager.CurrentReality == Normal
            ? data.visibleInNormalReality
            : data.visibleInOtherReality;
        spriteRenderer.enabled = visible;
    }
}
```

---

## InteractableBehaviour Pseudocode

```csharp
public class InteractableBehaviour : MonoBehaviour
{
    public InteractableData data;

    void OnMouseDown()
    {
        InteractionManager.Instance.TryInteract(data.id);
    }
}
```

---

## The Single Most Important Thing

**Every step is driven by flags and data, not by checking object state.**

Bad:
```csharp
if (staffKeyObject.isPickedUp && doorObject.isOpen) { ... }
```

Good:
```csharp
if (GameManager.HasFlag("HasStaffKey") && GameManager.HasFlag("DoorOpened")) { ... }
```

---

## When Stuck

| Error | Check |
|-------|-------|
| "Interactable not found" | Does the id in data match the id you're passing to TryInteract()? |
| Flag not updating | Is the resultFlag field set in InteractableData? |
| Room doesn't load | Does the linkedRoomId exist in GameManager.BuildDefaultRooms()? |
| Visibility not changing | Is RealityVisibilityController attached? Are visibleInNormal/OtherReality set? |
| System resets between rooms | Is DontDestroyOnLoad called? |

---

## Success Criteria (Play Mode Checklist)

- [ ] Game starts, Vestibule loads
- [ ] Click StaffKey, it disappears
- [ ] Click FrontDoor, nothing happens (key required!)
- [ ] Click StaffKey location, nothing happens (already picked up)
- [ ] Click FrontDoor, it opens, sets DoorOpened = true
- [ ] Click HallwayEntrance, loads Hallway
- [ ] See HallwayObstacle in Hallway
- [ ] Click HallwayObstacle, see message in console
- [ ] Click ShiftTrigger, reality changes (visual feedback)
- [ ] HallwayObstacle disappears
- [ ] Click ShiftTrigger again, reality changes back
- [ ] HallwayObstacle reappears
- [ ] Click VestibuleExit, loads Vestibule
- [ ] All state preserved (no resets)
- [ ] Console is clean (no errors)

✓ All 14 checks pass = MVP is complete.

---

## Phase Execution Time

- Phase 1 (Scenes & Hierarchy): 30 min
- Phase 2 (Interactables): 45 min
- Phase 3 (Scripts): 30 min
- Phase 4 (Data): 20 min
- Phase 5 (Validation): 30 min

**Total: ~2.5 hours to working MVP**

---

## Full Details Available In

- **[MVP-Implementation-Quickstart.md](./MVP-Implementation-Quickstart.md)** — step-by-step phases
- **[MVP-Data-Specification.md](./MVP-Data-Specification.md)** — exact field values
- **[MVP-Concrete-Hierarchy.md](./MVP-Concrete-Hierarchy.md)** — exact GameObject positions/properties
- **[MVP-Flag-Sequence.md](./MVP-Flag-Sequence.md)** — complete state machine
