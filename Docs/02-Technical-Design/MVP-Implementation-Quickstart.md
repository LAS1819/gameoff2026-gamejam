# MVP Implementation Quickstart

## Purpose

This is the **executable checklist** for building the MVP. Every item here maps to concrete documentation. When you complete each section, the MVP will be ready for Play Mode validation.

---

## Phase 1: Scene & GameObject Setup

**Deliverable:** Bootstrap scene and Main scene with all GameObjects created and named correctly.

### 1.1 Create Bootstrap Scene

- [ ] In Unity, create new scene: `Scenes/Bootstrap.unity`
- [ ] Create GameObject: `Systems` (root parent)
- [ ] Do NOT add any other components; this is a container only
- [ ] Create child GameObjects under Systems (one per manager):
  - [ ] `GameManager`
  - [ ] `RoomManager`
  - [ ] `InventoryManager`
  - [ ] `InteractionManager`
  - [ ] `RealityManager`
  - [ ] `EventManager`
  - [ ] `SaveManager`
  - [ ] `UIManager`
- [ ] Do NOT add scripts yet; just structure

**Reference:** [MVP-Concrete-Hierarchy.md](./MVP-Concrete-Hierarchy.md) — Scene: Bootstrap

---

### 1.2 Create Main Gameplay Scene

- [ ] In Unity, create new scene: `Scenes/Main.unity`
- [ ] Create GameObject: `Camera` with Camera component
  - [ ] Position: (0, 0, -10) or suitable for your art
  - [ ] Orthographic: true
  - [ ] Size: 5 or 10 (adjust based on art scale)
- [ ] Create GameObject: `Rooms` (empty parent)
- [ ] Create GameObject: `FX` (empty parent, reserved for effects)
- [ ] Create GameObject: `UI` (empty parent, reserved for UI)

**Reference:** [MVP-Concrete-Hierarchy.md](./MVP-Concrete-Hierarchy.md) — Scene: Main Gameplay

---

### 1.3 Create Vestibule Room Structure

Inside `Rooms`, create:

- [ ] GameObject: `Vestibule` (room parent)
  - [ ] Child: `Visuals` (empty parent)
    - [ ] Child: `Background` (SpriteRenderer)
      - [ ] Sprite: placeholder or vestibule art
      - [ ] Sorting Order: 0
  - [ ] Child: `Props` (empty parent)
    - [ ] Child: `DoorFrame` (SpriteRenderer, optional decorative element)
      - [ ] Sprite: door frame art
      - [ ] Sorting Order: 1
  - [ ] Child: `Interactables` (empty parent)
    - [ ] Child: `StaffKey` (GameObject)
    - [ ] Child: `FrontDoor` (GameObject)
    - [ ] Child: `HallwayEntrance` (GameObject)

**Reference:** [MVP-Concrete-Hierarchy.md](./MVP-Concrete-Hierarchy.md) — Vestibule Room

---

### 1.4 Create Hallway Room Structure

Inside `Rooms`, create:

- [ ] GameObject: `Hallway` (room parent)
  - [ ] Child: `Visuals` (empty parent)
    - [ ] Child: `Background` (SpriteRenderer)
      - [ ] Sprite: placeholder or hallway art
      - [ ] Sorting Order: 0
  - [ ] Child: `Props` (empty parent)
    - [ ] Child: `Walls` (SpriteRenderer, optional decorative element)
      - [ ] Sprite: corridor/walls art
      - [ ] Sorting Order: 1
  - [ ] Child: `Interactables` (empty parent)
    - [ ] Child: `HallwayObstacle` (GameObject)
    - [ ] Child: `ShiftTrigger` (GameObject)
    - [ ] Child: `VestibuleExit` (GameObject)

**Reference:** [MVP-Concrete-Hierarchy.md](./MVP-Concrete-Hierarchy.md) — Hallway Room

---

## Phase 2: Interactable Setup

**Deliverable:** All 6 interactables configured with exact data matching the spec.

### 2.1 StaffKey (Vestibule)

