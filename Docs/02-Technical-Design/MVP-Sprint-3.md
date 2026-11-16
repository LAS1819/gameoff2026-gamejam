# MVP Sprint 3 - Library cerrada + Bedroom PC / Floppy Disk como puente al Basement

- Fecha inicio: 16/11/2026
- Fecha objetivo: 22/11/2026
- Estado: 🟡 EN CURSO - `Library` cerrada y bloque actual: `Bedroom` + `PC` + `Floppy Disk` + código del `Basement`
- Creado: 12/11/2026
- Actualizado: 16/11/2026

## Objetivo

Este sprint cierra la parte narrativa y funcional de la `Library` y, a la vez, define el siguiente bloque de progresión del MVP: el ordenador personal de la `Bedroom` y el uso del `Floppy Disk` encontrado dentro del `Worn Book`.

La idea es dejar clara la lógica del juego en términos de progresión:

- `Library`: revela el secreto, entrega la pista y el item oculto.
- `Bedroom` + `PC`: transforma ese item oculto en un código útil.
- `Basement`: se cierra la cadena narrativa y funcional con la promesa del siguiente bloque.

La prioridad no es crear un sistema complejo, sino cerrar el ciclo de progresión de forma legible, mantenible y jugable.

---

## Baseline heredada del Sprint 2

Este sprint asume como contrato fijo todo lo definido en el Sprint 2:

- Clic izquierdo: interactuar, recoger o usar el item seleccionado.
- Clic derecho: `Examine` directo, sin menús ni side-effects.
- Input System de Unity, sin APIs legacy del `Input` clásico.
- Descripciones por realidad con fallback: `Other Side` -> `Normal` -> general.
- Estructura mínima de datos y scripts: `RoomData`, `InteractableData`, `ItemData` y lógica de interacción sin sobrediseño.
- No se exige migración a `ScriptableObject` todavía; se revisará solo si el contenido crece más de lo previsto.

Esto nos permite mantener el enfoque del sprint en la progresión del juego, no en reestructurar la base del sistema.

---

## Estado actual de la `Library`

La `Library` está en una buena posición como room MVP:

- la room carga correctamente desde el `Hallway`;
- la estantería interactiva funciona con la lógica de la `Other Side`;
- el `Worn Book` puede ser seleccionado, usado y reparado en el estado correcto;
- la `Shelf Secret Door` se abre y oculta la zona del `Basement`;
- `Basement Door` y `Code Panel` quedan condicionados a la apertura del secreto;
- la room cumple su papel de “promesa narrativa y funcional”.

En otras palabras, la `Library` ya ha hecho su trabajo: deja el misterio y entrega el primer item útil del siguiente tramo.

Lo que falta no es más contenido en la `Library`, sino cerrar la cadena de progreso con la `Bedroom`.

---

## Alcance del sprint

### Incluido

- cierre funcional y estabilidad de la `Library` MVP;
- validación del flujo `Worn Book` -> `Floppy Disk`;
- creación del  `Bedroom Personal Computer` o `Computer` interactuable;
- uso del `Floppy Disk` con el ordenador para obtener un `Basement Code`;
- activación del flujo de progreso del `Basement Door` / `Code Panel`;
- validación del loop completo de la progresión entre `Library`, `Bedroom` y el siguiente bloque.

### No incluido

- resolver completamente el `Basement` o abrir una narrativa larga de su interior;
- crear una arquitectura compleja de paneles, UI, puzzles o sistemas de archivos;
- añadir demasiados objetos interactivos o ramificaciones de contenido;
- introducir un “mundo” extra fuera del alcance del MVP;
- VFX, audio final o pulido artístico exhaustivo.

---

## Plan de implementación actual

### Bloque A — Cierre de la `Library` (validado)

Objetivo:

- asegurar que la `Library` funciona como room elegante y legible;
- mantener la lógica del `Shelf Secret Door` estable;
- no añadir ruido extra ni más contenido que complique la room.

Estado:

- listo como MVP de room de memoria;
- no requiere más ampliación a no ser que surjan bugs de gameplay.

### Bloque B — Ordenador Personal de la `Bedroom`

Objetivo:

- crear el interactuable principal de la `Bedroom` para usar el `Floppy Disk`;
- dejarlo como una pieza minimalista, clara y rápida de jugar;
- que el jugador entienda de un vistazo que el PC es el siguiente punto de progreso.

