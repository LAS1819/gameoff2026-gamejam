# MVP Concrete Hierarchy

## Overview

This document defines the exact GameObject hierarchy for the MVP scenes. Every object is named, positioned, and configured for immediate implementation in Unity.

---

## Scene: Bootstrap

This scene loads once at startup and persists throughout the game.

```text
Scene: Bootstrap
└── Systems (GameObject)
    ├── GameManager (script: GameManager.cs)
    ├── RoomManager (script: RoomManager.cs)
    ├── InventoryManager (script: InventoryManager.cs)
    ├── InteractionManager (script: InteractionManager.cs)
    ├── RealityManager (script: RealityManager.cs)
    ├── EventManager (script: EventManager.cs, stub for MVP)
    ├── SaveManager (script: SaveManager.cs, stub for MVP)
    └── UIManager (script: UIManager.cs, stub for MVP)
```

### Properties

- **Systems** — parent GameObject, marked with `DontDestroyOnLoad`
  - No Transform changes
  - No visual components
  - Purpose: logical container for runtime managers

- **GameManager**
  - Script: `GameManager.cs`
  - Properties:
    - Calls `InitializeMVP()` on startup
    - Builds `RoomData` for Vestibule and Hallway
    - Loads first room (Vestibule)
    - Stores global flags

- **RoomManager**
  - Script: `RoomManager.cs`
  - Receives room registry from GameManager
  - Tracks current room
  - Refreshes interactable visibility when reality or flags change

- **InventoryManager**
  - Script: `InventoryManager.cs`
  - Tracks items in player inventory
  - Syncs item possession with flags

- **InteractionManager**
  - Script: `InteractionManager.cs`
  - Receives interaction requests from scene objects
  - Resolves Pickup, Use, Transition, Toggle actions
  - Updates flags and broadcasts results

- **RealityManager**
  - Script: `RealityManager.cs`
  - Stores current reality state (Normal or OtherSide)
  - Toggles reality and notifies RoomManager to refresh

- **EventManager** (stub)
  - Script: `EventManager.cs`
  - Empty for MVP; reserved for music/sfx/narrative triggers

- **SaveManager** (stub)
  - Script: `SaveManager.cs`
  - Empty for MVP; reserved for save/load logic

- **UIManager** (stub)
  - Script: `UIManager.cs`
  - Empty for MVP; reserved for text feedback and inventory UI

---

## Scene: Main Gameplay

This scene contains all MVP rooms and interactables. It loads after Bootstrap and persists for the entire game.

```text
Scene: Main
├── Camera (Camera component, static position)
├── Rooms (GameObject, container for all rooms)
│   ├── Vestibule (GameObject, room container)
│   │   ├── Visuals (GameObject)
│   │   │   └── Background (SpriteRenderer, static background image)
│   │   ├── Props (GameObject)
│   │   │   └── DoorFrame (SpriteRenderer, decorative)
│   │   └── Interactables (GameObject, container for clickables)
│   │       ├── StaffKey (GameObject)
│   │       │   ├── SpriteRenderer (sprite of key)
│   │       │   ├── BoxCollider2D (trigger)
│   │       │   ├── InteractableBehaviour (script)
│   │       │   └── InteractableData (serialized)
│   │       ├── FrontDoor (GameObject)
│   │       │   ├── SpriteRenderer (sprite of door)
│   │       │   ├── BoxCollider2D (trigger)
│   │       │   ├── InteractableBehaviour (script)
│   │       │   └── InteractableData (serialized)
│   │       └── HallwayEntrance (GameObject)
│   │           ├── SpriteRenderer (sprite or marker)
│   │           ├── BoxCollider2D (trigger)
│   │           ├── InteractableBehaviour (script)
│   │           └── InteractableData (serialized)
│   └── Hallway (GameObject, room container)
│       ├── Visuals (GameObject)
│       │   └── Background (SpriteRenderer, static background image)
│       ├── Props (GameObject)
│       │   └── Walls (SpriteRenderer, decorative)
│       └── Interactables (GameObject, container for clickables)
│           ├── HallwayObstacle (GameObject)
│           │   ├── SpriteRenderer (sprite of stone figure)
│           │   ├── BoxCollider2D (trigger)
│           │   ├── InteractableBehaviour (script)
│           │   └── InteractableData (serialized)
│           ├── ShiftTrigger (GameObject)
│           │   ├── SpriteRenderer (sprite or invisible marker)
│           │   ├── BoxCollider2D (trigger)
│           │   ├── InteractableBehaviour (script)
│           │   └── InteractableData (serialized)
│           └── VestibuleExit (GameObject)
│               ├── SpriteRenderer (sprite or marker)
│               ├── BoxCollider2D (trigger)
│               ├── InteractableBehaviour (script)
│               └── InteractableData (serialized)
└── FX (GameObject, reserved for visual effects)
```

### Camera Setup