- [ ] GameObject: `StaffKey`
- [ ] Add Component: SpriteRenderer
  - [ ] Sprite: key sprite
  - [ ] Sorting Order: 2
  - [ ] Position: (3, 2, 0) or suitable position
- [ ] Add Component: BoxCollider2D
  - [ ] Is Trigger: true
  - [ ] Size: fit the sprite (e.g., 0.5, 0.5)
- [ ] Add Script: `InteractableBehaviour.cs` (or existing equivalent)
- [ ] Add Serialized InteractableData:
  - [ ] id: `staff_key`
  - [ ] displayName: `Staff Key`
  - [ ] description: `An ornate brass key. It's warm to the touch.`
  - [ ] interactionType: `Pickup`
  - [ ] visibleInNormalReality: `true`
  - [ ] visibleInOtherReality: `false`
  - [ ] itemId: `staff_key`
  - [ ] requiredItemId: (leave empty/null)
  - [ ] resultFlag: `HasStaffKey`
  - [ ] consumeAfterUse: `true`

**Reference:** [MVP-Data-Specification.md](./MVP-Data-Specification.md) — Section 6, Vestibule, Interactable 1

---

### 2.2 FrontDoor (Vestibule)

- [ ] GameObject: `FrontDoor`
- [ ] Add Component: SpriteRenderer
  - [ ] Sprite: door sprite
  - [ ] Sorting Order: 2
  - [ ] Position: (5, 0, 0) or suitable position
- [ ] Add Component: BoxCollider2D
  - [ ] Is Trigger: true
  - [ ] Size: fit the sprite (e.g., 2, 4)
- [ ] Add Script: `InteractableBehaviour.cs`
- [ ] Add Serialized InteractableData:
  - [ ] id: `front_door`
  - [ ] displayName: `Front Door`
  - [ ] description: `A heavy wooden door, locked with a brass keyhole.`
  - [ ] interactionType: `Use`
  - [ ] visibleInNormalReality: `true`
  - [ ] visibleInOtherReality: `true`
  - [ ] itemId: (leave empty/null)
  - [ ] requiredItemId: `staff_key`
  - [ ] resultFlag: `DoorOpened`
  - [ ] consumeAfterUse: `false`

**Reference:** [MVP-Data-Specification.md](./MVP-Data-Specification.md) — Section 6, Vestibule, Interactable 2

---

### 2.3 HallwayEntrance (Vestibule)

- [ ] GameObject: `HallwayEntrance`
- [ ] Add Component: SpriteRenderer (optional, can be invisible)
  - [ ] Sprite: transition marker or invisible sprite
  - [ ] Sorting Order: 1
  - [ ] Position: (7, 0, 0) or suitable position
- [ ] Add Component: BoxCollider2D
  - [ ] Is Trigger: true
  - [ ] Size: (1, 3) or larger area
- [ ] Add Script: `InteractableBehaviour.cs`
- [ ] Add Serialized InteractableData:
  - [ ] id: `hallway_entrance`
  - [ ] displayName: `Hallway`
  - [ ] description: `You can see further into the darkness...`
  - [ ] interactionType: `Transition`
  - [ ] visibleInNormalReality: `true`
  - [ ] visibleInOtherReality: `true`
  - [ ] itemId: (leave empty/null)
  - [ ] requiredItemId: (leave empty/null)
  - [ ] resultFlag: (leave empty/null)
  - [ ] linkedRoomId: `hallway`
  - [ ] consumeAfterUse: `false`

**Reference:** [MVP-Data-Specification.md](./MVP-Data-Specification.md) — Section 6, Vestibule, Interactable 3

---

### 2.4 HallwayObstacle (Hallway)

- [ ] GameObject: `HallwayObstacle`
- [ ] Add Component: SpriteRenderer
  - [ ] Sprite: stone figure or obstacle sprite
  - [ ] Sorting Order: 2
  - [ ] Position: (5, 0, 0) or suitable position
- [ ] Add Component: BoxCollider2D
  - [ ] Is Trigger: true
  - [ ] Size: fit the sprite (e.g., 2, 3)
