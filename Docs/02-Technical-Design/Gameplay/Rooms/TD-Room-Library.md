# Room Library — Technical Spec

- Versión 0.3
- Creado: 14/11/2026
- Actualizado: 15/11/2026

---

## 1. Premisa del Room

La `Library` es la primera `Room` del juego y debe funcionar como un “archivo de memoria” más que como una biblioteca literal. Su intención es comunicar que el edificio no es un espacio neutral: está hecho de recuerdos, de piezas de historia y de versiones conflictivas del mismo hecho.

Su función técnica es:

- introducir una `Room` centrada en la memoria archivada y la interpretación del pasado;
- reforzar la idea de que la misma información cambia de significado entre realidades;
- entregar el `Item` oculto clave (`Floppy Disk` / `Disquete`) como enlace con la `Bedroom`;
- sugerir el acceso al `Basement` sin resolverlo todavía como un puzle completo;
- preparar la base para rooms futuras con contenido más ambiguo o interpretativo.

La `Library` debe sentirse importante sin volverse un sistema complejo. El foco no es la cantidad de objetos ni la profundidad documental, sino la sensación de que el espacio guarda algo que no está del todo bien.

---

## 2. Principios de diseño

1. Una sola idea central debe dominar la Room: la memoria está archivada y puede cambiar de significado.
2. El verdadero “puzzle” de la Room debe ser simple: `Worn Book` + `Interactive Shelf` + `Other Side`.
3. El `Basement` debe ser una promesa, no un segundo bloque completo de gameplay.
4. Los props y las estanterías sostienen la narrativa; no deben competir con el objetivo central.
5. El contenido interactivo debe estar centrado en 1-2 piezas clave, no en una biblioteca de documentos.

---

## 2. Definición de RoomData

```text
RoomData
- id: "library"
- displayName: "Library"
- connectedRoomIds: ["hallway"]
- interactables:
  - worn_book
  - interactive_shelf
  - shelf_secret_door
  - right_shelf
  - basement_door
  - code_panel
  - library_exit
  - (futuro) hidden_note
```

### Estado inicial

- Reality: Normal
- La Room se ve ordenada, tranquila y ligeramente inquietante.
- Las estanterías parecen contener recuerdos guardados con cuidado.
- El `Worn Book` es visible y puede ser recogido o examinado.
- La puerta del `Basement` está oculta y no presenta señales claras de acceso.
- La resolución del secreto se activa cuando la Room se interpreta correctamente en `Other Side`.

---

## 3. Interactables principales

### 3.1 WornBook

```text
id: "worn_book"
displayName: "Worn Book"
interactionType: Pickup
itemId: "worn_book"
visibleInNormalReality: true
visibleInOtherReality: false
resultFlag: WornBookFound
consumeAfterUse: false
```

#### Propósito

- ser el elemento central de la Room;
- introducir la idea de que un objeto puede estar “contaminado” por la realidad;
- ocultar el `Floppy Disk` dentro del libro como recompensa narrativa y funcional;
- funcionar como trigger de la ocultación del secreto del `Basement`.

#### Reglas técnicas

- El `Worn Book` puede recogerse directamente o examinarse antes de recogerlo.
- Su descripción debe cambiar ligeramente entre `Normal` y `Other Side`, sin necesidad de reinventar toda la lógica de texto.
- Al interactuar con él mediante `Examine` desde el inventario, puede revelarse la presencia del item oculto.
- Debe quedar claro que es más que un “libro viejo”: tiene significado, peso emocional y valor funcional.
- El `Worn Book` debe poder devolverse a la estantería si se retira el secreto, restableciendo el estado inicial de la `Shelf Secret Door`.
- Su función no es solo ser una llave: es la pieza que hace que la Room “hable” en las dos realidades.

---

### 3.2 InteractiveShelf

```text
id: "interactive_shelf"
displayName: "Interactive Shelf"
interactionType: Use
visibleInNormalReality: true
visibleInOtherReality: true
requiresItem: "worn_book"
resultFlag: SpecialShelfActivated
```

