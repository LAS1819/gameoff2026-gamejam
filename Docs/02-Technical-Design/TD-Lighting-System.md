# TD-Lighting-System - Luz por Room, Realidad y Linterna

- Version: 1.1
- Created: 2026-11-27
- Status: Implemented for `0.1.6-proto`; cursor-follow light deferred
- Scope: `Main.unity`, room catalog, HUD, Forest depth and Basement interactables

---

## 1. Objective

This document defines the shared lighting system that changes ambient
illumination by Room and Reality without moving lighting logic into individual
interactables.

Sprint 4 Block 5 is limited to:

- a Global Light 2D profile for each Room;
- Normal and Other Side variants;
- a lantern toggle;
- interaction feedback for dark Basement objects;
- clean reset on Room change and restart.

It does not include battery drain, inventory consumption, a separate flashlight
puzzle, dynamic shadows or combat.

## Local Sprite Lights

The baseline also supports three existing local Sprite Light 2D objects:

- `HorizonSignLight`: child of `HorizonSign`; active when the sign is unlocked
  after the Main Menu introduction;
- `CeilingBeamLight`: child of `CeilingLamp`; active while
  `BedroomLightOn` is true in Normal and Other Side;
- `AmbulanceLights`: Forest Props root; contains `RedLight` and `BlueLight`.

Local lights supplement the Global Light 2D profile. They do not replace the
profile's base intensity or color.

---

## 2. Existing integration

The prototype already provides:

- `RoomManager`: active Room and `RoomChanged`;
- `RealityManager`: active Reality and `RealityChanged`;
- `GameManager`: possession flags and reset;
- `InteractionGuard`: one validation point for interaction gates;
- `HUDController` / `HUDInventoryView`: permanent lantern icon;
- `HasFlashlight`: possession flag set in the Bedroom;
- a lantern atlas with three states;
- URP 2D and the `640 x 360` Pixel Perfect Camera;
- Lit Sprite materials in the default URP 2D renderer.

Existing code IDs remain unchanged:

```text
Lantern concept: Flashlight
Possession flag: HasFlashlight
Item/pickup ID: flashlight_tool
```

The active state is separate from possession:

```text
HasFlashlight = the player obtained the tool
FlashlightActive = the tool is currently toggled on
```

This prevents active state from leaking after a Room change or restart.
`FlashlightActive` should be stored in `GameManager.Flags` or in the Lantern
controller plus reset by `GameManager.InitializeMVP`; either option is valid,
but the flag/property must be reset explicitly and observed by the interaction
gate and HUD.

---

## 3. Data model

Add lighting fields to `RoomData` as explicit configuration.

```text
RoomLightingProfile
- normalIntensity
- normalColor
- otherSideIntensity
- otherSideColor
- lanternAllowed
```

The implementation should keep `RoomData` simple:

```csharp
public class RoomLightingProfile
{
    public float normalIntensity;
    public Color normalColor;
    public float otherSideIntensity;
    public Color otherSideColor;
    public bool lanternAllowed;
}
```

Add only one optional interaction field:

```text
requiresLantern
```

It defaults to `false` and is enabled only for interactables that need
visibility or darkness feedback in the Basement.

---

## 4. Starting profiles

These are playtest starting points, not final art approval values.

| Room | Normal | Other Side | Lantern |
| --- | --- | --- | --- |
| Main Menu | 0.55 / neutral | 0.55 / neutral | Not allowed |
| Lobby | 1.00 / warm white | 0.78 / cool violet | Not allowed |
| Hallway | 0.90 / warm white | 0.58 / cool violet | Not allowed |
| Bedroom | 0.58 / warm white | 0.42 / cool violet | Not allowed |
| Library | 0.84 / warm white | 0.56 / cool violet | Not allowed |
| Basement | 0.06 / warm amber | 0.045 / cold blue | Allowed |
| Forest | 0.28 / neutral dark | Normal only | Not allowed |

Mixing rules:

- Main Menu: after Start, add `0.30` intensity and blend toward amber
  `(1.00, 0.78, 0.35)` while preserving the base profile.
- Bedroom: ceiling light adds `0.18` intensity and warm amber tint; the
  powered computer adds `0.10` intensity and blends in the green
  `#239063` screen tone. Normal and Other Side keep separate base intensity
  and color before both effects are applied.