- **Name:** `Camera`
- **Type:** GameObject with Camera component
- **Position:** fixed, centered on Vestibule at game start
- **Orthographic:** true (2D)
- **Size:** choose based on art direction (e.g., 5, 10, etc.)
- **No movement:** camera is static for MVP; room transitions do not animate

### Vestibule Room

#### Structure

```text
Vestibule
├── Visuals
│   └── Background
├── Props
│   └── DoorFrame
└── Interactables
    ├── StaffKey
    ├── FrontDoor
    └── HallwayEntrance
```

#### Visuals/Background

- **Name:** `Background`
- **Type:** SpriteRenderer
- **Sprite:** (placeholder or final art) the vestibule background
- **Sorting Order:** 0 (background layer)
- **Position:** (0, 0, 0)
- **Scale:** (1, 1, 1)
- **Active:** always true

#### Props/DoorFrame

- **Name:** `DoorFrame`
- **Type:** SpriteRenderer
- **Sprite:** (placeholder or final art) door frame or decorative prop
- **Sorting Order:** 1 (mid-layer)
- **Position:** (example: 5, 0, 0)
- **Scale:** (1, 1, 1)
- **Active:** always true
- **Collider:** no collider (purely visual)

#### Interactables/StaffKey

- **Name:** `StaffKey`
- **Type:** GameObject
- **Position:** (example: 3, 2, 0)
- **Components:**
  - **SpriteRenderer**
    - Sprite: key sprite
    - Sorting Order: 2
  - **BoxCollider2D**
    - Is Trigger: true
    - Size: (0.5, 0.5) or fit sprite
  - **InteractableBehaviour** (script)
    - Calls `InteractionManager.TryInteract(interactableId)` on click
  - **InteractableData** (serialized from MVP-Data-Specification.md)
    - id: "staff_key"
    - displayName: "Staff Key"
    - description: "An ornate brass key. It's warm to the touch."
    - interactionType: Pickup
    - visibleInNormalReality: true
    - visibleInOtherReality: false
    - itemId: "staff_key"
    - resultFlag: "HasStaffKey"
    - consumeAfterUse: true

#### Interactables/FrontDoor

- **Name:** `FrontDoor`
- **Type:** GameObject
- **Position:** (example: 5, 0, 0)
- **Components:**
  - **SpriteRenderer**
    - Sprite: door sprite
    - Sorting Order: 2
  - **BoxCollider2D**
    - Is Trigger: true
    - Size: fit sprite (e.g., 2, 4)
  - **InteractableBehaviour** (script)
  - **InteractableData** (serialized)
    - id: "front_door"
    - displayName: "Front Door"
    - description: "A heavy wooden door, locked with a brass keyhole."
    - interactionType: Use
    - visibleInNormalReality: true
    - visibleInOtherReality: true
    - requiredItemId: "staff_key"
    - resultFlag: "DoorOpened"
    - consumeAfterUse: false

#### Interactables/HallwayEntrance

- **Name:** `HallwayEntrance`
- **Type:** GameObject
- **Position:** (example: 7, 0, 0)
- **Components:**
  - **SpriteRenderer** (optional, can be invisible)
    - Sprite: transition marker or invisible sprite
    - Sorting Order: 1
  - **BoxCollider2D**
    - Is Trigger: true
    - Size: (1, 3) or larger, area to trigger transition
  - **InteractableBehaviour** (script)
  - **InteractableData** (serialized)
    - id: "hallway_entrance"
    - displayName: "Hallway"
    - description: "You can see further into the darkness..."
    - interactionType: Transition
    - visibleInNormalReality: true
    - visibleInOtherReality: true
    - requiredItemId: null
    - resultFlag: null
    - linkedRoomId: "hallway"

### Hallway Room

#### Structure

```text
Hallway
├── Visuals
│   └── Background
├── Props
│   └── Walls
└── Interactables
    ├── HallwayObstacle
    ├── ShiftTrigger
    └── VestibuleExit
```

#### Visuals/Background

- **Name:** `Background`
- **Type:** SpriteRenderer
- **Sprite:** (placeholder or final art) the hallway background
- **Sorting Order:** 0
- **Position:** (0, 0, 0)
- **Scale:** (1, 1, 1)
- **Active:** always true

#### Props/Walls

- **Name:** `Walls`
- **Type:** SpriteRenderer
- **Sprite:** (placeholder or final art) walls or corridor visuals
- **Sorting Order:** 1
- **Position:** (example: 0, 0, 0)
- **Scale:** (1, 1, 1)
- **Active:** always true

#### Interactables/HallwayObstacle

- **Name:** `HallwayObstacle`
- **Type:** GameObject
- **Position:** (example: 5, 0, 0)
- **Components:**
  - **SpriteRenderer**
    - Sprite: stone figure or obstacle sprite
    - Sorting Order: 2
  - **BoxCollider2D**
    - Is Trigger: true
    - Size: fit sprite (e.g., 2, 3)
  - **InteractableBehaviour** (script)
  - **RealityVisibilityController** (script, updates SpriteRenderer.enabled based on visibleInNormalReality/visibleInOtherReality)
  - **InteractableData** (serialized)
    - id: "hallway_obstacle"
    - displayName: "Massive Stone Figure"
    - description: "A towering stone figure blocks your path. It's watching you."
    - interactionType: Examine
    - visibleInNormalReality: true
    - visibleInOtherReality: false
    - resultFlag: "RealityUnlocked"
    - consumeAfterUse: false

