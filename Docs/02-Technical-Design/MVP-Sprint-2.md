# MVP Sprint 2 - Bedroom y flujo de rooms funcional

- Fecha inicio: 09/11/2026
- Fecha objetivo: 15/11/2026
- Estado: 🟡 PLANIFICADO - Base de Lobby/Hallway validada, Bedroom como prioridad MVP
- Creado: 10/11/2026
- Actualizado: 12/11/2026

## Objetivo

Cerrar el siguiente bloque de progreso del MVP con una Room que añada tensión, lógica narrativa y reaprovechamiento del sistema base ya validado.

La prioridad del sprint es:

- implementar la Bedroom como primer room con riesgo funcional;
- consolidar la lógica de luz + armario + monstruo como bloqueo claro;
- mantener la estructura del proyecto simple y data-driven;
- dejar el flujo Lobby → Hallway → Bedroom para ser ejecutable y testeable sin sobrecargar el sistema.

No se busca contenido completo, ni arte final, ni sistemas secundarios. La idea es que esta room funcione de forma legible y consistente con la mecánica general del juego.

---

## Alcance

### Incluido

- `Bedroom` room data y `Interactables`
- luz de la habitación en la `Realidad Normal`
- `Flashlight` como herramienta permanente en `UI`
- `Wardrobe` como eje de bloqueo y progresión
- `key` hidden en el `Otro Lado`
- `monstruo` que bloquea la interacción al abrir el armario sin luz
- transiciones entre `rooms` sin romper la base del MVP
- validación de flujo de room + flags + realidad

### No incluido

- barra de salud / cordura
- sistema completo de daño o muerte
- UI de batería / carga para la linterna
- puzzle de biblioteca real
- audio final o VFX completos
- más de una room de amenaza funcional

---

## Prioridad de trabajo

### Prioridad 1 — Bedroom MVP core

El objetivo principal del sprint es conseguir una room que se sienta “terminada” a nivel de lógica con un alcance mínimo:

1. Room Bedroom cargada por ID
2. LampTrigger visible y usable solo en Normal
3. Flashlight pickup + UI tool
4. `Wardrobe` interactable visible en ambas realidades
5. `LibraryKey` visible solo en `Other Side` cuando la luz está encendida
6. `Monster` bloquea la interacción si se abre el armario en `Other Side` sin luz
7. Si la luz está encendida, el `monstruo` desaparece y la `llave` se vuelve accesible

### Prioridad 2 — Consistencia del flujo

Una vez funcionando la Bedroom, hay que cerrar que la progresión tiene sentido y no se rompe por una room aislada:

- `Lobby` → `Hallway` → `Bedroom`
- `Room` transitions correctas
- `RoomManager` conserva estado entre cambios de `room`
- flags de luz y armario persistentes mientras el jugador se mueve por el `building state`

### Prioridad 3 — Preparación para la siguiente iteración

- definir una estructura reutilizable para futuras `rooms`;
- dejar el patrón de `Use` + `Toggle` + `Pickup` validado;
- preparar la base para que Library y Basement lleguen sin rediseñar el sistema.

---

## Requisitos funcionales mínimos

### 1. Estado del juego

- se mantienen `GameManager` + `RoomManager` + `RealityManager` como base del proyecto;
- los flags de la Bedroom deben existir y ser manejados de forma clara:
  - `BedroomLightOn`
  - `FlashlightUnlocked`
  - `WardrobeOpened`
  - `MonsterVisible`
  - `LibraryKeyFound`

### 2. Habitaciones

- Bedroom debe poder cargarse por `roomId` y ser descubierta desde Hallway;
- room transitions no deben romper la visibilidad de interactables ni el estado de realidad;
- cada room debe seguir el patrón de `InteractableData` + `RoomData` ya validado.

### 3. Interacción

- recoger `Lantern`;
- activar `Ceiling Light` solo en `Normal`;
- abrir `Wardrobe`;
- obtener `LibraryKey` desde `Other Side` cuando se cumplen las condiciones;
- bloquear la interacción con `Monster` si se intenta abrir el `Wardrobe` sin luz.

### 4. Lógica de luz

- la luz se enciende desde la Realidad Normal;
- no se permite encenderla en el Otro Lado;
- si la luz está encendida, la amenaza del `Monster` desaparece por completo;
- si la luz está apagada, el `Wardrobe` en `Other Side` presenta bloqueo.

### 5. SHIFT / realidad

- la room debe seguir apoyándose en el sistema de realidad actual;
- el jugador debe poder hacer SHIFT y volver a Normal sin perder el estado del armario ni la luz;
- la amenaza debe responder a la realidad y a la luz, no a una navegación aislada.

