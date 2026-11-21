# Room Forest - Technical Spec

- Version 0.3
- Created: 20/11/2026
- Updated: 21/11/2026

## 1. Room data

```text
id: "forest"
displayName: "Forest"
connectedRoomIds:
  - "basement"
```

The Forest is added to the runtime room catalog and is reachable from the resolved Basement.

## 2. Scene hierarchy

```text
Main
  Rooms
    Forest
      Visuals
        BackgroundNormal
          Sky
          CentralPath
          BorderTrees
          OuterForest
      Interactables
        ForestAdvance
        ForestRevelationLetter
      Props
        Table
        CeilingLight        
```

The four background visual objects represent depth-independent layers:

- `Sky`: Capa -3.
- `OuterForest`: Capa -2.
- `BorderTrees`: Capa -1.
- `CentralPath`: Capa 0.

The scene must remain fixed. The depth is represented by swapping five sprites in all four layers.
The `Table` and `ForestRevelationLetter` are hidden until the player completes the fifth
depth level and requests the final transition.

## 3. Depth controller

`ForestDepthController` owns the visual transition:

- reads `GameManager.ForestDepthLevel`;
- listens to `ForestDepthChanged`;
- replaces each `SpriteRenderer.sprite` with the matching frame;
- clamps frame indices to the available array length.

The controller uses the existing spritesheets:

- `spritesheet-room-forest-background-central-path-v1.png`
- `spritesheet-room-forest-background-border-trees-v1.png`
- `spritesheet-room-forest-background-outer-forest-v1.png`
- `spritesheet-room-forest-background-sky-v1.png`
The `Sky` is the lowest visual layer and darkens with each depth level.

## 4. Forest advance

The room contains an invisible collider named `ForestAdvance`.

```text
id: "forest_advance"
interactionType: Use
visibleInNormalReality: true
visibleInOtherReality: true
resultFlag: ForestProgressAdvanced
```

Behaviour:

1. A primary click calls the existing interaction pipeline.
2. `ResolveForestAdvance` increments `ForestDepthLevel`.
3. Values 0-5 update the five forest frames.
4. At depth 5, the same click advances to state 6.
5. State 6 is clamped and no longer changes the forest frames.
6. `ForestDepthChanged` updates all four visual layers.
7. State 6 reveals `Table` and `ForestRevelationLetter`
   and disables `ForestAdvance`.

The advance must not consume an inventory item.

## 5. Basement transition

The existing `forest_exit_door` in the Basement becomes a transition interactable:

```text
interactionType: Transition
linkedRoomId: "forest"
resultFlag: ForestExitUnlocked
```

The transition is available only when:

- `PressureValveRepaired == true`;
- `SteamEnabled == true`;
- `DenialManifestationRejected == true`.

The existing special resolver blocks the transition in `OtherSide` and loads Forest in `Normal`.

## 6. Flags

| Flag | Description |
| --- | --- |
| ForestEntered | Forest has been loaded |
| ForestExitUnlocked | The resolved Basement allows entry to Forest |
| ForestProgressAdvanced | At least one Forest depth step has been taken |
| ForestFinalTableVisible | The final table and letter have appeared |
| ForestLetterRead | The player has opened and read the revelation letter |
| ForestEndingReached | The player has reached the terminal ending screen |

The five optional Memory flags must also exist in runtime state:

```text
Memory_01
Memory_02
Memory_03
Memory_04
Memory_05
```

The ending tier is calculated once, exclusively from the number of true
Memory flags:

- `0-2`: Final Malo;
- `3-4`: Final Neutral;
- `5`: Final Verdadero.

The current depth is stored as `GameManager.ForestDepthLevel`, with values 0
to 6:

- `0`: Forest entry.
- `1-5`: depth frames.
- `6`: Habitación Final / table and ForestRevelationLetter state.

## 7. MVP scope

Maintain:

- one invisible click target;
- five synchronized forest depth frames;
- four visible background layers;
- one final table and letter GameObject;
- one letter text overlay;
- one terminal ending screen;
- a return transition to the Basement;
- no inventory or puzzle requirement for the letter.

Defer:

- parallax movement;
- monster spawning behind trees;
- SHIFT-specific Forest variants;
- item or puzzle dependencies;
- dynamic object migration between depth layers.

## 8. Final depth transition

At depth 5, the five forest frames remain visible and `ForestAdvance` remains
the active target. The next primary click does not change the sky or the other
forest layers again. Instead it performs the impossible transition documented
by the Forest GDD:

```text
Depth 5
  -> ForestFinalTableVisible = true
  -> ForestAdvance hidden
  -> Table / ForestRevelationLetter visible
  -> Depth = 6
```

The existing four layers may remain in place as the visual base. No room
reload and no visible slide or fade are required for the MVP. The appearance
of the table must feel physically impossible through composition and scale,
not through a complex transition system.

