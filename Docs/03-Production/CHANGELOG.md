# Changelog

All notable changes to this project will be documented in this file.

## [0.1.6-proto] - 2026-11-27 - Lighting and Forest implementation

**Added:**

- Added room and reality profiles for the Global Light 2D system.
- Added the HORIZON sign Sprite Light 2D and activation with the intro/entrance state.
- Added the Bedroom ceiling-beam Sprite Light 2D controlled by `BedroomLightOn`.
- Added progressive red/blue Ambulance Lights in Forest levels 1-5.
- Added the Forest level 1 direct-start baseline.
- Added final Ceiling/ceiling-light presentation in Forest level 6.

**Changed:**

- Forest now starts at level 1 and advances through levels 2-5 before level 6.
- Forest backgrounds follow the 1-5 frame sequence without reusing level 5 by accident.
- Forest Ambulance Lights scale from 0.20 to 1.00 and lower Y position from 3.00 to 0.00.
- Forest global light decreases from 0.24 at level 1 to 0.06 at level 6.
- The Forest sky keeps its original Sprite and color; no sky tint is applied.
- Lantern, HORIZON, ceiling and Forest local lights follow existing flags and replay reset.

**Fixed:**

- Fixed direct Forest start hiding the Ambulance Lights at level 1.
- Fixed the Forest level 5 -> 6 transition requiring a second click.
- Fixed stale Forest depth/light state after Room and Reality changes.
- Fixed the final level activating table, letter and ceiling light in one synchronized refresh.
- Fixed local light state restoration after Room changes and replay.
- Cursor-follow light remains deferred for the next iteration.

---

## [0.1.5-proto] - 2026-11-27 - SFX foundation

**Added:**

- Added a reusable `SFXManager` with a central root volume and per-cue volumes.
- Added 13 Mono OGG SFX clips covering UI, navigation, inventory and world events.
- Routed one-shot effects and the steam loop through the `SFX` Audio Mixer group.
- Added Sonic Pi source scripts for interface, navigation, inventory and world event SFX.
- Added UI clicks for interactables, inventory slots, modal buttons and SHIFT.
- Added SHIFT, room transition, pickup/use, Story, Memory, door, shelf, lamp and pressure valve cues.
- Added a continuous steam loop controlled by Basement, Other Side and `SteamEnabled` state.
- Added modal button fixes so Next/Continue advance exactly once.
- Updated audio technical documentation and Sprint 4 SFX validation notes.

**Changed:**

- SFX playback now uses semantic cue IDs instead of direct `AudioSource.Play()` calls from gameplay objects.
- One-shot SFX share a dedicated source and the steam loop has a separate source.
- Individual cue volumes combine with the central SFX root volume.
- Memory discovery now plays one dedicated cue when the overlay opens.
- Room transitions and Forest level advances use the room transition cue.
- Modal close actions route through a centralized UI click handler.

**Fixed:**

- Fixed modal Next/Continue buttons no longer responding consistently.
- Fixed duplicate possible UI click playback in Story overlays.
- Fixed the steam loop remaining active outside the Basement/Other Side condition.
- Fixed SFX event subscription duplication after scene/runtime lifecycle changes.
- Fixed invalid item and denied interaction feedback to use the UI denied cue.
- Fixed door transitions to use the shared door/transition cue.

---

## [0.1.4-proto] - 2026-11-25 - Main Menu and Intro room flow

**Added:**

- Main Menu implemented as the first `RoomData` before the Lobby
- Reusable scene hierarchy for final Background, Game Title, Horizon sign, Start, Settings, Exit, and Entrance Door
- Final Aseprite Main Menu background, title, button sprites, entrance door, and HORIZON on/off states
- Brief two-page Intro displayed through the existing Story Overlay system
- Dedicated Main Menu game-design and technical-design room documents
- Main Menu runtime flags for intro started and entrance unlocked
- Two-page introductory overlay with `Next` and `Continue` controls

**Changed:**