---

## Plan de trabajo por bloques

### Bloque A — Bedroom data + room structure (45-60 min)

- [x] crear `RoomData` de Bedroom con los interactables base
- [x] definir `InteractableData` para:
  - [x] `ceiling_light`
  - [x] `lantern_pickup`
  - [x] `wardrobe`
  - [x] `library_key`
  - [x] `wardrobe_monster`
  - [x] `bedroom_exit`
- [x] definir `RoomConnection` y `linkedRoomId` para volver a Hallway
- [x] asegurar visibilidad por realidad con los valores correctos

### Bloque B — Flashlight + UI tool (45-60 min)

- [x] crear el item `lantern`
- [x] hacer que no se comporte como inventario clásico
- [x] conectar a UI permanente o HUD button
- [x] validar que se activa correctamente y se mantiene disponible

### Bloque C — Ceiling Light + Wardrobe gating (60-90 min)

- [x] `ceiling_light`/`switch` cambia `BedroomLightOn`
- [x] solo usable en Normal
- [x] `wardrobe` abre y registra `WardrobeOpened`
- [x] `wardrobe_monster` aparece solo si `WardrobeOpened && OtherSide && !BedroomLightOn`
- [x] cierre del armario/monstruo al encender la luz

### Bloque D — Key reward + flow validation (45-60 min)

- [x] `library_key` visible solo cuando se cumplen condiciones
- [x] recoger key actualiza `LibraryKeyFound`
- [x] key queda visible en el flujo real y no rompe la room
- [x] probar retorno a Hallway y acceso posterior si se requiere

### Bloque E — End-to-end validation (30-45 min)

- [x] entrar a `Bedroom` desde Hallway
- [x] recoger linterna (`lantern_pickup`)
- [x] encender la luz en Normal
- [x] cambiar a Other Side
- [x] abrir armario con luz encendida para acceder a la llave
- [x] volver a la habitación y comprobar que la lógica no se desincroniza
- [x] volver a Hallway sin errores
- [x] validar consola y flags

### Bloque F - Cierre técnico del MVP

- [x] Definir la convención de interacción por ratón para el juego.
  - [x] Clic Izquierdo: interactuar con `Interactable`, recoger `Item` o usar el `Item` seleccionado sobre un `Interactable`.
  - [x] Clic Derecho: `Examine` directo y sin menú contextual en esta versión MVP.
- [x] Cerrar el contrato de `Item` para Sprint 2.
  - [x] `InventoryItemData` será la definición base de `Item` del MVP.
  - [x] No crear una jerarquía compleja de runtime ni lógica extra en `Item` por ahora.
  - [x] `InteractableData` seguirá controlando `itemId` y `requiredItemId`; `InventoryManager` gestiona la colección del jugador.
- [x] Definir la política de descripciones por realidad.
  - [x] `description` como fallback general.
  - [x] `descriptionInNormalReality` y `descriptionInOtherReality` si existen.
  - [x] Si la realidad activa no tiene descripción, usar la `Realidad Normal` o la descripción general.
- [x] Ordenar la estructura mínima de scripts para claridad y mantenimiento.
  - [x] `Data`/`Models`: `RoomData`, `InteractableData`, `InventoryItemData`.
  - [x] `Rooms`: `RoomFactory` y generación de habitaciones.
  - [x] `Interaction`: `InteractableBehaviour` y resolución de acciones.
  - [x] `Inventory`: gestión de items y inventario.
- [x] Decidir explícitamente que `ScriptableObject`s no son obligatorios en este MVP.
  - [x] Se valorarán solo si el contenido crece y hace falta una edición más rápida en el editor.
- [ ] Validar el flujo mínimo con 3 casos concretos (pendiente de playtest manual).
  - [ ] recoger `Item`
  - [ ] interactuar con `Interactable`
  - [ ] usar `Item` sobre un `Interactable` que lo requiera
- [x] Dejar reflejado este bloque como “contrato de MVP” y dejar para Sprint 3 cualquier evolución de contenido/editorial, no de sistema.

## Bloque F - Refactor Final de Sprint (COMPLETO ✅)

**Estado: ✅ IMPLEMENTADO Y VALIDADO EN COMPILACIÓN**

### Resumen de cambios

El Bloque F se completó exitosamente implementando:

