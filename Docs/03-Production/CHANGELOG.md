# Changelog

All notable changes to this project will be documented in this file.

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
