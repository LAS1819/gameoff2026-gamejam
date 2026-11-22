# MVP Sprint 3 - Complete prototype route and ending loop

- Start: 2026-11-12
- Closed: 2026-11-23
- Status: Complete prototype route - Memory discovery polish in progress
- Update: 2026-11-22
- Release milestone: `0.1.0-proto`

## Objective achieved

Sprint 3 closed the complete playable route:

```text
Library / Bedroom progression
  -> Basement
    -> Forest
      -> Revelation Letter
        -> Ending
          -> Lobby
```

The goal was no longer limited to opening the Basement. The sprint delivered the Basement, Forest, final letter interaction, ending evaluation, and restart loop.

## Implemented blocks

### Library and Bedroom

- Worn Book and Floppy Disk flow
- Bedroom computer Tower / Monitor behavior
- Basement Code acquisition
- Library secret shelf, Code Panel, and Basement access
- left-click interaction / right-click examine contract

### Basement

- Personal Effects Box evidence
- Valve Handle progression item
- Pressure Release Valve
- Steam Pipes
- denial manifestation rejection
- Forest exit unlocked from the resolved sequence

### Forest

- five depth frames
- synchronized Sky / Outer Forest / Border Trees / Central Path layers
- terminal level 6
- background removal in the final black room
- Table, Ceiling Light, and Revelation Letter activation
- no return route to the Basement

### Memory discovery polish - in progress

- [x] Memory data model and catalog implemented
- [x] five room interactables configured
- [x] first-discovery flags and repeat protection implemented
- [x] non-modal discovery overlay implemented in the HUD
- [ ] assign final concept-art sprites
- [ ] complete playtest coverage for the five discoveries
- define the five optional Memory interactables in the Other Side
- activate one persistent Memory flag on the first examination
- prevent repeated presentation of an already discovered Memory
- add a short overlay for the discovery event
- reuse the concept art of the active Room in the MVP overlay
- keep the overlay non-modal and without a visible Memory counter
- preserve the existing ending count of `0-2 / 3-4 / 5` Memories
- validate the five discoveries, including return and restart behavior

### Letter and ending

- reusable UI Toolkit Story Overlay
- letter text selected by recovered Memory count
- three ending tiers
- ending title, farewell, Memory summary, and provisional credits
- visible Continue and Restart controls
- keyboard and pointer support
- clean reset to Lobby

### Runtime cleanup

- removed null-propagation from runtime scripts
- removed deprecated object lookup API usage
- translated runtime ending and overlay copy to English
- centralized Story Overlay behavior in the HUD
- documented Sprint 4 localization and Credits-scene direction

## Acceptance result

All release blockers are closed:

- [x] Library progression completed
- [x] Bedroom computer progression completed
- [x] Basement fully playable
- [x] Basement puzzle and manifestation synchronized
- [x] Forest reachable from Basement
- [x] Forest depth 0-6 works
- [x] final letter appears correctly
- [x] Letter and Ending overlays are readable and clickable
- [x] ending depends on recovered Memories
- [x] restart returns to a clean Lobby
- [x] Unity compiles with 0 errors and 0 warnings

## Deferred to Sprint 4

- final narrative copy for the three letters and endings
- final antique-letter artwork
- dedicated Credits scene
- English / Spanish localization
- final SFX and audio mix
- performance, accessibility, and playtest pass
- WebGL and Windows builds
- itch.io delivery page

## Scope decision

Sprint 4 is a polish and delivery sprint, not a new gameplay expansion. The prototype loop is complete; additional features should be rejected unless they directly improve clarity, stability, or submission quality.

## Release handoff

`0.1.0-proto` is ready to move from `develop` into `main` once the documentation release pass is committed and the branches are synchronized. The next source-control milestones are the `v0.1.0-proto` tag and GitHub Release.
