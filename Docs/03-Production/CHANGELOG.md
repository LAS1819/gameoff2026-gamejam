# Changelog

All notable changes to this project will be documented in this file.

## [0.0.5-proto] - 2026-11-06

### Added

- Hallway placeholder doors for the Library and Bedroom, registered as valid interactables in the hallway room data
- `AddHallwayInteractables()` helper in `GameManager` to keep the room setup structured and easier to read
- Debug console feedback for `library_door` and `bedroom_door` when examined in the correct reality

### Changed

- Hallway room setup was refactored out of the inline room factory flow to keep the prototype cleaner and reduce `GameManager` sprawl
- `InteractionManager.ResolveExamine()` now emits the interactable description in logs and blocks interaction when the player is not in the Other Side
- The two hallway doors were defined as visible only in the Other Side, matching the intended gameplay gate around the hallway obstacle

### Fixed

- Prevented the placeholder doors from being interactable in the normal reality before the obstacle is cleared
- Corrected the hidden toggle behavior by making the hallway room data include the doors as real interactables instead of relying only on Scene objects
- Avoided false-positive clickability for library/bedroom triggers before the reality shift is active

### Notes

- This is the prototype pass that stabilizes the hallway side-room placeholder flow and validates the intended gating logic before the next content iteration
- The actual Library and Bedroom rooms are still placeholders; the interaction is now correctly tied to the Other Side and ready for the next content pass

---

## [0.0.4-proto] - 2026-11-06

### Added

- Runtime SHIFT UI implemented with UI Toolkit + Panel Renderer
- Clean icon-only interaction using a `VisualElement` instead of the native Unity Button
- Room-based SHIFT gating: only enabled in the Hallway by default
- Visual state switching between normal and Other Side via CSS classes
- Hover, pressed, disabled feedback handled in USS

### Changed

- Shift logic separated from visual styling to keep the UI layer clean and avoid default button background artifacts
- Updated the technical setup docs to reflect the actual Panel Renderer implementation and the final icon workflow
- Adjusted the runtime UI so styling lives in `ShiftUI.uxml` + `shift-button-style-sheet.uss` instead of in C# code

### Fixed

- Removed the default button background that was appearing behind the SHIFT icon
- Repaired the runtime binding flow for the UI Toolkit clean icon implementation
- Prevented SHIFT from being triggered in rooms where it is not allowed

### Notes

- MVP loop remains focused on Hallway reality shifting as the technical core of the prototype
- This release stabilizes the runtime UI layer and brings the SHIFT interaction closer to the final intended presentation

---

## [0.0.3-proto] - 2026-11-05

### Added

- Input System upgrade for LEFT SHIFT handling
- Hallway configuration guide (HALLWAY-SETUP.md)
- SHIFT Mechanic complete design specification
- InputHandler script with Input System Package support
- Amuleto icon design and progression documentation

### Changed

- Migrated from legacy Input.GetKeyDown() to Input System Package APIs
- Updated technical documentation with Input System requirements and implementation details

### Notes

- Player Settings requires "Active Input Handling" = "Input System Package"
- Hallway ready for scene configuration
- Next step: User configures HallwayObstacle, ShiftTrigger, and LobbyExit in Unity

---

## [0.0.2-proto] - 2026-11-04

### Added

- First playable prototype loop for the MVP:
  - StaffKey pickup flow
  - FrontDoor item validation and unlock progression
  - HallwayEntrance transition into the next room
- Simplified MVP architecture centered on a single Main scene
- Data-driven room and interactable setup for rapid iteration
- Room visibility and interaction state logic for the playable lobby loop
- Updated MVP technical documentation reflecting the progress of the sprint

### Changed

- Removed the Bootstrap scene complexity from the active MVP workflow to reduce cross-scene debugging
- Consolidated room loading and visibility handling around the Main scene structure
- Refined GameManager, RoomManager, and InteractionManager flow to favor clarity and faster validation
- Updated sprint notes to reflect the actual state of development: Lobby complete, Hallway in progress

### Fixed

- Redundant room loading calls that were generating noisy logs and confusing the flow
- Scene hierarchy assumptions that caused room lookup issues in early testing
- Transition logic friction by standardizing room IDs and visibility handling

### Notes

- This is the first prototype release candidate for the playable loop.
- The lobby flow is validated and the room transition foundation is working.
- The Hallway remains a focused next step, intended for the following iteration.

---

## [0.0.1-proto] - 2026-11-03

### Added

- Initial project foundation: documentation, concept notes, and technical scaffolding
- Core design direction for the point-and-click psychological horror prototype
- MVP planning materials and sprint structure for the first playable build
- Early notes on room progression, interactables, and flags
- First version of the technical architecture and implementation roadmap

### Changed

- Established the initial project documentation structure in Docs/01-Game-Design and Docs/02-Technical-Design
- Defined the MVP goal, room flow, and technical scope for the first prototype iteration

### Notes

- This release is a conceptual and planning baseline rather than a playable build.
- It captures the initial idea storm, scope framing, and technical direction for the prototype.