- Forest: entry starts at level 1 with global intensity `0.24`. It decreases
  through levels 2-5 at `0.20`, `0.16`, `0.12` and `0.08`, reaching
  `0.06` at level 6. The sky sprite and color stay unchanged.
- Basement: without the flashlight, keep the very low base. Flashlight on adds
  `0.32` in Normal and `0.26` in Other Side and blends toward warm amber.

Recommended Basement active states:

```text
Inactive Normal: base 0.06
Inactive Other Side: base 0.045
Active Normal: base 0.06 + lantern 0.32
Active Other Side: base 0.045 + lantern 0.26
Local sprite/point light: 1.00-1.20 intensity
```

The global value must never make the Basement fully bright. Lantern on raises
the visible baseline enough to read interactables while preserving a dark edge.

---

## 5. Runtime components

### 5.1 `LanternController`

This lightweight scene component owns the Global Light 2D behavior and the
flashlight state. It subscribes once to `RoomChanged` and `RealityChanged`,
applies the current `RoomLightingProfile`, disables itself outside Basement and
restores a fallback profile if configuration is missing.

Suggested scene hierarchy:

```text
Main
├── Camera
├── Rooms
├── HUDRoot
└── Lighting
    └── GlobalLight2D + LanternController
```

One global light in `Main` avoids duplicating lights in every room.
It owns:

- read `HasFlashlight`;
- keep `FlashlightActive`;
- reject toggle outside Basement;
- ignore toggle input while a Story or Memory overlay is visible;
- disable automatically on `RoomChanged`;
- disable automatically on restart;
- refresh UI after state changes.

No battery or timeout should be implemented.

### 5.3 `RoomData` and catalog

Profiles belong in `DefaultRoomCatalog` or a single shared profile catalog
referenced by the room catalog. They must not be inferred from sprite names or
calculated in interactable scripts.

`RoomManager.LoadRoom(...)` remains responsible for room activation.
`RealityManager.SetReality(...)` remains responsible for Reality state.
The lighting component listens to existing events instead of polling.

---

## 6. Lantern input and HUD

The lantern icon exposes three states:

| State | Atlas sprite | Meaning |
| --- | --- | --- |
| 0 | `spritesheet-ui-icon-lantern-v1_0` | Not obtained |
| 1 | `spritesheet-ui-icon-lantern-v1_1` | Obtained / off |
| 2 | `spritesheet-ui-icon-lantern-v1_2` | Obtained / on |

Input:

- left click on the lantern icon;
- optional `F` key for keyboard parity.

The existing icon currently uses `pickingMode = Ignore`; the implementation must
change it to `PickingMode.Position`, bind `ClickEvent` and keep the `F` path as
an equivalent toggle. The icon must not overlap or replace the SHIFT button.

The exact control must be printed in the HUD/itch controls if space allows.
No new control prompt popup is required.

The icon remains visible but disabled outside the Basement. When a valid toggle
occurs:

1. set `FlashlightActive`;
2. apply the active profile;
3. refresh the atlas state;
4. reuse `SFXCue.LightSwitch`.

Invalid toggles reuse `SFXCue.UiDenied` and show:

```text
The flashlight is only useful where the room is dark.
```

---

## 7. Cursor Light 2D - deferred

The cursor-follow light is intentionally not implemented in the baseline.
It will be a later isolated extension after global lighting, gating and QA are
stable.

No cursor-tracking code, cookie sprite or second local Light 2D are required by
the current implementation.

---

## 8. Basement interaction gating

Darkness is a rule, not a free-form description check.

`InteractionGuard.CanProcess(...)` must run before an interaction is resolved
and must check:

```text
1. room and Reality visibility;
2. requiresLantern;
3. required inventory item;
4. resolved interaction.
```

An interactable with `requiresLantern = true` is blocked unless:

- current Room is Basement;
- the player has `HasFlashlight`;
- `FlashlightActive` is true.

Starting critical list:

| Interactable | Reason |
| --- | --- |
| `storage_shelf_left` | ambiguous storage / dark examination |
| `storage_shelf_right` | ambiguous storage / hidden arrangement |
| `personal_effects_box` | principal evidence |
| `pipelines` | mechanical clue |
| `pressure_release_valve` | critical `Use` action |
| `denial_manifestation` | distorted narrative shape |

