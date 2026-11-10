# Room Architecture Pattern - MVP

- Versión 0.2
- Creado: 10/11/2026
- Actualizado: 10/11/2026

---

## Objetivo

Mantener `GameManager` como gestor de arranque y dejar la definición de rooms/interactables en una capa de contenido. Esto reduce responsabilidad, evita “God Manager” y facilita añadir Lobby/Hallway/Bedroom sin mezclar runtime state con data del juego.

## Regla base

- `GameManager`: bootstrapping, flags globales, carga inicial.
- `DefaultRoomCatalog` / `RoomFactory`: define rooms y `InteractableData`.
- `RoomManager`: gestiona room activa + estados de room.
- `InteractableBehaviour`: solo click + visibilidad.
- `InteractableData`: fuente de verdad del interactable.

Seleccionamos Opción A: data en código para el MVP.

---

## Problema actual

`GameManager` ya hace demasiado:

- runtime systems;
- flags base;
- creación de `RoomData`;
- creación de `InteractableData`;
- carga de rooms.

Esto funciona, pero ya está mezclando contenido, flujo y estado global. Para un MVP es válido, pero no es una base escalable.

---

## Estructura recomendada

### 1. GameManager

Debe quedar como:

- `EnsureRuntimeSystems()`
- `InitializeMVP()`
- `SetFlag()` / `HasFlag()`
- `SetCurrentRoom()`
- `LoadRoom()` inicial

No debe tener la data de cada room dentro del mismo archivo.

### 2. DefaultRoomCatalog

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

Aquí va toda la construcción de:

- `RoomData`
- `InteractableData`
- `connectedRoomIds`
- `requiredItemId`
- `resultFlag`
- `linkedRoomId`
- visibilidad por realidad

### 3. RoomFactory (opcional)

Útil si se quiere materializar GameObjects a partir de `RoomData`, pero no obligatorio para MVP. En una primera fase, `RoomManager` puede consumir directamente la data y crear objetos runtime.

### 4. RoomManager

Responsabilidades:

- almacenar rooms;
- cargar room actual;
- evaluar visibilidad del interactable según room + realidad;
- mantener el estado de la room.

### 5. InteractableBehaviour

Debe recibir `InteractableData` y delegar en `InteractionManager.TryInteract(data)`. No debe crear data ni definir rooms.

### 6. InteractableData

La configuración del interactable debe vivir aquí:

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

## Patrón de MVP

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

Esto ya limpia mucho el flujo sin introducir complejidad innecesaria.

---

## Decisión del editor

No mezclar dos fuentes de verdad.

Para este proyecto, la ruta correcta es:

- data en código para rooms/interactables;
- editor solo para prefabs, colliders, sprites, referencias visuales.

Eso evita inconsistencias y hace más legible la producción del MVP.

---

## Separación que buscamos

```text
GameManager        -> bootstrap + flags + room start
DefaultRoomCatalog -> room definitions and interactable data
RoomManager        -> active room and room logic
InteractableBehaviour -> visual interaction bridge
InteractionManager -> actual resolution of actions
InteractableData   -> configuration source of truth
```

---

## Recomendación final

Mantener esta arquitectura simple:

- `GameManager` no define rooms a mano.
- `DefaultRoomCatalog` crea la data del juego.
- `RoomManager` gestiona room state.
- `InteractableBehaviour` solo responde a interacción.

Con eso, Lobby/Hallway/Bedroom quedan bien separados, el proyecto sigue siendo simple y la deuda técnica no crece innecesariamente.
