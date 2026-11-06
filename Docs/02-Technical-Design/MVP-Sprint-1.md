# MVP Sprint 1 - Base del flujo jugable

- Fecha inicio: 03/11/2026 (miércoles)
- Fecha objetivo: 08/11/2026 (domingo)
- Estado: 🟡 IN PROGRESS - Lobby Complete, Hallway Final Push
- Creado: 03/11/2026
- Actualizado: 05/11/2026 (sesión Sprint 1 - Cierre MVP Base)

## Objetivo

Entregar una primera versión funcional del MVP centrada en validar la arquitectura principal del juego y demostrar que el flujo mínimo de juego funciona de extremo a extremo:

- entrada al vestíbulo
- recoger la llave del personal
- abrir la puerta
- entrar al pasillo
- activar SHIFT
- cambiar a la otra realidad
- remover el obstáculo
- continuar el recorrido

No se busca contenido completo ni pulido artístico. El objetivo principal es comprobar que los sistemas base están bien conectados y pueden sostener la propuesta del juego.

---

## Alcance del sprint

### Incluido

- Sistema global de estado: GameManager
- Sistema de flags
- Habitaciones mínimas: Vestíbulo y Pasillo Principal
- Objeto interactivo: Llave del Personal
- Interacción básica: examinar, recoger, usar
- Inventario mínimo
- Cambio de sala
- Sistema SHIFT básico
- Cambio de realidad entre Normal y Other Side
- UI mínima para feedback
- Elementos visuales temporales para validar progresión

### No incluido

- Sistema temporal completo
- Eventos dinámicos
- Finales
- Biblioteca, sótano, sala ritual
- Guardado avanzado
- Audio definitivo
- Arte final

---

## Issue / entregable principal

### Título

`MVP Sprint 1 - Validar arquitectura y flujo mínimo SHIFT`

### Descripción

Implementar la base técnica y el flujo jugable mínimo para verificar que:

1. el jugador puede interactuar con el entorno,
2. el inventario funciona,
3. las flags gestionan la progresión,
4. la habitación y la realidad cambian correctamente,
5. el cambio de realidad permite resolver el obstáculo principal del primer tramo del juego.

---

## Requisitos funcionales mínimos

### 1. Game state y flags

- Crear un sistema global de estado con acceso persistente durante la partida.
- Implementar `SetFlag()` y `HasFlag()`.
- Mantener al menos estas flags del MVP:
  - `HasStaffKey`
  - `DoorOpened`
  - `RealityUnlocked`

### 2. Habitaciones

- Cargar habitaciones por ID.
- Cambiar entre Vestíbulo y Pasillo Principal.
- Actualizar interactivos visibles en cada sala.
- Mantener la posición del jugador entre cambios de sala.

### 3. Inventario

- Añadir objetos al inventario.
- Eliminar objetos consumidos.
- Consultar si un objeto está disponible.
- Usar un objeto sobre un interactivo del escenario.

### 4. Interacción

- Examinar objetos o elementos del escenario.
- Recoger la llave.
- Usar la llave sobre la puerta.
- Resolver el acceso a la siguiente zona.

### 5. SHIFT / realidad

- Permitir alternar entre Normal y Other Side.
- Cambiar el fondo o visualización del escenario.
- Mostrar/ocultar interactivos según la realidad activa.
- El obstáculo del pasillo debe desaparecer en la realidad correcta.

---

## Tareas por prioridad

### Tarea 1 - Base técnica ✅ COMPLETE

- [x] Crear GameManager
- [x] Crear sistema de flags
- [x] Definir Data models para Room, Interactable e InventoryItem
- [x] Preparar estructura base de escenas y prefabs

### Tarea 2 - Sistema de habitaciones ✅ COMPLETE

- [x] Crear Sprites necesarios para Lobby
- [x] Implementar RoomManager
- [x] Cargar sala activa
- [x] Cambiar de sala
- [x] Actualizar lista de elementos interactivos

### Tarea 3 - Sistema de interacción y inventario ✅ COMPLETE

- [x] Implementar InventoryManager
- [x] Implementar InteractionManager
- [x] Añadir acción de examinar
- [x] Añadir acción de recoger
- [x] Añadir acción de usar objeto sobre interactivo

### Tarea 4 - Sistema SHIFT / realidad 🔴 IN PROGRESS (PRIORIDAD AHORA)

