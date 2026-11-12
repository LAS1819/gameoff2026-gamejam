# MVP Sprint 3 - Library teaser and basement setup

- Fecha inicio: 12/11/2026
- Fecha objetivo: 19/11/2026
- Estado: 🟡 PLANIFICADO - Bedroom cerrada, Library como siguiente paso mínimo y jugable
- Creado: 12/11/2026
- Actualizado: 12/11/2026

## Objetivo

Avanzar una vez más en la progresión del MVP sin perder el ritmo ni complicar el sistema. La prioridad del sprint es materializar la `Library` como la siguiente `Room` narrativa y funcional, pero con un alcance pequeño, legible y reutilizable.

La idea no es convertir la `Library` en un sistema completo de libros, archivos y puzles encadenados. La idea es dejar una `Room` clara, con presencia narrativa, un objeto clave, un cambio de significado entre realidades y una puerta o acceso secreto que deje preparada la siguiente iteración del juego.

Por eso, este sprint debe priorizar:

- avanzar la historia y la progresión;
- mantener la arquitectura simple y data-driven;
- reforzar el tono de memoria, recuerdo y verdad manipulada;
- preparar el camino hacia el `Basement` sin convertir la room en un puzle enorme.

---

## Alcance

### Incluido

- `Library` room data y `Interactables` básicos
- libro principal y objeto oculto dentro de él
- estantería o elemento especial que actúa como clave visual
- cambio de significado del mismo contenido entre `Realidad Normal` y `Other Side`
- una puerta o acceso al `Sótano` como teaser de progresión
- salida de vuelta al `Hallway`
- validación del flujo con la `Bedroom` y la lógica de `SHIFT`

### No incluido

- biblioteca completa con docenas de libros interactivos
- sistema completo de clasificación de documentos o estanterías
- puzzle de Sótano completo
- múltiples cascadas de mecánicas secundarias
- audio o VFX finales
- más sistemas de progresión que los estrictamente necesarios

---

## Prioridad de trabajo

### Prioridad 1 — Library mínima funcional

El objetivo principal es que la `Library` se sienta como una `Room` distinta de la `Bedroom` y que tenga un punto de progresión claro:

1. `Library` carga como room desde `Hallway` (se necesita `Library Key`)
2. existe un libro principal claramente identificable
3. el libro contiene un objeto oculto (`Disquete`)
4. ese item es relevante para la progresión futura
5. la estantería o el conjunto de libros revela un secreto una vez se activa la lógica de la `Other Side`
6. hay un acceso o puerta que sugiere el siguiente bloque del juego

### Prioridad 2 — Preparación para el Sótano

La `Library` no tiene que resolverse entera, pero sí debe dejarla preparada para que el siguiente bloque sea más fácil de construir:

- la progresión del `Disquete` debe quedar clara;
- la puerta o acceso debe sentirse “por descubrir” y no arbitrario;
- la tensión narrativa debe sentirse más fuerte en `Other Side`;
- debe quedar una base para el `Código`/`Candado` del `Basement` sin gastar demasiado esfuerzo ahora.

### Prioridad 3 — Refuerzo del tono

La `Library` debe ser la `Room` de:

- memoria archivada;
- recuerdo manipulable;
- negación y distancia narrativa;
- diferencia entre lo que ocurrió y lo que se recuerda.

No se busca que sea enorme. Se busca que se sienta importante como `Room`.

---

## Requisitos funcionales mínimos

### 1. Estructura de room

- `Library` debe estar integrada al `RoomManager` como una room más del sistema.
- Debe poder cargarse desde el `Hallway` con un `linkedRoomId` claro (y con la llave correspondiente).
- Debe mantener el patrón actual de `RoomData` + `InteractableData` + `flags`.

### 2. Interacción principal

- un `Worn Book` o `Memory Book` debe ser el objeto central;
- su comportamiento debe cambiar entre realidades;
- al examinarlo, debe revelar un objeto oculto o pista clave;
- ese item debe ser útil, no decorativo.

### 3. Lógica de realidad

- la misma estantería o mismo libro debe comportarse distinto entre `Normal` y `Other Side`;
- el cambio de significado debe quedar visible sin necesidad de crear un sistema complejo;
- la room debe reforzar la idea de que la memoria cambia según la realidad.

### 4. Progresión de la room

- el jugador debe poder avanzar a la siguiente etapa sin que la room se rompa;
- la room debe sugerir que lo que se ha encontrado aquí es importante para el siguiente bloque;
- la salida o secreto de la room no debe depender de un montante de lógica difícil de mantener.

---

## Plan de trabajo por bloques