- Fresh game startup now loads `MainMenu` in Editor and builds
- Start hides menu actions, preserves the graffiti title, and opens the Intro
- Continuing the Intro illuminates the HORIZON sign and enables the Entrance Door
- Entrance Door transitions to a clean Lobby in Normal Reality
- Ending Restart clears runtime state and returns to Main Menu to replay the Intro
- Intro text is divided into two readable pages to prevent title/body overlap
- Main Menu Start, Settings, and Exit buttons use warm tint and slight scale feedback on hover/press
- Main Menu hides Inventory, SHIFT, and Lantern while retaining feedback for Settings and Exit
- Room lookup accepts normalized room names such as `MainMenu` and `main_menu`

**Fixed:**

- Fixed menu actions requiring a separate Canvas or gameplay scene
- Fixed the Intro mode sharing the Letter-to-Ending close behavior
- Fixed WebGL Exit implying that a browser tab can be closed programmatically

**Notes:**

- Settings is intentionally a feedback-only placeholder in `0.1.4-proto`
- Exit closes Windows builds, reports browser-tab behavior in WebGL, and is disabled in Editor
- Final Aseprite assets are assigned; no replacement pass is required for 0.1.4
- Fade and pulse polish remain deferred from the previous block

---

## [0.1.3-proto] - 2026-11-24 - Interaction feedback and shared overlay backgrounds

**Added:**

- Custom 32x32 retro cursor with a precise hotspot and automatic fallback to the system cursor
- Dedicated cursor technical-design guidance in `TD-Cursor.md`
- Pixel-art 9-slice backgrounds for Memory, Revelation Letter, and Ending overlays
- Memory panel background with an archive-inspired blue-gray frame
- Parchment-style Revelation Letter background
- Dark wood and gold Ending/Summary background
- Clear cream and gold text colors for the dark Ending/Summary panel
- Dedicated technical-design guidance for overlay backgrounds in `TD-Overlay-Backgrounds.md`

**Changed:**

- Memory interactables now unlock and reopen Memories only through right-click Examine
- Left-click Use on a Memory interactable provides a contextual prompt to examine it more closely
- Memory, Letter, and Ending panels now use flexible UI Toolkit layouts instead of brittle absolute text positioning
- Ending titles split by final type and outcome into two readable lines
- Inventory panel remains lower and slightly transparent without changing slot dimensions
- The Revelation Letter and final Table are raised above the Inventory panel interaction area
- USS cursor styles removed to avoid runtime cursor warnings; the HUD now owns the single global cursor
- Final Story Overlay text now uses a light palette against the dark Ending background while the Letter preserves dark text on paper
- Sprint 4 documentation now distinguishes implemented behavior from deferred transition and reality effects

**Fixed:**

- Fixed Memory overlays only being available once after discovery
- Fixed Personal Effects Box visibility and Normal/Other Side use semantics
- Fixed duplicate Valve Handle collection
- Fixed Revelation Letter and Ending text clipping inconsistently across loops
- Fixed the Ending title overlapping or being cut by its surrounding layout
- Fixed the Inventory panel covering the Revelation Letter and Personal Effects Box
- Fixed runtime warnings caused by UI Toolkit `cursor: link` declarations
- Fixed dark text becoming unreadable over the Ending/Summary background

**QA/Deferred:**

- Room transition fade is documented but remains visually deferred
- Reality change pulse is documented but remains visually deferred
- Lantern interaction, local 2D lighting, and cursor-follow lighting remain outside the MVP

**Notes:**

- `0.1.3-proto` closes Sprint 4 Block 2 interaction and feedback polish
- The release preserves the complete playable loop and adds no new room or mechanic
- Fade/pulse implementation remains an optional follow-up rather than a delivery blocker
- Remaining work is final build validation, itch.io delivery preparation, localization, credits, and submission polish

---

## [0.1.2-proto] - 2026-11-24 - Shared UI foundations and replay stabilization

**Added:**

- Press Start 2P title/accent font and Pixelify Sans body font for UI Toolkit
- Retro inventory slot states for Empty, Full, and Selected using a three-frame sprite sheet
- Nine-slice inventory panel background with a contrasting 9-slice-aware frame
- Graphic inventory icons for Staff Key, Library Key, Worn Book, Floppy Disk, Basement Code, and Valve Handle
- Technical-design guidance for inventory slot sprites, item sprites, import settings, and fallback behavior
- Replay verification coverage for Bedroom, Library, Basement, and Forest state restoration