- [x] Crear Sprites necesarios para Hallway
- [x] Implementar RealityManager
- [x] Cambiar entre realidades (código core)
- [ ] **Conectar InputHandler → RealityManager (LEFT SHIFT)**
- [ ] **Actualizar RealityManager para cambiar backgrounds dinámicamente**
- [ ] **Configurar BackgroundNormal y BackgroundOtherSide en Hallway**
- [ ] **Hacer que HallwayObstacle desaparezca en El Otro Lado (visibilidad dinámica)**
- [ ] **Crear puertas placeholder para Library y Bedroom (logs en click)**

### Tarea 5 - Validación del flujo MVP 🔴 IN PROGRESS

- [x] Vestíbulo: recoger llave ✅
- [x] Puerta: activar flag y abrir acceso ✅
- [x] Pasillo: cargar nueva habitación ✅
- [ ] **SHIFT (LEFT SHIFT): cambiar realidad visualmente con backgrounds**
- [ ] **El obstáculo desaparece en El Otro Lado**
- [ ] **Puertas Library/Bedroom responden (logs)**
- [ ] **Flujo completo sin errores**: Lobby → Hallway → SHIFT → Toggle realidad → Puertas funcionales

### Tarea 6 - Presentación mínima ⏳ NOT STARTED

- [ ] UI básica para inventario y feedback de texto
- [ ] fondos provisionales (Normal y OtherSide en Hallway)
- [ ] transiciones de realidad compactas
- [ ] comprobación visual del flujo completo
- [ ] Demo grabada del MVP funcionando end-to-end

---

## Criterios de aceptación

Este sprint se considera completo si es posible grabar una demostración en la que se vea, de manera funcional:

1. entrar en el Vestíbulo,
2. recoger la Llave del Personal,
3. usar la llave sobre la puerta,
4. entrar al Pasillo Principal,
5. presionar LEFT SHIFT para cambiar de realidad,
6. ver que el fondo cambia y el obstáculo desaparece,
7. presionar LEFT SHIFT de nuevo para volver a la realidad normal,
8. ver que el obstáculo reaparece y el fondo vuelve,
9. hacer click en las puertas de Library y Bedroom (verán logs confirmando el click),
10. completar la secuencia sin errores de lógica ni de flujo.

---

## Checklist de cierre - Ready para Demo 1

Antes de dar por cerrada la entrega del sprint, se recomienda validar esta lista en orden y no movernos a contenido nuevo hasta que todo lo que esté marcado como bloqueante esté verde.

### Bloqueantes del MVP (para demostrar el flujo principal)

- [ ] Entrada al Vestíbulo desde la escena base funciona sin errores.
- [ ] La llave del personal se recoge correctamente y entra al inventario.
- [ ] La puerta del vestíbulo requiere la llave y se desbloquea con la flag `DoorOpened`.
- [ ] El jugador entra al Pasillo Principal y la sala cambia correctamente.
- [ ] `LEFT SHIFT` activa el cambio de realidad desde el teclado.
- [ ] El `RealityManager` cambia el fondo visualmente entre `Normal` y `Other Side`.
- [ ] El obstáculo del pasillo desaparece en `Other Side` y vuelve en `Normal`.
- [ ] Las puertas placeholder de `Library` y `Bedroom` responden con log y no rompen el flujo.
- [ ] El sistema de flags no se desincroniza durante la ejecución (sin `HasStaffKey`/`DoorOpened`/`RealityUnlocked` inconsistentes).
- [ ] No hay errores críticos en consola durante una ejecución completa del flujo.
- [ ] La secuencia puede repetirse en al menos 2 ejecuciones consecutivas sin bloqueos.

### Validación visual mínima de la demo

- [ ] Se ve claramente el cambio de fondo al activar SHIFT.
- [ ] El obstáculo se oculta/aparece de forma visible y consistente.
- [ ] La transición entre rooms mantiene la sensación de continuidad del recorrido.
- [ ] El feedback de consola es claro y útil para validar el progreso.
- [ ] El flujo queda grabable sin necesidad de explicar cada acción antes de la toma.

### Guardas de scope para el cierre del sprint

- [ ] No se añade contenido de `Library` ni `Bedroom` completo antes de la demo 1.
- [ ] No se implementan sistemas grandes de UI o narrativa si bloquean la validación del flujo.
- [ ] Si se trabaja extra fuera del MVP, se limita a pulido mínimo que no altere la arquitectura base.
- [ ] Toda mejora post-demo debe estar documentada como tarea de siguiente sprint o follow-up.