Reglas:

- el `Computer` debe ser un interactuable muy claro en la `Bedroom`;
- no hace falta un sistema de escritorio real; basta con un `Use` directo;
- si el jugador tiene el `Floppy Disk`, se activa la secuencia de lectura;
- si no lo tiene, el ordenador responde con un feedback narrativo corto y claro.

### Bloque C — `Floppy Disk` y su revelación

Objetivo:

- sacar el contenido oculto del `Worn Book` de forma clara y funcional;
- que el inventario no sea confuso ni “mágico” para el jugador;
- que el item tenga una razón narrativa y una razón mecánica.

Reglas:

- el `Floppy Disk` no se entrega a lo loco ni por azar;
- se descubre al examinar el `Worn Book` o al interactuar con él desde el inventario;
- el item debe quedar claramente disponible en el inventario;
- solo se puede usar en el `Computer` de la `Bedroom`.

### Bloque D — Código del `Basement` y resolución del siguiente paso

Objetivo:

- convertir el `Floppy Disk` en una recompensa clara del flujo;
- producir el `Basement Code` de forma simple y legible;
- preparar el acceso a la siguiente fase del juego sin abrir el `Basement` completo.

Reglas:

- el `Computer` genera o desbloquea un `Basement Code` cuando se usa con el `Floppy Disk`;
- la flag `BasementCodeAcquired` debe quedar activada;
- el `Code Panel` o `Basement Door` debe pasar a estado usable sólo cuando la secuencia se ha completado;
- la resolución debe ser clara, aunque no enorme.

### Bloque E — Validación de progresión

Objetivo:

- comprobar que el jugador puede avanzar de forma ordenada por toda la cadena del juego;
- asegurarse de que no hay huecos narrativos ni estados ambiguos;
- cerrar el MVP con un recorrido corto y sólido.

Secuencia esperada:

1. entrar en la `Library`;
2. encontrar el `Worn Book`;
3. descubrir el `Floppy Disk`;
4. volver a la `Bedroom`;
5. usar el `Computer` con el `Floppy Disk`;
6. obtener el `Basement Code`;
7. volver a la `Library` y comprobar que la puerta secreta y el `Code Panel` responden al estado correcto;
8. confirmar que la progresión sigue teniendo sentido y no rompe el MVP.

---

## Requisitos funcionales mínimos

### 1. Room setup

- `Library` integrada y estable en el `RoomManager`.
- `Bedroom` con un elemento claro de interacción: `Computer`.
- `Hallway` y `RoomManager` siguen funcionando sin romper la base del juego.

### 2. Interacción principal

- el `Worn Book` mantiene su papel central de room;
- el `Floppy Disk` es el puente funcional hacia la siguiente fase;
- el `Computer` es el punto de resolución claro y no ambiguo.

### 3. Lógica de premio y progreso

- el `Floppy Disk` debe tener valor funcional y narrativo;
- el `Computer` debe convertirlo en una recompensa directa;
- la `Basement Code` debe ser obtenible de forma lineal y entendible.

### 4. Progresión mínima

- el jugador entiende por qué ha encontrado el item y qué hace con él;
- la `Bedroom` no se convierte en un mini-puzzle pesado;
- el `Basement` queda sugerido, no resuelto, como corresponde al MVP.

---

## Plan de trabajo por bloques

### Bloque A - Cierre de `Library`

- validar que la `Shelf Secret Door` se oculta y muestra bien en ambas realidades;
- garantizar que la room no tiene más bugs de flujo;
- confirmar que el `Worn Book` y el `Floppy Disk` forman un solo arco narrativo coherente.

### Bloque B - `Bedroom` Personal Computer (30-45 min)

- crear el interactuable del ordenador en la `Bedroom`;
- definir su feedback cuando no hay `Floppy Disk`;
- dejarlo preparado para la lectura del disco.

### Bloque C - `Floppy Disk` flow (30-45 min)

- asegurar que el disco se descubre desde el `Worn Book`;
- mostrarlo en el inventario con una descripción clara;
- dejar una secuencia de uso simple y legible.

### Bloque D - `Basement Code` acquisition (30-60 min)