**Changed:**

- Inventory slots now render as `48 x 48` pixel-art elements with `48 x 48` item icons
- Inventory panel spacing and slot separation now preserve readable contrast around the frame
- Shared HUD styles now use Pixelify Sans for readable text and Press Start 2P for titles and primary accent actions
- Library secret shelf, Bedroom persistent state, and Forest depth visuals now synchronize with progression flags when a room is re-entered
- Forest entry is explicitly restricted to Normal Reality after the Basement sequence is resolved

**Fixed:**

- Fixed Bedroom restarting with the light on, wardrobe open, computer powered, disk inserted, and Library Key already resolved
- Fixed Library secret shelf remaining open after restart and closing only by animation after re-entering
- Fixed Forest Exit Door resolving or opening the Forest from Other Side
- Fixed the first Forest depth frame loading with inactive or missing background renderers
- Fixed Forest entry forcing the wrong reality and leaving the Normal background disabled
- Preserved fallback labels and inventory interaction behavior when an item icon is unavailable

**Notes:**

- `0.1.2-proto` closes Sprint 4 Block 1, shared UI foundations
- The release candidate keeps the complete playable loop and adds no new puzzle or room
- Remaining work is build validation, itch.io submission, final P0/P1 QA, and later visual polish blocks
- The next code milestone is interaction and feedback polish, not a new gameplay system

---

## [0.1.1-proto] - 2026-11-23 - Memory discovery pass and test-build readiness

**Added:**

- Five optional Memories with persistent Memory flags and right-click examination
- Memory discovery overlay for concise narrative presentation
- Five 320x112 pixel-art Memory sprites for the Lobby, Hallway, Bedroom, Library, and Basement
- Ten-second configurable Memory overlay duration through `HUDController.memoryDiscoveryDuration`
- Continue button for closing Memory overlays before the automatic timeout
- Memory_01 discovery support in the Lobby in Normal Reality
- Memory_02 discovery support in the Hallway in the Other Side
- Memory test interactables in every playable Room
- Initial documentation for the final 0.1.1-proto test-build milestone

**Changed:**

- Documentation normalized for the complete `0.1.1-proto` state and final Sprint 4 delivery scope
- Added the final polish, QA, build, itch.io, localization, credits, and release-cut plan
- Corrected WebGL / Windows resolution and fullscreen documentation to match Player Settings
- Marked Sprints 1-3 as closed historical records
- Distinguished implemented behavior from deferred or discarded design
- Memory presentation now uses a dedicated non-modal overlay instead of the modal Story Overlay
- Memory sprites use 320x112 horizontal pixel compositions with Point filtering
- Memory overlay art uses `scale-to-fit` to preserve complete silhouettes
- Memory display time is configurable from the Unity Inspector
- Sprint 3 documentation now records Memory discovery as a complete MVP block
- MVP specification and overview now identify `0.1.1-proto` as the test-build milestone
- HUD responsibilities remain split between inventory, story overlays, and Memory discovery views

**Fixed:**

- Prevented left-click Use actions from discovering Memories
- Restricted Memory discovery to right-click Examine except where Memory_01 is explicitly available in Lobby Normal Reality
- Prevented repeat examinations from reopening or reactivating an already recovered Memory
- Prevented generic Examine result flags from changing unrelated state
- Fixed Memory_01 and Memory_02 sprites remaining unassigned in the HUD scene
- Fixed Forest Room initial depth not applying when entering from the resolved Basement
- Fixed initial overlay visibility and disabled modal controls at startup
- Fixed Wardrobe, monster, switch, and Library Door interaction regressions
- Fixed repeated interaction logic bypassing required-item and reality checks
- Fixed Story Overlay and Memory overlay lifecycle after restart

**Notes:**

- `0.1.1-proto` is the current test-build candidate for WebGL and Windows delivery
- The remaining work is room-by-room final interactable placement, polish, accessibility, localization, and final build validation
- Memory 03-05 scene interactables remain in test position until the final Room pass assigns the intended objects
- The release does not introduce a persistent save system or post-MVP collection UI
- Sprint 4 is the polish and delivery sprint for builds, Itch.io, Credits, localization, and final narrative copy

