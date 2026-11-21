# Room Forest - Technical Spec

- Version 0.7
- Created: 20/11/2026
- Updated: 21/11/2026

## 1. Room data

```text
id: "forest"
displayName: "Forest"
connectedRoomIds:
  - none
```

The Forest is added to the runtime room catalog and is reachable from the
resolved Basement. It is a terminal room: its only gameplay exit is forward
to the final letter and ending state. It must not expose a transition back to
the Basement.

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

## 5. Entry and terminal transition

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

Once the Forest is loaded, it has no return transition. `BasementExitDoor`
must not be present in the Forest scene hierarchy or room catalog.

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

Current implemented MVP:

- one invisible click target;
- five synchronized forest depth frames;
- four visible background layers;
- one final table and letter GameObject;
- one reusable Story Overlay for letter and ending states;
- visible Continue and Restart buttons;
- a placeholder parchment-style panel ready for a final art background;
- no inventory or puzzle requirement for the letter.

Defer:

- parallax movement;
- monster spawning behind trees;
- SHIFT-specific Forest variants;
- item or puzzle dependencies;
- dynamic object migration between depth layers.
- final letter copy and tier texts.
- a dedicated Credits scene.

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
interactionType: Use
visibleInNormalReality: true
visibleInOtherReality: true
resultFlag: ForestLetterRead
```

Visibility requirements:

- `ForestFinalTableVisible == true`;
- `GameManager.ForestDepthLevel == 6`;

Primary and secondary clicks may open or examine the letter. The interaction
must not consume an item or permanently hide the letter. Reading the letter
sets `ForestLetterRead`, but the scene object remains available if the player
closes and opens the panel again.

For the playable MVP:

- primary click executes `Use`;
- `Use` sets `ForestLetterRead` and shows a temporary debug-style UI message;
- right click executes `Examine`;
- `Examine` shows the coherent line:
  "A letter rests on the table, written in my father's handwriting.";

The debug-style primary feedback is intentionally temporary and used to verify
the interaction path. It is not the final narrative letter panel.

## 10. Letter presentation

The letter is presented as a modal narrative panel inside the existing HUD
UI Toolkit instead of by the temporary `feedback-text` label.

The conceptual structure is:

```text
HUD runtime tree
  hud-root
    existing gameplay controls
    StoryOverlay
      StoryOverlayBackdrop
      StoryOverlayPanel
        StoryOverlayEyebrow
        StoryOverlayTitle
        StoryOverlayBody
        StoryOverlayHint
```

`StoryOverlay` is a reusable HUD mode rather than a Forest-only GameObject.
It can later show letters, diary pages, terminal messages, or final farewells.
The Forest supplies the title and body text; the overlay provides the shared
presentation and close behaviour. The runtime tree is constructed by
`HUDController` because the current `PanelRenderer` does not recalculate a
hidden modal subtree reliably. The authored USS classes remain the styling
surface for the final antique-letter art.

State and behaviour:

- primary click opens `StoryOverlay`;
- right click keeps the current short examination text;
- opening the panel sets `ForestLetterRead`;
- the overlay pauses all gameplay interaction;
- SHIFT, inventory, lantern, and feedback controls are hidden;
- the Forest letter stays visible in the scene behind the overlay;
- clicking `Continue` or pressing Escape closes the overlay;
- closing the letter does not automatically close the game;
- closing the letter opens the terminal ending state.

The MVP should not add a separate LetterPanel and EndingPanel as independent
systems. A generic `StoryOverlay` with a configurable type is simpler:

```text
StoryOverlayMode
  Letter
  Ending
```

The `Letter` mode uses the close text "Continue"; the `Ending` mode uses the
ending content and may replace `StoryOverlayHint` with "Restart".

Planned HUD API:

```text
ShowStoryOverlay(title, body, hint, mode)
HideStoryOverlay()
ShowEndingOverlay(title, farewell, credits, memoryCount)
SetGameplayControlsVisible(bool)
HandleStoryOverlayEscape()
```

This keeps narrative content in callers and presentation in `HUDController`.
`UIManager` may expose the same operations as a lightweight facade.

Opening the overlay:

1. hides normal gameplay controls;
2. stores no scene or Room dependency inside the HUD;
3. accepts plain title and body strings;
4. registers close and Escape actions;
5. blocks interaction with the scene through HUD state.

Closing the letter:

1. hides `StoryOverlay`;
2. keeps gameplay controls hidden;
3. sets `ForestEndingReached`;
4. switches to the `Ending` mode directly.

No per-memory letter text should create a separate scene or room. The letter
body is selected once from the total Memory count:

| Memory count | Ending tier | Text behaviour |
| --- | --- | --- |
| 0-2 | Bad Final | The letter preserves the protagonist's denial and remains incomplete. |
| 3-4 | Neutral Final | The letter admits part of the truth but leaves the farewell unresolved. |
| 5 | True Final | The letter accepts the father's death and the protagonist's own death. |

The first implementation should use one `LetterText` value per tier.
Alternate objects or partial room illumination are deferred until the game
has one complete and testable ending loop.

## 11. Letter content structure

The letter should not explain the whole plot as exposition. It should be
written as a short message that confirms only what the protagonist is ready
to accept.

Use a short reading format:

- 1 title;
- 1-3 short paragraphs;
- approximately 40-70 words;
- no item or inventory references;
- one clear farewell sentence.

Suggested structure:

```text
Title:
  To [player-controlled protagonist name]

Body:
  [1-3 short paragraphs]
  [One line describing what occurred]
  [One final farewell]

Tier-specific final line:
  Bad Final: "I am not ready to leave."
  Neutral Final: "I understood too little, too late."
  True Final: "It is time to go together."
