# Room Architecture Pattern - MVP

- Versión 0.4
- Creado: 10/11/2026
- Actualizado: 10/11/2026

---

## Objetivo

Mantener `GameManager` como gestor de arranque y mover la definición de rooms/interactables a una capa de contenido. Así evitamos mezclar runtime state con data del juego.

### Regla base

- `GameManager`: bootstrap, flags globales, room inicial.
- `DefaultRoomCatalog`: define rooms y `InteractableData`.
- `RoomFactory`: opcional, materializa GameObjects desde la data.
- `RoomManager`: room activa y visibilidad.
- `InteractableBehaviour`: click + visibilidad.
- `InteractableData`: fuente de verdad.

Seleccionamos Opción A: data en código para el MVP.

---

## Problema actual

`GameManager` ya hace demasiado:

- runtime systems;
- flags base;
- creación de `RoomData`;
- creación de `InteractableData`;
- carga de rooms.

Funciona, pero mezcla contenido, flujo y estado global. No es sostenible a medio plazo.

---

## Estructura recomendada

### `GameManager`

Responsabilidad:

- `EnsureRuntimeSystems()`
- `InitializeMVP()`
- `SetFlag()` / `HasFlag()`
- `SetCurrentRoom()`
- cargar room inicial

Debe quedar limpio y no construir rooms manualmente.

### `DefaultRoomCatalog`

```csharp
public static class DefaultRoomCatalog
{
    public static List<RoomData> BuildMvpRooms()
    {
        var lobby = BuildLobby();
        var hallway = BuildHallway();
        var bedroom = BuildBedroom();
        return new List<RoomData> { lobby, hallway, bedroom };
    }
}
```

Responsabilidad:

- construir `RoomData` por room;
- crear `InteractableData`;
- definir `connectedRoomIds`, `requiredItemId`, `linkedRoomId`, flags etc.

### `RoomFactory` (opcional, recomendado si se instancian objetos)

```csharp
public class RoomFactory : MonoBehaviour
{
    public GameObject CreateRoom(RoomData room) { ... }
    public GameObject CreateInteractable(InteractableData data) { ... }
}
```

Responsabilidad:

- instanciar GO de room y interactables;
- enlazar `InteractableBehaviour` con `InteractableData`;
- aplicar visibilidad inicial.

### `RoomManager`

Responsabilidad:

- `SetRooms(List<RoomData>)`
- `LoadRoom(string roomId)`
- `GetCurrentRoom()`
- `IsInteractableVisible(InteractableData data)`
- gestionar estado activo de cada room

### `InteractableBehaviour`

```csharp
public class InteractableBehaviour : MonoBehaviour
{
    public void Configure(InteractableData data) { ... }
    private void OnMouseDown() { InteractionManager.Instance.TryInteract(data); }
}
```

Responsabilidad:

- recibir `InteractableData`;
- click + visibilidad;
- delegar al `InteractionManager`.

No debe crear data ni decidir qué room existe.

### `InteractionManager`

Responsabilidad:

- `TryInteract(InteractableData data)`
- `TryPickup`, `TryUse`, `TryTransition`, `TryToggle`
- validar items y flags

### `InteractableData`

Fuente de verdad del interactable.

Debe incluir:

- id
- displayName
- description
- interactionType
- visibleInNormalReality
- visibleInOtherReality
- requiredItemId
- resultFlag
- linkedRoomId
- consumeAfterUse

---

## Clases concretas recomendadas

- `Assets/Scripts/Rooms/DefaultRoomCatalog.cs`
- `Assets/Scripts/Rooms/RoomManager.cs`
- `Assets/Scripts/Rooms/RoomFactory.cs` (opcional)
- `Assets/Scripts/InteractableBehaviour.cs`
- `Assets/Scripts/InteractionManager.cs`
- `Assets/Scripts/GameManager.cs`

`RoomData` y `InteractableData` ya existen y deben mantenerse como modelos centrales.

---

## Patrón de inicialización

```csharp
public void InitializeMVP()
{
    Flags.Clear();
    SetFlag(GameFlagNames.HasStaffKey, false);
    SetFlag(GameFlagNames.DoorOpened, false);
    SetFlag(GameFlagNames.RealityUnlocked, false);
    SetFlag(GameFlagNames.ShiftPerformed, false);

    EnsureRuntimeSystems();

    if (RoomManager.Instance != null)
    {
        RoomManager.Instance.SetRooms(DefaultRoomCatalog.BuildMvpRooms());
        RoomManager.Instance.LoadRoom(GameManager.LobbyRoomId);
    }
}
```

Esto deja `GameManager` limpio y separa claramente contenido y runtime.

---

## Decisión del editor

No mezclar dos fuentes de verdad.

Para este proyecto:

- data en código para rooms/interactables;
- editor solo para prefabs, colliders, sprites y ajustes visuales.

Esto evita inconsistencias y mantiene el MVP legible.

---

## Separación final

```text
GameManager        -> bootstrap + flags + room start
DefaultRoomCatalog -> room definitions and interactable data
RoomFactory        -> instantiate room content (optional)
RoomManager        -> room active state and visibility
InteractableBehaviour -> click + visibility bridge
InteractionManager -> resolve actions and requirements
InteractableData   -> source of truth
```

---

## Recomendación final

Mantener esta arquitectura simple:

- `GameManager` no define rooms a mano;
- `DefaultRoomCatalog` crea la data del juego;
- `RoomManager` gestiona room state;
- `InteractableBehaviour` solo responde a interacción;
- `RoomFactory` materializa el contenido si hace falta.

Con eso, Lobby/Hallway/Bedroom quedan bien separados y el proyecto sigue simple.
