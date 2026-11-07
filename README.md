# Game Off 2026 - Game Jam Project

This repository contains the prototype for the Game Off 2026 game jam entry.

## Project goal

The MVP focuses on validating the core loop of the game:

- enter the lobby
- collect the staff key
- unlock the door
- enter the hallway
- activate SHIFT
- change reality
- remove the obstacle and continue the route

The implementation prioritizes a strong technical foundation and a playable vertical slice over art polish or full content scope.

## Repository structure

```text
Assets/             Unity project assets and scripts
Docs/               project documentation, split by type
ProjectSettings/     Unity project settings
Packages/            Unity package configuration
UserSettings/        local editor settings
```

## Documentation

The project documentation is organized by type to keep it readable and maintainable.

- `Docs/README.md` — recommended structure and naming convention
- `Docs/01-Game-Design/` — GDD, puzzle flow, visual references and MVP summary
- `Docs/02-Technical-Design/` — technical architecture, sprint plan and scene/object hierarchy

## MVP status

The current implementation is a technical foundation for the first sprint:

- shared data models
- global game state with flags
- room management
- inventory and interaction flow
- reality switching support for normal / alternate reality
- runtime HUD pass based on Panel Renderer for the MVP polish layer

## Unity setup

1. Open the project in Unity.
2. Load the main gameplay scene.
3. Validate the progression in Play Mode.
4. Keep the scope limited to the MVP loop before expanding content.

## Public repository note

This project is public because it is required by the game jam, so documentation and setup instructions should remain clear and reusable for external reviewers.
