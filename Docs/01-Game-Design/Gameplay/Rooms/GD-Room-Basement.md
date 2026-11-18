# GD Room - Basement Design

- Versión 0.2
- Creado: 17/11/2026
- Actualizado: 18/11/2026

---

## 1. Qué representa esta Room

El `Basement` es la primera Room donde la mentira del protagonista deja de ser “ambiente” y se vuelve evidencia.

Si la `Bedroom` es intimidad y la `Library` es memoria archivada, el `Basement` es maquinaria emocional oculta: el lugar donde lo reprimido empieza a hacer ruido.

Representa:

- la verdad que ya no puede seguir almacenada;
- la culpa convertida en presión;
- la furia contenida (cañerías, vapor, ruido);
- el duelo que deja de ser teoría y se vuelve experiencia física.

Narrativamente, es el punto donde el jugador siente: “esto ya no es decoración, aquí hay algo que sostiene toda la mentira”.

---

## 2. Qué ocurre en esta Room

El jugador llega desde la `Library` tras abrir el acceso con el `Basement Code`.

La sala está en penumbra extrema. Hay luz global mínima, suficiente para ubicarse, pero insuficiente para leer el espacio con claridad.

Aquí aparece una regla nueva y clara:

- sin `Linterna` activa, se puede `Examinar`, pero la lectura es ambigua;
- sin `Linterna` activa, las interacciones de `Usar` no se permiten;
- con `Linterna` activa, el jugador obtiene la lectura correcta del entorno y puede operar el interactuable clave.

---

## 3. Papel de la Room en la progresión

El `Basement` cumple cuatro funciones:

1. Consolidar la `Linterna` como mecánica jugable real (no solo icono de HUD).
2. Entregar una evidencia concreta de que la versión del protagonista está rota.
3. Introducir una amenaza bloqueante de tipo psicológico (sin combate complejo).
4. Cerrar el “primer acto” de investigación: de `Library` (pista) -> `Bedroom` (código) -> `Basement` (verdad material).

---

## 4. Reglas de luz y uso de la Linterna

### Principio base

La `Linterna` solo debe estar habilitada en rooms diseñadas para ello (MVP: `Basement`).

### Método recomendado para MVP

Seguir el mismo patrón de activación por room usado en `SHIFT`:

- icono siempre visible en HUD;
- uso habilitado solo en rooms permitidas;
- feedback claro cuando no está permitido.

### Apoyo con luz global

Además del gating por room, el `Basement` debe tener una `Global Light 2D` muy baja para reforzar la necesidad de la linterna.

No conviene depender solo de un umbral automático en MVP. Lo más robusto es:

- **gating explícito por Room** (autoridad principal);
- **intensidad de luz global** como refuerzo visual y narrativo.

---

## 5. Interactables principales

### 5.1 Estanterías de Almacenamiento / Storage Shelves

Las estanterías son el núcleo de exploración de la `Room`.

**Visual:**

- Estanterías metálicas, algo oxidadas.
- Cajas de cartón deterioradas.
- Objetos etiquetados.
- Herramientas olvidadas.
- Pertenencias almacenadas desde hace años.

**Función Principal:**

- Obligar al jugador a usar la `Linterna`.
- Reforzar la sensación de búsqueda.
- Ocultar la `Evidencia Principal` y los objetos necesarios para avanzar.

**Sin Linterna:**

- Las formas resultan confusas.
- Las etiquetas son ilegibles.
- Los textos de `Examine` son ambiguos o incompletos.

---

### 5.2 Caja de Efectos Personales / Personal Effects Box

Es la `Evidencia Principal` del Basement.

Se encuentra oculta entre las estanterías y sólo puede identificarse correctamente utilizando la `Linterna`.

**Contenido Posible:**

- Fotografías.
- Documentos.
- Objetos personales (llaves, cartas, recuerdos).
- Recuerdos asociados al incidente del padre.