- definir el código o la recompensa del ordenador;
- activar `BasementCodeAcquired` en `GameManager`;
- dejar el `Code Panel` y `Basement Door` listos para ser usados cuando la secuencia lo requiera.

### Bloque E - Integración con `Library` (20-30 min)

- asegurarse de que el estado del secreto y del door queda consistente con la progresión del código;
- validar que la room no “se rompe” al volver desde la `Bedroom`.

### Bloque F - Validación end-to-end (30 min)

- entrar desde `Hallway`;
- recoger el `Worn Book`;
- descubrir el `Floppy Disk`;
- volver a `Bedroom`;
- usar el `Computer`;
- obtener el código;
- volver a `Library`;
- comprobar les estados del `Basement Door` y el `Code Panel`.

---

## Regla de diseño para este sprint

### Mantener

- la arquitectura data-driven actual;
- la lógica de `SHIFT` como eje principal del juego;
- la solución simple y legible;
- la progresión como un flujo claro y lineal.

### Simplificar

- el ordenador de la `Bedroom` debe ser mínimo y funcional;
- no hace falta UI compleja ni “desktop simulado”;
- no hace falta un sistema de archivos o un minijuego de ordenador;
- el `Floppy Disk` debe ser útil y directo, no un objeto sin valor.

### Evitar

- “sistemas de ordenador” demasiado elaborados;
- múltiples capas de interacción innecesarias;
- ramificaciones de contenido que no aporten narrativa ni progresión;
- ampliar el `Basement` completo antes de cerrar el puente funcional.

---

## Criterios de aceptación

El sprint se considera válido si:

1. La `Library` está cerrada como room MVP y no introduce bugs funcionales adicionales.
2. El `Worn Book` conduce claramente a un `Floppy Disk` revelador.
3. El `Bedroom` tiene un `Computer` o equivalente interactuable y usable.
4. El `Floppy Disk` se usa con el ordenador para obtener un objetivo funcional claro.
5. La secuencia produce un `Basement Code` o equivalente que complete el puente hacia el siguiente bloque.
6. La lógica del `Basement Door` y `Code Panel` queda preparada para la siguiente tirada sin romper el diseño actual.
7. La progresión se entiende sin necesidad de cargar demasiados sistemas distintos.
8. El flujo del juego sigue siendo corto, legible y coherente.

---

## Checklist final

### Cross-system / polish

- [ ] `UI Inventory`: revisar claridad del inventario y feedback visual.
- [ ] `UI Inventory Navigation`: evaluar si se necesita más espacio para más de tres items en el inventario.
- [ ] `Inventory / Item Consumption`: dejar claro cuándo un item se consume o se usa.
- [ ] `Inventory / Item Feedback`: mostrar feedback visible en la UI cuando un item cambia de estado.
- [ ] `SFX`: añadir efectos base para interacciones clave y cambio de realidad.
- [ ] `Music`: reforzar atmósfera con la música adecuada del juego.

### Bloqueantes

- [ ] `Library` cerrada y estable
- [ ] `Worn Book` entrega `Floppy Disk` claramente
- [ ] `Bedroom Computer` interactuable y visible
- [ ] `Floppy Disk` se usa con el ordenador
- [ ] se obtiene el código o premio del `Basement`
- [ ] la secuencia no rompe el flujo global

### Funcional

- [ ] `SHIFT` sigue funcionando dentro de la room
- [ ] la `Library` y la `Bedroom` mantienen la coherencia narrativa
- [ ] el jugador entiende qué ha descubierto y por qué importa
- [ ] la siguiente fase queda preparada de forma clara

### Scope guard

- [ ] no se resuelve el `Basement` completo
- [ ] no se sobreingenieriza el ordenador
- [ ] no se introduce contenido que rompa el MVP
- [ ] la solución sigue siendo simple, clara y mantenible

---

## Recomendación técnica final

La mejor opción para este sprint es la más simple que cierra la progresión sin crear una arquitectura artificial.

La `Library` ya ha cumplido su papel. Ahora hay que cerrar el puente entre el secreto de la estantería y el siguiente bloque del juego con un ordenador de la `Bedroom` mínimo, legible y funcional. El `Floppy Disk` debe ser la clave de esa transición, y el `Basement Code` la recompensa clara que justifique el siguiente paso.

No hace falta más diseño en la `Library`. Lo esencial es cerrar la cadena de progresión de forma elegante y muy directa.
