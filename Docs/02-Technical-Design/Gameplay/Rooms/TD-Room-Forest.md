# Room Forest - Technical Spec

- Version 0.4
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

The scene must remain fixed. The depth is represented by swapping five sprites
in all four layers.

At scene load, the level 6 objects start inactive:

- `ForestRevelationLetter`;
- `Table`;
- `CeilingLight`.

The Forest background remains active for depth levels 0-5. At depth 6,
`BackgroundNormal` is disabled and the level 6 props become active.

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
8. State 6 also reveals `CeilingLight`.
9. State 6 disables `BackgroundNormal`, leaving the black scene environment
   behind the table and its props.

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

For the playable MVP:

- primary click executes `Use`;
- `Use` sets `ForestLetterRead` and shows a temporary debug-style UI message;
- right click executes `Examine`;
- `Examine` shows the coherent line:
  "A letter rests on the table, written in my father's handwriting.";

The debug-style primary feedback is intentionally temporary and used to verify
the interaction path. It is not the final narrative letter panel.

## 10. Letter presentation

The first MVP keeps the existing `feedback-text` HUD for this interaction.
This avoids adding a full letter panel before the complete letter copy and
ending screen are defined.

The primary click:

- sets `ForestLetterRead`;
- shows a temporary debug-style feedback message.

The right-click examine:

- does not set `ForestLetterRead`;
- shows: "A letter rests on the table, written in my father's handwriting.";
- keeps the interaction readable and repeatable.

A full `LetterPanel` remains a planned presentation step after the MVP text
and ending copy are approved.

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

The final letter panel and terminal ending screen remain outside this
implementation pass. They will be added after the debug interaction is
confirmed in Play Mode.

When implemented, the ending should still reuse the existing HUD instead of
creating another room:

1. hide gameplay scene interactables;
2. hide the SHIFT and inventory HUD;
3. show the letter or ending panel;
4. display one farewell line appropriate to the tier;
5. display `Recovered Memories: X/5`;
6. display closing credits;
7. offer `Restart`.

No new room is required after the letter.

## 13. Recommended implementation sequence

1. ✅ Change `ForestDepthLevel` to support values 0-6.
2. ✅ Add and initialise the Forest final flags.
3. ✅ Reveal `Table`, `CeilingLight`, and `ForestRevelationLetter` at depth 6.
4. ✅ Add `forest_revelation_letter` to the Forest catalog.
5. ✅ Add primary debug feedback and right-click examination text.
6. Implement the letter panel.
7. Add the three Memory-tier strings.
8. Add the terminal ending panel.
9. Resolve the ending tier from `Memory_01` through `Memory_05`.
10. Implement replay reset and verify the complete loop.

## 14. MVP acceptance criteria

- entering Forest starts at depth 0;
- five clicks reach the first final table transition state;
- the sixth depth step reveals the table and letter;
- the same table/letter state cannot advance the forest beyond depth 6;
- primary click shows temporary debug text and sets `ForestLetterRead`;
- right click shows the revelation letter examination text;
- the final state does not expose the five forest background layers;
- a final terminal screen and replay remain planned follow-up work;
- no ritual room, inventory item, or additional puzzle is required.