**Objetivo Narrativo:**

- No explicar completamente lo ocurrido.
- Demostrar que la versión del protagonista contiene contradicciones.
- Introducir dudas imposibles de ignorar.

La caja debe sentirse como una prueba material de que algo no encaja en la historia que el protagonista se ha contado a sí mismo durante mucho tiempo.

---

### 5.3 Manivela de Válvula / Valve Handle

Objeto de progreso de la `Room`.

La `Manivela de Válvula` se encuentra oculta entra cajas y otros elementos almacenados.

**Características:**

- Requiere exploración activa.
- Sólo puede encontrarse con la `Linterna` activa.
- Se incorpora al inventario temporalmente.

Su función es restaurar una válvula incompleta e inutilizable.

La existencia de una válvula sin manivela visible en la `Room`, debe comunicarse al jugador antes o después de encontrar la pieza para generar una relación clara entre problema y solución, asegurando que el jugador entienda la necesidad de la `Manivela de Válvula`.

---

### 5.4 Válvula de Descarga / Pressure Release Valve

Elemento principal de resolución de la `Room`.

La `Válvula de Descarga` existe en ambas realidades. En la `Realidad Normal` puede accionarse. En el `Otro Lado` aparece dañada y no puede manipularse.

**En Realidad Normal:**

- Puede accionarse tras instalar la `Manivela de Válvula`.
- El Jugador escucha el movimiento de tuberías y presión liberándose.
- No existe un efecto visual importante.

**En El Otro Lado:**

- Las tuberías aparecen rotas.
- El vapor emerge por las tuberías rotas o fisuras.
- El vapor liberado en la `Realidad Normal` produce consecuencias visibles en el `Otro Lado`.

**Animación:**

- Sprites Animados: 3 o 4 sprites, animación Loop, transparencia y movimiento vertical.

Narrativamente la válvula simboliza la liberación de una presión emocional acumulada. No representa una máquina, representa una verdad que ya no puede seguir contenida.

### 5.5 Luces de Ambulancia / Ambulance Lights

Sólo aparecen en el `Otro Lado`.

Se manifiestan como destellos rojos-azulados suaves e irregulares.

**Características:**

- Sólo aparecen en el `Otro Lado`.
- No modifican la jugabilidad.
- Funcionan como recordatorios fragmentados del día de la muerte del padre.

**Ejemplos de Examinar:**

- "Durante unos instantes creo oír voces."
- "Las luces parpadean como si intentaran recordarme algo."
- "No quiero pensar en aquel día."
- "El parpadeo de las luces me hace sentir observado."
- "Siento que algo intenta comunicarse a través de los destellos."

Representan fragmentos del recuerdo asociado al fallecimiento del padre. No explican el acontecimiento. Sólo evocan su presencia.

---

### 5.6 Puerta de salida / transición

Conecta el `Basement` con la siguiente fase de progresión del juego.

**Condición:**

- La `Manifestación de Negación` debe haber sido disipada.
- (en el `Otro Lado`) La `Válvula de Descarga` debe estar apagada (tras haber disipado a la `Manifestación de Negación`).
- (en la `Realidad Normal`) no es necesario que esté apagada.

No introduce nuevos puzles. Su función es reforzar que algo importante ha cambiado y que ya no es posible regresar emocionalmente al estado anterior de la historia.

---

## 6. Manifestación de Negación / Denial Manifestation

La `Manifestación de Negación` es el equivalente conceptual del `wardrobe-monster` (`Bedroom`), adaptado al `Basement`. Sin embargo, en esta `Room` no funciona como un depredador. Funciona como un bloqueo emocional y narrativo que impide al jugador avanzar hasta cumplir ciertas condiciones.

**Características:**

- Sólo aparece físicamente en el `Otro Lado`.
- Bloquea el acceso a la salida del `Basement`.
- Permanece con un movimiento espasmódico y errático, reforzando la sensación de bloqueo y tensión emocional.
- Genera tensión mediante presencia y sonido.
- No requiere combate.