#### Propósito

- convertir la estantería en el punto de resolución del secreto de la `Room`;
- relacionar el libro, el espacio y la revelación del acceso oculto;
- preparar la lógica del `SHIFT` y la manipulación de la realidad sin convertirlo en un puzle pesado.

#### Reglas técnicas

- En `Normal`, la estantería debe verse como una estantería normal, sin indicios de hueco oculto.
- En `Other Side`, la estantería debe mostrar una zona vacía o una ausencia visual donde se activa la interacción.
- El colisionador interactivo debe cambiar según la realidad: cubre la estantería entera en `Normal` y solo el hueco o zona activa en `Other Side`.
- El `Worn Book` debe poder colocarse ahí solo cuando la Room está en `Other Side`.
- Al colocarlo, la `Shelf Secret Door` debe desplazarse hacia la derecha y quedar detrás de la `Right Shelf`.
- Si el item se retira, el secreto debe poder revertirse con una animación simple y limpia.
- La Room debe ofrecer una señal visual de que “algo cambió” una vez la estantería se abre, incluso si no se resuelve el `Basement` por completo.

---

### 3.3 ShelfSecretDoor

```text
id: "shelf_secret_door"
displayName: "Shelf Secret Door"
interactionType: Prop
visibleInNormalReality: true
visibleInOtherReality: true
resultFlag: BasementSecretRevealed
```

#### Propósito

- ser la pieza visual que materializa el secreto de la Room;
- ocultar la existencia del `Basement` detrás de la composición de la estantería;
- dar una sensación de arquitectura “cambiada” por la realidad.

#### Reglas técnicas

- Debe permanecer invisible en `Normal` y aparente de forma neutral, como parte del mueble.
- En `Other Side` debe mover su panel o pared no mostrada, revelando un acceso o un hueco que apunta al siguiente bloque.
- Debe ser un elemento muy simple visualmente y más conceptual que interactivo.
- No debe incluir lógica de puzle adicional; es una “revelación” del espacio.

---

### 3.4 BasementDoor

```text
id: "basement_door"
displayName: "Basement Door"
interactionType: Use
visibleInNormalReality: false
visibleInOtherReality: true
requiresItem: "basement_code"
resultFlag: BasementUnlocked
```

#### Propósito

- funcionar como teaser del siguiente bloque del juego;
- prometer una zona distinta sin abrirla todavía del todo;
- cerrar la estructura de progresión de la Room con una sugerencia clara.

#### Reglas técnicas

- No debe ser una resolución completa del Sótano.
- Debe aparecer únicamente cuando la lógica de la estantería y la realidad dejan claro que algo “se abre”.
- La contraseña o item requerido puede llegar más tarde; con el MVP basta con presentar la promesa del siguiente espacio.
- Su función es narrativa y narrativa-funcional, no un puzzle de tamaño completo.

---

### 3.5 LibraryExit

```text
id: "library_exit"
displayName: "Library Exit"
interactionType: Transition
linkedRoomId: "hallway"
visibleInNormalReality: true
visibleInOtherReality: true
```

#### Propósito

- permitir volver al pasillo principal;
- mantener la continuidad del edificio;
- ofrecer una salida clara sin añadir complejidad.

#### Reglas técnicas

- Debe permanecer visible siempre.
- No debe depender de flags de Room.
- Se reutiliza la misma lógica de transición que en resto de rooms del MVP.

---

### 3.6 CodePanel

```text
id: "code_panel"
displayName: "Code Panel"
interactionType: Use
visibleInNormalReality: true
visibleInOtherReality: true
requiresItem: "basement_code"
resultFlag: BasementDoorUnlocked
```

#### Propósito

- permitir al jugador introducir un código o interactuar con un panel de manera narrativa;
- conectar la `Library` con la resolución del siguiente bloque;
- reforzar la mecánica de items que desbloquean progresión.

#### Reglas técnicas

- Sólo visible cuando la `Shelf Secret Door` se ha movido o activado.
- Solo debe permitir la interacción si el jugador posee el item requerido.
- Su función es narrativa-funcional, no un puzzle complejo.

