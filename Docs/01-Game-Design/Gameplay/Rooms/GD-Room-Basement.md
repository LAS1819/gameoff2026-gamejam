# GD Room - Basement Design

- Versión 0.1
- Creado: 17/11/2026
- Actualizado: 17/11/2026

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

La Room debe contener una pieza central de evidencia y no más de 2-3 interacciones principales.

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

### 5.1 Consola de Presión / Registro de Mantenimiento (evidencia central)

- Interactuable principal de la Room.
- Sin linterna: lectura incompleta/ambigua.
- Con linterna: lectura íntegra con información clave sobre el incidente del padre.
- Debe sentirse como una prueba material, no como lore opcional largo.

### 5.2 Válvula de Descarga (activación/comprobación)

- Interacción funcional simple para estabilizar presión/ruido.
- Solo usable con linterna activa.
- Al resolverla, desbloquea el estado final de la Room o la salida narrativa.

### 5.3 Puerta de salida / transición

- Mantiene continuidad con `Library` y siguiente bloque.
- No introduce puzle extra en esta iteración.

---

## 6. Amenaza bloqueante (MVP)

Se mantiene el patrón del `wardrobe-monster`, adaptado al `Basement`.

- No es combate.
- Es bloqueo de interacción y presión emocional.
- Aparece en momentos de vulnerabilidad (oscuridad + intento de forzar avance sin luz).

Regla clara:

- si el jugador intenta forzar el uso clave sin linterna, la manifestación bloquea;
- al activar linterna y cumplir condición de la Room, la amenaza se disipa o retrocede.

Debe leerse como “bloqueo mental del protagonista”, no como enemigo táctico.

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

- acceso por `Library Door` + `Basement Code`;
- 1 evidencia central + 1 activación;
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
