# Memories Design

- **Versión del documento**: 0.2
- **Creado**: 2026-11-19
- **Última actualización**: 2026-11-22

## Objetivo

Las Memorias representan momentos de verdad que el protagonista es capaz de recordar durante su viaje. No son objetos tradicionales ni forman parte del inventario. Cada una de ellas reconstruye una etapa diferente de la relación entre el protagonista y su padre.

Su función es:

- Ampliar la comprensión de la historia
- Reforzar los temas de duelo, memoria y aceptación
- Influir en el comportamiento del Bosque
- Determinar los finales disponibles

## Reglas generales

- Son completamente opcionales.
- Existe una Memoria asociada a cada Room principal.
- Sólo pueden descubrirse en El Otro Lado.
- Se obtienen mediante la acción Examinar sobre un interactuable concreto.
- Activan Flags persistentes.
- No aparecen en la interfaz durante la partida (solo se muestran en la pantalla final).
- El jugador desconoce cuántas existen.
- Cada Memoria representa una etapa distinta de la vida compartida entre padre e hijo.

## Lista de Memorias

### Memory_01 - Primeros Pasos

- **Room:** Lobby
- **Interactuable:** Cuadro familiar
- **Etapa:** Infancia
- **Contenido:** El protagonista recuerda una tarde de su infancia junto a su padre. No ocurre nada extraordinario. Recuerda la sensación de seguridad de caminar junto a él mientras le enseñaba cosas del mundo que parecían enormes y misteriosas.
- **Tema:**
  - Protección
  - Admiración
  - Vínculo paternal

---

### Memory_02 - El Fósil

- **Room:** Hallway
- **Interactuable:** Mancha de humedad con forma de amonite
- **Etapa:** Adolescencia
- **Contenido:** El protagonista recuerda aquellos días en los que salía con su padre a buscar fósiles y el paso del tiempo desde entonces. Su padre solía decir que incluso las cosas desaparecidas dejan marcas que permanecen durante años. Ahora comprende que quizá estaba intentando enseñarle algo más importante.
- **Tema:**
  - Cambio
  - Crecimiento
  - Permanencia de los recuerdos

---

### Memory_03 - La Ventana Cerrada

- **Room:** Bedroom
- **Interactuable:** Ventana tapiada
- **Etapa:** Pubertad
- **Contenido:** Recuerda una época en la que empezó a encerrarse en sí mismo. Mientras intentaba comprender quién era, su padre seguía esperando al otro lado de la puerta con paciencia. Durante años creyó que no le entendía. Ahora recuerda que siempre estuvo allí.
- **Tema:**
  - Aislamiento
  - Comprensión
  - Distancia emocional

---

### Memory_04 - Las 11:30

- **Room:** Library
- **Interactuable:** Reloj de pared roto
- **Etapa:** Adultez
- **Contenido:** Recuerda la última vez que vio a su padre, que pudo escuchar su voz. El reloj marcaba las 11:30. Una conversación aparentemente normal. Una de tantas. Ninguno de los dos sabía que algún día echaría de menos incluso los momentos más corrientes.
- **Tema:**
  - Rutina
  - Nostalgia
  - Fragilidad del tiempo

---

### Memory_05 - El Huerto

- **Room:** Basement
- **Interactuable:** Caja de herramientas
- **Etapa:** Últimos años
- **Contenido:** La caja contiene herramientas gastadas por el uso. El protagonista recuerda a su padre trabajando como voluntario y presidente de unos huertos ecológicos. Nunca hablaba de aquello como un trabajo. Hablaba de las personas a las que ayudaba, de lo orgulloso que se sentía cuando algo crecía bien y de los proyectos que podían hacerse juntos. Hacía tiempo que no lo veía tan feliz. Aquella fue una de las últimas etapas de su vida. Y una de las pocas en las que realmente parecía estar en paz.
- **Tema:**
  - Legado
  - Felicidad sencilla
  - Últimos años compartidos
  - Paz interior

---

## Implementación

### Flags

- Memory_01
- Memory_02
- Memory_03
- Memory_04
- Memory_05

---

### Relación con los finales

- 0-2 Memorias: Final Malo.
- 3-4 Memorias: Final Neutral.
- 5 Memorias: Final Verdadero.

### Pantalla Final

La pantalla de desenlace muestra:

- Recovered Memories: X/5

Con ello se comunica al jugador que existen secretos narrativos sin convertir la partida en una colección obligatoria.
