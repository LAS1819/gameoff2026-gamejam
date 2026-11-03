# Game Object Hierarchy

This document defines the expected hierarchy of objects in each scene and clarifies the responsibility of each GameObject and its internal components.

## General hierarchy

Every scene should follow the same base structure so the runtime logic remains predictable.

```text
Scene Root
├── Bootstrap
│   ├── GameManager
│   ├── RoomManager
│   ├── InventoryManager
│   ├── InteractionManager
│   ├── RealityManager
│   └── UIManager
├── Player
│   ├── PlayerController
│   ├── InteractionTrigger
│   └── Animator
├── Environment
│   ├── RoomVisuals
│   ├── Props
│   └── StaticColliders
├── Interactables
│   ├── StaffKey
│   ├── FrontDoor
│   ├── HallwayObstacle
│   └── RoomTransition
└── RealityState
    ├── NormalRealityObjects
    └── OtherSideRealityObjects
```

## Responsibilities by object

### `Bootstrap`

The scene bootstrap contains the runtime managers that must survive scene transitions.

- `GameManager` — global flags, game state, room id and progression state.
- `RoomManager` — room registry, current room and interactive refresh.
- `InventoryManager` — item tracking and validation.
- `InteractionManager` — interaction resolution and item consumption.
- `RealityManager` — normal / alternate reality switching.
- `UIManager` — text feedback, inventory view and room notifications.

### `Player`

The player object owns movement, interaction and state awareness.

Expected components:

- `PlayerController`
- `InputReader`
- `InteractionTrigger`
- `Collider2D` or equivalent
- `Rigidbody2D` if needed
- `Animator` (optional for prototype)

Responsibilities:

- move inside the room
- trigger interactions
- keep gameplay state in sync with room transitions

### `Environment`

This container holds world elements that are not interactive by themselves.

Typical objects:

- background layers
- floor and walls
- decorative props
- room signage
- transition markers

Responsibilities:

- provide scene context
- separate static geometry from dynamic gameplay objects
- allow the reality system to show or hide alternate visual layers

### `Interactables`

Each interactive object should be separated as an individual GameObject under a common root.

#### `StaffKey`

- `InteractionType: Pickup`
- `itemId: staff_key`
- `resultFlag: HasStaffKey`
- visible in the normal reality only
- removed from the scene once picked up

#### `FrontDoor`

- `InteractionType: Use`
- `requiredItemId: staff_key`
- `resultFlag: DoorOpened`
- can only be used after the key is in the inventory
- opens access to the hallway room

#### `HallwayObstacle`

- `InteractionType: Use`
- tied to the `RealityUnlocked` / `SHIFT` mechanic
- visible in the normal reality
- hidden in the alternate reality
- acts as the visual checkpoint for the first hallway obstacle

#### `RoomTransition`

- `InteractionType: Transition` or `EnterRoom`
- links the current room to the next one
- updates the current room id and refreshes active interactables

## Recommended components for interactables

Each interactive object should have the same core pattern:

- `Collider2D`
- `SpriteRenderer`
- `InteractableBehaviour` (or equivalent runtime script)
- `InteractableData` reference
- optional `RealityVisibilityController`

This keeps the scene data-driven and reduces hardcoded logic per object.

## Scene examples

### Vestibule

```text
Vestibule Root
├── DoorFrame
├── StaffKey
├── FrontDoor
├── ExitMarker
└── RoomTransitionArea
```

Responsibilities:

- key pickup
- door unlock sequence
- access to next room

### Main Hallway

```text
Hallway Root
├── HallwayObstacle
├── ShiftTrigger
├── AlternateRealityVisuals
├── CorridorBackground
└── ExitRoute
```

Responsibilities:

- room transition validation
- reality switch effect
- obstacle removal / visibility toggle
- forward progression checkpoint

## Best practices

- Keep one root container per room for organization.
- Name objects in English and use clear purpose-based names.
- Keep all gameplay data in `InteractableData` / `RoomData` instead of embedding logic directly in the scene.
- Do not hardcode scene-specific rules into the runtime managers.
- Use visibility toggles through the reality system instead of duplicating logic across objects.

## Related technical files

- `Technical-Design.md`
- `MVP-Sprint-1.md`
- `Assets/Scripts/GameManager.cs`
- `Assets/Scripts/RoomManager.cs`
- `Assets/Scripts/InteractionManager.cs`
- `Assets/Scripts/RealityManager.cs`
