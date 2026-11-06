# GameObject Hierarchy

This document defines the expected hierarchy of objects in each scene. It reflects the actual MVP architecture: **point-and-click horror, no player movement, data-driven design.**

---

## 🎯 Core Principle

**No Player avatar. No movement system. Everything is click-based interactables and room transitions.**

---

## Bootstrap Scene Hierarchy

This scene loads once at startup and persists forever (`DontDestroyOnLoad`).

```text
Bootstrap
└── Systems
    ├── GameManager
    ├── RoomManager
    ├── InventoryManager
    ├── InteractionManager
    ├── RealityManager
    ├── EventManager
    ├── SaveManager
    └── UIManager
```

### Bootstrap Managers

| Manager | Responsibility |
| --------- | ----------------- |
| **GameManager** | Global flags, room state, initialization, DontDestroyOnLoad |
| **RoomManager** | Room registry, room loading, interactable visibility refresh |
| **InventoryManager** | Item tracking, possession validation, item ownership sync with flags |
| **InteractionManager** | Interaction resolution, click handling, progression logic |
| **RealityManager** | Current reality state (Normal/OtherSide), visibility toggle, propagation |
| **EventManager** | (Stub for MVP) Reserved for events, music triggers, narrative |
| **SaveManager** | (Stub for MVP) Reserved for save/load logic |
| **UIManager** | (Stub for MVP) Reserved for text feedback, inventory UI |

---

## Main Scene Hierarchy

This scene contains all gameplay. Rooms persist in this scene; room transitions are logical (not scene loads).

```text
Main
├── Camera
│   └── (orthographic, static position, centered on Lobby at start)
│
├── UI
│   └── ShiftPanel
│       ├── PanelRenderer (PanelSettings + ShiftUI.uxml)
│       └── UIShiftButton (binds "shift-button" to RealityManager.ToggleReality)
│
└── Rooms
    ├── Lobby
    │   ├── Visuals
    │   │   └── Background (SpriteRenderer, Sorting Order 0)
    │   ├── Props
    │   │   └── DoorFrame (SpriteRenderer, Sorting Order 1, optional)
    │   └── Interactables
    │       ├── StaffKey
    │       ├── FrontDoor
    │       └── MainHallwayEntrance
    │
    └── MainHallway
        ├── Visuals
        │   └── Background (SpriteRenderer, Sorting Order 0)
        ├── Props
        │   └── Walls (SpriteRenderer, Sorting Order 1, optional)
        └── Interactables
            ├── HallwayObstacle
            ├── ShiftTrigger
            └── MainHallwayExit
```

### Runtime UI (UI Toolkit + Panel Renderer)

- SHIFT icon/button is implemented with **UI Toolkit runtime** via **PanelRenderer**.
- `ShiftUI.uxml` must contain a button with `name="shift-button"`.
- `UIShiftButton` script binds click callback during panel reload and calls `RealityManager.Instance.ToggleReality()`.
- Avoid mixing with legacy Canvas/Button setup for SHIFT to prevent duplicated or conflicting configuration.

---

## Room Structure Pattern

Every room follows the same structure:

```text
RoomName (root container)
├── Visuals (visual layer)
│   └── Background (SpriteRenderer)
├── Props (decorative, non-interactive)
│   └── [any decorative objects]
└── Interactables (clickable objects)
    └── [6 interactables total across 2 rooms]
```

### Visuals Folder

Contains all static visual elements:

- **Background** (SpriteRenderer, Sorting Order 0)
  - The room background image
  - Always visible

### Props Folder

Contains decorative, non-interactive elements:

- Examples: door frame, walls, corridor details
- All have SpriteRenderer (Sorting Order 1 typically)
- No Collider2D (they are purely visual)
- Not clickable

### Interactables Folder

Contains all clickable objects. Each interactable is a separate GameObject with:

- **SpriteRenderer** (Sorting Order 2 typically)
- **BoxCollider2D** (IsTrigger = true)
- **InteractableBehaviour** script
- **InteractableData** (serialized in Inspector)
- Visibility handled by **InteractableBehaviour + RoomManager**

