# Room Library — Technical Spec

- Versión 0.1
- Creado: 14/11/2026
- Actualizado: 14/11/2026

---

## 1. Objetivo de la Room

La Library es la primera Room del juego diseñada como espacio de memoria archivada, ambigüedad y revelación progresiva.

Su función técnica es:

- introducir una Room centrada en la memoria y el archivo de recuerdos;
- reforzar la idea de que la misma información cambia de significado entre realidades;
- entregar el item oculto clave (`Floppy Disk` / `Disquete`) que encadena con la Bedroom;
- sugerir el acceso al `Basement` sin resolverlo todavía como un puzle completo;
- sentar una base reutilizable para rooms con contenido interpretativo más profundo.

Es una Room de progresión narrativa y observación, no de contenido infinito. Tiene que sentirse “importante” sin convertirse en un sistema gigante de libros o archivos.

---

## 2. Definición de RoomData

```text
RoomData
- id: "library"
- displayName: "Library"
- connectedRoomIds: ["hallway"]
- interactables:
  - memory_shelves
  - worn_book
  - special_shelf
  - basement_door
  - library_exit
  - hidden_note (primer Interactable/Item relacionado con `Documentos`)
```

### Estado inicial

- Reality: Normal
- La Room se ve ordenada, silenciosa, casi tranquila.
- Las estanterías parecen contener recuerdos guardados con cuidado.
- El `Worn Book` es visible y puede ser recogido o examinado.
- La puerta del `Basement` está oculta tras una `Estantería Especial`.
- El secreto del acceso se activa cuando la Room se interpreta correctamente en el Otro Lado.

---

## 3. Interactables principales

### 3.1 MemoryShelves

```text
id: "memory_shelves"
displayName: "Memory Shelves"
interactionType: Examine
visibleInNormalReality: true
visibleInOtherReality: true
resultFlag: LibraryMemoriesObserved
```

#### Propósito

- actuar como el elemento visual y narrativo central de la Room;
- reforzar la idea de recuerdos archivados y clasificados;
- dar pistas concretas sobre la relación con el padre y sobre la manipulación de la memoria.

#### Reglas técnicas

- Deben existir como un set de estanterías o una zona de visualización principal.
- Los títulos, descripciones y mensajes deben cambiar ligeramente entre realidades.
- No hace falta crear docenas de libros individuales; basta con una composición concentrada y un par de textos destacados.
- Deben aportar valor narrativo y no servir solo como “decoración”.

---

### 3.2 WornBook

```text
id: "worn_book"
displayName: "Worn Book"
interactionType: Pickup
itemId: "worn_book"
visibleInNormalReality: true
visibleInOtherReality: true
resultFlag: WornBookFound
consumeAfterUse: false
```

#### Propósito

- ser el objeto principal de la Room;
- introducir la idea de que los objetos pueden estar “contaminados” o alterados por la realidad;
- ocultar el `Disquete` dentro del libro y dar sentido a la recolección (obtenido tras `Examinar` el `Worn Book`).

#### Reglas técnicas

- El book puede recogerse directamente o examinarse antes de recogerlo.
- La descripción del libro debe cambiar entre `Normal` y `Other Side`.
- Al interactuar con él mediante `Examinar`, puede revelarse la presencia del item oculto.
- Debe quedar claro que el objeto tiene un valor superior al de un “simple libro”.
- El `Worn Book` puede usarse en el `Special Shelf` para activar el secreto de la Room, sin necesidad de haber obtenido antes el `Disquete`. Por lo que debe poder sacarse de nuevo del estante si es necesario. Haciendo que el `Special Shelf` vuelva a su estado inicial.

---

### 3.3 SpecialShelf

```text
id: "special_shelf"
displayName: "Special Shelf"
interactionType: Use
visibleInNormalReality: true
visibleInOtherReality: true
requiresItem: "worn_book"
resultFlag: SpecialShelfActivated
```

#### Propósito

- convertir la estantería en un punto de resolución del secreto de la Room;
- definir la relación entre el libro recogido y el acceso oculto al `Basement`;
- preparar la lógica del `SHIFT` y la manipulación del espacio.

#### Reglas técnicas

- La estantería tiene un hueco o un orden concreto.
- El `Worn Book` debe poder colocarse allí como solución de la Room.
- Cuando el item está colocado, la realidad activa debe dejar ver el secreto oculto.
- La estantería debe poder volver a su estado inicial si se retira el `Worn Book`.
- Se implementará una animación para que la estantería se desplace hacia un lado al activar el secreto. Debe poder revertirse si se retira el `Worn Book`.
- Una vez hecha la interacción, la room debe sugerir que la verdad del lugar se desplaza cuando se cambia a la otra realidad.

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

- ser el “teaser” principal del siguiente bloque del juego;
- dejar una promesa de nueva zona sin abrir completamente el Sótano;
- encadenar la Room con el siguiente nivel de progresión.

#### Reglas técnicas

- Debe permanecer oculto o no revelado hasta que la lógica de la estantería y el cambio de realidad lo deje visible.
- No hace falta crear un puzle gigante: basta con que la puerta haga visible la zona “siguiente”.
- La lógica del candado debe poder reutilizarse en el futuro.
- El item necesario puede ser un `Código` adquirido antes de volver a esta Room, pero se plantea implementarlo cuando se termine con la versión MVP.

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
- mantener la continuidad del espacio del edificio;
- aportar una salida clara para la Room sin complejidad adicional.