### Recomendación de prioridad para las 48 horas que quedan

- [ ] Dar prioridad a cerrar la demo 1 con flujo estable y grabable.
- [ ] Dejar para después la UI de inventario y feedback más elaborada.
- [ ] Si se hace pulido extra, hacerlo como mejora local y no como bloqueante del MVP.

### Post-demo: mejoras valiosas pero no esenciales para Sprint 1

- [ ] Inventario visual con iconos por item y estado de recogida.
- [ ] Cuadro de feedback textual con mensajes temporales al interactuar.
- [ ] Transición leve entre habitaciones (fade / crossfade / overlay).
- [ ] Transición visual entre realidad normal y Other Side (más suave que un cambio abrupto).
- [ ] Feedback audio o FX mínimo para reforzar el momento SHIFT.

> Recomendación clave: estas mejoras son buenas, pero no deben convertirse en una segunda arquitectura. Para este proyecto, vale con una implementación mínima y funcional: inventario básico, feedback textual y una transición muy simple. Eso mantiene la entrega del sprint centrada en la prueba del flujo principal, sin inflar la scope.

---

## FOCUS SESSION - Sprint 1 Final Push (05/11/2026)

### Estrategia de cierre: MVP Base → Demo Funcional

En esta sesión nos enfocamos en **TERMINAR EL MVP BASE** (Opción A) con el mínimo viable:

1. **Hallway SHIFT System Completo** (60-90 min)
   - Conectar InputHandler ↔ RealityManager para LEFT SHIFT
   - RealityManager cambia BackgroundNormal ↔ BackgroundOtherSide
   - HallwayObstacle desaparece/aparece dinámicamente
   - Validar transiciones visuales

2. **Puertas Placeholder Library/Bedroom** (20-30 min)
   - Crear dos GameObjects interactivos en Hallway
   - Al clickear: log en consola confirmando la room
   - Validar que el sistema de click funciona

3. **Validación End-to-End** (20 min)
   - Grabar demo del flujo completo
   - Documentar cualquier bug

### Alcance deliberadamente limitado

- ❌ NO implementar Library/Bedroom rooms completas
- ❌ NO implementar UI avanzada
- ❌ NO agregar más interactables de los necesarios
- ✅ Validar que la arquitectura base aguanta el flujo principal
- ✅ Demostrar que SHIFT funciona como se intenta usar en el juego

---

El sprint estará terminado cuando:

- el flujo principal del MVP funciona sin bloquearse,
- no hay errores críticos en la lógica de flags,
- el inventario y la interacción responden correctamente,
- la transición entre realidades es visible y consistente,
- la secuencia puede repetirse con éxito en una sesión corta.

---

## Propuesta de polish mínimo para la demo 1

Este no es un sprint de contenido ni de sistemas nuevos. Es una capa de acabado mínimo para que la demo se sienta clara, legible y profesional sin romper la arquitectura del MVP.

### Objetivo

- dar feedback inmediato al jugador cuando recoge un objeto,
- mostrar qué items tiene el personaje,
- hacer que los cambios de room y de realidad no se sientan bruscos,
- mantener el código simple y jugable para dos personas en una game jam.

### Principio de diseño

- no crear un sistema gigante,
- no añadir más de una UI principal y un overlay de transición,
- mantener todo conectado con los managers existentes,
- priorizar legibilidad sobre arte sofisticado.

### 1) Inventario mínimo visual

#### Estructura recomendada

- `Canvas`
  - `HUD`
    - `InventoryPanel`
      - `Slot_1`
      - `Slot_2`
      - `Slot_3`
      - etc.
  - `FeedbackPanel`
  - `TransitionOverlay`

#### Requisitos

- Un panel pequeño en la parte inferior o superior.
- 2-4 slots fijos para la demo.
- Cada slot muestra icono + estado vacío/ocupado.
- El primer item es la `Staff Key`.
- La UI se actualiza al `InventoryManager.ItemAdded` y `ItemRemoved`.
- Si más adelante se añade más contenido, el sistema ampliará slots sin tocar la lógica principal.

#### Implementación mínima

- No hace falta un inventario complejo por escenas.
- Solo necesitamos un `Image` por slot y un `Text` opcional con nombre.
- La lógica puede vivir en `UIManager` con un método `RefreshInventoryUI()`.
- Si el item tiene icono asociado, se puede asignar desde el `InventoryItemData` o dejarlo como referencia directa en el slot.