Library and Forest transitions stay accessible when their existing flags allow
them. They are navigation, not key evidence. This keeps the player able to leave
the Basement if the lantern is off.

The gate applies to both left-click `Use` and right-click `Examine`.

Dark feedback examples:

```text
It is too dark to use the valve. The handle is lost in a moving shape.
It is too dark to examine the box. Personal details dissolve into blurred memories.
The shelves disappear into the darkness. I cannot tell which objects are real.
```

Every blocked key interaction shows a short narrative message and
`SFXCue.UiDenied`; it must not consume items or set flags.

---

## 9. Room and Reality transitions

### Entering Basement

- apply the Basement profile for the current Reality;
- keep the flashlight off;
- keep the HUD lantern sprite in obtained/off state.

### Leaving Basement

- set `FlashlightActive = false`;
- apply destination Room profile;
- refresh HUD lantern state.

### Reality change in Basement

- apply the Basement profile for Normal or Other Side;
- preserve active state;
- refresh interactables normally.

### Restart

- clear `FlashlightActive`;
- clear possession in `InitializeMVP`;
- clear inventory as existing behavior does;
- reset Global Light through the next Room load.

---

## 10. Implementation sequence

1. Add Global Light 2D to `Main.unity`.
2. Add `RoomLightingProfile` and profiles to the room catalog.
3. Add `LanternController` and subscribe to existing events.
4. Verify all rooms in Normal and supported realities.
5. Keep the Basement baseline without local cursor light.
6. Add `LanternController` state and HUD click.
7. Add `F` input without changing SHIFT handling.
8. Add `requiresLantern` to key Basement interactables.
9. Add blocked interaction feedback.
10. Add the active lantern atlas state.
11. Validate Library -> Basement -> Library and Forest exit.

Do not create a battery system, battery UI or standalone flashlight scene.

---

## 11. Validation matrix

| Case | Expected result |
| --- | --- |
| obtain lantern in Bedroom | HUD state changes 0 -> 1 |
| toggle outside Basement | denied, state remains off |
| enter Basement with lantern off | low ambient profile, no local light |
| toggle in Basement | state 2, Global Light intensity/color improves |
| left/right click key Basement object while off | dark feedback, no flag/item change |
| left/right click key Basement object while on | normal interaction resolves |
| SHIFT in Basement | profile changes, active state remains |
| Library -> Basement -> Library | lantern off and no stale light |
| restart | possession and active state reset |
| 640 x 360 WebGL | global light and HUD states remain readable |
| 640 x 360 Windows | no gap or sorting mismatch |

Additional checks:

- no stale global-light intensity after repeated Room changes;
- no active lantern state in rooms that disallow lantern;
- no missing global reference after UI reload;
- no consumed item or result flag on blocked interactions;
- no console warnings in cold start or restart.

---

## 12. Likely affected files

Runtime:

- `Assets/Scripts/Data/Models/MVPDataModels.cs`
- `Assets/Scripts/Rooms/DefaultRoomCatalog.cs`
- partial room catalog files where needed
- `Assets/Scripts/Rooms/RoomManager.cs`
- `Assets/Scripts/RealityManager.cs`
- `Assets/Scripts/Interaction/InteractionGuard.cs`
- `Assets/Scripts/UI/HUDController.cs`
- `Assets/Scripts/UI/HUDInventoryView.cs`
- `Assets/Scripts/Core/GameManager.cs`
- new `Assets/Scripts/Lighting/LanternController.cs`

Scene/UI:

- `Assets/Scenes/Main.unity`
- `Assets/UI/HUD.uxml`
- `Assets/UI/HUD-style-sheet.uss`
- lantern atlas Sprite 2 reference

Documentation:

- this document;
- `TD-Room-Basement.md`;
- `GD-Room-Basement.md`;
- `Puzzle-Flow.md`;
- `TD-HUD.md`;
- `MVP-SPECIFICATION.md`;
- `MVP-Sprint-4.md`.

---

## 13. Block completion criteria

Block 5 is complete only when:

- all rooms use an explicit lighting profile;
- lantern possession is independent from active state;
- global light improves the Basement in both realities;
- key interactions are blocked in the dark;
- blocked interactions do not consume items or set flags;
- Room/reality transitions do not leak light state;
- Restart restores the baseline;
- Editor, WebGL and Windows pass the validation matrix.
