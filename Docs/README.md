# Documentation index

The documentation is organized by purpose. Public-facing status always takes priority over older planning text when two documents appear to disagree.

## Current project state

- Prototype version: `0.1.3-proto`
- Unity: `6000.6.9f1`
- Status: complete playable vertical slice with interaction and feedback polish
- Current sprint: Sprint 4, final polish, validation, and delivery

## Structure

```text
Docs/
  README.md
  01-Game-Design/
    Game-GDD.md
    MVP-Overview.md
    Story/
    Gameplay/
    Visual/
    Audio/
  02-Technical-Design/
    README.md
    MVP-SPECIFICATION.md
    MVP-Sprint-1.md
    MVP-Sprint-2.md
    MVP-Sprint-3.md
    MVP-Sprint-4.md
    Technical-Design.md
    GameObject-Hierarchy.md
    Gameplay/
    UI/
    Audio/
  03-Production/
    CHANGELOG.md
    BUILD-0.1.1-PROTO-WEBGL-WINDOWS.md
    ITCH-IO-DELIVERY-0.1.1-PROTO.md
```

## Recommended reading

1. [01-Game-Design/MVP-Overview.md](./01-Game-Design/MVP-Overview.md) - current playable scope
2. [01-Game-Design/Gameplay/Puzzle-Flow.md](./01-Game-Design/Gameplay/Puzzle-Flow.md) - complete room progression
3. [02-Technical-Design/MVP-SPECIFICATION.md](./02-Technical-Design/MVP-SPECIFICATION.md) - current technical MVP
4. [02-Technical-Design/MVP-Sprint-3.md](./02-Technical-Design/MVP-Sprint-3.md) - Sprint 3 closeout
5. [02-Technical-Design/MVP-Sprint-4.md](./02-Technical-Design/MVP-Sprint-4.md) - active final sprint
6. [03-Production/CHANGELOG.md](./03-Production/CHANGELOG.md) - released prototype milestones
7. [03-Production/BUILD-0.1.1-PROTO-WEBGL-WINDOWS.md](./03-Production/BUILD-0.1.1-PROTO-WEBGL-WINDOWS.md) - WebGL and Windows build settings
8. [03-Production/ITCH-IO-DELIVERY-0.1.1-PROTO.md](./03-Production/ITCH-IO-DELIVERY-0.1.1-PROTO.md) - itch.io copy and publication checklist

## Design

Use `01-Game-Design/` for:

- narrative intent and lore
- room design
- visual and audio direction
- gameplay flow
- Memory and ending rules

## Technical design

Use `02-Technical-Design/` for:

- runtime systems and room contracts
- scene hierarchy and interactables
- HUD and input behavior
- sprint implementation records
- validation and implementation constraints

## Production

Use `03-Production/` for:

- release notes and changelog entries
- delivery milestones
- future production summaries
- build and release checklists per platform
- itch.io delivery copy and publication checklist

## Document status policy

- Current documents describe the behavior of `0.1.3-proto`.
- Sprint documents are implementation records and may contain historical decisions.
- Deferred items are explicit; they must not be mistaken for unfinished blockers.
- New design should update the existing relevant document before creating another one.