```

### Runtime implementation status

The MVP Story Overlay is implemented for testing:

- the primary letter click opens `StoryOverlayMode.Letter`;
- a three-tier Spanish letter is selected from `Memory_01` to `Memory_05`;
- the overlay hides gameplay controls while visible;
- Escape or the close button changes Letter mode to Ending mode;
- Ending mode shows the tier title, farewell, credits, and Memory count;
- the restart button calls the clean runtime reset and loads the Lobby;
- the letter and right-click examination remain repeatable;
- the overlay uses parchment-style placeholder colors until final art is added.

Tier meaning:

| Tier | Meaning | Emotional result |
| --- | --- | --- |
| Bad | The protagonist still rejects the evidence and farewell. | Denial and unfinished loss. |
| Neutral | The protagonist understands the death but not the full implications. | Partial acceptance and unresolved guilt. |
| True | The protagonist accepts the father's death and his own. | Farewell, mourning, and quiet hope. |

## 12. Encounter and ending

After closing the letter panel, the game enters a terminal Ending state:

1. set `ForestEndingReached`;
2. hide gameplay scene interactables;
3. hide SHIFT, inventory, lantern, and feedback;
4. keep the final table scene visible behind the overlay;
5. show the `Ending` mode of `StoryOverlay`;
6. display the tier title and farewell line;
7. display `Recovered Memories: X/5`;
8. display closing credits;
9. offer `Restart`.

For the current MVP, the ending uses the same reusable overlay as the letter:

- **Farewell:** title and tier-specific line;
- **Credits placeholder:** a short credit line followed by the Memory count and
  replay action.

The Memory count is a factual closing summary, not a collectible score.
`Restart` resets runtime flags and inventory, resets
`ForestDepthLevel` to 0, and loads the Lobby. The browser build should not
rely on a forced browser tab close.

No new room is required after the letter. The overlay completes the game
loop.

## 12.1 Credits scene plan - Sprint 4

The in-overlay credits are valid for the current Game Loop, but the target
structure for Sprint 4 is a separate `Credits` scene.

Recommended flow:

```text
Main
  Forest Letter
    -> StoryOverlay Letter
      -> StoryOverlay Ending
        -> Credits scene
          -> return to Main / Lobby
```

Load `Credits` additively instead of replacing `Main` in the MVP. This keeps:

- the persistent `GameManager`;
- the resolved ending data;
- the inventory/reset logic;
- the existing Room catalog;
- the return path without recreating runtime Singletons.

Suggested scene hierarchy:

```text
Credits
  Camera
  CreditsCanvas
    CreditsTitle
    CreditsScroll
    EndingSummary
    MemoryCount
    RestartButton
```

When entering Credits:

1. preserve `ForestEndingReached`;
2. hide the gameplay HUD in Main;
3. disable interaction input in Main;
4. display the selected ending title and Memory count;
5. play or scroll the credits.

When `Restart game` is pressed:

1. reset flags and inventory;
2. reset `ForestDepthLevel` to 0;
3. load or return to the Lobby in Main;
4. restore normal gameplay controls;
5. unload the Credits scene.

Credits should remain a small presentation scene, not a new gameplay manager.
The existing `GameManager` owns progress, while a single `CreditsController`
owns presentation and the restart command.

The alternative of replacing `Main` with `Credits` is possible, but would
require a deterministic post-load bootstrap to rebuild `RoomManager` and load
the Lobby. Additive loading is the recommended first implementation for
Sprint 4 because it avoids extra scene lifecycle state.

## 13. Recommended implementation sequence

1. ✅ Change `ForestDepthLevel` to support values 0-6.
2. ✅ Add and initialise the Forest final flags.
3. ✅ Reveal `Table`, `CeilingLight`, and `ForestRevelationLetter` at depth 6.
4. ✅ Add `forest_revelation_letter` to the Forest catalog.
5. ✅ Add primary debug feedback and right-click examination text.
6. ✅ Replace the primary debug feedback with `StoryOverlay`.
7. ✅ Add `StoryOverlayMode.Letter` to the existing HUD.
8. ✅ Add the three Memory-tier letter strings.
9. ✅ Add `StoryOverlayMode.Ending`.
10. ✅ Resolve the ending tier from `Memory_01` through `Memory_05`.
11. ✅ Add credits and `Recovered Memories: X/5`.
12. ✅ Implement replay reset.
13. Verify the complete Basement -> Forest -> Letter -> Ending loop.

### Future localization - Sprint 4

All runtime copy should remain in English until a real localization pipeline
is introduced. A future English/Spanish language selector should use Unity
Localization rather than branches or parallel text files in `HUDController`.

Recommended keys:

```text
forest.letter.bad.title
forest.letter.bad.body
forest.letter.neutral.title
forest.letter.neutral.body
forest.letter.true.title
forest.letter.true.body
ui.story_overlay.continue
ui.story_overlay.restart
ui.ending.memory_count
```

Current package status: `com.unity.localization` is not installed. For Sprint
4, install the package only when localization is started, create English and
Spanish locales, and migrate display strings without hand-rolled language
state. The UI Toolkit labels should receive values from localization services
rather than storing separate Spanish and English variables.

## 14. MVP acceptance criteria

- entering Forest starts at depth 0;
- five clicks reach the first final table transition state;
- the sixth depth step reveals the table and letter;
- the same table/letter state cannot advance the forest beyond depth 6;
- primary click opens or replaces the debug interaction with `StoryOverlay`;
- right click shows the revelation letter examination text;
- Escape or the close hint closes the overlay;
- closing the letter opens the ending state;
- the ending state shows its Memory tier and recovered Memory count;
- `Restart` restores the Lobby and runtime defaults;
- the final state does not expose the four forest background layers;
- no ritual room, inventory item, or additional puzzle is required.