### Bloque A — Library room data (30-45 min)

- crear la `Library` room con sus `Interactables` básicos;
- definir `book`, `shelf`, `basement_secret`, `library_exit`;
- asegurar que la `Room` se carga y se visualiza con el sistema actual.

### Bloque B — Libro principal y objeto oculto (45-60 min)

- definir la mecánica del `Worn Book`;
- decidir el item oculto que se obtiene (`Disquete`) y su utilidad narrativa (se usará en `Ordenador` de la `Bedroom`);
- asegurar que el objeto tenga sentido en la progresión y no sea un “extra” sin consequence (ofrece `Código` para el `Basement` al usarlo en el `Ordenador` de la `Bedroom`).

### Bloque C — Realidad y estantería (45-60 min)

- vincular la diferencia visual o textual entre `Normal` y `Other Side`;
- dejar claro que la estantería y los títulos cambian cuando se cambia de realidad;
- dejar un primer “punto de secreto” que se revele en `Other Side`.

### Bloque D — Teaser del Sótano / salida (30-45 min)

- preparar un paso o puerta visible tras resolver la lógica mínima;
- no hacer un puzle final completo; solo dejar la puerta como una promesa de siguiente contenido;
- mantener esta parte simple y narrativa, no compleja.

### Bloque E — Validación end-to-end (30 min)

- entrar desde `Hallway` con llave `Library Key`;
- comprobar libro + item oculto;
- cambiar de realidad y ver cómo la room reacciona;
- comprobar salida y flujo general;
- asegurarse de que no rompe la lógica previa de `Bedroom` ni de `SHIFT`.

---

## Regla de diseño para este sprint

### Mantener

- la estructura simple de `Rooms` basada en `RoomData`;
- la lógica de `SHIFT` como eje central de la room;
- un objeto principal con significado narrativo claro;
- una sensación de memoria manipulada y de verdad distorsionada;
- la room como un paso de historia, no como una mini-sistema completo.

### Simplificar

- no crear decenas de libros interactivos;
- no hacer puzzles de clasificación o búsquedas de texto;
- no introducir sistemas secundarios que no sean necesarios;
- no abrir el Sótano completo ni convertir la room en una nueva metroidvania.

### Dejar fuera del sprint

- sistema completo de basements / puzzle de archivos;
- múltiples ramas de progresión;
- raids de lore complejos;
- puzzles de varios pasos con estados muy variados;
- contenido de arte final o VFX exhaustivos.

---

## Criterios de aceptación del sprint

La `Library` se considera lista para esta iteración si:

1. El jugador entra desde `Hallway` con llave `Library Key`.
2. Ve una room distinta de `Bedroom` y con identidad propia.
3. Encuentra un elemento clave central (libro o estantería) con un significado claro.
4. Ese elemento cambia de significado o de apariencia entre realidades.
5. Descubre un objeto oculto (`Disquete`).
6. La room sugiere la presencia de un secreto o una puerta del `Sótano` sin necesitar un puzle enorme.
7. La room no rompe la base del sistema ni complica la topos de gameplay actual.
8. La room funciona como preparación narrativa para el siguiente bloque del proyecto.

---

## Checklist de cierre del sprint

### Bloqueantes

- [ ] `Library` room data creada
- [ ] libro principal identificable
- [ ] item oculto recuperable
- [ ] cambio entre realidades visible
- [ ] salida o secreto del Sótano sugerido
- [ ] flujo sin errores de `room loading`

### Funcional

- [ ] `SHIFT` sigue funcionando dentro de la room
- [ ] la room no rompe la base de la `Bedroom`
- [ ] `Hallway` y `Library` tienen transición coherente
- [ ] el jugador entiende qué ha encontrado y por qué importa

### Scope guard

- [ ] no se añade lógica de puzzle complejo
- [ ] no se resuelve el `Basement` completo
- [ ] no se abre la siguiente iteración con sistemas de más
- [ ] la room sigue dentro del enfoque MVP y del game jam

---

## Recomendación técnica final

Este sprint debe ser una prueba de disciplina, no de ambición.

La mejor versión de la `Library` ahora no es la que abarque todo el mundo de la memoria. Es la que comunica muy bien que:

- el edificio está hecho de recuerdos;
- la memoria se reescribe con la realidad;
- la verdad no es una sola versión;
- el siguiente bloque de progresión aparece con forma de secreto.

Si esta room funciona con claridad, el siguiente paso hacia el `Basement` será mucho más fácil y mucho menos frágil.

En resumen: la `Library` debe ser una room narrativa fuerte, pequeña, clara y útil, no un sistema gigante de contenido.
