# Game Off 2026 - TECHNICAL DESIGN

- **Versión:** 0.1.1-proto
- **Creación:** 03/11/2026
- **Última actualización:** 23/11/2026

---

## 1. Objetivo técnico

El proyecto se construirá mediante sistemas genéricos basados en datos.

La lógica específica de puzles, habitaciones y eventos deberá configurarse mediante datos y no mediante código específico para cada caso.

- **Objetivos:**
  - Reducir código repetido.
  - Facilitar la creación de contenido.
  - Simplificar el mantenimiento.
  - Permitir construir el juego completo reutilizando los mismos sistemas. [GameOff26-GameIdea-1 | Word], [GameOff26-...zleFlow-v1 | Word]

---

## 2. Arquitectura principal

El juego se divide en cuatro sistemas principales:

```plaintext
Room System
Reality System
Interaction System
Inventory System
```

Y sistemas auxiliares:

```plaintext
Event System
Save System
HUD / Story Overlay
```

---

## 3. Managers

### GameManager

- Responsabilidades:
  - Inicialización global.
  - Referencias generales.
  - Estado general del juego.

Singleton.

### RoomManager

- **Responsabilidades:**
  - Cargar habitaciones.
  - Cambiar habitaciones.
  - Gestionar elementos presentes en cada sala.

### RealityManager

- **Responsabilidades:**
  - Controlar la realidad activa.
- **Estados:**
  - Normal
  - OtherSide
- **Funciones:**
  - SwitchReality()
  - SetReality()

### InventoryManager

- **Responsabilidades:**
  - Añadir objetos.
  - Eliminar objetos.
  - Consultar objetos disponibles.

### InteractionManager

- **Responsabilidades:**
  - Detectar interacción.
  - Resolver acciones.
  - Lanzar resultados.

### EventManager

- **Responsabilidades:**
  - Escuchar eventos.
  - Activar contenido opcional.
  - Evaluar condiciones.

---

## 4. Sistema de habitaciones

Cada habitación se define mediante un RoomData.

### RoomData

```plaintext
string id
string displayName
List<InteractableData>
List<string> connectedRoomIds
```

Ejemplo:

```plaintext
Vestibulo
Pasillo
Biblioteca
Dormitorio
Sotano
Bosque
```

[GameOff26-...zleFlow-v1 | Word], [GameOff26-GameIdea-1 | Word]

---

## 5. Sistema de realidad

### RealityType

```plaintext
Normal
OtherSide
```

El sistema debe permitir: SwitchReality()

Cuando ocurre:

- cambia fondo.
- actualiza interactivos.
- lanza efectos visuales.
- actualiza estados visibles.
[GameOff26-GameIdea-1 | Word], [GameOff26-...l-Guide-v1 | Word]

---

## 6. Sistema de objetos

### InventoryItemData

```plaintext
string id
string displayName
Sprite icon
string description
```

Ejemplos:

```plaintext
StaffKey
Fuse
Medallion
Photo
```

[GameOff26-...zleFlow-v1 | Word]

---

## 7. Sistema de interactivos

### InteractableData

```plaintext
string id
string displayName
bool visibleNormal
bool visibleOtherSide
InteractionType interactionType
string requiredItem
string resultFlag
```

### InteractionType

```plaintext
Examine
Pickup
Use
Transition
```

[GameOff26-GameIdea-1 | Word]

---

## 8. Sistema de Flags

Toda la progresión debe depender de Flags.

### GameFlag

```plaintext
HasStaffKey
DoorOpened
FoundCode
HasFuse
GeneratorEnabled
ForestFinalTableVisible
ForestLetterRead
ForestEndingReached
```

Los puzles nunca consultan objetos directamente.

Consultarían:

- HasFlag()
- SetFlag()

---

## 9. Sistema de eventos

### EventData

```plaintext
string id
List<GameFlag>
RealityType
Action
```

Ejemplos:

```plaintext
LibraryGhost
HiddenItem
SecretEnding
```

The ending loop uses the existing HUD as a reusable `StoryOverlay` for the
letter and final tier. A separate `Credits` scene is planned for Sprint 4 and
should be loaded additively so the persistent game state can return to the
Lobby without rebuilding the global systems.

---

## 10. MVP

El MVP técnico debe permitir:

```mermaid
graph TD
    A[Vestíbulo] --> B[Recoger llave]
    B --> C[Flag activada]
    C --> D[Abrir puerta]
    D --> E[Cambiar de sala]
    E --> F[SHIFT]
    F --> G[Actualizar escenario]
```

Cuando eso funcione:

✅ Arquitectura validada.
✅ Vertical Slice posible.
✅ Riesgo técnico principal eliminado.

---
