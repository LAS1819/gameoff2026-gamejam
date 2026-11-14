# Game Off 2026 — Horror Game Jam Prototype

This repository contains the prototype for a short narrative horror experience built for the Game Off 2026 jam.

## Project aim

The project focuses on validating a clean, playable MVP loop before expanding content:

- enter the Lobby
- collect the Staff Key
- unlock the exit / route forward
- move through the Hallway
- use SHIFT to change reality
- progress through the Bedroom logic
- continue into the Library and beyond as the system expands

The priority is a solid technical foundation and a playable vertical slice over art polish or broad content scope.

## Current status

The game is in active MVP development with the core systems validated:

- room loading and transitions
- inventory and interaction flow
- reality switching between normal / alternate reality
- room flags and game state management
- click-based interaction contract for left / right mouse actions
- documentation and sprint planning aligned to MVP scope

## Repository structure

```text
Assets/             Unity project files and scripts
Docs/               design, technical documentation and sprint plans
ProjectSettings/     Unity project settings
Packages/            Unity package configuration
UserSettings/        local editor settings
```

## Documentation map

- [Docs/README.md](Docs/README.md) — documentation structure and naming conventions
- [Docs/01-Game-Design](Docs/01-Game-Design) — GDD, gameplay flow and room design
- [Docs/02-Technical-Design](Docs/02-Technical-Design) — technical architecture, MVP specification and sprint docs
- [Docs/03-Production](Docs/03-Production) — production notes and changelog

Key technical docs:

- [Docs/02-Technical-Design/MVP-SPECIFICATION.md](Docs/02-Technical-Design/MVP-SPECIFICATION.md)
- [Docs/02-Technical-Design/MVP-Sprint-1.md](Docs/02-Technical-Design/MVP-Sprint-1.md)
- [Docs/02-Technical-Design/MVP-Sprint-2.md](Docs/02-Technical-Design/MVP-Sprint-2.md)
- [Docs/02-Technical-Design/MVP-Sprint-3.md](Docs/02-Technical-Design/MVP-Sprint-3.md)

## Unity setup

1. Open the project in Unity.
2. Load the primary gameplay scene.
3. Validate the flow in Play Mode.
4. Keep scope tight and avoid expanding systems before the MVP loop is stable.

## Public repository note

This repository is public as required by the game jam. The documentation aims to stay clear, structured and understandable for external reviewers while preserving a clean internal workflow.
