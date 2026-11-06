# MVP Sprint 1 - Base del flujo jugable

- Fecha inicio: 03/11/2026
- Fecha objetivo: 08/11/2026
- Estado: 🟡 IN PROGRESS - Lobby validado, Hallway final push
- Creado: 03/11/2026
- Actualizado: 06/11/2026

## Objetivo

Validar la arquitectura del juego y demostrar el flujo mínimo de extremo a extremo:

- entrar al vestíbulo
- recoger la llave del personal
- abrir la puerta
- entrar al pasillo
- activar SHIFT
- cambiar a la otra realidad
- eliminar el obstáculo
- continuar el recorrido

No se busca contenido completo ni pulido artístico. El objetivo es comprobar que los sistemas base están bien conectados.

---

## Alcance

### Incluido

- GameManager global con flags
- RoomManager y carga por room ID
- InventoryManager + InteractionManager
- Lógica básica de examinar, recoger, usar
- Sistema SHIFT y cambio de realidad
- Hallway con obstacle gating
- Puertas placeholder de Library/Bedroom (logs, no rooms reales)

### No incluido

- Biblioteca / sótano / sala ritual reales
- Save avanzado
- Audio final
- Arte final
- UI compleja o narrativa profunda

---

## Requisitos funcionales mínimos

### 1. Estado del juego

- SetFlag() y HasFlag() en GameManager
- Flags mínimas: HasStaffKey, DoorOpened, RealityUnlocked, ShiftPerformed

### 2. Habitaciones

- Cargar rooms por ID
- Cambiar entre Lobby y Hallway
- Actualizar interactivos visibles por sala
- Mantener continuidad del flujo

### 3. Inventario

- Añadir y eliminar objetos
- Consultar disponibilidad
- Usar un item sobre un interactivo

### 4. Interacción

- Examinar objetos
- Recoger llave
- Usar llave sobre puerta
- Resolver acceso al siguiente tramo

### 5. SHIFT / realidad

- Alternar entre Normal y Other Side
- Cambiar fondo visual
- Mostrar/ocultar interactivos según la realidad
- Obstáculo desaparece en la realidad correcta

---

## Estado actual

### Base técnica ✅

- [x] GameManager
- [x] sistema de flags
- [x] modelos de Room / Interactable / InventoryItem
- [x] estructura base de escenas y scripts

### Habitaciones ✅

- [x] Lobby
- [x] Hallway
- [x] transición entre salas
- [x] interactivos visibles por room

### Interacción e inventario ✅

- [x] recoger llave
- [x] usar llave sobre puerta
- [x] inventario mínimo
- [x] lógica de interacción centralizada

### SHIFT / realidad ✅

- [x] RealityManager funcional
- [x] cambio entre realidades
- [x] backgrounds por room
- [x] obstacle oculto en Other Side
- [x] puertas placeholder en Hallway

### Validación del flujo ✅

- [x] Lobby: recoger llave
- [x] puerta: abrir acceso
- [x] Hallway: cambiar de room
- [x] SHIFT: cambiar realidad
- [x] obstáculo desaparece / reaparece
- [x] puertas placeholder responden con log

---

## Criterios de aceptación del sprint

La demo se considera correcta si se puede grabar una secuencia funcional con:

1. entrada al Vestíbulo
2. recogida de la Llave del Personal
3. uso de la llave sobre la puerta
4. entrada al Pasillo Principal
5. LEFT SHIFT para cambiar de realidad
6. fondo y obstáculo actualizados visualmente
7. LEFT SHIFT para volver a la normalidad
8. click en las puertas placeholder de Library y Bedroom (log de confirmación)
9. secuencia sin errores lógicos ni de flujo

---

## Checklist de cierre antes de la demo

### Bloqueantes

- [ ] flujo completo Lobby → Hallway funciona repetidamente
- [ ] LEFT SHIFT responde desde teclado
- [ ] RealityManager cambia visualmente la realidad
- [ ] obstáculo desaparece/en aparece correctamente
- [ ] puertas placeholder no rompen la progresión
- [ ] no hay errores críticos en consola
- [ ] flags no se desincronizan

### Visual / presentación mínima

- [ ] cambio de fondo visible
- [ ] realidades diferenciadas de forma clara
- [ ] feedback de consola útil
- [ ] la secuencia es grabable sin explicación extra

### Scope guard

- [ ] no se implementan Library/Bedroom reales antes de la demo
- [ ] no se añade sistema grande si complica la demo
- [ ] cualquier extra queda para siguiente sprint

---

## Polish mínimo recomendado para la demo 1

Esto no es un sistema nuevo: es una capa ligera para que la demo se sienta clara y terminada sin romper la arquitectura actual.

### Objetivo

- feedback al recoger item
- mostrar lo que lleva el personaje
- hacer transición entre rooms y realidades menos brusca
- mantener el código simple y mucho más manejable para 2 personas

### 1) HUD centralizado con Panel Renderer

Usar el mismo patrón que ya tiene el SHIFT button:

- un único PanelRenderer para todo el HUD
- VisualTreeAsset + USS para los estilos
- binding mediante RegisterUIReloadCallback()
- eventos para actualizar inventario, feedback y overlay

Recomendación:

- HUDRoot con InventoryPanel, FeedbackText, TransitionOverlay, RealityPulse
- el ShiftButton ya no vive como UI aislada; forma parte del mismo HUD y se actualiza desde el mismo controller
- el UIShiftButton solo manejaría el click y delegaría el estado visual al HUD root

Esto favorece coherencia, evita duplicación de bindings y deja el sistema preparado para crecer sin sobre-diseñar.

### 2) Inventario visual mínimo

- 2-4 slots fijos
- iconos simples o placeholders
- InventoryManager.ItemAdded / ItemRemoved actualiza el panel
- Staff Key es el primer item visible

### 3) Feedback textual

- panel pequeño con texto temporal
- UIManager.ShowFeedback('...')
- mensaje de 1-2 segundos
- solo el último mensaje necesita mostrarse

### 4) Transiciones mínimas

- fade negro entre rooms
- pulse corto entre realidades
- duración breve (0.25s-0.5s)
- no más cinematografía que eso

### 5) Estructura recomendada

- GameManager: flags y estado global
- InventoryManager: items
- InteractionManager: acciones del jugador
- RoomManager: rooms y transiciones
- RealityManager: cambio de mundo
- UIManager / HUDController: feedback + inventory + overlay

### 6) Qué no hacer

- no crear un manager de transiciones separado
- no construir UI compleja con más de un HUD principal
- no entrar en Library/Bedroom real
- no usar UIDocument runtime si ya tenemos PanelRenderer validado

---

## Plan de trabajo realista para 2 personas

### Bloque A — UI básica (60-90 min)

- [ ] crear HUD root con PanelRenderer
- [ ] panel de inventario
- [ ] feedback textual
- [ ] conectar ShiftButton al HUD centralizado

### Bloque B — Transiciones (45-60 min)

- [ ] fade entre room changes
- [ ] pulse de realidad
- [ ] validación visual del flujo

### Bloque C — Demo final (30-45 min)

- [ ] probar flujo completo de extremo a extremo
- [ ] revisar consola y feedback
- [ ] grabar demo

---

## Conclusión

El MVP base ya está bien encaminado. La prioridad ahora es cerrar la demo con flujo estable, evidenciar que SHIFT funciona y evitar que el polish mínimo convierta el sprint en un proyecto paralelo. El objetivo es seguir siendo simple, legible y terminable.

El botón SHIFT debe integrarse dentro del HUD centralizado, no como pieza aislada: eso hace más limpia la UI y mantiene el patrón ya validado en el proyecto.