---

## Lobby Room Details

```text
Lobby
├── Visuals
│   └── Background
│       • Sprite: Lobby background image
│       • Sorting Order: 0
│       • Position: (0, 0, 0)
│
├── Props (optional)
│   └── DoorFrame
│       • Sprite: door frame decorative image
│       • Sorting Order: 1
│       • Position: (5, 0, 0)
│       • No Collider2D
│
└── Interactables
    │
    ├── StaffKey
    │   • Position: (3, 2, 0)
    │   • SpriteRenderer: key sprite, Sorting Order 2
    │   • BoxCollider2D: IsTrigger = true, Size (0.5, 0.5)
    │   • InteractableBehaviour: calls TryInteract("staff_key")
    │   • InteractableData:
    │       - id: "staff_key"
    │       - type: Pickup
    │       - itemId: "staff_key"
    │       - resultFlag: "HasStaffKey"
    │       - visibleNormal: true
    │       - visibleOther: false
    │
    ├── FrontDoor
    │   • Position: (5, 0, 0)
    │   • SpriteRenderer: door sprite, Sorting Order 2
    │   • BoxCollider2D: IsTrigger = true, Size (2, 4)
    │   • InteractableBehaviour: calls TryInteract("front_door")
    │   • InteractableData:
    │       - id: "front_door"
    │       - type: Use
    │       - requiredItemId: "staff_key"
    │       - resultFlag: "DoorOpened"
    │       - visibleNormal: true
    │       - visibleOther: true
    │
    └── HallwayEntrance
        • Position: (7, 0, 0)
        • SpriteRenderer: marker sprite (optional, can be invisible)
        • BoxCollider2D: IsTrigger = true, Size (1, 3)
        • InteractableBehaviour: calls TryInteract("hallway_entrance")
        • InteractableData:
            - id: "hallway_entrance"
            - type: Transition
            - linkedRoomId: "hallway"
            - visibleNormal: true
            - visibleOther: true
```

---

## Hallway Room Details

```text
Hallway
├── Visuals
│   └── Background
│       • Sprite: hallway background image
│       • Sorting Order: 0
│       • Position: (0, 0, 0)
│
├── Props (optional)
│   └── Walls
│       • Sprite: corridor/walls decorative image
│       • Sorting Order: 1
│       • Position: (0, 0, 0)
│       • No Collider2D
│
└── Interactables
    │
    ├── HallwayObstacle
    │   • Position: (5, 0, 0)
    │   • SpriteRenderer: stone figure sprite, Sorting Order 2
    │   • BoxCollider2D: IsTrigger = true, Size (2, 3)
    │   • InteractableBehaviour: calls TryInteract("hallway_obstacle")
    │   • Visibility auto-updated by InteractableBehaviour using RoomManager rules
    │   • InteractableData:
    │       - id: "hallway_obstacle"
    │       - type: Examine
    │       - resultFlag: "RealityUnlocked"
    │       - visibleNormal: true ← KEY: only visible in Normal
    │       - visibleOther: false ← KEY: hidden in OtherSide
    │
    ├── ShiftTrigger
    │   • Position: (10, 0, 0)
    │   • SpriteRenderer: shimmer sprite (optional, can be invisible)
    │   • BoxCollider2D: IsTrigger = true, Size (2, 2)
    │   • InteractableBehaviour: calls TryInteract("shift_trigger")
    │   • InteractableData:
    │       - id: "shift_trigger"
    │       - type: Toggle
    │       - resultFlag: "ShiftPerformed"
    │       - visibleNormal: true
    │       - visibleOther: true
    │
    └── LobbyExit
        • Position: (-5, 0, 0)
        • SpriteRenderer: marker sprite (optional, can be invisible)
        • BoxCollider2D: IsTrigger = true, Size (1, 3)
        • InteractableBehaviour: calls TryInteract("lobby_exit")
        • InteractableData:
            - id: "lobby_exit"
            - type: Transition
            - linkedRoomId: "lobby"
            - visibleNormal: true
            - visibleOther: true
```

