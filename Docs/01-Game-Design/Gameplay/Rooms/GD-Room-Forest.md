# GD Room - Forest Design

> **Estado `0.1.1-proto`:** Forest, profundidades 0-6, carta, tres finales y
> restart están implementados. Es la ruta terminal; no vuelve al Basement.

- **Versión 0.1**
- **Creado:** 19/11/2026
- **Última actualización:** 23/11/2026

---

## Qué representa esta Room

El Bosque es la última Room del juego. Representa el último día en que el protagonista conservó una esperanza real de que todo pudiera salir bien: el día en que encontró a su padre aún con vida dentro de la ambulancia. Es el recuerdo más doloroso porque se sitúa exactamente en el límite entre la esperanza y la aceptación.

A diferencia de las demás Rooms, el Bosque no representa una fase de la negación. Representa el momento en que la negación ya no puede sostenerse.

Mientras el Lobby simboliza la Negación, el Hallway la primera grieta, la Bedroom el origen emocional del recuerdo, la Library la reinterpretación de la memoria y el Basement la verdad emergiendo, el Bosque representa la Aceptación final. Es el final del viaje.

---

## Qué ocurrió realmente

Era finales de mayo. Hacía calor. Mucho calor. El aire resultaba pesado y casi inmóvil. Ese fue el día en que el protagonista encontró a su padre.

Durante meses se aferró a la idea de que su padre simplemente había desaparecido. Sin embargo, este recuerdo conserva la verdad que intentó ocultar.

Al final del camino había unas luces. Luces que nunca fue capaz de olvidar. Y aquel fue el último instante en el que creyó que todavía podía salvarse.

---

## Objetivo de la Room

El Bosque tiene tres objetivos:

- Concluir la progresión narrativa.
- Evaluar las Memorias obtenidas durante la partida.
- Conducir al jugador al encuentro final con su padre.

La Room no gira alrededor de obtener objetos. Gira alrededor de aceptar una interpretación de los acontecimientos. Por ello, cada Memoria obtenida modifica la experiencia de la sala. Las Memorias dejan de ser coleccionables narrativos y pasan a convertirse en elementos activos del desenlace.

El jugador puede interpretar que las luces pertenecían a una ambulancia. Sin embargo, el juego nunca lo confirma explícitamente. La intención es preservar una pequeña ambigüedad incluso después del desenlace.

---

## Estructura General

La Room se divide en seis Fases. Las cinco primeras representan una aproximación gradual hacia las luces. La sexta fase representa la Convergencia.

### Estructura de Fases

```plaintext
Fase 1 - Memoria 01
↓
Fase 2 - Memoria 02
↓
Fase 3 - Memoria 03
↓
Fase 4 - Memoria 04
↓
Fase 5 - Memoria 05
↓
Fase 6 - Convergencia
↓
Habitación Final
```

---

## Concepto Visual

El jugador observa un bosque oscuro y aparentemente infinito. A lo lejos se perciben destellos intermitentes. Luces rojas. Luces azules. Pero a esta distancia todavía no parecen sirenas. Parecen simples luces misteriosas flotando entre los árboles.

Con cada avance:

- Las luces aumentan de tamaño
- La intensidad es mayor
- El sonido se vuelve más reconocible
- Aparecen más detalles del recuerdo

El jugador cree avanzar hacia una respuesta. En realidad está avanzando hacia el recuerdo que lleva evitando durante meses.

**Importante:**

Las luces nunca llegan a revelarse completamente. Al llegar a la profundidad 6,
Forest se convierte en la sala final con mesa, luz y carta.

El Bosque genera continuamente la sensación de que existe una respuesta física al final del recorrido. La Habitación Final revela que la respuesta siempre fue emocional.

---

## Sistema de Fases y Memorias

Cada Fase está vinculada a una Memoria concreta.

**Regla principal:**

- Si la Memoria correspondiente fue descubierta:
  - Ocurre un evento positivo;
  - El protagonista acepta parte de la verdad;
  - Aparecen elementos visuales más cálidos;
  - La progresión es más tranquila.