- [ ] Add Script: `InteractableBehaviour.cs`
- [ ] Add Script: `RealityVisibilityController.cs` (update SpriteRenderer based on visibility)
- [ ] Add Serialized InteractableData:
  - [ ] id: `hallway_obstacle`
  - [ ] displayName: `Massive Stone Figure`
  - [ ] description: `A towering stone figure blocks your path. It's watching you.`
  - [ ] interactionType: `Examine`
  - [ ] visibleInNormalReality: `true`
  - [ ] visibleInOtherReality: `false`
  - [ ] itemId: (leave empty/null)
  - [ ] requiredItemId: (leave empty/null)
  - [ ] resultFlag: `RealityUnlocked`
  - [ ] consumeAfterUse: `false`

**Reference:** [MVP-Data-Specification.md](./MVP-Data-Specification.md) — Section 6, Hallway, Interactable 1

---

### 2.5 ShiftTrigger (Hallway)

- [ ] GameObject: `ShiftTrigger`
- [ ] Add Component: SpriteRenderer (optional, can be invisible or subtle)
  - [ ] Sprite: shimmer or invisible sprite
  - [ ] Sorting Order: 1
  - [ ] Position: (10, 0, 0) or suitable position
- [ ] Add Component: BoxCollider2D
  - [ ] Is Trigger: true
  - [ ] Size: (2, 2)
- [ ] Add Script: `InteractableBehaviour.cs`
- [ ] Add Serialized InteractableData:
  - [ ] id: `shift_trigger`
  - [ ] displayName: `The Other Side`
  - [ ] description: `Reality feels thin here...`
  - [ ] interactionType: `Toggle`
  - [ ] visibleInNormalReality: `true`
  - [ ] visibleInOtherReality: `true`
  - [ ] itemId: (leave empty/null)
  - [ ] requiredItemId: (leave empty/null)
  - [ ] resultFlag: `ShiftPerformed`
  - [ ] consumeAfterUse: `false`

**Reference:** [MVP-Data-Specification.md](./MVP-Data-Specification.md) — Section 6, Hallway, Interactable 2

---

### 2.6 VestibuleExit (Hallway)

- [ ] GameObject: `VestibuleExit`
- [ ] Add Component: SpriteRenderer (optional, can be invisible)
  - [ ] Sprite: marker or invisible sprite
  - [ ] Sorting Order: 1
  - [ ] Position: (-5, 0, 0) or suitable position
- [ ] Add Component: BoxCollider2D
  - [ ] Is Trigger: true
  - [ ] Size: (1, 3)
- [ ] Add Script: `InteractableBehaviour.cs`
- [ ] Add Serialized InteractableData:
  - [ ] id: `vestibule_exit`
  - [ ] displayName: `Back to Vestibule`
  - [ ] description: `The path back.`
  - [ ] interactionType: `Transition`
  - [ ] visibleInNormalReality: `true`
  - [ ] visibleInOtherReality: `true`
  - [ ] itemId: (leave empty/null)
  - [ ] requiredItemId: (leave empty/null)
  - [ ] resultFlag: (leave empty/null)
  - [ ] linkedRoomId: `vestibule`
  - [ ] consumeAfterUse: `false`

**Reference:** [MVP-Data-Specification.md](./MVP-Data-Specification.md) — Section 6, Hallway, Interactable 3

---

## Phase 3: Script Configuration

**Deliverable:** All manager scripts attached and configured correctly.

### 3.1 Attach Scripts to Bootstrap Managers

In the Bootstrap scene, add scripts to each manager GameObject:

- [ ] GameManager (GameObject) → add script: `GameManager.cs`
  - Should call `InitializeMVP()` on Awake
  - Should build `RoomData` for Vestibule and Hallway
  - Should load room "vestibule"
- [ ] RoomManager (GameObject) → add script: `RoomManager.cs`
  - Should receive room registry from GameManager
  - Should load "vestibule" on startup