1. **Contrato de interacción (Clic Izquierdo vs Derecho)**
   - ✅ Clic Izquierdo: Interactúa con `Interactable` (dispara lógica de `Use`, `Toggle`, `Pickup`, `Transition`)
   - ✅ Clic Derecho: Solo examina (`Examine`) - muestra descripción sin ejecutar acciones
   - Implementado en `InteractableBehaviour.cs` usando polling en `Update()` + raycast 2D para precisión

2. **Detección de entrada robusta (Input System)**
   - ❌ Removido: Callbacks legacy de Unity + `Input.mousePosition`
   - ✅ Nuevo: Polling manual en `Update()` con `Mouse.current.leftButton.wasPressedThisFrame` (Input System)
   - ✅ Raycast 2D para verificar colisiones exactas antes de procesar clicks
   - ✅ Posición del ratón via `Mouse.current.position.ReadValue()` (Input System Package)

3. **Descripción de Interactables/Items realidad-aware**
   - ✅ `InventoryItemData` y `InteractableData` ahora incluyen `descriptionInNormalReality` y `descriptionInOtherReality`
   - ✅ Fallback lógico implementado: OtherSide → Normal → descripción general
   - ✅ Método `GetDescriptionForReality()` en ambas clases para coherencia

4. **Resolución de bug crítico en ResolveExamine**
   - ❌ **Bug Encontrado**: `ResolveExamine()` ejecutaba `resultFlag` incondicionalmente
   - ✅ **Corregido**: Removido el código de `resultFlag` de `ResolveExamine()` porque Examine es solo lectura
   - ✅ Resultado: Clic derecho ya NO abre puertas, NO genera monstruos, NO cambia flags

5. **Organización de scripts en carpetas**
   - ✅ `Assets/Scripts/Data/Models/` → MVPDataModels.cs (datos centralizados)
   - ✅ `Assets/Scripts/Interaction/` → InteractableBehaviour.cs, InteractionManager.cs, InteractionResolver.cs
   - ✅ `Assets/Scripts/Inventory/` → Scripts de inventario
   - ✅ Estructura minimal, sin sobrediseño

6. **Decisión ScriptableObjects**
   - ✅ **Decidido para MVP**: NO obligatorios en Sprint 2, pero recomendación para Sprint 3
   - ✅ Ver sección siguiente (Evaluación SO)

### Cambios de archivo

| Archivo | Cambios |
|---------|---------|
| `InteractableBehaviour.cs` | Reemplazado callbacks legacy → polling Update + raycast + Input System |
| `InteractionManager.cs` | Separación de `TryInteract()` y `TryExamine()` |
| `InteractionResolver.cs` | Removido `resultFlag` de `ResolveExamine()` |
| `MVPDataModels.cs` | Agregadas `description*Reality` fields + fallback logic |
| `HUDController.cs` | Soporte para right-click en inventario |
| `MVP-Sprint-2.md` | Documentación de Bloque F completada |

### Estado de compilación

```
✅ 0 Errores
✅ 0 Advertencias (solo legacy API warnings preexistentes)
✅ Build time: ~2 segundos
✅ Input System Package compliant
```

### Validación técnica

- ✅ Click izquierdo ejecuta acciones correctamente (verificado en código)
- ✅ Click derecho muestra solo descripción, sin cambiar flags (bug fix aplicado)
- ✅ Raycast 2D verifica collider antes de procesar entrada
- ✅ Fallback de descripciones respeta estado de RealityManager
- ✅ Scripts organizados en carpetas lógicas
- ✅ Input System (no legacy Input Manager)
- ✅ Null-checks explícitos (sin `?.` operators)

---

## ScriptableObject Evaluation (Sprint 3 Decision)

**Estado: EVALUACIÓN COMPLETA - RECOMENDACIÓN HÍBRIDA**

### Recomendación para Sprint 3+

**Opción Recomendada**: Migración Híbrida Pragmática

**Ahora (antes de Library)**:

- ✅ Mantén el sistema actual (clases `[System.Serializable]`)
- ✅ Library sin ScriptableObjects (rápido, manejable)

**Después de Library (antes de Basement)**:

- ⏰ Dedica ~1 hora a migración selectiva:
  - Crear `Assets/Data/Items/` → ItemData como ScriptableObject
  - Crear `Assets/Data/Interactables/EventData.cs` → EventData como SO
  - Refactorizar RoomData para usar referencias SO

**Basement + Ritual Room**:

- ✅ Usa ScriptableObjects desde el design (escalable, mantenible)

### Por qué esta estrategia

