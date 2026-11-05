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