---

## [0.1.0-proto] - 2026-11-21 - Forest room and complete ending game loop

**Added:**

- Forest room integrated after the resolved Basement sequence
- Five synchronized depth states using separate Sky, Outer Forest, Border Trees, and Central Path layers
- Sixth depth state that removes the forest background and reveals the final Table, Ceiling Light, and Revelation Letter
- Functional Forest Revelation Letter interactable
- Reusable HUD Story Overlay for Letter and Ending presentation
- Three provisional letter and ending variants based on recovered Memories
- Ending summary with recovered Memories and provisional credits
- Restart flow that resets runtime state, closes the overlay, and returns to the Lobby
- Visible Continue and Restart buttons with mouse, keyboard, and direct pointer support
- Forest gameplay, visual, technical-design, and ending-flow documentation
- Sprint 4 planning for a dedicated Credits scene and English/Spanish localization

**Changed:**

- The Forest replaces the old Ritual Room as the final playable location
- Forest progression is terminal and no longer offers a return transition to the Basement
- Forest depth now supports levels 0-6
- Forest UI and ending copy now use English runtime text
- Forest spritesheets use 4096px maximum size to avoid downscaling the 3200px-wide atlases
- Story Overlay presentation is reusable and ready for future final letter artwork
- Letter and Ending use explicit content zones so text and controls remain inside the 640x360 viewport
- Removed null-propagation and deprecated Unity API usage from runtime scripts
- Updated Memory ending conditions to 0-2, 3-4, and 5 recovered Memories
- Updated the documented game flow to include the final room, credits, and restart loop

**Fixed:**

- Correct Basement-to-Forest transition and room catalog registration
- Correct initial disabled state for the level 6 Table, Ceiling Light, and Letter
- Correct Forest background disabling when the final room appears
- Correct Story Overlay interaction blocking while Letter or Ending is open
- Fixed overlay controls being outside the 640x360 viewport
- Prevented the letter body from overlapping Continue
- Separated Ending title, farewell text, Memory count, credits, and Restart into non-overlapping regions
- Fixed the Ending title dropping into the Restart button action area
- Fixed Restart leaving a stale overlay or incorrect Forest depth after returning to the Lobby
- Corrected obsolete object lookup usage that produced Unity compilation warnings

**Notes:**

- `0.1.0-proto` is the first complete playable loop: Basement → Forest → Letter → Ending → Lobby
- Letter copy, credits content, and parchment artwork remain provisional
- A dedicated Credits scene and Unity Localization setup are planned for Sprint 4
- Forest parallax, additional manifestations, and advanced visual events remain outside this prototype scope
- Unity validation completed with 0 compilation errors and 0 warnings

---

## [0.0.11-proto] - 2026-11-19 - Basement room MVP and narrative pressure resolution

**Added:**

- Basement room integrated into the main room catalog and Library-to-Basement progression flow
- Personal Effects Box evidence interaction for the Basement, including the Valve Handle obtained while exploring the Other Side
- Valve Handle inventory item and visual installed-handle state on the Pressure Release Valve
- Pressure Release Valve functional in Normal Reality and hidden in the Other Side
- Animated Steam Pipes that activate after repairing the pressure valve
- Denial Manifestation loop and a short 1.35-second unreal disappearance animation
- Storage Shelves and Basement Pipeline interactables
- Invisible Forest Exit Door interactable unlocked after resolving the pressure and manifestation sequence
- Library and Lobby return transitions for the Basement
- Complete Basement sprite, spritesheet and scene asset set
- Expanded Basement game-design and technical-design documentation

**Changed:**

- The Basement Code is now used only on the Library Code Panel and is consumed upon successful use
- The Basement Door remains permanently accessible after the Code Panel is resolved
- Personal Effects Box now acts as the hidden Valve Handle source rather than exposing a separate pickup
- Pipelines and Storage Shelf visuals switch correctly between Normal and Other Side states
- Denial Manifestation now disappears only after changing to the Other Side with the valve repaired and steam enabled
- Forest Exit Door feedback now changes by reality: clear in Normal, blocked by burning steam in Other Side
- Basement conditional visibility and scene-to-catalog binding are synchronized across reality and room transitions
- The MVP scope remains separate from the future Lantern, Global Light and battery mechanics