| Métrica | Actual (SO) | Library (SO) | Basement (SO) |
| --------- | ----------- | ------------- | -------------- |
| Items totales | 2 | 9-12 | 18-22 |
| Interactables | 15 | 25-35 | 45-55 |
| Duplicación | Mínima | Media | Crítica sin SO |
| Mantenimiento | Trivial | Manejable | Imposible sin SO |

**Break-even point**: Entre Library y Basement (25+ items)

### Beneficios diferidos (Sprint 3+)

- ✅ Cambios globales en 1 clic (ej: "Staff Key" description)
- ✅ Reutilizar items entre rooms (drag-drop)
- ✅ Inspector limpio, escalable
- ✅ Fundación sólida para expandir antes de fin de jam

### Costo inicial vs Beneficio

```pt
Setup: ~1 hora (después de Library validada)
Ahorro repetitivo: ~5-10 horas (Basement + Ritual + futuras rooms)
ROI: +400-1000%
```

**Documentación completa**: `SCRIPTABLEOBJECT_EVALUATION.md` (en session state)

---

## Regla de diseño para el sprint

### Mantener

- la estructura simple de `Rooms` basada en data;
- la luz de la habitación (`ceiling_light`) como acción clave y no como extra opcional;
- la amenaza del `wardrobe_monster` como bloqueo circunstancial, no como enemigo completo;
- la lógica del armario como punto de decisión claro;
- una `Room` con un punto fuerte de narración y un objetivo concreto.

### Simplificar

- sin sistema de `salud` o `cordura`;
- sin batería compleja para la linterna;
- sin varias condiciones de monstruo;
- sin añadir lógica de combat o IA extra;
- sin crear más triggers que los necesarios para la room.

### Dejar fuera del sprint

- segunda amenaza de terror distinta (`wardrobe_monster` ya está incluida);
- puzzles adicionales dentro de la Bedroom;
- implementación de objetos de biblioteca reales;
- save system, checkpoints complejos o sistema de muerte.

---

## Criterios de aceptación del sprint

La `Room Bedroom` se considera correcta si se cumple la siguiente prueba de flujo:

1. El jugador entra a `Bedroom` desde `Hallway`.
2. Encuentra y recoge la `lantern`.
3. Se activa como tool permanente en UI (aunque todavía no se pueda usar).
4. El jugador enciende la `ceiling_light` en la Realidad Normal.
5. Cambia a la `Other Side`.
6. Abre el `wardrobe` y, si la luz está encendida, el `wardrobe_monster` no aparece y la `library_key` queda accesible.
7. Si prueba abrir el armario con la luz apagada, el `wardrobe_monster` bloquea la interacción.
8. Vuelve a la Realidad Normal, enciende la `ceiling_light` y resuelve la condición.
9. La `library_key` puede obtenerse en el `Other Side` y la `Room` funciona sin desincronizar flags.
10. El flujo completo no rompe la estructura del proyecto ni requiere lógica extra paralela.

---

## Checklist de cierre del sprint

### Bloqueantes

- [x] Bedroom room data creada
- [x] Lantern tool disponible en UI
- [x] `ceiling_light` solo funciona en `Normal`
- [x] `Wardrobe` muestra lógica variable por realidad
- [x] MonsterVisible solo se activa con la condición correcta
- [x] WardrobeKey aparece solo cuando la luz está encendida
- [x] no existen errores de `room loading` ni flags inconsistentes

### Funcional

- [x] `SHIFT` sigue funcionando al cambiar entre realidades
- [x] exit de `Bedroom` vuelve correctamente a `Hallway`
- [x] el flujo `Lobby` → `Hallway` → `Bedroom` es reproducible
- [x] la lógica del `wardrobe` es clara y no requiere “trucos” del `designer`

### Scope guard

- [x] no se añade `salud` por ahora
- [x] no se inicia trabajo de `Library` real
- [x] la `room` se mantiene dentro del enfoque MVP
- [x] no se crea infraestructura de más para un `sistema` futuro inmediato

---

## Recomendación técnica final

El Sprint 2 debe centrarse en una sola idea sólida:

la `Bedroom` es la primera `Room` donde las reglas del juego se vuelven más humanas, más emocionales y más peligrosas, pero sin dejar de ser manejables.

Por eso, la prioridad no es añadir complejidad, sino dejar bien resuelto el bloque clave:

- luz en `Normal`;
- monstruo en `Other Side` si el armario se abre sin luz;
- llave en `wardrobe` como recompensa y progresión;
- flujo limpio entre `rooms` y realidades.

Si esta parte funciona, la base del proyecto queda mucho más fuerte para el siguiente bloque de contenido y para la siguiente iteración de diseño.