#### Interactables/ShiftTrigger

- **Name:** `ShiftTrigger`
- **Type:** GameObject
- **Position:** (example: 10, 0, 0)
- **Components:**
  - **SpriteRenderer** (optional, can be invisible or subtle)
    - Sprite: shimmer or invisible sprite
    - Sorting Order: 1
  - **BoxCollider2D**
    - Is Trigger: true
    - Size: (2, 2)
  - **InteractableBehaviour** (script)
  - **InteractableData** (serialized)
    - id: "shift_trigger"
    - displayName: "The Other Side"
    - description: "Reality feels thin here..."
    - interactionType: Toggle
    - visibleInNormalReality: true
    - visibleInOtherReality: true
    - resultFlag: "ShiftPerformed"
    - consumeAfterUse: false

#### Interactables/VestibuleExit

- **Name:** `VestibuleExit`
- **Type:** GameObject
- **Position:** (example: -5, 0, 0)
- **Components:**
  - **SpriteRenderer** (optional, can be invisible)
    - Sprite: marker or invisible sprite
    - Sorting Order: 1
  - **BoxCollider2D**
    - Is Trigger: true
    - Size: (1, 3)
  - **InteractableBehaviour** (script)
  - **InteractableData** (serialized)
    - id: "vestibule_exit"
    - displayName: "Back to Vestibule"
    - description: "The path back."
    - interactionType: Transition
    - visibleInNormalReality: true
    - visibleInOtherReality: true
    - linkedRoomId: "vestibule"

---

## GameObject Naming Convention

- **Room containers:** PascalCase, descriptive (e.g., `Vestibule`, `Hallway`)
- **Logical groups:** PascalCase, descriptive (e.g., `Visuals`, `Props`, `Interactables`, `Systems`)
- **Individual objects:** PascalCase, descriptive (e.g., `StaffKey`, `FrontDoor`, `HallwayObstacle`)
- **All names in English**

---

## Component Attachment Summary

### Every Interactable Must Have

1. **SpriteRenderer** (visual representation)
2. **BoxCollider2D** (interaction trigger, IsTriggger = true)
3. **InteractableBehaviour** (script to call InteractionManager)
4. **InteractableData** (configuration, serialized in inspector)
5. **RealityVisibilityController** (if object visibility depends on reality state)

### Every Room Must Have

1. **Parent GameObject** named for the room (e.g., `Vestibule`)
2. **Visuals** sub-container with background SpriteRenderer
3. **Props** sub-container with decorative, non-interactive objects
4. **Interactables** sub-container with all clickable objects

### Bootstrap Scene Must Have

1. **Systems** parent GameObject
2. One child per manager script (GameManager, RoomManager, etc.)
3. All marked with `DontDestroyOnLoad`

---

## Scene Loading Strategy

1. **Bootstrap** loads first and creates all manager instances
2. **Main Gameplay** loads and contains all rooms
3. **RoomManager.LoadRoom("vestibule")** is called at startup to display the first room
4. **Room transitions** via Transition interactables call `RoomManager.LoadRoom(linkedRoomId)` without scene reload
5. **All rooms persist** in the Main scene; only active room interactables are clickable

---

## Validation Checklist

Before declaring the hierarchy ready:

- [ ] Bootstrap scene exists with Systems parent
- [ ] All 8 managers are children of Systems
- [ ] Systems is marked `DontDestroyOnLoad`
- [ ] Main scene exists with Camera, Rooms, FX
- [ ] Vestibule room container exists
- [ ] Vestibule has Visuals, Props, Interactables subcontainers
- [ ] StaffKey, FrontDoor, HallwayEntrance exist in Vestibule/Interactables
- [ ] Each interactable has SpriteRenderer, BoxCollider2D, InteractableBehaviour, InteractableData
- [ ] Hallway room container exists
- [ ] Hallway has Visuals, Props, Interactables subcontainers
- [ ] HallwayObstacle, ShiftTrigger, VestibuleExit exist in Hallway/Interactables
- [ ] Camera is positioned and configured
- [ ] No physics Rigidbody2D components (all colliders are Is Trigger = true)
- [ ] All interactables have valid InteractableData matching MVP-Data-Specification.md

---

## Related Files

- `MVP-Data-Specification.md` — exact field values for InteractableData
- `MVP-Flag-Sequence.md` — execution flow and when flags are set
- `Assets/Scripts/GameManager.cs` — initializes rooms
- `Assets/Scripts/RoomManager.cs` — loads rooms and refreshes interactables
- `Assets/Scripts/InteractionManager.cs` — resolves interactions