**Fixed:**

- Restored repeatable Basement access from the Library
- Prevented the Basement Code from being consumed or required by the wrong interactable
- Removed incorrect Library shelf sprites from Basement storage shelves
- Prevented Denial Manifestation from binding or rendering as a Pipeline
- Fixed Pipelines remaining on the wrong reality sprite
- Fixed Pressure Release Valve visibility and use rules across realities
- Fixed Forest Exit Door unlocking before the puzzle was resolved
- Fixed Steam Pipes and installed Valve Handle visual synchronization
- Fixed stale manifestation visibility during reality refresh
- Fixed the manifestación `SpriteRenderer` being disabled during its rejection fade
- Fixed room refresh logic hiding the rejection animation before it finished
- Strengthened exact scene-object names against mismatched room catalog data
- Removed runtime placeholder Basement generation so the room is entirely scene-wired for manual construction

**Notes:**

- This release completes the Base MVP implementation of the Basement room
- The room now covers evidence discovery, item-based repair, reality pressure release and psychological manifestation rejection
- Global Light 2D, Lantern gating, battery state and other light mechanics remain intentionally deferred
- The Forest, future room expansion and advanced combat or movement systems remain outside the current MVP scope
- The next milestone can focus on the final Lantern and lighting implementation without changing the resolved Basement puzzle flow

---

## [0.0.10-proto] - 2026-11-17 - Bedroom and Library progression pass completion

**Added:**

- Bedroom computer interaction flow with a split Tower + Monitor setup, support for powering the device, and a dedicated `Floppy Disk` insertion route
- Basement progression item logic that grants a `basement_code` after the monitor interaction is resolved correctly with the inserted disk
- Library shelf and Worn Book interaction flow with the secret-door reveal logic and the hidden room activation path behind the shelf
- Room catalog split into dedicated lobby, hallway, bedroom, and library files to keep the runtime data model easier to read and extend
- Visual and scene assets for the Bedroom computer and the Library shelf / Worn Book progression path needed by the current MVP loop
- Documentation updates for the Bedroom, Library, Hallway, story beats, and sprint planning so the current room flow is internally consistent

**Changed:**

- Refactored the interaction resolution pipeline around a clearer contract between item use, state mutation, and required-item validation
- Tightened the separation between “required item consumed for use” and “interactable itself consumed after successful use” so doors remain persistent while the key item still resolves correctly
- Simplified the room setup structure to keep the prototype gameplay-first and avoid introducing heavy abstraction before the content demand justifies it
- Updated runtime lifecycle and room visibility behavior so hallway and library doors respond to the current reality and room state without disappearing or reappearing unpredictably
- Kept the project architecture intentionally lightweight while moving toward a clearer, more maintainable room + interactable + inventory model

**Fixed:**

- Resolved the Bedroom monitor bug where the power state was not updating correctly when the player used the screen without the expected item flow
- Fixed the issue where the Library door could disappear unexpectedly after leaving/re-entering the room because the consume logic was tied to the wrong object state
- Corrected stale inventory selection behavior so an invalid item-use attempt no longer leaves the selected item in a misleading state
- Repaired the tower/monitor interaction logic so the monitor properly returns description feedback when powered off and triggers the correct progression once the disk is in place
- Stabilized the shelf reveal flow and the hidden room activation so the secret-door state is consistent across room transitions and reality changes

**Notes:**

- This release closes the current Library + Bedroom progression pass and leaves the project in a strong MVP state for the main room loop before the next room expansion
- The prototype remains intentionally focused on playable loop clarity, narrative progression, and maintainability rather than broad feature expansion
- The next major milestone is the Basement room, which should be designed as a compact evidence-focused room that resolves the current hidden truth without over-expanding the MVP scope

---

## [0.0.9-proto] - 2026-11-14 - Mouse interaction and input system integration

**Added:**