En la `Realidad Normal` no puede verse directamente, pero lo puede percibir:

- Sombras.
- Sonidos distantes.
- Sensación de presencia.
- Alteraciones visuales sutiles.

**Resolución:**

1. Encontrar la `Manivela`.
2. Reparar la `Válvula de Descarga`.
3. Activarla en la `Realidad Normal` (en el `Otro Lado` no se puede, pues está rota).
4. Cambiar al `Otro Lado`.
5. Observar cómo el vapor emerge por las tuberías rotas.
6. La `Manifestación de Negación` se distorsiona y retrocede/desaparece.
7. El jugador vuelve a la `Realidad Normal` y puede pasar por la salida del `Basement`.
8. En el `Otro Lado`, si se quiere pasar por la salida, primero debe cerrar la `Válvula de Descarga` para que el vapor deje de bloquear el camino.

**Importante:**

- La `Manifestación de Negación` no es derrotada. Simplemente pierde la capacidad de bloquear el avance del jugador temporalmente.
- Debe interpretarse como una representación del mecanismo de negación del personaje protagonista, no como una criatura física convencional.

---

## 7. Iconografía y tono visual del Basement

### Realidad Normal

- estructura de sótano funcional, deteriorada;
- tuberías oxidadas;
- iluminación muy baja y fría;
- sensación de espacio técnico abandonado.

### El Otro Lado

- más contraste y sombras duras;
- sirenas de ambulancia intermitentes (flash emocional);
- vapor y respiración mecánica agresiva;
- goteo acentuado en beats narrativos.

### Simbolismo ambiental

- cañerías rotas -> furia;
- vapor -> ira contenida;
- goteras -> lágrimas;
- flashes de sirena -> recuerdo del día de la muerte del padre.

Estos recursos deben ser breves, legibles y dirigidos a la narrativa, no a espectáculo VFX complejo.

---

## 8. Reglas narrativas específicas

- El `Basement` no responde “qué pasó” completo; entrega evidencia suficiente para quebrar la versión falsa.
- El texto de examen sin linterna debe insinuar confusión, negación y ruido.
- El texto con linterna debe ser directo y concreto.
- La Room debe terminar con sensación de avance irreversible.

---

## 9. Scope guard del MVP

### Mantener

- 1 evidencia central + 1 activación;
- 2-3 interacciones principales en total.
- gating de linterna por room;
- examen ambiguo sin linterna y correcto con linterna;
- bloqueo de `Use` sin linterna;
- manifestación bloqueante simple.

### Simplificar

- no batería en esta iteración;
- no IA compleja de enemigo;
- no múltiples máquinas con lógica distinta;
- no árbol grande de estados.

### Dejar fuera por ahora

- consumo de batería;
- persecución completa del enemigo;
- puzles secundarios paralelos;
- extensión jugable del `Huerto`.

El `Huerto` se mantiene como semilla narrativa para una iteración posterior (room separada o extensión controlada del `Basement`).

---

## 10. Información clave

- El `Basement` es la room de evidencia y maquinaria oculta.
- La `Linterna` pasa de ser promesa a sistema jugable real.
- Sin linterna: examen ambiguo; uso bloqueado.
- Con linterna: lectura correcta + progreso.
- La amenaza funciona como bloqueo psicológico, no como combate.
- La atmósfera debe sostener duelo, culpa y ruptura de la mentira.

---

## 11. Resumen corto

El `Basement` es el punto donde la historia deja de insinuar y empieza a demostrar. El jugador entra con código desde la `Library`, se enfrenta a una oscuridad que exige linterna, interpreta una evidencia central y supera un bloqueo emocional representado por una manifestación hostil. Es una Room de pocas interacciones, alto peso narrativo y alcance controlado para MVP.