- [ ] InventoryManager (GameObject) → add script: `InventoryManager.cs`
- [ ] InteractionManager (GameObject) → add script: `InteractionManager.cs`
- [ ] RealityManager (GameObject) → add script: `RealityManager.cs`
- [ ] EventManager (GameObject) → add script: `EventManager.cs` (can be stub)
- [ ] SaveManager (GameObject) → add script: `SaveManager.cs` (can be stub)
- [ ] UIManager (GameObject) → add script: `UIManager.cs` (can be stub)

**Reference:** [Assets/Scripts/](../../../Assets/Scripts/)

---

### 3.2 Mark Systems as DontDestroyOnLoad

Ensure `Systems` GameObject is marked as persistent:

- [ ] `GameManager.cs` calls `DontDestroyOnLoad(gameObject)` in Awake

**Reference:** [Assets/Scripts/GameManager.cs](../../../Assets/Scripts/GameManager.cs)

---

### 3.3 Create InteractableBehaviour Script

Create or use an existing script that handles click interactions:

```csharp
public class InteractableBehaviour : MonoBehaviour
{
    public InteractableData data;

    private void OnMouseDown()
    {
        if (InteractionManager.Instance != null)
        {
            InteractionManager.Instance.TryInteract(data.id);
        }
    }
}
```

- [ ] Create `Assets/Scripts/InteractableBehaviour.cs`
- [ ] Attach to all 6 interactables
- [ ] Set the `data` field in Inspector for each interactable

**Reference:** [MVP-Concrete-Hierarchy.md](./MVP-Concrete-Hierarchy.md) — Component Attachment Summary

---

### 3.4 Create RealityVisibilityController Script (Optional)

For objects that should toggle visibility based on reality:

```csharp
public class RealityVisibilityController : MonoBehaviour
{
    public InteractableData data;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        RealityManager.Instance.RealityChanged += OnRealityChanged;
        RefreshVisibility();
    }

    private void OnRealityChanged(RealityType newReality)
    {
        RefreshVisibility();
    }

    private void RefreshVisibility()
    {
        bool shouldBeVisible = RealityManager.Instance.CurrentReality == RealityType.Normal
            ? data.visibleInNormalReality
            : data.visibleInOtherReality;

        spriteRenderer.enabled = shouldBeVisible;
    }

    private void OnDestroy()
    {
        if (RealityManager.Instance != null)
            RealityManager.Instance.RealityChanged -= OnRealityChanged;
    }
}
```

- [ ] Create `Assets/Scripts/RealityVisibilityController.cs`
- [ ] Attach to `HallwayObstacle` (and any other reality-dependent objects)
- [ ] Set the `data` field in Inspector

**Reference:** [MVP-Flag-Sequence.md](./MVP-Flag-Sequence.md) — Step 5: Perform Reality Shift

---

## Phase 4: Data Initialization

**Deliverable:** GameManager correctly builds and loads rooms at startup.

### 4.1 Update GameManager.BuildDefaultRooms()

Ensure `GameManager.cs` has a `BuildDefaultRooms()` method that creates RoomData for Vestibule and Hallway:

- [ ] Method creates vestibule RoomData with 3 interactables
- [ ] Method creates hallway RoomData with 3 interactables
- [ ] Method registers both rooms in a dictionary
- [ ] Method calls `RoomManager.SetRooms(roomRegistry)`

**Reference:** [MVP-Data-Specification.md](./MVP-Data-Specification.md) — Section 8: Data Initialization

---

### 4.2 Verify Flag Initialization

- [ ] GameManager initializes all 4 MVP flags to false on startup
  - [ ] `HasStaffKey = false`
  - [ ] `DoorOpened = false`
  - [ ] `RealityUnlocked = false`
  - [ ] `ShiftPerformed = false`

**Reference:** [MVP-Flag-Sequence.md](./MVP-Flag-Sequence.md) — Section: MVP Global Flags

---

## Phase 5: Play Mode Validation

**Deliverable:** Complete execution of the MVP sequence from game start to reality shift.

### 5.1 Set Startup Scenes

- [ ] File → Build Settings → Scenes In Build
- [ ] Add: `Scenes/Bootstrap.unity` (Scene 0)
- [ ] Add: `Scenes/Main.unity` (Scene 1)
- [ ] Bootstrap is loaded first; Main is loaded after