### 2) Feedback textual mínimo

#### Objetivo

- confirmar cada acción importante,
- explicar al usuario qué ha pasado,
- dar sensación de “sistema vivo” sin necesidad de diálogos largos.

#### Mensajes recomendados

- `Has recogido la Llave del Personal.`
- `La puerta está cerrada. Necesitas una llave.`
- `La puerta se abre con un chasquido.`
- `Has entrado en el Pasillo Principal.`
- `El otro lado te devuelve la visión de la realidad.`

#### Implementación

- Un panel de texto central con un `TextMeshProUGUI` o `Text`.
- `UIManager.ShowFeedback(string message, float duration = 1.5f)`
- El mensaje aparece con fade o slide suave y desaparece tras un tiempo.
- No hace falta cola ni historial; solo el último mensaje es suficiente para la demo.

### 3) Transición mínima entre rooms

#### Opción recomendada

- Fade negro sencillo `CanvasGroup` o `Image` full-screen.
- El `RoomManager` activa overlay, carga la room, y luego desactiva overlay.

#### Flujo

1. `RoomManager.LoadRoom()` empieza la transición.
2. Overlay se vuelve visible con alpha 0 -> 1.
3. Se cambia la room.
4. Overlay vuelve a alpha 1 -> 0.

#### Duración recomendada

- 0.25s a 0.5s, no más.
- Debe sentirse corto y limpio, no cinematográfico.

### 4) Transición mínima entre realidades

#### Opción recomendada

- Un `Image` o `CanvasGroup` con color tint que se activa al cambiar de realidad.
- El cambio de fondo ya lo hace `RealityManager`; la transición solo suaviza la percepción visual.

#### Reglas

- Si la transición es muy corta, mejor que un efecto largo.
- La idea es comunicar un “cambio de mundo”, no crear un segundo mini-puzzle.

#### Implementación simple

- `RealityManager` puede disparar un evento `RealityChanged` o invocar un `UIManager.TriggerRealityPulse()`.
- El pulse puede ser un color de 0.2s a 0.4s con alpha de 0.15-0.25.

### 5) Estructura técnica mínima

#### Managers involucrados

- `GameManager`: estado global y flags
- `InventoryManager`: items recogidos
- `InteractionManager`: acciones del jugador
- `RoomManager`: carga de habitaciones y transiciones
- `RealityManager`: cambio de mundo
- `UIManager`: feedback, inventario y overlay

#### Uso recomendado de UI Toolkit con Panel Renderer

Para el pulido mínimo de la demo, conviene seguir el mismo patrón que ya usamos con [UIShiftButton.cs](../../Assets/Scripts/UI/UIShiftButton.cs) y [shift-button-style-sheet.uss](../../Assets/UI/shift-button-style-sheet.uss):

- usar `PanelRenderer` en lugar de `UIDocument` si la intención es mostrar UI runtime en la escena,
- mantener la lógica en scripts C# y el estilo en `.uss`,
- separar claramente el árbol visual (`.uxml`) del comportamiento (`PanelRenderer` + script),
- hacer binding después de cada recarga del panel y no depender de `Find` o `GameObject.Find` para cada elemento.

Esto es más estable para una jam porque evita el patrón legacy de `UIDocument` y encaja mejor con un sistema de HUD dinámico, al mismo tiempo que reutiliza lo que ya hemos validado con el botón SHIFT.

#### Estructura recomendada para el HUD

- `HUDRoot` (GameObject con `PanelRenderer`)
  - `InventoryPanel`
    - `slot-0`
    - `slot-1`
    - `slot-2`
  - `FeedbackText`
  - `TransitionOverlay`
  - `RealityPulse`

#### Regla de implementación

- el `PanelRenderer` debe llevar `PanelSettings` y `VisualTreeAsset` asignados,
- el script escucha `RegisterUIReloadCallback()` para enlazar `VisualElement`s,
- los cambios de estado se realizan con clases CSS (`.visible`, `.hidden`, `.full`, `.pulse`) y no con `SetEnabled()` para montar estados visuales,
- la lógica del inventario se actualiza por eventos (`ItemAdded`, `ItemRemoved`, `RoomChanged`, `RealityChanged`).

#### Patrón de comportamiento sugerido

