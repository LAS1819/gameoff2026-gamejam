# Documentation structure

This folder is organized by content type so the project stays easy to navigate across design, implementation and production work.

## Current structure

```text
Docs/
  README.md
  01-Game-Design/
    Game-GDD.md
    MVP-Overview.md
    Puzzle-Flow.md
    Story/
    Gameplay/
    Visual/
  02-Technical-Design/
    README.md
    MVP-SPECIFICATION.md
    MVP-Sprint-1.md
    MVP-Sprint-2.md
    MVP-Sprint-3.md
    Technical-Design.md
    GameObject-Hierarchy.md
    Gameplay/
    UI/
  03-Production/
    CHANGELOG.md
```

## Recommended reading flow

For the active MVP, the practical order is:

1. [02-Technical-Design/MVP-SPECIFICATION.md](./02-Technical-Design/MVP-SPECIFICATION.md) — core spec and execution flow
2. [02-Technical-Design/MVP-Sprint-1.md](./02-Technical-Design/MVP-Sprint-1.md) — base playable loop
3. [02-Technical-Design/MVP-Sprint-2.md](./02-Technical-Design/MVP-Sprint-2.md) — Bedroom and room-flow closure
4. [02-Technical-Design/MVP-Sprint-3.md](./02-Technical-Design/MVP-Sprint-3.md) — Library and Basement preparation
5. [01-Game-Design/Gameplay/Puzzle-Flow.md](./01-Game-Design/Gameplay/Puzzle-Flow.md) — gameplay sequencing and content intent

## Purpose of each folder

### `01-Game-Design/`

Design direction, narrative, puzzle flow, room intent and visual references.

### `02-Technical-Design/`

Architecture, sprint planning, room/data structure, interaction flow and implementation constraints.

### `03-Production/`

Changelog, delivery notes and project progress tracking.

## Working rule

If a document describes gameplay behaviour, it belongs under `01-Game-Design` or `02-Technical-Design` depending on whether it is design or technical logic. If it is a project workflow or release note, it belongs under `03-Production`.