## 4. Props y composición visual

### 4.1 RightShelf

```text
id: "right_shelf"
displayName: "Right Shelf"
interactionType: Examine
visibleInNormalReality: true
visibleInOtherReality: true
resultFlag: LibraryMemoriesObserved
```

#### Propósito

- ser el elemento visual y narrativo central de la Room;
- reforzar la idea de que el ambiente está “clasificado” por memoria;
- sugerir la relación con el padre o con el pasado sin explicar demasiado.
- actuar como capa superior visual para que la `Shelf Secret Door` pase por detrás.

#### Reglas técnicas

- Debe existir como bloque visual principal, no como una colección de libros aislados.
- Sus textos y descripciones deben cambiar ligeramente entre realidades, pero siempre con un tono parecido.
- No hace falta crear una biblioteca enorme ni demasiados objetos interactivos.
- Diseñado para servir de soporte visual a la sorpresa de la estantería, no para competir con ella.

---

### 4.2 Background y atmosfera

El fondo de la Room debe comunicar una sensación de archivo, orden y memoria, no de ciudad ni de lugar de ocio. La composición visual debe reforzar lo siguiente:

- orden cuidadosamente mantenido;
- una sensación de archivo antiguo o personal;
- contradicción entre la calma visible y la presencia “errónea” de la realidad alternativa;
- aire de pasillo de recuerdos, no de biblioteca pública.

Con esto, el `Worn Book` y la estantería se convierten en los elementos protagonistas, mientras el fondo ofrece contexto.

---

## 5. Items del Room

### 5.1 WornBook

```text
ItemData
- id: "worn_book"
- displayName: "Worn Book"
- type: StoryKey
- inventoryMode: StandardInventory
- descriptionChangesByReality: true
```

#### Propósito

- ser el item principal de la `Room`;
- introducir la mecánica de “item con significado cambiante entre realidades”;
- ocultar la pista del siguiente paso.

#### Reglas técnicas

- Debe poder entrar en el inventario del jugador.
- Su descripción cambia según la realidad actual.
- Debe ocultar un `Floppy Disk` o revelar una pista útil tras su examen.

---

### 5.2 FloppyDisk / Disquete

```text
ItemData
- id: "floppy_disk"
- displayName: "Floppy Disk"
- type: Utility
- inventoryMode: StandardInventory
- unlocks: "bedroom_computer"
```

#### Propósito

- conectar la `Library` con la `Bedroom` y con la resolución del siguiente bloque;
- completar la lógica de “memoria archivada” y “pista útil”;
- ser la base para obtener un `Código` que pueda usarse más adelante.

#### Reglas técnicas

- No debe recogerse sin más: debe estar oculto dentro del `Worn Book` o revelarse mediante `Examine`.
- Debe aportar progresión, no solo contenido decorativo.
- No debe ser un objeto repetitivo; debe tener peso narrativo y funcional.

---

### 5.3 BasementCode

```text
ItemData
- id: "basement_code"
- displayName: "Basement Code"
- type: KeyItem
- inventoryMode: StandardInventory
- unlocks: "basement_door"
```

#### Propósito

- dejar clara la relación entre la `Bedroom` y la `Library`;
- dar al jugador una pieza de progresión que prepara el próximo bloque.

#### Reglas técnicas

- El código se obtiene a partir de la interacción con el `Floppy Disk` en la `Bedroom`.
- Debe ser un objetivo claro, no un detalle adicional.
- Su utilidad en el `Code Panel` de la `Library` debe ser visible, aunque la puerta real del `Basement` no se resuelva todavía completamente.

---

## 6. Flags y estado relevantes

```text
LibraryMemoriesObserved
WornBookFound
SpecialShelfActivated
BasementSecretRevealed
BasementUnlocked
FloppyDiskRecovered
BasementCodeAcquired
```

### Regla de estado