#### Reglas técnicas

- Debe permanecer siempre visible.
- No requiere flags especiales.
- Debe usar la misma lógica de transición que el resto de rooms de la base MVP.

---

## 4. Items del Room

### 4.1 WornBook

```text
ItemData
- id: "worn_book"
- displayName: "Worn Book"
- type: StoryKey
- inventoryMode: StandardInventory
- descriptionChangesByReality: true
```

#### Propósito

- ser el item principal que da sentido a la Room;
- introducir la mecánica de “item con significado cambiante entre realidades”;
- ocultar la pista del siguiente paso.

#### Reglas técnicas

- Debe poder entrar en el inventario del jugador.
- Su descripción cambia según la realidad actual.
- Debe ocultar un item importante dentro suyo o revelar una pista útil tras ser examinado.

---

### 4.2 FloppyDisk / Disquete

```text
ItemData
- id: "floppy_disk"
- displayName: "Floppy Disk"
- type: Utility
- inventoryMode: StandardInventory
- unlocks: "bedroom_computer"
```

#### Propósito

- ser el item que conecta la Library con la Bedroom y con la parte posterior del progreso;
- completar la lógica de “recuerdo archivado” y “pista útil”;
- ser la base para obtener el `Código` que luego se usa en el `Basement`.

#### Reglas técnicas

- El disk no debe recogerse sin más. Debe estar oculto dentro del `Worn Book` o revelarse mediante `Examine`.
- La intención es que el item se convierta en un nodo de progresión entre rooms.
- No debe ser un objeto repetitivo; debe tener peso narrativo y funcional.

---

### 4.3 BasementCode

```text
ItemData
- id: "basement_code"
- displayName: "Basement Code"
- type: KeyItem
- inventoryMode: StandardInventory
- unlocks: "basement_door"
```

#### Propósito

- dejar clara la relación entre la Bedroom y la Library;
- dar al jugador una pieza de progresión que queda preparada para el siguiente bloque.

#### Reglas técnicas

- El código se obtiene a partir de la interacción de la `Bedroom` con el disk.
- Debe ser un item de progreso, no un objeto decorativo.
- Su utilidad en la `Library` debe ser clara, aunque la puerta real del `Basement` no se resuelva todavía completamente.

---

## 5. Flags y estado relevantes

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

- El `Worn Book` puede empezar visible en ambas realidades, pero su lectura o significado debe variar según la realidad activa.
- El secreto del `Basement` debe depender de la lógica de `SHIFT` + estantería + item, no de una llave que se obtiene “por casualidad”.
- La Room debe sostenerse con flags simples, no con un árbol de estado enorme.

---

## 6. Flow de progresión

### Secuencia recomendada para MVP

1. El jugador entra en la `Library` desde el `Hallway`, mediante la llave `Library Key` obtenida en el `Wardrobe` del `Bedroom`.
2. Observa la estantería y el espacio como un archivo de recuerdos.
3. Encuentra o examina el `Worn Book`.
4. Descubre que contiene un `Disquete` oculto.
5. Recupera el item y vuelve al flujo principal.
6. Cuando se usa en la `Bedroom` / ordenador, se obtiene un `Código` útil.
7. El jugador vuelve a la `Library` y coloca el `Worn Book` en la `Special Shelf`.
8. Cambia a `Other Side`.
9. La estantería revela el acceso al `Basement`.
10. El `Basement Door` aparece como puerta activa o pendiente de resolución.

---

## 7. Scope guard del MVP

### Mantener

- 1 elemento principal fuerte (`Worn Book`)
- 1 secreto / acceso a sugerencia narrativa
- 1 item oculto útil
- cambio de texto y significado entre realidades
- presencia visual clara de la idea de “memoria archivada”

### Simplificar

- no crear docenas de libros interactivos;
- no hacer un sistema de clasificación de archivos;
- no convertir la Room en un puzle de navegación múltiple;
- no introducir funciones de documentos complejas ni lectura larga.

### Dejar fuera del sprint

- sistema de biblioteca completa con buscadores, índices o registros;
- múltiples variantes de puzzle por cada estantería;
- VFX o animaciones complejas para cada libro;
- lógica de “reconstrucción de historias” que no aporte progreso inmediato.

---

## 8. Recomendación final

La `Library` debe sentirse como la Room de la memoria archivada, no como una biblioteca recreativa.

El objetivo no es que el jugador “recoja muchos objetos”, sino que entienda que el edificio está hecho de recuerdos, que esos recuerdos cambian y que cada realidad distorsiona la historia. Eso la convierte en una pieza clave del juego y una preparación sólida para el `Basement`.

La mejor versión del MVP es aquella que deja claro:

- la memoria está archivada;
- la realidad cambia el significado;
- el secreto está oculto en la forma en que se recuerda.

---

## 9. Resumen corto

La `Library` es la Room de la memoria guardada. Aquí el jugador encuentra el `Worn Book`, descubre el `Disquete` oculto, entiende que la misma información cambia según la realidad y deja preparado el acceso al `Basement` como una promesa narrativa concreta y funcional.
