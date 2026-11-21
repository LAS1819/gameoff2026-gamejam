# Documentation index

The documentation is organized by purpose. Public-facing status always takes priority over older planning text when two documents appear to disagree.

## Current project state

- Prototype version: `0.1.0-proto`
- Unity: `6000.6.9f1`
- Status: complete playable MVP loop
- Current sprint: Sprint 3 closed; Sprint 4 polish and delivery preparation

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
    Technical-Design.md
    GameObject-Hierarchy.md
    Gameplay/
    UI/
    Audio/
  03-Production/
    CHANGELOG.md
```

## Recommended reading

1. [01-Game-Design/MVP-Overview.md](./01-Game-Design/MVP-Overview.md) - current playable scope
2. [01-Game-Design/Gameplay/Puzzle-Flow.md](./01-Game-Design/Gameplay/Puzzle-Flow.md) - complete room progression
3. [02-Technical-Design/MVP-SPECIFICATION.md](./02-Technical-Design/MVP-SPECIFICATION.md) - current technical MVP
4. [02-Technical-Design/MVP-Sprint-3.md](./02-Technical-Design/MVP-Sprint-3.md) - Sprint 3 closeout
5. [03-Production/CHANGELOG.md](./03-Production/CHANGELOG.md) - released prototype milestones

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

## Document status policy

- Current documents describe the behavior of `0.1.0-proto`.
- Sprint documents are implementation records and may contain historical decisions.
- Deferred items are explicit; they must not be mistaken for unfinished blockers.
- New design should update the existing relevant document before creating another one.
