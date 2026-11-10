# Documentation structure

This folder is organized by document type so the project stays easy to navigate during the sprint.

## Recommended structure

```text
Docs/
  README.md
  00-Project-Overview/
    Project-Overview.md
    Team-Notes.md
  01-Game-Design/
    Game-GDD.md
    MVP-Overview.md
    Puzzle-Flow.md
    Room-Lobby.md
    Room-Hallway.md
    Visual-Guide.md
  02-Technical-Design/
    README.md (← MVP documentation index)
    Technical-Design.md
    MVP-Sprint-1.md
    MVP-Quick-Reference.md (← print this!)
    MVP-Implementation-Quickstart.md (← start here)
    MVP-Data-Specification.md
    MVP-Concrete-Hierarchy.md
    MVP-Flag-Sequence.md
    GameObject-Hierarchy.md
  03-Production/
    Backlog.md
    Sprint-Board.md
    Risk-Log.md
  04-Research/
    Reference-Links.md
    User-Testing-Notes.md
```

### MVP Documentation Set (in `02-Technical-Design/`)

**For building the MVP, follow this reading order:**

1. **[MVP-Quick-Reference.md](./02-Technical-Design/MVP-Quick-Reference.md)** — 5-minute overview (print this)
2. **[MVP-Implementation-Quickstart.md](./02-Technical-Design/MVP-Implementation-Quickstart.md)** — phase-by-phase checklist
3. **[MVP-Concrete-Hierarchy.md](./02-Technical-Design/MVP-Concrete-Hierarchy.md)** — exact GameObject structure
4. **[MVP-Data-Specification.md](./02-Technical-Design/MVP-Data-Specification.md)** — exact data values
5. **[MVP-Flag-Sequence.md](./02-Technical-Design/MVP-Flag-Sequence.md)** — execution flow and validation

**Central index:** [02-Technical-Design/README.md](./02-Technical-Design/README.md)

## Naming conventions

- Use English folder names and file names.
- Keep categories by content type, not by scene or author.
- Prefer consistent, descriptive names such as `Game-GDD.md`, `MVP-Sprint-1.md`, `GameObject-Hierarchy.md`.
- Avoid mixing game design, production and technical notes in the same document.

## Purpose of each folder

### `01-Game-Design/`

Contains the game vision, rules, progression, visual references and design intent.

### `02-Technical-Design/`

Contains technical architecture, sprint planning, runtime systems and scene/object responsibilities.

### `03-Production/`

Contains planning, progress tracking, risks and iteration notes.

### `04-Research/`

Contains references, inspiration, technical validation notes and test observations.

## Working rule

If a document affects gameplay behaviour, it should live under `01-Game-Design` or `02-Technical-Design` depending on whether it is design or technical logic. If it is a project workflow document, it belongs under production or research.
