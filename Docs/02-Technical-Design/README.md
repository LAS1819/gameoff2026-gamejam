# Technical design

This folder contains the active technical reference for the `0.1.4-proto` vertical slice and the implementation history behind it.

## Start here

- [MVP-SPECIFICATION.md](./MVP-SPECIFICATION.md) - current end-to-end technical contract
- [Technical-Design.md](./Technical-Design.md) - architecture principles
- [MVP-Sprint-3.md](./MVP-Sprint-3.md) - Sprint 3 closeout and Sprint 4 handoff
- [MVP-Sprint-4.md](./MVP-Sprint-4.md) - active polish, QA, build, and delivery sprint
- [GameObject-Hierarchy.md](./GameObject-Hierarchy.md) - scene hierarchy conventions

## Implemented room documents

| Room | Game design | Technical design |
| --- | --- | --- |
| Main Menu | [GD-Room-Main-Menu.md](../01-Game-Design/Gameplay/Rooms/GD-Room-Main-Menu.md) | [TD-Room-Main-Menu.md](./Gameplay/Rooms/TD-Room-Main-Menu.md) |
| Lobby | [GD-Room-Lobby.md](../01-Game-Design/Gameplay/Rooms/GD-Room-Lobby.md) | [TD-Room-Lobby.md](./Gameplay/Rooms/TD-Room-Lobby.md) |
| Hallway | [GD-Room-Hallway.md](../01-Game-Design/Gameplay/Rooms/GD-Room-Hallway.md) | [TD-Room-Hallway.md](./Gameplay/Rooms/TD-Room-Hallway.md) |
| Bedroom | [GD-Room-Bedroom.md](../01-Game-Design/Gameplay/Rooms/GD-Room-Bedroom.md) | [TD-Room-Bedroom.md](./Gameplay/Rooms/TD-Room-Bedroom.md) |
| Library | [GD-Room-Library.md](../01-Game-Design/Gameplay/Rooms/GD-Room-Library.md) | [TD-Room-Library.md](./Gameplay/Rooms/TD-Room-Library.md) |
| Basement | [GD-Room-Basement.md](../01-Game-Design/Gameplay/Rooms/GD-Room-Basement.md) | [TD-Room-Basement.md](./Gameplay/Rooms/TD-Room-Basement.md) |
| Forest | [GD-Room-Forest.md](../01-Game-Design/Gameplay/Rooms/GD-Room-Forest.md) | [TD-Room-Forest.md](./Gameplay/Rooms/TD-Room-Forest.md) |

## Systems

- [Interaction model](./Gameplay/TD-Interactables.md)
- [Memories](./Gameplay/TD-Memories.md)
- [Room architecture](./Gameplay/Rooms/TD-Room-Architecture.md)
- [Lighting and Lantern system](./TD-Lighting-System.md)
- [HUD](./UI/TD-HUD.md)
- [Sprint 4 fonts](./UI/TD-Fonts.md)
- [Sprint 4 inventory](./UI/TD-Inventory.md)
- [Sprint 4 item sprites](./UI/TD-Item-Sprites.md)
- [Sprint 4 cursor](./UI/TD-Cursor.md)
- [Sprint 4 overlay backgrounds](./UI/TD-Overlay-Backgrounds.md)
- [Audio](./Audio/TD-Audio-System.md)
- [SFX](./Audio/TD-SFX.md)
- [Music](./Audio/TD-Music.md)
- [Ambience](./Audio/TD-Ambience.md)

## Sprint history

1. [MVP-Sprint-1.md](./MVP-Sprint-1.md) - Lobby and SHIFT foundation
2. [MVP-Sprint-2.md](./MVP-Sprint-2.md) - Bedroom, Library, interaction, and HUD stabilization
3. [MVP-Sprint-3.md](./MVP-Sprint-3.md) - complete Basement, Forest, and ending loop
4. [MVP-Sprint-4.md](./MVP-Sprint-4.md) - final polish and delivery

## Current architecture rules

- single gameplay scene in the prototype
- data-driven Room catalog
- flag-based progression
- explicit scene-object references preferred at runtime
- left click changes state; right click examines
- Unity Input System only
- no null-propagation operators in runtime scripts
- UI Toolkit through `PanelRenderer`
- Lantern activation and global room/reality lighting are planned for Sprint 4 Block 5
- no persistent save or global clock in the `0.1.4-proto` scope

Older sprint documents remain as historical records. They should not override the current `0.1.4-proto` specification.
