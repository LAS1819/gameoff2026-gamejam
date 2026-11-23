# Memories Design

- **Versión del documento**: 0.4
- **Creado**: 2026-11-19
- **Última actualización**: 2026-11-22

## Objetivo

Las Memorias representan momentos de verdad que el protagonista es capaz de recordar durante su viaje. No son objetos tradicionales ni forman parte del inventario. Cada una de ellas reconstruye una etapa diferente de la relación entre el protagonista y su padre.

Su función es:

- Ampliar la comprensión de la historia
- Reforzar los temas de duelo, memoria y aceptación
- Influir en el comportamiento del Bosque
- Determinar los finales disponibles

Las Memorias no explican por sí mismas la muerte del padre. Actúan como reconstrucciones progresivas de una relación que el protagonista ha tratado de mantener intacta en su memoria. Su acumulación permite que el jugador comprenda que la búsqueda de respuestas nunca fue el verdadero objetivo del viaje.

## Reglas generales

- Son completamente opcionales.
- Existe una Memoria asociada a cada Room principal.
- `Memory_01 - Primeros Pasos` puede descubrirse en la Realidad Normal del Lobby.
- Las demás Memorias sólo pueden descubrirse en El Otro Lado.
- Se descubren mediante la acción `Examinar` sobre un interactuable concreto.
- La primera vez que se descubren activan una Flag persistente durante la partida.
- Después de descubrirse, repetir el examen no debe reactivar ni mostrar el mismo recuerdo otra vez.
- No aparecen en la interfaz durante la partida (solo se muestran en la pantalla final).
- El jugador desconoce cuántas existen.
- Cada Memoria representa una etapa distinta de la vida compartida entre padre e hijo.
- Su presentación debe ser breve y legible. Deben sentirse como un recuerdo interior, no como una entrada larga de documentación o una explicación directa.

La ubicación de las Memorias sigue el significado narrativo de las Rooms:

- **Lobby:** origen seguro de la relación y principio de la negación.
- **Hallway:** aparición de la primera grieta en la memoria.
- **Bedroom:** recuerdo personal ligado al aislamiento emocional.
- **Library:** reinterpretación de la historia que el protagonista había archivado.
- **Basement:** prueba emocional de que la verdad ya no puede permanecer enterrada.

## Lista de Memorias

### Memory_01 - Primeros Pasos

- **Room:** Lobby
- **Interactuable:** Cuadro familiar
- **Realidad:** Realidad Normal
- **Etapa:** Infancia
- **Contenido:** El protagonista recuerda una tarde de su infancia junto a su padre. No ocurre nada extraordinario: caminan juntos mientras él le señala cosas del mundo que parecen enormes y misteriosas. La memoria conserva una sensación simple de seguridad y de admiración hacia alguien que entonces sabía cómo hacer que todo tuviera sentido.
- **Tema:**
  - Protección
  - Admiración
  - Vínculo paternal

- **Función narrativa:** Establece qué significaba el padre antes de que la relación se volviera difícil o silenciosa. No presenta pistas sobre su muerte.

---

### Memory_02 - El Fósil

- **Room:** Hallway
- **Interactuable:** Mancha de humedad con forma de amonita
- **Etapa:** Adolescencia
- **Contenido:** El protagonista recuerda las tardes en las que salía con su padre a buscar fósiles. Su padre solía decir que incluso las cosas desaparecidas dejan marcas que pueden perdurar durante millones de años. En aquel momento, la frase parecía una explicación sencilla sobre el pasado. Ahora comprende que hablaba de algo menos concreto: crecer no borra aquello que una persona ha significado.
- **Tema:**
  - Cambio
  - Crecimiento
  - Permanencia de los recuerdos

- **Función narrativa:** Introduce la idea de que el pasado no desaparece, aunque cambie su forma. La grieta no es todavía la verdad de la muerte, sino la primera sospecha de que recordar también implica aceptar una ausencia.

---

### Memory_03 - La Ventana Cerrada

- **Room:** Bedroom
- **Interactuable:** Ventana tapiada
- **Etapa:** Pubertad
- **Contenido:** El protagonista recuerda una época en la que empezó a encerrarse en sí mismo. Mientras trataba de comprender quién era, su padre seguía esperando al otro lado de la puerta con paciencia. Durante mucho tiempo creyó que no le entendía; ahora recuerda que nunca dejó de estar presente, aunque ambos tuvieran dificultades para expresar lo que sentían.
- **Tema:**
  - Aislamiento
  - Comprensión
  - Distancia emocional

- **Función narrativa:** Personaliza el origen de la negación. La separación no comenzó con la muerte: ya existía en momentos donde padre e hijo dejaron de encontrar las palabras adecuadas.

---

### Memory_04 - Las 11:30

- **Room:** Library
- **Interactuable:** Reloj de pared roto
- **Etapa:** Adultez
- **Contenido:** Recuerda la última conversación normal que mantuvo con su padre. El reloj marcaba las 11:30. Hablaban de asuntos corrientes, sin despedirse ni dar importancia al momento. Ninguno imaginó que aquel intercambio cotidiano sería el último.
- **Tema:**
  - Rutina
  - Nostalgia
  - Fragilidad del tiempo

- **Función narrativa:** Convierte una escena ordinaria en una ausencia retroactiva. Es la ruptura entre la vida que parecía continuar y el momento que el protagonista nunca supo que debía preservar.