- Mouse interaction contract for the MVP: left click resolves `Interactable` actions and item use / pickup flows, while right click is reserved for `Examine` only
- Input System-based click handling using polling and raycast validation so the runtime responds only to the intended button and collider
- Reality-aware description support for interactables and items with normal / other-side fallback logic aligned to the current data model
- Documentation of the final Sprint 2 technical decision: the current system stays lightweight, and ScriptableObjects remain optional unless content scale forces a migration

**Changed:**

- Refactored the interaction flow around explicit left/right click resolution so examine behavior is separated from active game-state resolution
- Simplified the interaction architecture by keeping the room, interactable, and inventory data contracts clear and easy to extend without adding unnecessary abstraction layers
- Kept the gameplay loop aligned with the MVP scope: fast iteration, no premature frameworking, and no extra systems before the content demand is proven
- Kept the runtime docs consistent with the final technical decisions for the Sprint 2 closeout and the Library/Basement preparation path

**Fixed:**

- Resolved the critical bug where right-clicking an interactable could still trigger the underlying `resultFlag` logic and mutate state during `Examine`
- Fixed the Input System incompatibility caused by legacy `UnityEngine.Input.mousePosition` reads while the project had active Input System Package enabled in Player Settings
- Prevented false-positive interaction resolution by validating the hit target with a 2D raycast before processing the click event
- Corrected the mismatch between “read-only examine” and “state-changing interaction” so only actual interactions mutate room or progression flags

**Notes:**

- This release closes the core interaction contract for the current MVP and stabilizes the final click-resolution model before the Library pass begins
- The project keeps the current data-first architecture as the default while leaving ScriptableObjects as a clear future migration option only if content scale requires it
- The current branch is now ready for the next room-expansion pass with the interaction rules and technical contract already frozen and documented

---

## [0.0.8-proto] - 2026-11-12 - Complete Bedroom flow and interaction stabilization

**Added:**

- Complete Bedroom flow in the main runtime loop, including the wardrobe, light switch, monster visibility, library key, hallway exit, and window interactables
- Lantern HUD state and persistent icon behavior aligned with the existing shift-button UI patterns
- Nightstand interaction that grants the flashlight state without creating a false inventory item, keeping the HUD-driven lantern logic consistent
- Generic repeated-interaction fallback feedback so exhausted or already-used interactables do not produce misleading state transitions
- Runtime room bootstrap adjustments that keep the Lobby visible at startup and prevent the scene from starting in the wrong room
- Audio output routing through the `MainAudioMixer` `Music` group so generated AudioSources respect the intended project mixer structure

**Changed:**

- Refined the interaction model to handle room-specific visibility, reality-aware descriptions, and conditional object activation more consistently across scenes
- Updated `GameManager` and `RoomManager` runtime setup to ensure the active Room and Room catalog are initialized in the correct order on startup
- Switched Shift detection to the Input System package path and removed the legacy `Input.GetKeyDown` usage that conflicted with the active Player Settings configuration
- Kept the Lantern and HUD logic aligned with the broader MVP requirement: the item is a persistent world state indicator rather than a regular inventory slot item
- Continued the project’s minimal, gameplay-first architecture without adding unnecessary abstraction layers or broad framework systems

**Fixed:**

- Resolved the startup room issue where the Lobby could fail to appear when entering Play because the room loader was relying on a stale CurrentRoom condition
- Fixed the Shift input failure caused by mixing legacy `UnityEngine.Input` calls with the active Input System package setup
- Corrected the Nightstand logic so it does not create phantom inventory entries or consume the object incorrectly after the first pickup
- Prevented the bedroom door and related scene state from being shown under the wrong room or reality conditions
- Ensured runtime AudioSource generation follows the configured mixer hierarchy instead of bypassing the mixer output path

**Notes:**

- This release completes the Bedroom-focused MVP work on the current branch and consolidates the room, UI, interaction, and runtime stability changes behind a single versioned milestone
- The project remains intentionally scoped to a minimal, playable loop for the game-jam MVP rather than expanding into full inventory or backend systems
- The current branch is ready for a final review and release candidate pass before broader content work begins

---

## [0.0.7-proto] - 2026-11-10 - Initial audio system foundation

**Added:**