---

### 5.2 Validation Sequence

Follow this exact sequence in Play Mode:

1. [ ] **Start Game**
   - [ ] Verify Vestibule loads
   - [ ] Verify StaffKey is visible
   - [ ] Verify FrontDoor is visible
   - [ ] Verify HallwayEntrance is visible
   - [ ] Open Console; verify no errors

2. [ ] **Pick Up Staff Key**
   - [ ] Click StaffKey in scene
   - [ ] Verify StaffKey disappears from scene
   - [ ] Verify Console shows interaction event
   - [ ] Verify `HasStaffKey` flag is set to true

3. [ ] **Use Key on Door**
   - [ ] Click FrontDoor
   - [ ] Verify FrontDoor sprite changes or opens
   - [ ] Verify Console shows interaction event
   - [ ] Verify `DoorOpened` flag is set to true

4. [ ] **Enter Hallway**
   - [ ] Click HallwayEntrance
   - [ ] Verify scene transitions to Hallway
   - [ ] Verify HallwayObstacle is visible
   - [ ] Verify ShiftTrigger is visible
   - [ ] Verify VestibuleExit is visible

5. [ ] **Examine Obstacle**
   - [ ] Click HallwayObstacle
   - [ ] Verify Console shows examination description
   - [ ] Verify `RealityUnlocked` flag is set to true

6. [ ] **Perform Reality Shift**
   - [ ] Click ShiftTrigger
   - [ ] Verify reality changes (visual effect, background change, or other feedback)
   - [ ] Verify HallwayObstacle disappears (not visible in OtherSide)
   - [ ] Verify `ShiftPerformed` flag is set to true

7. [ ] **Shift Back**
   - [ ] Click ShiftTrigger again
   - [ ] Verify reality changes back to Normal
   - [ ] Verify HallwayObstacle reappears

8. [ ] **Return to Vestibule**
   - [ ] Click VestibuleExit
   - [ ] Verify scene transitions back to Vestibule
   - [ ] Verify all flags are preserved (not reset)
   - [ ] Verify StaffKey is still gone (consumed state preserved)

**Reference:** [MVP-Flag-Sequence.md](./MVP-Flag-Sequence.md) — Play Mode Validation Sequence

---

## Phase 6: Finalization

- [ ] All 4 phases complete
- [ ] Play Mode validation passes all 8 steps
- [ ] No Console errors
- [ ] Commit changes to git
- [ ] Update sprint progress in [MVP-Sprint-1.md](./MVP-Sprint-1.md)

---

## Troubleshooting

| Issue | Solution | Reference |
|-------|----------|-----------|
| "Interactable not found" error | Verify interactable `id` matches exactly in InteractableData | [MVP-Data-Specification.md](./MVP-Data-Specification.md) |
| Flag not being set | Check InteractionManager logic; verify resultFlag field is not empty | [MVP-Flag-Sequence.md](./MVP-Flag-Sequence.md) |
| Room doesn't load | Verify linkedRoomId is correct and room exists in registry | [MVP-Data-Specification.md](./MVP-Data-Specification.md) |
| Object visibility not updating | Check RealityVisibilityController script is attached and visibleInNormalReality/visibleInOtherReality are set | [MVP-Concrete-Hierarchy.md](./MVP-Concrete-Hierarchy.md) |
| Systems not persisting | Verify `DontDestroyOnLoad` is called on Systems GameObject | [MVP-Concrete-Hierarchy.md](./MVP-Concrete-Hierarchy.md) — Scene: Bootstrap |

---

## Related Documentation

- [MVP-Data-Specification.md](./MVP-Data-Specification.md) — exact field values for all data
- [MVP-Concrete-Hierarchy.md](./MVP-Concrete-Hierarchy.md) — GameObject hierarchy and naming
- [MVP-Flag-Sequence.md](./MVP-Flag-Sequence.md) — execution flow and state transitions
- [MVP-Sprint-1.md](./MVP-Sprint-1.md) — sprint goals and acceptance criteria
- [Technical-Design.md](./Technical-Design.md) — high-level architecture overview