---

## Component Pattern for Every Interactable

Every interactable object must have this exact pattern:

```csharp
GameObject (named for the interactable)
├── SpriteRenderer
│   ├── Sprite: (the visual)
│   ├── Sorting Order: 2 (above background and props)
│   └── Position: [as specified in room details]
│
├── BoxCollider2D
│   ├── IsTrigger: true
│   ├── Size: [as specified]
│   └── (no physics body, pure trigger)
│
├── InteractableBehaviour (script)
│   ├── Calls InteractionManager.TryInteract(data) on OnMouseDown
│
├── InteractableData (serialized in Inspector)
│   ├── id: [as specified]
│   ├── displayName: [human-readable]
│   ├── description: [examination text]
│   ├── interactionType: [Pickup, Use, Examine, or Transition]
│   ├── visibleInNormalReality: [true/false]
│   ├── visibleInOtherReality: [true/false]
│   ├── itemId: [if Pickup]
│   ├── requiredItemId: [if Use]
│   ├── resultFlag: [if sets a flag]
│   └── linkedRoomId: [if Transition]
│
└── Visibility logic is centralized in RoomManager.IsInteractableVisible
```

---

## Camera Setup

The Camera should be configured once and remain static for the MVP:

```text
Camera
├── Camera component
│   ├── Orthographic: true (2D game)
│   ├── Size: 5 to 10 (depending on art scale)
│   ├── Position: (0, 0, -10) or suitable for your art
│   └── No animation, no following, no movement
```

The camera is **static** for the entire game. It does not follow the player (no player exists) and does not animate on room transitions.

---

## What's NOT in the Hierarchy

### ❌ No Player

- No `Player` GameObject
- No `PlayerController` script
- No `Rigidbody2D` on the player
- No character animation system
- No movement input handling

**Why?** This is point-and-click, not exploration. The protagonist is never visible.

### ❌ No Duplicated Reality Hierarchies

```text
❌ WRONG:
RealityState
├── NormalRealityObjects
└── OtherSideRealityObjects
```

```text
✅ CORRECT:
├── HallwayObstacle (has properties: visibleNormal: true, visibleOther: false)
└── RealityManager updates visibility at runtime
```

**Why?** Simpler, more maintainable, scales better. One object, two visibility states.

### ❌ No Physics Objects

- No `Rigidbody2D` (all colliders are triggers)
- No gravity
- No forces
- No platforming

**Why?** Pure interaction-based gameplay. Physics unnecessary.

### ❌ No Animation Rigs

- No `Animator` for character movement
- No skeleton/bones
- No walking animations

**Why?** No moving character. Only state changes (door opens, obstacle disappears).

---

## Scene Transitions and Room Changes

**Rooms persist in the Main scene.** Room transitions are **logical**, not scene loads:

1. Player clicks `HallwayEntrance` (Transition interactable)
2. `InteractionManager.TryInteract("hallway_entrance")`
3. `RoomManager.LoadRoom("hallway")`
4. Lobby interactables are hidden/disabled
5. Hallway interactables are shown/enabled
6. Current room is now "hallway"

**No scene load. No Bootstrap reload. State persists.**

---

## Best Practices

1. **Keep one root container per room** for organization
2. **Name objects in English** using clear, descriptive names
3. **Keep all game logic in data** (InteractableData, RoomData)
4. **Do not hardcode scene-specific rules** into manager scripts
5. **Use visibility toggles** via RealityManager for reality changes
6. **All interactables follow the same pattern** (SpriteRenderer + BoxCollider2D + InteractableBehaviour + InteractableData)
7. **Managers are persistent** and decoupled from scene structure

---

## Summary

- **Bootstrap:** 8 persistent managers (DontDestroyOnLoad)
- **Main:** 2 rooms × 3 interactables each = 6 clickables total
- **No Player, no movement, no physics, no animation rigs**
- **Data-driven:** All logic lives in RoomData and InteractableData
- **Point-and-click:** Pure interaction-based gameplay

Everything else is reference or legacy. Start with this.