- Initial runtime audio foundation with a dedicated `AudioManager` and `MusicManager` under the main runtime hierarchy
- Looping Theme Father soundtrack playback tied to the game's runtime lifecycle
- Audio mixer baseline with `Master`, `Music`, `Ambient`, and `SFX` groups for clean future expansion
- Unity import and workflow guidance for music/ambient vs SFX handling in the technical design document
- Audio system design notes capturing the runtime architecture, mixer guidance, and migration path for future content

**Changed:**

- Wired `GameManager` to initialize the audio system alongside the rest of the runtime managers
- Kept the music system aligned with the current project pattern so the audio layer responds to runtime state instead of bypassing the game's existing architecture
- Reworked the technical design doc to keep the audio specification compact, practical, and consistent with the current sprint scope

**Fixed:**

- Ensured the music source is explicitly assigned to the `Music` bus so it reaches the mixer output instead of being left un-routed
- Reduced the risk of unmaintainable audio design by avoiding a `Resources`-based dependency in the first runtime pass

**Notes:**

- This pass establishes the initial audio runtime foundation without over-expanding the feature set beyond the current MVP scope
- The project is still intentionally scoped to a single global theme while the actual Library and other rooms remain deferred for future content passes
- The next step is to expand the audio system only when additional rooms or narrative cues require separate ambience and cue logic

---

## [0.0.6-proto] - 2026-11-07 - Central HUD and inventory interaction improvements

**Added:**

- Central runtime HUD built around a single `HUDRoot` with Panel Renderer, UXML, and USS for the MVP demo layer
- Inventory slot state and selected-item highlighting so the player can clearly see what has been picked up and what is currently prepared for use
- Shared SHIFT gate logic in `RealityManager` so the room restriction is enforced consistently in both keyboard and UI paths
- `TryToggleReality()` and `IsShiftAllowedInCurrentRoom()` helpers to centralize the reality-toggle rules without duplicating logic
- Runtime feedback and overlay states in `HUDController` for room transitions and reality changes

**Changed:**

- Consolidated the HUD and SHIFT interaction into one runtime source of truth to avoid duplicate UI surfaces and conflicting click handlers
- Updated `InteractionManager` to require an explicit item selection before a required item can be used on a target interactable, aligning with the Staff Key + Front Door flow
- Refined the HUD lifecycle so `UIManager` delegates to `HUDController` instead of leaving the UI layer half-wired or disconnected from gameplay events
- Cleaned the stylesheet to remove unsupported UI Toolkit patterns and keep the HUD compatible with the current runtime UI stack

**Fixed:**

- Prevented LEFT SHIFT from toggling reality outside the allowed room, matching the same restriction already applied to the HUD button
- Removed the duplicate/legacy ShiftPanel conflict risk by keeping the control path centralized in the HUD root
- Ensured inventory slots visibly reflect item presence and selection after pickup instead of only toggling a generic empty/full style
- Stopped UI Toolkit warnings by replacing unsupported `inset`, `gap`, and pseudo-selector patterns with compatible layout rules

**Notes:**

- This pass stabilizes the demo-ready HUD layer and the core MVP interaction flow without expanding scope into extra rooms or content systems
- The prototype remains focused on the Lobby → Hallway loop, while the actual Library and Bedroom rooms are still intentionally deferred for the next iteration

---

## [0.0.5-proto] - 2026-11-06 - Hallway side-room placeholder flow and gating logic

**Added:**

- Hallway placeholder doors for the Library and Bedroom, registered as valid interactables in the hallway room data
- `AddHallwayInteractables()` helper in `GameManager` to keep the room setup structured and easier to read
- Debug console feedback for `library_door` and `bedroom_door` when examined in the correct reality

**Changed:**

- Hallway room setup was refactored out of the inline room factory flow to keep the prototype cleaner and reduce `GameManager` sprawl
- `InteractionManager.ResolveExamine()` now emits the interactable description in logs and blocks interaction when the player is not in the Other Side
- The two hallway doors were defined as visible only in the Other Side, matching the intended gameplay gate around the hallway obstacle

**Fixed:**

- Prevented the placeholder doors from being interactable in the normal reality before the obstacle is cleared
- Corrected the hidden toggle behavior by making the hallway room data include the doors as real interactables instead of relying only on Scene objects
- Avoided false-positive clickability for library/bedroom triggers before the reality shift is active