---

### Memory_05 - El Huerto

- **Room:** Basement
- **Interactuable:** Caja de herramientas
- **Etapa:** Últimos años
- **Contenido:** La caja contiene herramientas gastadas por el uso. El protagonista recuerda a su padre participando como voluntario y presidente de unos huertos ecológicos. Nunca hablaba de aquello como un trabajo: hablaba de las personas a las que ayudaba, de los proyectos compartidos y del orgullo sencillo de ver crecer lo que habían cuidado. Hacía tiempo que no lo veía tan feliz. Aquella fue una de las últimas etapas de su vida y una de las pocas en las que parecía estar plenamente en paz.
- **Tema:**
  - Legado
  - Felicidad sencilla
  - Últimos años compartidos
  - Paz interior

- **Función narrativa:** Presenta al padre como una persona cuyo legado no estaba en una gran revelación, sino en sus acciones y en su capacidad de crear comunidad. Es el recuerdo más cercano a la verdad que el protagonista intenta aceptar.

---

## Implementación

### Flags

- Memory_01
- Memory_02
- Memory_03
- Memory_04
- Memory_05

### Descubrimiento

Al examinar por primera vez el interactuable asociado:

1. El juego presenta la Memoria como una breve secuencia narrativa.
2. Se activa la Flag correspondiente.
3. El interactuable conserva su estado de descubierto.
4. Las siguientes examinaciones no vuelven a mostrar la misma secuencia.

La presentación debe detener brevemente la interacción normal, pero no mostrar progresión numérica ni nombres internos al jugador.

### Feedback visual de descubrimiento

Cada descubrimiento debe incluir un feedback claramente distinto de un `Examine` común.

El MVP recomendado utiliza una imagen superpuesta sobre la Room activa durante unos segundos:

- la imagen conserva la composición de la Room;
- puede usar temporalmente el concept art existente de la Room;
- puede aplicarse un tratamiento monocromático, con papel, tinta o lápiz;
- el título de la Memoria aparece con una presentación breve;
- el texto completo permanece corto y legible;
- la imagen desaparece por fade y el jugador recupera el control sin esperar a un botón.

Duración orientativa:

- aparición: 0.2-0.4 segundos;
- presentación: hasta 10 segundos;
- desaparición: 0.4-0.6 segundos.

La secuencia debe reforzar que algo relevante ha sido descubierto, sin bloquear el juego ni mostrar el contador `X/5`. La señal positiva puede acompañarse de un pequeño sonido o música, pero no debe depender de él.
El jugador puede cerrar el mensaje anticipadamente mediante el botón `Continue`.

#### Representación visual para la entrega

Para la primera entrega se utilizarán imágenes rápidas creadas en Aseprite. No se pretende representar escenas detalladas ni reproducir una vista completa de cada Room.

Cada imagen debe mostrar una silueta sencilla del protagonista junto a su padre, acompañada de un elemento mínimo que identifique la Memoria:

- **Memory_01 - Primeros Pasos:** caminando juntos, con una silueta infantil delante del padre.
- **Memory_02 - El Fósil:** ambos agachados o observando una amonita.
- **Memory_03 - La Ventana Cerrada:** el padre al otro lado de una puerta o una ventana cerrada.
- **Memory_04 - Las 11:30:** dos figuras silenciosas junto a un reloj fijo en 11:30.
- **Memory_05 - El Huerto:** padre e hijo junto a herramientas o plantas sencillas.

La representación debe mantenerse simbólica y reconocible:

- el padre y el protagonista son siluetas oscuras;
- el momento se distingue por un único objeto o gesto;
- el fondo es limitado, suave y legible;
- no se muestran expresiones faciales detalladas;
- no se revela explícitamente la muerte ni el accidente.

Las imágenes se muestran dentro de `MemoryDiscoveryOverlay`, no sobre el escenario. Esta decisión evita modificar los fondos de las Rooms y mantiene visible la relación entre el recuerdo y la realidad donde fue descubierto.

#### Evolución a ilustraciones propias

Después de la entrega inicial pueden refinarse las imágenes sustituyendo los primeros bocetos por composiciones exclusivas:

- deben representar un momento concreto, no una vista genérica de la Room;
- pueden usar dibujo a lápiz o bolígrafo con poco detalle;
- deben conservar una identidad visual coherente con el proyecto;
- pueden mantener la composición del Room como referencia espacial.

Una ilustración por Memoria es preferible a efectos animados complejos. El objetivo es dar peso emocional al recuerdo sin crear un sistema visual costoso.

**Opinión de diseño:** esta idea encaja bien con el tono melancólico y el alcance de una game jam. Es importante que el overlay parezca una huella mental dentro del mundo, no una pantalla independiente ni una colección. El `Story Overlay` utilizado para la carta y los finales no debe reutilizarse para este efecto: las Memorias necesitan ser más breves, menos modales y más inmediatas.

---

### Relación con los finales

- 0-2 Memorias: Final Malo.
- 3-4 Memorias: Final Neutral.
- 5 Memorias: Final Verdadero.

### Pantalla Final

La pantalla de desenlace muestra:

- Recovered Memories: X/5

Con ello se comunica al jugador que existen secretos narrativos sin convertir la partida en una colección obligatoria.
