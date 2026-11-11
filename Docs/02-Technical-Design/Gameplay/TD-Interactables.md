# TD - Interactables

- Versión: 0.1
- Estado: en uso en el MVP y base para futuras rooms
- Creado: 11/11/2026
- Actualizado: 11/11/2026

## 1. Propósito

Los interactables son la capa de juego que permite que cada `Room` se sienta viva sin convertir cada objeto en un script específico.

El patrón actual del proyecto es:

- `InteractableData`: define la identidad y el comportamiento del objeto
- `InteractableBehaviour`: componente reutilizable para responder al clic
- `InteractionManager`: resuelve el resultado de la interacción
- `RoomManager`: decide si el interactuable está visible según la `Room` y la `Realidad`

Esto mantiene el sistema simple, data-driven y fácil de extender sin crear un `MonoBehaviour` por cada objeto del juego.

---

## 2. Modelo base

El modelo principal vive en `MVPDataModels.cs` y tiene esta estructura:

```csharp
public enum InteractionType
{
    Examine,
    Pickup,
    Use,
    Transition,
    Toggle
}

public class InteractableData
{
    public string id;
    public string displayName;
    public string description;
    public InteractionType interactionType;

    public bool visibleInNormalReality = true;
    public bool visibleInOtherReality = true;

    public string itemId;
    public string requiredItemId;
    public string resultFlag;
    public string linkedRoomId;
    public bool consumeAfterUse;
}
```

### Campos clave

- `id`: identificador único del `Interactuable` dentro del juego
- `displayName`: nombre visible para feedback o `UI`
- `description`: texto de examen o contexto del `Interactuable`
- `interactionType`: tipo de interacción a resolver
- `visibleInNormalReality` / `visibleInOtherReality`: visibilidad por `Realidad`
- `itemId`: `Item` que entrega al recoger el `Interactuable`
- `requiredItemId`: `Item` necesario para activar el `Interactuable`
- `resultFlag`: flag que se actualiza al resolver la acción
- `linkedRoomId`: `Room` destino para transiciones
- `consumeAfterUse`: si se elimina tras usarse

### Regla base

Todo `Interactuable` debe tener un `id` estable, un `interactionType` claro y un comportamiento definido en el `InteractionManager`.

---

## 3. Comportamiento actual del sistema

### 3.1. `InteractableBehaviour`

`InteractableBehaviour` es el único componente reutilizable que se pone sobre los objetos `Interactuables` visibles.

Responsabilidades:

- escuchar clics del mouse
- leer su `InteractableData`
- mostrar/ocultar según `Room` + `Realidad`
- llamar a `InteractionManager.Instance.TryInteract(data)`

En runtime se usa `Configure(InteractableData data)` para asociar el GO con sus datos.

### 3.2. `RoomManager`

`RoomManager` decide si un `Interactuable` está visible en la `Room` actual.

Se comprueba:

- si el `Interactuable` pertenece a la `Room` activa
- si el `Interactuable` ya ha sido consumido
- si el jugador ya obtuvo el `Item` asociado
- si la `Realidad` actual permite que sea visible

Esto evita duplicar lógica en cada objeto y deja la visibilidad centralizada.

### 3.3. `InteractionManager`

`InteractionManager` es el motor de resolución del sistema.

Cuando se interacciona:

- verifica si el `Interactuable` es visible y válido
- comprueba `requiredItemId` si hace falta
- resuelve según `interactionType`
- actualiza flags del juego
- marca consumidos algunos objetos
- recarga estado visual si hace falta

---

## 4. Tipos de interactables en uso

### 4.1. `Examine`

Usado para describir o contextualizar un objeto.

Ejemplos:

- `Puerta` cerrada sin acceso real
- `Ventana` de la habitación
- obstáculo de texto que informa del mundo

Comportamiento actual:

- muestra descripción y/o feedback visual
- puede activar un flag si aplica
- no requiere `Item` ni transición

### 4.2. `Pickup`

Usado para obtener un `Item` o recurso del juego.

Ejemplos:

- `Staff Key`
- `Library Key`
- `Flashlight`

Comportamiento actual:

- añade el `Item` al `Inventario`
- opcionalmente activa `resultFlag`
- puede marcarlos como consumidos con `consumeAfterUse`

### 4.3. `Use`

Usado para objetos que requieren ser usados, no necesariamente recogidos.

Ejemplos:

- `Front Door`
- objetos que necesitan un `Item` para abrirse o activarse
- herramientas del entorno

Comportamiento actual:

- valida `requiredItemId`
- puede activar flag si el uso tuvo éxito
- puede aplicar consumo si así se define

### 4.4. `Toggle`

Usado para cambiar estado del entorno o del mundo.

Ejemplos:

- `Realidad` / `Shift Trigger`
- interruptor de luz
- activadores de ambiente

Comportamiento actual:

- puede alternar `RealityType`
- puede activar flags como `BedroomLightOn` o `ShiftPerformed`
- se usa para cambiar estado sin complejidad adicional

### 4.5. `Transition`

Usado para cambiar de `Room`.

Ejemplos:

- `Lobby` → `Hallway`
- `Hallway` → `Bedroom`
- `Bedroom` → `Hallway`

Comportamiento actual:

- usa `linkedRoomId`
- llama a `RoomManager.LoadRoom(...)`
- mantiene la continuidad del juego y de la realidad

---

## 5. Reglas de visibilidad y realidad

Los `Interactuables` no tienen lógica propia para decidir si están ocultos: esa decisión la toma `RoomManager`.

Actualmente la lógica principal es:

- si la `Room` activa no tiene ese `Interactuable`, no aparece
- si está consumido, no aparece
- si la `Realidad` no lo permite, no aparece
- si tiene `Item` requerido y no está activo, no puede usarse

Ejemplo del flujo actual:

- `front_door` es visible en `Lobby`
- `hallway_entrance` sólo puede interactuarse cuando `DoorOpened == true`
- `shift_trigger` solo participa en la `Room` y la `Realidad` correctas
- la visibilidad puede cambiar si el mundo actual cambia de `Normal` a `Other Side`

---

## 6. Lo que ya está en funcionamiento

El sistema está ya validado en esta base para:

- recoger `Items` básicos (`staff_key`)
- usar `Item` requerido para pasar de `Room` o abrir una puerta
- activar/ocultar `Interactuables` por `Room`
- cambiar la `Realidad` con un trigger de toggle
- cargar `Rooms` por ID
- definir `Rooms` enteras con `RoomData` + `InteractableData`

Esto es precisamente la base que permite crear nuevas `Rooms` sin rehacer el motor.

---

## 7. Qué viene en siguientes iteraciones

En el Sprint 2 y siguientes, la intención es ampliar el sistema con tipos más específicos, pero sin romper el patrón actual.

### Nuevos patrones recomendados

1. `BlockedByCondition`
   - interactuable visible pero inactivo
   - se activa solo con `resultFlag` o condición de room

2. `Tool` / `PermanentItem`
   - item reutilizable sin caer en un inventario tipo ad hoc
   - útil para flashlight, lamp, keycards, etc.

3. `StatefulTrigger`
   - cambia visualmente según flags
   - permite varias fases de una misma room

4. `LockedDoor`
   - interactionType `Use`
   - `requiredItemId` + `resultFlag`
   - ideal para puertas, cajas o cerraduras específicas

5. `OneShotEvent`
   - se puede consumir una vez y cambiar la room/estado de manera irreversible

### Regla para no sobreingenieriar

No conviene crear un script especial para cada interactuable si la lógica es solo una variación del mismo comportamiento genérico. La regla recomendada es:

- si el objeto cambia por estado, usa `resultFlag`
- si cambia por `Room` o `Realidad`, usa `RoomManager`
- si cambia por `Item` o `Interacción`, usa `InteractionManager`
- solo crea un script específico si la lógica es claramente distinta

---

## 8. Checklist de diseño para nuevos interactuables

Antes de crear cualquier nuevo `Interactuable`, comprobar:

- [ ] tiene `id` único
- [ ] está asociado a una `Room` concreta
- [ ] tiene `interactionType` correcto
- [ ] define visibilidad por `Realidad` si aplica
- [ ] si necesita `Item`, está reflejado en `requiredItemId`
- [ ] si recompensa algo, está definido en `ItemId` o `resultFlag`
- [ ] si es transitorio, se marca `consumeAfterUse`
- [ ] si afecta a room flow, tiene `linkedRoomId` o flag de estado

---

## 9. Conclusión

Los `Interactuables` ya son un sistema funcional, centralizado y reutilizable.

La decisión clave del proyecto es mantener una base simple:

- un modelado de datos claro,
- una resolución centralizada,
- una visibilidad por `Room`/`Realidad`,
- y un comportamiento genérico que permite crecer sin colapsar el MVP.

Esto deja espacio para el Sprint 2 y futuras rooms sin hacer refactors costosos ni crear una arquitectura demasiado pesada.