**Notes:**

- This is the prototype pass that stabilizes the hallway side-room placeholder flow and validates the intended gating logic before the next content iteration
- The actual Library and Bedroom rooms are still placeholders; the interaction is now correctly tied to the Other Side and ready for the next content pass

---

## [0.0.4-proto] - 2026-11-06 - Runtime SHIFT UI and visual state improvements

**Added:**

- Runtime SHIFT UI implemented with UI Toolkit + Panel Renderer
- Clean icon-only interaction using a `VisualElement` instead of the native Unity Button
- Room-based SHIFT gating: only enabled in the Hallway by default
- Visual state switching between normal and Other Side via CSS classes
- Hover, pressed, disabled feedback handled in USS

**Changed:**

- Shift logic separated from visual styling to keep the UI layer clean and avoid default button background artifacts
- Updated the technical setup docs to reflect the actual Panel Renderer implementation and the final icon workflow
- Adjusted the runtime UI so styling lives in `ShiftUI.uxml` + `shift-button-style-sheet.uss` instead of in C# code

**Fixed:**

- Removed the default button background that was appearing behind the SHIFT icon
- Repaired the runtime binding flow for the UI Toolkit clean icon implementation
- Prevented SHIFT from being triggered in rooms where it is not allowed

**Notes:**

- MVP loop remains focused on Hallway reality shifting as the technical core of the prototype
- This release stabilizes the runtime UI layer and brings the SHIFT interaction closer to the final intended presentation

---

## [0.0.3-proto] - 2026-11-05 - Input System integration and SHIFT mechanic design

**Added:**

- Input System upgrade for LEFT SHIFT handling
- Hallway configuration guide (HALLWAY-SETUP.md)
- SHIFT Mechanic complete design specification
- InputHandler script with Input System Package support
- Amuleto icon design and progression documentation

**Changed:**

- Migrated from legacy Input.GetKeyDown() to Input System Package APIs
- Updated technical documentation with Input System requirements and implementation details

**Notes:**

- Player Settings requires "Active Input Handling" = "Input System Package"
- Hallway ready for scene configuration
- Next step: User configures HallwayObstacle, ShiftTrigger, and LobbyExit in Unity

---

## [0.0.2-proto] - 2026-11-04 - First playable prototype loop

**Added:**

- First playable prototype loop for the MVP:
  - StaffKey pickup flow
  - FrontDoor item validation and unlock progression
  - HallwayEntrance transition into the next room
- Simplified MVP architecture centered on a single Main scene
- Data-driven room and interactable setup for rapid iteration
- Room visibility and interaction state logic for the playable lobby loop
- Updated MVP technical documentation reflecting the progress of the sprint

**Changed:**

- Removed the Bootstrap scene complexity from the active MVP workflow to reduce cross-scene debugging
- Consolidated room loading and visibility handling around the Main scene structure
- Refined GameManager, RoomManager, and InteractionManager flow to favor clarity and faster validation
- Updated sprint notes to reflect the actual state of development: Lobby complete, Hallway in progress

**Fixed:**

- Redundant room loading calls that were generating noisy logs and confusing the flow
- Scene hierarchy assumptions that caused room lookup issues in early testing
- Transition logic friction by standardizing room IDs and visibility handling

**Notes:**

- This is the first prototype release candidate for the playable loop.
- The lobby flow is validated and the room transition foundation is working.
- The Hallway remains a focused next step, intended for the following iteration.

---

## [0.0.1-proto] - 2026-11-03 - Initial release

**Added:**

- Initial project foundation: documentation, concept notes, and technical scaffolding
- Core design direction for the point-and-click psychological horror prototype
- MVP planning materials and sprint structure for the first playable build
- Early notes on room progression, interactables, and flags
- First version of the technical architecture and implementation roadmap

**Changed:**

- Established the initial project documentation structure in Docs/01-Game-Design and Docs/02-Technical-Design
- Defined the MVP goal, room flow, and technical scope for the first prototype iteration

**Notes:**

- This release is a conceptual and planning baseline rather than a playable build.
- It captures the initial idea storm, scope framing, and technical direction for the prototype.
