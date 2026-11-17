# Room Basement — Technical Spec

- Versión 0.1
- Creado: 17/11/2026
- Actualizado: 17/11/2026

---

## 1. Objetivo de la Room

El `Basement` es la primera Room donde la `Linterna` deja de ser un estado de posesión y pasa a ser una condición de jugabilidad.

Su función técnica es:

- introducir luz global por room con intensidad muy baja;
- habilitar uso de linterna sólo en rooms autorizadas (MVP: `basement`);
- aplicar gating de interacción según estado de linterna;
- mantener el patrón de amenaza bloqueante sin IA compleja;
- entregar una evidencia narrativa fuerte con solo 1-2 interacciones críticas.

---

## 2. Definición de RoomData (MVP)

```text
RoomData
- id: "basement"
- displayName: "Basement"
- connectedRoomIds: ["library"]
- interactables:
  - basement_entry_door
  - pressure_console
  - release_valve
  - basement_blocker
  - basement_exit
- lightingProfile:
  - globalLightIntensity: 0.10
  - lanternAllowed: true
```

### Estado inicial

- Reality: Normal
- Luz global mínima (usable para orientación básica, no para lectura fina).
- `LanternAllowed = true` en esta room.
- `LanternActive = false` por defecto al entrar.
- Consola visible, pero su lectura útil depende de linterna activa.
- La válvula no permite `Use` sin linterna.

---

## 3. Contrato técnico de luz por Room

### 3.1 Decisión para MVP

No usar lógica automática compleja basada solo en umbral.

Usar contrato explícito por room, equivalente al patrón del botón `SHIFT`:

- `LanternAllowedRooms = { "basement" }`
- si la room actual no está en esa lista, no se permite toggle de linterna;
- feedback en HUD cuando el jugador intenta activarla fuera de sala permitida.

### 3.2 Global Light 2D por room

Cada room puede declarar una intensidad global objetivo:

```text
RoomLightingProfile
- roomId
- globalLightIntensity (0..1)
- lanternAllowed (bool)
```

Para el `Basement`, valor recomendado MVP:

- `globalLightIntensity = 0.10` (ajustable en playtest entre 0.08 y 0.18).

Esto no reemplaza el gating de room; lo refuerza visualmente.

---

## 4. Linterna con Sprite Light 2D acoplada a cursor

```text
LanternController (runtime)
- input: toggle (HUD + tecla)
- checks: LanternUnlocked && LanternAllowed(currentRoom)
- state: LanternActive
- light source: Light2D (Point o Freeform)
- position source: cursor world position
```

### Reglas MVP

- La linterna solo se puede activar si fue obtenida en `Bedroom`.
- Si la room no permite linterna, `LanternActive` se fuerza a `false`.
- Al cambiar de room, reevaluar permiso y apagar si corresponde.
- No hay batería en esta iteración.

---

## 5. Interactables principales

### 5.1 BasementEntryDoor

```text
id: "basement_entry_door"
displayName: "Basement Entry"
interactionType: Transition
linkedRoomId: "basement"
requiredItemId: "basement_code"
resultFlag: BasementEntered
```

---

### 5.2 PressureConsole (evidencia central)

```text
id: "pressure_console"
displayName: "Pressure Console"
interactionType: Examine
visibleInNormalReality: true
visibleInOtherReality: true
resultFlag: BasementEvidenceFound
```

#### Regla de lectura

- `LanternActive == false` -> devuelve descripción ambigua (sin detalle útil).
- `LanternActive == true` -> devuelve descripción completa y activa evidencia real.

---

### 5.3 ReleaseValve (activación)

```text
id: "release_valve"
displayName: "Release Valve"
interactionType: Use
visibleInNormalReality: true
visibleInOtherReality: true
resultFlag: BasementPressureReleased
```

#### Regla de uso

- `LanternActive == false` -> uso denegado con feedback explícito.
- `LanternActive == true` -> uso permitido, activa flag y desbloquea salida narrativa.

---

### 5.4 BasementBlocker (amenaza bloqueante)

