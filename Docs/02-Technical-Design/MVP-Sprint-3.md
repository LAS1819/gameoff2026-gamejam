# MVP Sprint 3 - Library y preparación para Basement

- Fecha inicio: 12/11/2026
- Fecha objetivo: 19/11/2026
- Estado: 🟡 EN PLANEAMIENTO - primero `Library`, luego preparación para `Basement`
- Creado: 12/11/2026
- Actualizado: 14/11/2026

## Objetivo

Este sprint cierra la transición entre la Bedroom validada y la siguiente fase de contenido del MVP: la `Library`.

La prioridad es clara: construir una room pequeña, legible y narrativa que funcione como punto de narrativa, progresión y preparación para el `Basement`, sin abrir un sistema demasiado grande ni romper la arquitectura actual.

La `Library` no debe ser una "mega-room" de contenido; debe ser una pieza útil del juego que refuerce la identidad del edificio, la manipulación de la memoria y el cambio entre realidades.

---

## Baseline heredada del Sprint 2

Este sprint asume como contrato fijo todo lo definido en el Sprint 2:

- Clic izquierdo: interactuar, recoger o usar el item seleccionado.
- Clic derecho: `Examine` directo, sin menús ni side-effects.
- Input System de Unity, sin APIs legacy del `Input` clásico.
- Descripciones por realidad con fallback: `Other Side` -> `Normal` -> general.
- Estructura mínima de datos y scripts: `RoomData`, `InteractableData`, `ItemData` y lógica de interacción sin sobrediseño.
- No se exige migración a `ScriptableObject` todavía; se revisará solo si el contenido crece más de lo previsto.

Esto permite que el Sprint 3 se centre en contenido, room logic y progresión, no en reordenar la base del sistema.

---

## Alcance del sprint

### Incluido

- `Library` room data e interactables base
- libro principal o estantería como elemento central de la room
- item oculto con significado narrativo y funcional
- cambio de significado entre `Normal` y `Other Side`
- teaser de `Basement` o acceso secreto sugerido
- salida de vuelta al `Hallway`
- validación del flujo con `Bedroom`, `Hallway` y `SHIFT`

### No incluido

- biblioteca completa con decenas de libros interactivos
- sistema de archivos, documentos o estanterías complejas
- puzzle completo del `Basement`
- múltiples ramas de progresión o contenido secundario
- VFX, audio final o pulido artístico exhaustivo
- ingeniería extra no necesaria para el MVP

---

## Prioridad de trabajo

### Prioridad 1 — Library mínima funcional

La `Library` debe sentirse distinta de la `Bedroom` y debe producir un avance claro en la progresión.

1. Se carga desde `Hallway` con la llave o la lógica apropiada.
2. Tiene un elemento central identificable: libro o estantería.
3. Ese elemento revela un item oculto o pista útil.
4. La room tiene un secreto o salida que sugiere un paso más adelante.
5. La tensión de la `Other Side` se nota visual o textualmente.

### Prioridad 2 — Preparación para Basement

La `Library` debe dejar una base reutilizable para el siguiente bloque:

- la progresión del item oculto debe ser clara;
- la salida o acceso debe sentirse sugerente y no arbitrario;
- la room debe dejar pistas narrativas de lo que viene después;
- la lógica debe ser simple y mantenible.

### Prioridad 3 — Refuerzo de tono

La `Library` debe reforzar la identidad del edificio como un espacio de memoria y verdad manipulada.

No hace falta convertirla en un gran rompecabezas. Basta con que:

- la memoria cambie de forma visible entre realidades;
- el contenido parezca “verdadero” en una realidad y distorsionado en la otra;
- la room comunique que la verdad del edificio no es estable.

---

## Requisitos funcionales mínimos

### 1. Room setup

- `Library` integrada al `RoomManager` como room funcional más.
- Carga desde `Hallway` con `linkedRoomId` coherente.
- Sigue el modelo actual de `RoomData`, `InteractableData`, `ItemData` y flags.

### 2. Interacción principal

- El elemento central debe ser interactuable y memorable.
- Debe variar su comportamiento entre realidades.
- Al examinarlo, debe ofrecer una pista, un item oculto o una sensación de secretos.
- El objeto obtenido debe ser útil y no decorativo.

### 3. Lógica de realidad

