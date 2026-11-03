# MVP Sprint 1 - Base del flujo jugable

- Fecha inicio: 03/11/2026 (miércoles)
- Fecha objetivo: 08/11/2026 (domingo)
- Estado: Planeado

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

### Tarea 1 - Base técnica

- [ ] Crear GameManager
- [ ] Crear sistema de flags
- [ ] Definir Data models para Room, Interactable e InventoryItem
- [ ] Preparar estructura base de escenas y prefabs

### Tarea 2 - Sistema de habitaciones

- [ ] Implementar RoomManager
- [ ] Cargar sala activa
- [ ] Cambiar de sala
- [ ] Actualizar lista de elementos interactivos

### Tarea 3 - Sistema de interacción y inventario

- [ ] Implementar InventoryManager
- [ ] Implementar InteractionManager
- [ ] Añadir acción de examinar
- [ ] Añadir acción de recoger
- [ ] Añadir acción de usar objeto sobre interactivo

### Tarea 4 - Sistema SHIFT / realidad

- [ ] Implementar RealityManager
- [ ] Cambiar entre realidades
- [ ] Actualizar los elementos que cambian visualmente
- [ ] Hacer que el obstáculo del pasillo desaparezca en la realidad correcta

### Tarea 5 - Validación del flujo MVP

- [ ] Vestíbulo: recoger llave
- [ ] Puerta: activar flag y abrir acceso
- [ ] Pasillo: cargar nueva habitación
- [ ] SHIFT: cambiar visualmente de realidad
- [ ] El obstáculo desaparece
- [ ] El jugador puede continuar hacia la siguiente zona

### Tarea 6 - Presentación mínima

- [ ] UI básica para inventario y feedback de texto
- [ ] fondos provisionales
- [ ] transiciones de realidad compactas
- [ ] comprobación visual del flujo completo

---

## Criterios de aceptación

Este sprint se considera completo si es posible grabar una demostración en la que se vea, de manera funcional:

1. entrar en el Vestíbulo,
2. recoger la Llave del Personal,
3. usar la llave sobre la puerta,
4. entrar al Pasillo Principal,
5. activar SHIFT,
6. cambiar a la otra realidad,
7. ver que el obstáculo desaparece,
8. poder avanzar sin errores de lógica ni de flujo.

---

## Definición de terminado

El sprint estará terminado cuando:

- el flujo principal del MVP funciona sin bloquearse,
- no hay errores críticos en la lógica de flags,
- el inventario y la interacción responden correctamente,
- la transición entre realidades es visible y consistente,
- la secuencia puede repetirse con éxito en una sesión corta.

---

## Riesgos y restricciones

- No ampliar alcance más allá del MVP.
- No empezar por contenido opcional ni contenido posterior al pasillo.
- Si un sistema tarda demasiado en integrarse, simplificarlo antes que añadir complejidad.
- Priorizar que el flujo funcione sobre pulir visualmente.

---

## Resultado esperado

La entrega del sprint debe servir como base arquitectónica para el resto del proyecto: un juego mínimo, ejecutable, con una progresión real y validada, listo para ampliarse en siguientes sprints.