The final table is a separate visual GameObject and must not be implemented
inside the spritesheets.

## 9. Letter interactable

The letter uses the existing `InteractableBehaviour` and `InteractableData`
systems:

```text
id: "forest_revelation_letter"
displayName: "Letter"
interactionType: Examine
visibleInNormalReality: true
visibleInOtherReality: true
resultFlag: ForestLetterRead
```

Visibility requirements:

- `ForestFinalTableVisible == true`;
- `GameManager.ForestDepthLevel == 6`;
- `ForestLetterRead == false`.

Primary and secondary clicks may open the letter. The interaction must not
consume an item or permanently hide the letter before the reading is complete.

## 10. Letter presentation

The temporary `feedback-text` HUD is not suitable for this moment because it
is short-lived and visually competes with the scene. The MVP should extend the
existing HUD UI Toolkit document with one central `LetterPanel`.

Recommended elements:

```text
HUD.uxml
  LetterPanel
    LetterTitle
    LetterBody
    LetterCloseHint

  EndingPanel
    EndingTitle
    EndingBody
    MemoriesCount
    RestartButton
```

The letter panel should:

- occupy a centered, fixed-size region;
- use a solid or nearly solid panel background;
- pause gameplay interactions while visible;
- hide the normal feedback, inventory, SHIFT, and lantern controls;
- close on the `LetterCloseHint` click or Escape;
- set `ForestLetterRead` when the panel opens.

For a clear and testable MVP, set `ForestLetterRead` when the panel opens.
This prevents repeated interaction but keeps the visual letter as a readable
object. Closing the panel is what advances to the ending screen.

No per-memory letter text should create a separate scene or room. The text is
selected once from the total Memory count:

| Memory count | Ending tier | Text behaviour |
| --- | --- | --- |
| 0-2 | Final Malo | The letter preserves the protagonist's denial and remains incomplete. |
| 3-4 | Final Neutral | The letter admits part of the truth but leaves the farewell unresolved. |
| 5 | Final Verdadero | The letter accepts the father's death and the protagonist's own death. |

The first implementation should use one `LetterText` value per tier.
Alternate objects or partial room illumination are deferred until the game
has one complete and testable ending loop.

## 11. Letter content structure

The letter should not explain the whole plot as exposition. It should be
written as a short message that confirms only what the protagonist is ready
to accept.

Suggested structure:

```text
Title:
  To [player-controlled protagonist name]

Body:
  [1-3 short paragraphs]
  [One line describing what occurred]
  [One final farewell]

Tier-specific final line:
  Bad: "I am not ready to leave."
  Neutral: "I understood too little, too late."
  True: "It is time to go together."
```

The exact final Spanish copy belongs to the narrative pass, but the technical
system only needs to receive a prepared string. It must not assemble the
letter paragraph by paragraph from items or object flags.

## 12. Encounter and ending

After the letter panel closes, the MVP should show a terminal ending state:

1. hide gameplay scene interactables;
2. hide the SHIFT and inventory HUD;
3. show the `EndingPanel`;
4. display one short farewell line appropriate to the tier;
5. display `Memories recuperadas: X/5`;
6. display closing credits;
7. offer `Volver a empezar` or `Salir`.

The MVP can implement `Volver a empezar` by resetting runtime flags and
loading the Lobby. `Salir` is optional in the browser build because WebGL
cannot reliably close its own tab. The ending screen may therefore offer only
a replay action and a passive credits section.

No new room is required after the letter. The ending panel completes the
game loop.

## 13. Recommended implementation sequence

1. Change `ForestDepthLevel` to support values 0-6.
2. Add and initialise the Forest final flags.
3. Reveal `FinalTable` and `Letter` at depth 6.
4. Add `forest_revelation_letter` to the Forest catalog.
5. Add `LetterPanel` to the existing HUD UXML/USS.
6. Add three strings for the 0-2, 3-4, and 5 Memory tiers.
7. Add `EndingPanel` to the existing HUD UXML/USS.
8. Resolve the ending tier from `Memory_01` through `Memory_05`.
9. Implement the optional replay reset.
10. Verify the complete Basement -> Forest -> Letter -> Ending loop.

## 14. MVP acceptance criteria

- entering Forest starts at depth 0;
- five clicks reach the first final table transition state;
- the sixth depth step reveals the table and letter;
- the same table/letter state cannot advance the forest beyond depth 6;
- reading the letter opens a centered, readable panel;
- closing the letter reaches a terminal screen;
- the terminal screen shows the recovered Memory count;
- the terminal screen does not expose gameplay controls;
- replay restores all runtime flags and starts again in the Lobby;
- no ritual room, inventory item, or additional puzzle is required.
