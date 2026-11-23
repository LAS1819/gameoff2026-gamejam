# Game Off 2026 - Psychological Horror Prototype

`0.1.1-proto` is a complete playable point-and-click horror prototype created for Game Off 2026.

The player explores a liminal building by switching between the Normal reality and the Other Side, recovering memories, and following the truth behind a father's disappearance. The current prototype includes the full route from the Lobby to the Forest, revelation letter, ending evaluation, and restart loop.

## Playable loop

```text
Lobby
  -> Hallway
  -> Bedroom / Library
  -> Basement
  -> Forest
  -> Revelation Letter
  -> Ending
  -> Restart to Lobby
```

The ending depends on the number of optional Memories recovered:

- 0-2 Memories: Bad Ending
- 3-4 Memories: Neutral Ending
- 5 Memories: True Ending

## Prototype highlights

- Fixed-camera point-and-click exploration
- Left click to interact, use, collect, or transition
- Right click to examine without changing game state
- Input System support for mouse and keyboard
- Reality switching between Normal and the Other Side
- Persistent Room and interaction Flags
- Bedroom computer and Floppy Disk progression
- Library secret-shelf and Basement access flow
- Basement pressure, steam, and denial manifestation sequence
- Four-layer Forest depth progression
- Revelation letter and reusable Story Overlay
- Ending summary, Memory count, and restart loop
- Runtime music and centralized HUD

## Controls

| Input | Action |
| --- | --- |
| Left click | Interact, use an item, collect, or transition |
| Right click | Examine |
| SHIFT / HUD button | Switch reality in supported rooms |
| Escape / Enter / Space | Advance or close the current story overlay |
| Restart game | Reset runtime progress and return to the Lobby |

## Unity setup

- Unity `6000.6.9f1`
- Universal Render Pipeline
- Unity Input System
- UI Toolkit through `PanelRenderer`

### Open the project

1. Open this repository root with Unity Hub.
2. Use Unity `6000.6.9f1`.
3. Open `Assets/Scenes/Main.unity`.
4. Enter Play Mode.

The prototype is contained in a single gameplay scene for the current vertical slice.

## Repository structure

```text
Assets/             Runtime scripts, scenes, visuals, audio, UI, and project content
Docs/               Game design, technical design, sprint records, and changelog
Packages/           Unity package configuration
ProjectSettings/    Unity project settings
```

## Documentation

Start with:

- [Documentation index](Docs/README.md)
- [Game Design Document](Docs/01-Game-Design/Game-GDD.md)
- [Current MVP overview](Docs/01-Game-Design/MVP-Overview.md)
- [Puzzle flow](Docs/01-Game-Design/Gameplay/Puzzle-Flow.md)
- [MVP specification](Docs/02-Technical-Design/MVP-SPECIFICATION.md)
- [Sprint 3 closeout](Docs/02-Technical-Design/MVP-Sprint-3.md)
- [Sprint 4 final polish and delivery](Docs/02-Technical-Design/MVP-Sprint-4.md)
- [Changelog](Docs/03-Production/CHANGELOG.md)
- [WebGL and Windows build guide](Docs/03-Production/BUILD-0.1.1-PROTO-WEBGL-WINDOWS.md)
- [itch.io delivery guide](Docs/03-Production/ITCH-IO-DELIVERY-0.1.1-PROTO.md)

Room-specific references:

- [Forest game design](Docs/01-Game-Design/Gameplay/Rooms/GD-Room-Forest.md)
- [Forest technical design](Docs/02-Technical-Design/Gameplay/Rooms/TD-Room-Forest.md)

## Current scope

This is a game jam prototype, not a final release.

The current `0.1.1-proto` candidate already contains the complete loop. The remaining work is delivery-oriented:

- final letter and ending copy
- final Story Overlay / antique-letter artwork
- final credits presentation
- optional Spanish localization if the delivery schedule remains safe
- additional SFX, visual polish, and accessibility validation
- validation of the candidate WebGL / Windows builds
- private itch.io page testing before public submission

## Code health

Runtime scripts currently follow the project Unity C# guidelines:

- Input System only
- explicit null handling
- no null-propagation operators
- no deprecated `FindObjectOfType` / `FindObjectsOfType` usage
- configuration/data reuse preferred over room-specific duplication

## Public repository note

This repository is public for the Game Off 2026 jam. It may contain work-in-progress assets, prototype copy, design notes, and historical sprint documents. Those artifacts are intentional and help document the development of the vertical slice.

## License

No project-wide source or asset license has been declared yet. Third-party packages and assets retain their original licenses.