- El `Worn Book` sólo puede aparecer en el `Other Side`, pero al examinarlo en el inventario debe cambiar según la realidad activa.
- La `Room` debe sostenerse con flags simples y claros.
- El secreto del `Basement` debe depender del `SHIFT` + `Interactive Shelf` + `Worn Book`, no de una llave que aparezca por casualidad.
- La `Room` no debería necesitar un árbol de estado enorme para funcionar bien.

---

## 7. Flow de progresión recomendado para MVP

1. El jugador entra en la `Library` desde el `Hallway`.
2. Observa la estantería y la sensación de archivo de recuerdos.
3. Encuentra o examina el `Worn Book`.
4. Descubre que contiene un `Disquete` oculto.
5. Recupera el item y vuelve al flujo principal.
6. Cuando el `Floppy Disk` se usa en la `Bedroom`, el jugador obtiene el `Basement Code` o una pista funcional.
7. El jugador vuelve a la `Library` y coloca el `Worn Book` en la `Interactive Shelf` en `Other Side`.
8. La estantería se abre y revela el secreto detrás.
9. La `Basement Door` aparece como promesa narrativa y funcional.
10. La Room termina con una sensación de “esto no es casualidad; el edificio está cambiando”.

---

## 8. Scope guard del MVP

### Mantener

- 1 elemento principal fuerte: `Worn Book`
- 1 secreto visible: `Shelf Secret Door`
- 1 item útil: `Floppy Disk`
- 1 promesa: `Basement Door`
- cambio de texto y significado entre realidades
- foco visual claro en la memoria archivada

### Simplificar

- no crear docenas de libros interactivos;
- no convertir la Room en un sistema de clasificación documental;
- no introducir más de un secreto principal;
- no hacer un mockup de “biblioteca completa”;
- no prolongar la progresión con lecturas largas o documentaciones complejas.

### Dejar fuera del sprint

- sistema completo de archivos, índices o categorías;
- varios puzles distintos por estantería;
- más de una zona secreta de acceso;
- VFX o animaciones complejas para cada elemento interactivo;
- lógica de “reconstrucción de historias” que no aporte progresión inmediata.

---

## 9. Recomendación final

La `Library` debe sentirse como la Room de la memoria guardada, no como una biblioteca recreativa. La idea no es que el jugador recoja muchos objetos, sino que comprenda que el edificio está construido con recuerdos que cambian de significado cuando la realidad cambia. Ese mensaje es la esencia de la Room.

La mejor versión del MVP es aquella que deja claro:

- la memoria está archivada;
- la realidad reinterpreta lo que se guarda;
- el secreto está oculto en la forma en que se recuerda.

Si se mantiene ese foco, la `Library` será una Room fuerte, clara y reutilizable para el resto del juego.

---

## 10. Mapeo visual de sprites del MVP

Los sprites añadidos ya encajan con esta idea y han de usarse así:

- `sprite-room-library-background-normal-v1.png`: fondo general de la Room en `Normal`. Contiene ya embebidos la `Exit Door`, `Basement Door` y `Code Panel`
- `sprite-library-shelf-right-v1.png`: estantería principal de la composición visual.
- `sprite-library-shelf-secret-door-v1.png`: puerta oculta / secreto de la estantería.
- `sprite-library-shelf-interactive-normal-v1.png`: estado normal de la estantería interactiva.
- `sprite-library-shelf-interactive-other-world-v1.png`: versión de la estantería en `Other Side`, cuando el hueco o la ausencia se vuelve legible.
- `sprite-library-worn-book-v1.png`: objeto principal, con una silueta apropiada para un libro viejo y significativo.

Esto ya ofrece una base visual muy sólida para un MVP: la composición de la Room queda clara, el `Worn Book` tiene identidad y la estantería funciona como eje narrativo y mecánico.

---

## 11. Resumen corto

La `Library` es la Room de la memoria archivada. Aquí el jugador encuentra el `Worn Book`, descubre el `Floppy Disk` oculto, entiende que la misma información cambia según la realidad y deja preparado el acceso al `Basement` como una promesa clara y funcional. Todo el diseño debe apoyar una sensación de archivo vivo, alterado y parcialmente oculto.