```text
id: "basement_blocker"
displayName: "Manifestation"
interactionType: StateTrigger
visibleInNormalReality: false
visibleInOtherReality: true
resultFlag: BasementBlockerVisible
```

#### Regla MVP

- Se activa cuando el jugador insiste en `Use` crítico sin linterna.
- Bloquea temporalmente interacción de la válvula.
- Se desactiva al cumplir condición de linterna activa y reintento válido.

Sin daño ni persecución compleja en esta iteración.

---

### 5.5 BasementExit

```text
id: "basement_exit"
displayName: "Basement Exit"
interactionType: Transition
linkedRoomId: "library"
visibleInNormalReality: true
visibleInOtherReality: true
```

---

## 6. Reglas de interacción con oscuridad

Contrato requerido para `InteractionManager`:

```text
If interactionType == Examine
    If interactable is dark-sensitive and LanternActive == false
        show obscured description
    Else
        show full description

If interactionType == Use
    If interactable requires lantern and LanternActive == false
        deny interaction + feedback
        optionally trigger blocker flag
    Else
        resolve use normally
```

Esto evita crear scripts únicos por interactuable y mantiene el sistema data-driven.

---

## 7. Flags de estado

| Flag | Descripción | Origen |
| --- | --- | --- |
| BasementEntered | Entrada al Basement completada | `basement_entry_door` |
| LanternAllowedInRoom | La room actual permite linterna | `RoomLightingProfile` |
| LanternActive | Linterna encendida | `LanternController` |
| BasementEvidenceFound | Evidencia real obtenida | `pressure_console` |
| BasementPressureReleased | Válvula usada correctamente | `release_valve` |
| BasementBlockerVisible | Bloqueo activo | `basement_blocker` |
| BasementResolved | Objetivo MVP del room completado | set al combinar evidencia + válvula |

Regla de cierre:

```text
If BasementEvidenceFound == true
   and BasementPressureReleased == true
then BasementResolved = true
```

---

## 8. VFX/SFX narrativos (controlados)

Eventos ambientales recomendados, sin afectar arquitectura principal:

- `AmbulanceSirenFlash` (one-shot o pulso corto por trigger narrativo).
- `SteamBurst` en tuberías.
- `DripEvent` en beat emocional.

Deben implementarse como eventos de room (flags + timeline simple), no como subsistema nuevo.

---

## 9. Flujo técnico del MVP

```text
Player enters Basement with basement_code
→ Room applies low GlobalLight2D intensity
→ Lantern is allowed in this room
→ player examines pressure_console
    - without lantern: ambiguous text
    - with lantern: full evidence
→ player tries release_valve
    - without lantern: blocked + feedback (+ blocker optional)
    - with lantern: success
→ BasementResolved flag set when evidence + valve are both complete
→ player exits room
```

---

## 10. Scope guard del MVP

### Mantener

- una sola room oscura real;
- linterna con gating por room;
- 1 evidencia + 1 activación;
- feedback claro en interacciones bloqueadas;
- blocker psicológico simple.

### Simplificar

- sin batería;
- sin IA avanzada;
- sin múltiples objetivos paralelos;
- sin puzles secundarios en cadena.

### Dejar fuera

- sistema de drenado de batería;
- persecución completa;
- expansión jugable del `Huerto`.

---

## 11. Extensión futura (no MVP): Huerto

Cuando el loop base funcione, el `Huerto` puede entrar como:

- room nueva conectada al `Basement`, o
- extensión narrativa puntual del mismo room.

Condición para abrir ese scope: `BasementResolved` validado en playtest sin fricción.

---

## 12. Resumen técnico

El `Basement` se implementa como room de baja luz con linterna habilitada por gating explícito de room. La interacción sigue el sistema existente (`InteractableData` + `InteractionManager`) y añade una regla clara: `Examine` puede ser ambiguo sin luz, pero `Use` crítico queda bloqueado hasta activar linterna. Con una evidencia central, una válvula y un blocker simple, la room queda jugable, coherente y terminable dentro del MVP.
