# Room Hallway — Technical Spec

- Versión 0.1
- Creado: 10/11/2026
- Actualizado: 10/11/2026

---

## 1. Objetivo de la Room

El Hallway es la primera Room en la que el juego valida la mecánica principal: cambio de realidad como solución a un bloqueo espacial.

Su función técnica es:

- introducir la diferencia entre realidad normal y Otro Lado;
- validar el sistema de visibilidad por realidad;
- activar el flujo de SHIFT como herramienta de progresión;
- ofrecer una transición a zonas futuras sin convertir la Room en un puzle complejo.

Es una Room de “gating” más que de recolección. Su valor técnico está en la lógica de bloqueo y solución.

---

## 2. Definición de RoomData

```text
RoomData
- id: "hallway"
- displayName: "Main Hallway"
- connectedRoomIds: ["lobby"]
- interactables:
  - hallway_obstacle
  - shift_trigger
  - lobby_exit
  - library_door (future)
  - bedroom_door (future)
```

### Estado inicial

- Reality: Normal
- La obstrucción del corredor está visible.
- El punto de SHIFT puede activarse sin restricciones.
- La salida a Lobby es siempre accesible.
- Las puertas laterales quedan fuera del MVP como placeholders.

---

## 3. Interactables principales

### 3.1 HallwayObstacle

```text
id: "hallway_obstacle"
displayName: "Hallway Obstacle"
interactionType: Examine
visibleInNormalReality: true
visibleInOtherReality: false
resultFlag: RealityUnlocked
```

#### Propósito

- bloquear la ruta principal en la Realidad Normal;
- demostrar que el espacio es persistente en una realidad y no en la otra;
- activar la regla de progresión del room.

#### Reglas técnicas

- La posición del obstacle debe coincidir con el bloqueo visual del pasillo.
- Debe ocultarse automáticamente al entrar al Otro Lado.
- En MVP no es necesario que haga daño ni que tenga animación compleja.
- Su única lógica relevante es: “si está visible, bloquea; si no, permite avanzar”.

---

### 3.2 ShiftTrigger

```text
id: "shift_trigger"
displayName: "Shifting Point"
interactionType: Toggle
visibleInNormalReality: true
visibleInOtherReality: true
resultFlag: ShiftPerformed
```

#### Propósito

- ser el punto sobre el que el sistema SHIFT se activa;
- marcar la transición entre “percibir” la realidad y “manipular” la realidad;
- ofrecer un punto claro para la UX del cambio de mundo.

#### Reglas técnicas

- El trigger debe ser accesible en cualquier realidad.
- Al usarlo, se activa la alternancia de mundos.
- El resultado no debe depender de item ni de flags previas.
- Conviene que el feedback visual del botón o del icono sea muy claro.

---

### 3.3 LobbyExit

```text
id: "lobby_exit"
displayName: "Lobby Exit"
interactionType: Transition
linkedRoomId: "lobby"
visibleInNormalReality: true
visibleInOtherReality: true
```

#### Propósito

- devolver al jugador al punto de partida;
- validar que la navegación entre habitaciones funciona en ambos sentidos;
- mantener el corredor como “zona de transición” y no como final.

#### Reglas técnicas

- No requiere flags extras.
- Debe permanecer visible siempre.
- Debe usar RoomManager.LoadRoom() o equivalente.

---

### 3.4 LibraryDoor / BedroomDoor (future placeholders)

```text
id: "library_door"
displayName: "Mysterious Door (Library)"
interactionType: Examine
visibleInNormalReality: false
visibleInOtherReality: true

id: "bedroom_door"
displayName: "Mysterious Door (Bedroom)"
interactionType: Examine
visibleInNormalReality: false
visibleInOtherReality: true
```

#### Propósito

- dejar una ruta de progresión futura sin bloquear el MVP;
- sugerir que el pasillo se conecta con otras piezas del edificio;
- preparar la estructura de contenido del siguiente bloque del juego.

#### Reglas técnicas

- Para MVP pueden ser solo “markers” visuales o placeholder interactables sin lógica funcional.
- El sistema debe poder activarlos más tarde sin cambiar la estructura base del room.

---

## 4. Items del Room

El Hallway no necesita un item principal en el MVP.

### Estado de items

```text
No inventory item in this room
```

Esto es intencional:

- la Room no se resuelve por coger un objeto;
- se resuelve por entender y usar la realidad;
- la progresión se basa más en `Flags` que en `Inventory`.

---

## 5. Flags y estado de progresión

| Flag | Descripción | Origen |
|------|-------------|--------|
| RealityUnlocked | El jugador ha comprendido que el bloqueo cambia con la realidad | `hallway_obstacle` |
| ShiftPerformed | El jugador ha usado o activado SHIFT en el room | `shift_trigger` |

### Regla sugerida

```text
ShiftPerformed = true
→ el jugador confirma que la mecánica principal funciona

RealityUnlocked = true
→ se puede considerar que la Room ha sido resuelta visualmente y narrativa
```

No es necesario forzar una win condition en esta Room. La prioridad es aprender la mecánica.

---

## 6. Reglas de visibilidad por realidad

### Normal Reality

- `hallway_obstacle` visible
- `shift_trigger` visible
- `lobby_exit` visible
- `library_door` visible pero inaccesible
- `bedroom_door` visible pero inaccesible

### Other Side

- `hallway_obstacle` oculto
- `shift_trigger` visible
- `lobby_exit` visible
- `library_door` visible y accesible (cerrada con Llave)
- `bedroom_door` visible y accesible

Esto permite que la Room se comporte como una “prueba de comprensión” del sistema sin hacer que la lógica de la room sea pesada.

---

## 7. Flujo técnico

```text
Load Room hallway
→ player enters in Normal
→ HallwayObstacle is visible
→ ShiftTrigger is active
→ player uses SHIFT
→ reality changes
→ HallwayObstacle disappears
→ route is now passable
→ player can exit to Lobby or continue to future room
```

### Secuencia de validación recomendada

1. Cargar Lobby.
2. Entrar en Hallway.
3. Verificar que `HallwayObstacle` es visible.
4. Activar SHIFT.
5. Comprobar que el obstacle desaparece.
6. Confirmar que el player puede seguir avanzando.
7. Volver a Lobby y repetir la transición.

---

## 8. MVP Scope recomendado

### Mantener

- 3 interactables core
- 1 flag de bloqueo visual
- 1 flag de uso de SHIFT
- 1 transición a Lobby
- visibilidad por realidad

### Simplificar / dejar fuera

- puertas laterales con lógica compleja
- eventos de narrativa ligados a puertas
- triggers extra por audio o partículas
- caminos alternativos no funcionales

---

## 9. Estructura sugerida en escena

```text
Hallway
├── Visuals
│   ├── BackgroundNormal
│   └── BackgroundOther
├── Interactables
│   ├── HallwayObstacle
│   ├── ShiftTrigger
│   ├── LobbyExit
│   ├── LibraryDoor (Log Locked with Key)
│   └── BedroomDoor (Log Description)
```

La idea es mantener un esquema limpio: el room se compone de un fondo y un conjunto de interactables, sin mezclar lógica extra en la escena.

---

## 10. Resumen técnico

El Hallway es la Room de validación del sistema principal del juego. No necesita muchos objetos ni complejidad narrativa; lo importante es que el jugador entienda que:

- la realidad cambia;
- la ruta cambia con ella;
- el bloqueo no es solo visual, sino funcional;
- la mecánica SHIFT es la herramienta central del juego.

Si esta Room funcione bien, el resto del proyecto queda mucho más sólido.