```csharp
private void HandlePanelReloaded(PanelRenderer panelRenderer, VisualElement root, int version)
{
    inventorySlots = new List<VisualElement>
    {
        root.Q<VisualElement>("slot-0"),
        root.Q<VisualElement>("slot-1"),
        root.Q<VisualElement>("slot-2")
    };

    feedbackText = root.Q<Label>("feedback-text");
    transitionOverlay = root.Q<VisualElement>("transition-overlay");
    RefreshHud();
}
```

Esto mantiene el patrón usado en el botón SHIFT: el binding se hace en una sola zona y luego la lógica visual se activa desde eventos. Resulta más sostenible que intentar gestionar cada elemento del HUD a mano en Update().

#### Qué no hacer

- No crear un manager específico para “TransitionEffects” si se puede hacer con una coroutine en `RoomManager`.
- No crear un sistema de inventario completo con save/load o UI modular, porque no lo necesitamos todavía.
- No introducir nuevas escenas ni arquetipos complejos.
- No usar `UIDocument` en runtime si ya tenemos una solución de Panel Renderer funcionando y validada.

### 6) Plan de trabajo realista

#### Bloque A - Inventario + feedback (60-90 min)

- [ ] crear `HUD` en la escena base
- [ ] panel de inventario con 2-4 slots
- [ ] mostrar `Staff Key` al recogerla
- [ ] `UIManager.ShowFeedback()` para mensajes temporales

#### Bloque B - Transiciones (45-60 min)

- [ ] overlay de fade para room changes
- [ ] pulse sutil entre realidades
- [ ] validar que no rompe la lógica de flags

#### Bloque C - Demo validation (30-45 min)

- [ ] correr flujo completo
- [ ] verificar UI y transiciones
- [ ] grabar la demo con el mínimo de ajustes

### 7) Criterio de éxito del polish mínimo

La demo está lista si:

- el jugador ve claramente que ha recogido la llave,
- entiende cuándo abre la puerta,
- no se pierde al entrar al pasillo,
- el cambio de realidad se percibe sin que el flujo se rompa,
- la transición entre rooms no genera lag ni confusión,
- el total sigue siendo 3-4 funciones de UI, no un proyecto aparte.

### Recomendación final

Para esta jam y para un solo desarrollador, esta propuesta es la mejor relación entre esfuerzo y valor: suficiente para que el juego se sienta pulido, pero todavía muy pequeña, muy controlable y fácil de terminar. Si esto va bien, la demo parece más sólida y el siguiente sprint puede empezar desde una base mucho más limpia.

---

## Documentación de referencia

Este sprint se apoya en tres documentos complementarios que detallan la implementación exacta:

1. **[MVP-Implementation-Quickstart.md](./MVP-Implementation-Quickstart.md)** ← **EMPEZAR AQUÍ**
   - Checklist ejecutable paso a paso
   - Fases de implementación (Scenes, GameObjects, Scripts, Validation)
   - Referencia cruzada a los otros documentos

2. **[MVP-Data-Specification.md](./MVP-Data-Specification.md)**
   - Definición exacta de `RoomData`, `InteractableData`, `InventoryItemData`
   - Valores concretos para cada objeto del MVP
   - Validación de campos y tipos

3. **[MVP-Concrete-Hierarchy.md](./MVP-Concrete-Hierarchy.md)**
   - Estructura exacta de GameObjects en ambas escenas
   - Propiedades de cada objeto (posición, sprite, collider, etc.)
   - Componentes requeridos por cada interactable

4. **[MVP-Flag-Sequence.md](./MVP-Flag-Sequence.md)**
   - Flujo exacto de interacciones y cambios de estado
   - Cuándo se activa cada flag y sus consecuencias
   - Secuencia de validación en Play Mode

---

## Riesgos y restricciones

- No ampliar alcance más allá del MVP.
- No empezar por contenido opcional ni contenido posterior al pasillo.
- Si un sistema tarda demasiado en integrarse, simplificarlo antes que añadir complejidad.
- Priorizar que el flujo funcione sobre pulir visualmente.

---

## Resultado esperado

La entrega del sprint debe servir como base arquitectónica para el resto del proyecto: un juego mínimo, ejecutable, con una progresión real y validada, listo para ampliarse en siguientes sprints.

El flujo debe completarse sin errores siguiendo exactamente la secuencia documentada en [MVP-Flag-Sequence.md](./MVP-Flag-Sequence.md).
