# MVP Specification - Start here

- Version: 0.1.6-proto
- Status: Complete playable prototype for test builds
- Sprint: 4 - final polish, validation, and delivery
- Last updated: 2026-11-27
- Primary scene: `Assets/Scenes/Main.unity`

## Prototype objective

Provide one complete, testable game loop from the Lobby to a Memory-driven ending, then return to the Lobby.

```text
Lobby
  -> Hallway
  -> Bedroom / Library
  -> Basement
  -> Forest
  -> Revelation Letter
  -> Ending
  -> Restart
```

## Playable systems

### Room System

Rooms are registered through `DefaultRoomCatalog` and loaded by `RoomManager`.

Current rooms:

- Lobby
- Hallway
- Bedroom
- Library
- Basement
- Forest

### Interaction System

- left click: interaction, use, pickup, or transition
- right click: examination only
- selected inventory items are validated through flags and interaction requirements
- interaction outcomes refresh conditional room state

### Reality System

SHIFT switches Normal / Other Side state in supported rooms. Room interactables, backgrounds, descriptions, and selected props react to the active reality.

Forest is a terminal room and does not expose a return transition.

### Inventory System

The prototype inventory contains progression items such as the Staff Key, Library Key, Worn Book, Floppy Disk, Basement Code, and Valve Handle.

Inventory presentation remains intentionally small and lightweight.

### Flag System

All major progression and conditional presentation use `GameManager` flags.

This includes:

- room access
- reality progression
- item discovery
- Basement pressure resolution
- Forest depth/final state
- recovered Memories
- ending state

### HUD and ending flow

The HUD uses UI Toolkit through `PanelRenderer`, UXML, and USS.

Implemented states:

- inventory slots
- feedback text
- SHIFT button
- Lantern indicator
- room and reality transition overlays
- reusable Story Overlay for the letter and ending
- Memory discovery overlay with five pixel-art sprites
- configurable `memoryDiscoveryDuration` and manual Continue control
- three ending variants
- recovered Memories summary
- restart to Lobby

## Forest terminal flow

```text
Level 1
  -> Level 2-5
    -> Level 6 final
      -> Table + Ceiling Light + Revelation Letter
        -> Letter Story Overlay
          -> Ending Story Overlay
            -> Restart / Lobby
```

## Ending rules

| Recovered Memories | Ending |
| --- | --- |
| 0-2 | Bad Ending |
| 3-4 | Neutral Ending |
| 5 | True Ending |

## Validation criteria

The prototype is accepted when:

- the complete room route is playable
- left/right click behavior is consistent
- SHIFT gating works in supported rooms
- Basement puzzle states remain synchronized
- Forest reaches the depth 6 final state
- letter, ending, and Memory summary render correctly
- Story Overlay controls remain inside the viewport
- Memory_01 works in Lobby Normal Reality
- Memory_02-05 work in the intended Other Side rooms
- Memory discovery left/right-click behavior is consistent
- Memory overlay displays artwork, text, Continue, and automatic timeout
- Block 5 Lantern setup is stable if included: possession and active state reset cleanly
- Block 5 key Basement interactions are blocked in the dark without consuming items or flags
- Block 5 global profiles do not reduce readability at `640 x 360`
- direct Forest start begins at level 1 with visible ambulance lights
- Forest levels 1-5 change frames, light position and global intensity consistently
- level 6 activates Table, ceiling light and Revelation Letter
- no cursor-follow light is required for the current release
- Restart resets flags, inventory, Forest depth, and active room
- Unity compiles without errors or warnings

## Sprint 4 work

- final narrative copy and letter art
- dedicated Credits scene
- final credits presentation
- optional English / Spanish localization if it does not endanger delivery
- final SFX and audio polish
- final accessibility pass
- WebGL and Windows delivery build validation
- itch.io private-draft and publication checks
- shared HUD, font, inventory, cursor, and Memory presentation polish
- room-by-room visual sweep
- Lantern and room/reality global-light baseline implemented in Sprint 4 Block 5; cursor-follow light deferred

## Technical rules

- use the Unity Input System
- avoid null-propagation operators
- avoid obsolete find APIs and legacy input
- keep room data centralized in `DefaultRoomCatalog`
- prefer existing systems over new managers
- preserve explicit and testable state transitions
- do not add gameplay systems unless they pass the Sprint 4 P2 scope gate
