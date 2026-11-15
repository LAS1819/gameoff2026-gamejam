# Room Lobby — Technical Spec

- Versión 0.1
- Creado: 10/11/2026
- Actualizado: 10/11/2026

---

## 1. Objetivo de la Room

La Lobby es la primera Room del MVP y actúa como zona de introducción, progresión mínima y comprobación del sistema base.

Su finalidad técnica es:

- cargar una escena inicial con un estado claro;
- enseñar la lógica de interacción por click;
- validar la obtención de un item y su uso en un interactable posterior;
- comprobar el cambio de room mediante una transición;
- validar visibilidad por realidad sin complejidad adicional.

---

## 2. Definición de RoomData

```text
RoomData
- id: "lobby"
- displayName: "Lobby"
- connectedRoomIds: ["hallway"]
- interactables:
  - staff_key
  - front_door
  - hallway_entrance
```

### Estado inicial

- Reality: Normal
- Los interactables están visibles/activos según su regla de realidad.
- La llave es el primer item de progresión del juego.
- La puerta interior está cerrada por defecto.
- La transición al Hallway queda bloqueada hasta que el jugador resuelve el flujo principal.

---

## 3. Interactables principales

### 3.1 StaffKey

```text
id: "staff_key"
displayName: "Staff Key"
interactionType: Pickup
itemId: "staff_key"
visibleInNormalReality: true
visibleInOtherReality: false
resultFlag: HasStaffKey
consumeAfterUse: true
```

#### Propósito

- Introducir la recolección básica.
- Dar acceso al primer bloqueo funcional.
- Comprobar el pipeline de item pickup: pickup → flag → use.

#### Comportamiento esperado

- Se ve solo en el `Mundo Normal` (no se permite cambiar al `Otro Lado`).
- Al hacer click, se añade el item al inventario o al registro de items.
- Se marca `HasStaffKey = true`.
- Se elimina de la escena tras su uso.

---

### 3.2 FrontDoor (Puerta Principal)

```text
id: "front_door"
displayName: "Front Door"
interactionType: Use
requiredItemId: "staff_key"
visibleInNormalReality: true
visibleInOtherReality: true
resultFlag: DoorOpened
```

#### Propósito

- Ser la primera barrera de uso con requisito de item.
- Validar la lógica “bloqueado hasta que se cumpla requisito”.
- Servir de prueba de que el sistema de Flags funciona correctamente.

#### Comportamiento esperado

- Si el jugador no tiene la llave: se muestra el mensaje/feedback de bloqueo.
- Si la tiene: se activa la puerta y se marca `DoorOpened = true`.
- La puerta debe seguir visible en ambas realidades si queremos mantener la consistencia visual del edificio.

---

### 3.3 HallwayEntrance (Entrada al Pasillo)

```text
id: "hallway_entrance"
displayName: "Main Hallway Entrance"
interactionType: Transition
linkedRoomId: "hallway"
visibleInNormalReality: true
visibleInOtherReality: true
```

#### Propósito

- Hacer el paso al siguiente room.
- Comprobar la lógica de enlace entre habitaciones.
- Validar que RoomManager y currentRoomId se actualizan correctamente.

#### Comportamiento esperado

- Visible siempre que el flujo lo permita.
- La transición carga la Room `hallway`.
- Tras entrar, la player state se conserva según el sistema de Flags.

---

## 4. Items del Room

### 4.1 StaffKey (Llave del Personal)

```text
ItemData
- id: "staff_key"
- displayName: "Staff Key"
- description: "A brass key marked with the staff insignia."
- type: Key
- inventoryUsable: false
- useContext: "front_door"
```

#### Reglas

- Puede ser recogido solo si el interactable está activo.
- No es un item de “uso libre”; su uso está vinculado a `front_door`.
- Se considera un item de progresión menor, orientado a un “tutorial de interacción”.

---

## 5. Flags y estado de progresión

| Flag | Descripción | Origen |
| ------ | ------------- | -------- |
| HasStaffKey | El jugador ha recogido la llave | `staff_key` |
| DoorOpened | La puerta interior ya está desbloqueada | `front_door` |

### Regla de activación

```text
HasStaffKey == true
→ FrontDoor puede abrirse

DoorOpened == true
→ HallwayEntrance queda disponible para transición
```

### Importante

La lógica de progreso no debería depender de un “sprite visible”, sino de Flags y de la validación de `requiredItemId`.

---

## 6. Flujo de ejecución técnico

```text
Start Room
→ Lobby loads
→ StaffKey visible (Normal)
→ Player clicks StaffKey
→ HasStaffKey = true
→ FrontDoor becomes usable
→ Player uses FrontDoor
→ DoorOpened = true
→ HallwayEntrance becomes active
→ Player transitions to hallway
```

---

## 7. Reglas de visibilidad [NO PERMITIDO CAMBIAR DE MUNDO EN ESTA SALA]

### Normal Reality

- `staff_key` visible
- `front_door` visible
- `hallway_entrance` visible

### Other Side [NO PERMITIDO EN ESTA SALA]

- `staff_key` oculto
- `front_door` visible
- `hallway_entrance` visible

Esto permite:

- que el jugador aprenda la diferencia entre “item interactivo” y “estructura permanente”;
- que la puerta siga teniendo presencia visual en ambos estados;
- que la llave sea un recurso asociado a la realidad inicial y a la lógica de progresión.

---

## 8. MVP Scope recomendado

### Mantener

- 3 interactables principales
- 1 item principal
- 2 flags de progresión
- 1 transición entre rooms
- visibilidad diferenciada por realidad (no se permite cambiar al Otro Lado)

### Dejar fuera del MVP

- animaciones complejas
- sonido específico por interacción
- overlays extra
- estados adicionales de puerta cerrada / rota / abierta

---

## 9. Estructura de datos sugerida en Inspector

```text
Lobby
├── Visuals
│   └── BackgroundNormal
├── Interactables
│   ├── StaffKey
│   │   └── InteractableData
│   ├── FrontDoor
│   │   └── InteractableData
│   └── HallwayEntrance
│       └── InteractableData
```

### Objetivo del esquema

- facil acceso visual;
- separación clara entre fondo, props, interactables;
- que cada Room pueda construirse como data-driven con el mínimo de lógica en escena.

---

## 10. Resumen técnico

La Lobby es la Room de validación del núcleo del sistema: recoge item, desbloquea barrera, activa transición. Por eso debe ser simple, legible y estable.

La documentación técnica aquí no intenta explicar la narrativa; intenta dejar claro:

- qué Room existe;
- qué interactables la componen;
- qué items genera;
- qué flags activan;
- qué condiciones de visibilidad y progresión deben cumplirse.

Con esta base, el siguiente Room puede seguir el mismo patrón sin duplicar lógica ni inventar estructuras distintas.