- Si la Memoria NO fue descubierta:
  - Ocurre un evento negativo;
  - Aparecen contradicciones;
  - El protagonista se resiste a recordar;
  - El entorno se vuelve más vacío.
  - El Bosque parece más silencioso.
  - Desaparecen sonidos ambientales.
  - Se pierden fragmentos de conversación.
  - Aparecen espacios vacíos entre los árboles.
  - Algunas respuestas nunca llegan a producirse.
  - El jugador siente ausencia más que amenaza.

La diferencia no debe bloquear el avance. Debe modificar la interpretación emocional del recorrido.

---

## Ejemplos de Eventos

### Memoria encontrada

- Voz del padre.
- Recuerdo agradable.
- Fragmento de conversación.
- Sensación de paz.
- Desaparición temporal de sombras.

### Memoria ausente

- Silencio.
- Frases incompletas.
- Distorsiones visuales.
- Apariciones ambiguas.
- Sensación de pérdida.

De este modo el propio jugador construye el desenlace a partir de aquello que decidió explorar durante la aventura.

---

## Sala final de Forest

El jugador espera encontrar el origen de las luces. Al alcanzar el último
nivel, el fondo desaparece y la mesa, la luz y la carta aparecen en la misma room.
No existe transición de escena adicional.

Es la última construcción mental creada por el protagonista. Es el último refugio antes de enfrentarse definitivamente a la verdad.

---

## La Carta de Revelación

En el centro de la habitación hay una mesa. Sobre ella descansa una carta. La carta nunca cambia completamente. Lo que cambia es su interpretación.

Dependiendo de las Memorias obtenidas:

- Parte del texto puede variar
- Ciertos objetos de la habitación pueden existir o no
- La iluminación puede cambiar
- Determinados recuerdos aparecen representados visualmente

La carta muestra la verdad que el protagonista está preparado para aceptar. No necesariamente toda la verdad.

---

## Encuentro Final

Tras interactuar con la carta comienza el encuentro final. La presencia del padre se percibe de distintas formas:

- Una voz
- Una silla vacía frente al jugador
- Una figura fuera de foco
- Una sensación de compañía

El juego evita mostrar explícitamente al padre. La conversación se produce mediante texto, voz o simbolismo visual.

---

## Diseño Técnico

### Sistema de profundidad MVP

La Room utiliza cinco niveles de profundidad controlados por un único contador.

La escena tiene cuatro elementos visuales principales:

- Capa -3 - `Sky`;
- Capa -2 - `Outer Forest`;
- Capa -1 - `Border Trees`;
- Capa 0 - `Central Path`.

Cada nivel sustituye simultáneamente el frame correspondiente de las cuatro capas. El cielo comienza azul y se oscurece progresivamente hasta el nivel 5.

Los cinco frames disponibles son:

- Forest_01
- Forest_02
- Forest_03
- Forest_04
- Forest_05

La `Habitación Final` se activa al completar el quinto nivel.

La mesa y la carta forman un sexto estado de profundidad que no requiere una
nueva sustitución de los cuatro spritesheets. Al completar el nivel 5, el
siguiente avance revela un GameObject de mesa y activa la carta interactuable.

### Elementos persistentes

Las luces de la ambulancia nunca desaparecen.

Simplemente:

- Escalan
- Aumentan su intensidad
- Cambian ligeramente de posición
- Revelan más detalles conforme avanza la profundidad

---

## Interacción de avance

La Room contiene un único interactuable invisible llamado `Forest Path`.

- El jugador hace click sobre la zona interactiva central.
- Cada clic incrementa el nivel de profundidad.
- Las cuatro capas cambian al mismo frame.
- Al alcanzar el nivel 5, el siguiente avance activa el estado final de nivel 6.
- El interactuable no consume objetos ni requiere puzles adicionales.

---

## Información Clave

- El Bosque representa la aceptación.
- Es la última Room del juego.
- Representa el día de la ambulancia.
- Las luces lejanas son las sirenas.
- La Room está dividida en seis fases.
- Cinco de las fases evalúan una Memoria.
- La sexta fase representa la Convergencia.
- Las Memorias modifican los eventos narrativos.
- La Habitación Final contiene la Carta de Revelación.
- La verdad mostrada depende de las Memorias obtenidas.
- El encuentro final con el padre determina el desenlace emocional de la historia.
