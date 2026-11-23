# Game Off 2026 - Prototype 0.1.1

- Version: 0.1.1-proto
- Updated: 2026-11-23
- Status: complete playable vertical slice
- Current sprint: 4 - final polish, QA, and delivery

## Objective

Deliver a short, coherent point-and-click horror experience before final content and presentation polish.

The prototype now validates the complete game loop, not only the initial SHIFT foundation.

## Playable content

### Rooms

- Lobby
- Hallway
- Bedroom
- Library
- Basement
- Forest

### Main interactions

- key-based door progression
- reality switching and conditional visibility
- Bedroom wardrobe, monster, Lantern, and computer flow
- Library Worn Book / Floppy Disk / secret shelf sequence
- Basement evidence, Valve Handle, pressure release, steam, and denial manifestation
- Forest depth advance and final black-room state
- Revelation Letter and Memory-dependent ending
- ending summary and restart loop

## Player inputs

- Left click: interact / use / collect / transition
- Right click: examine
- SHIFT or HUD button: change reality in supported rooms
- Continue / Restart controls in the Story Overlay

## Ending structure

Memories are optional, hidden progression Flags:

- 0-2 Memories: Bad Ending
- 3-4 Memories: Neutral Ending
- 5 Memories: True Ending

## Complete route

```text
Lobby -> Hallway -> Bedroom / Library -> Basement -> Forest
      -> Letter -> Ending summary -> Lobby
```

The Forest cannot return to the Basement. It is the terminal narrative route.

## Still deferred

- final copy and translations
- final letter artwork
- dedicated Credits scene
- additional SFX
- visual polish and accessibility checks
- final WebGL and Windows build validation
- private itch.io draft validation before public release

The full final-sprint plan is in
[MVP-Sprint-4.md](../02-Technical-Design/MVP-Sprint-4.md).

## Definition of prototype done

- all rooms are reachable from the main progression
- no blocking interaction gap remains
- the final letter can be read
- the three ending conditions are reachable through Memory flags
- the ending can return cleanly to the Lobby
- the build compiles without runtime script errors