- El mismo libro/estantería debe comportarse distinto en `Normal` y `Other Side`.
- El cambio debe ser visible sin introducir un sistema complejo.
- La room debe apoyar la idea de que la memoria y la verdad son frágiles.

### 4. Progresión mínima

- El jugador entiende qué ha encontrado y por qué importa.
- La room prepára la siguiente fase sin romper la base del juego.
- La salida o secreto debe apuntar al `Basement` sin requerir lógica gigantesca.

---

## Plan de trabajo por bloques

### Bloque A — Room data y estructura (30-45 min)

- crear `Library` como room cargable desde el `Hallway`;
- definir interactables base: libro, estantería, acceso/el secreto, salida;
- validar que se visualiza y carga con el sistema actual.

### Bloque B — Elemento central y item oculto (45-60 min)

- definir el objeto principal (`Worn Book`, `Memory Book` o equivalente);
- decidir el item oculto relevante (`Disquete`, `Key Fragment`, etc.);
- asegurar que su valor narrativo y funcional quede claro.

### Bloque C — Cambio entre realidades (45-60 min)

- vincular la lógica visual y textual de la room a `Normal` / `Other Side`;
- hacer visible que el contenido o la estantería cambia según la realidad;
- dejar un punto de descubrimiento que se vuelva legible en la realidad correcta.

### Bloque D — Teaser de Basement (30-45 min)

- preparar un acceso, secreto o puerta sugerente;
- mantenerlo simple y narrativo, no como puzzle completo;
- dejar una promesa para la siguiente iteración.

### Bloque E — Validación end-to-end (30 min)

- entrar desde `Hallway`;
- interactuar con la room;
- obtener el item oculto;
- comprobar cambio de realidad;
- ver la salida o acceso sugerido;
- confirmar que no rompe `Bedroom`, `Hallway` ni el flujo global.

---

## Regla de diseño para este sprint

### Mantener

- la arquitectura data-driven existente;
- la lógica de `SHIFT` como eje central del juego;
- la room como una pieza de progresión, no como un sistema independiente;
- una solución simple y legible para futuras rooms.

### Simplificar

- limitar el número de interactables;
- evitar puzzles complejos o ramificaciones de contenido;
- no abrir el `Basement` completo;
- no introducir sistemas que no sirvan directamente a la narrativa y la progresión.

### Evitar

- docenas de libros interactivos;
- varias capas de puzzle o estado por room;
- sistemas de scripting “de futuro” que no se necesiten ahora;
- crear contenido grande antes de validar la room mínima.

---

## Criterios de aceptación

La `Library` se considera lista si:

1. El jugador puede entrar en la room desde `Hallway` con la lógica adecuada.
2. La room tiene identidad propia respecto a la `Bedroom`.
3. El elemento central tiene sentido narrativo y funcional.
4. El cambio de realidad se percibe claramente.
5. El player obtiene un item oculto o pista relevante.
6. La room sugiere la siguiente fase del proyecto sin resolverla completa.
7. La room no rompe la infraestructura base ni aumenta la complejidad innecesariamente.
8. La room sirve como puente claro hacia el `Basement`.

---

## Checklist final

### Bloqueantes

- [ ] `Library` cargada y funcional desde `Hallway`
- [ ] elemento central identificable
- [ ] item oculto recuperable
- [ ] cambio entre realidades visible
- [ ] teaser de `Basement` o secret door sugerido
- [ ] sin errores de room loading ni flujo

### Funcional

- [ ] `SHIFT` sigue funcionando dentro de la room
- [ ] la room no rompe la base de `Bedroom`
- [ ] el jugador entiende qué ha encontrado y por qué importa
- [ ] la Library se siente preparada para la siguiente iteración

### Scope guard

- [ ] no se añade contenido que rompa el MVP
- [ ] no se resuelve el `Basement` completo
- [ ] no se sobreingenieriza la room
- [ ] la solución sigue siendo clara y mantenible

---

## Recomendación técnica final

La `Library` debe ser lo más limpia y legible posible: una room pequeña, importa su tono, genera progresión y marca la siguiente etapa del juego.

Esto es un sprint de disciplina, no de ambición. La mejor versión de la `Library` es la que hace sentir el edificio como un lugar de memoria distorsionada y prepara el `Basement` de manera natural, sin inflar la complejidad.

En resumen: la `Library` debe ser una room narrativa sólida, pequeña, útil y elegantemente simple.
