# Game Off 2026 - GUÍA VISUAL

- Versión 0.1 - Dirección Artística y Escala
- Creado: 03/11/2026
- Actualizado: 03/11/2026

Esta guía define las bases visuales del proyecto para mantener una producción coherente, rápida y realista para un único desarrollador, alineada con la estética pixel art establecida en el GDD y con los escenarios definidos en el Puzzle Flow. [GameOff26-GameIdea-1 | Word], [GameOff26-...zleFlow-v1 | Word], [GameOff26-...dea-1-v3 1 | PDF]

## 1. Filosofía Visual

El juego utilizará una estética: Retro Pixel Art + Presentación Moderna

- **Objetivos**:
  - Evocar aventuras clásicas de los 90.
  - Mantener una producción asumible para una Game Jam.
  - Aprovechar efectos modernos para potenciar la atmósfera.
  - Evitar las limitaciones extremas del pixel perfect clásico.
- **Principio general**: los gráficos deben ser suficientemente simples para producirlos rápido y suficientemente detallados para sostener la narrativa ambiental. [GameOff26-GameIdea-1 | Word], [GameOff26-...zleFlow-v1 | Word]

---

## 2. Resolución de trabajo

- **Resolución interna**: 640 x 360
- **Formato**: 16:9
- **Resolución objetivo**: 1920 x 1080
- **Escalado**: x3 exacto
- **Ventajas**:
  - Escalado limpio.
  - Compatible con WebGL.
  - Compatible con Windows.
  - Producción más rápida.
  - Fondos más manejables. [GameOff26-GameIdea-1 | Word]

---

## 3. Cámara

- **Tipo**:
- Cámara fija por escenario.
- Cada sala será una ilustración independiente.
- **No existirán**:
  - scroll lateral,
  - seguimiento de personaje,
  - zoom dinámico.
- **Ventajas**:
  - Menor carga artística.
  - Mayor detalle visual.
  - Producción más rápida. [GameOff26-GameIdea-1 | Word], [GameOff26-...zleFlow-v1 | Word]

---

## 4. Escala general

Tomando una resolución interna de:

- **Resolución interna**: 640 x 360
- **Escala recomendada**:
  - **Escenarios**
    - **Fondo completo**: 640 x 360
  - **Elementos importantes**
    - **Entre**: 24 x 24 y 48 x 48
  - **Puntos de interés**
    - **Mínimo**: 32 x 32 para garantizar visibilidad.

---

## 5. Tamaño de objetos

- **Objetos pequeños**
  - **Ejemplos**: Llaves, Notas, Interruptores, Cerillas
  - **Tamaño recomendado**: 16 x 16
- **Objetos medianos**
  - **Ejemplos**: Libros, Fotografías, Teléfonos, Fusibles
  - **Tamaño recomendado**: 24 x 24 o 32 x 32
- **Objetos importantes**
  - **Ejemplos**: Medallón, Símbolo Ritual, Objetos clave de puzle
  - **Tamaño recomendado**: 32 x 32 o superior.

---

## 6. Inventario

### Casillas

Tamaño recomendado: 64 x 64

### Iconos

Tamaño útil: 32 x 32, centrado dentro de cada casilla.

- **Objetivos**:
  - **Buena legibilidad.**
  - **Fácil producción.**
  - **Espacio suficiente para objetos complejos.** [GameOff26-GameIdea-1 | Word]

---

## 7. Interactivos en escenario

El jugador nunca debe preguntarse: "¿Eso se puede pulsar?"

### Regla

Todo elemento interactivo debe destacar visualmente mediante al menos uno de los siguientes recursos:

- **Contraste de color.**
- **Mayor iluminación.**
- **Silueta reconocible.**
- **Pequeña animación.**
- **Cursor contextual.**

---

## 8. Paleta de color

### Realidad Normal

- Predominio de:
  - Marrones apagados.
  - Verdes desaturados.
  - Grises.
  - Amarillos envejecidos.
- **Sensación:**
  - Abandono.
  - Decadencia.
  - Melancolía.


### El Otro Lado

- Predominio de:
  - Morados oscuros.
  - Azules profundos.
  - Negros.
  - Rojos puntuales.
- **Sensación:**
  - Distorsión.
  - Hostilidad.
  - Irrealidad.

### Geometría Similar

La diferencia principal será:

- **color**
- **iluminación**
- **elementos adicionales**
- **estados modificados**. [GameOff26-GameIdea-1 | Word]

---

## 9. Producción de escenarios

### Regla principal

No crear dos escenarios completamente diferentes.

- **Cada sala se compone de:**
  - Capa base
    - Escenario normal.
  - Variación
    - Escenario distorsionado.
    - Cambios permitidos:
      - Objetos añadidos.
      - Objetos eliminados.
      - Daños.
      - Sombras.
      - Símbolos.
      - Cambios de iluminación.
    - Objetivo:
      - Reutilizar entre el 70% y el 90% del trabajo artístico.

---

## 10. Animación

Animaciones mínimas.

### Prioridad Alta

- **Parpadeos.**
- **Luces.**
- **Niebla.**
- **Sombras.**
- **Transición SHIFT.**

### Prioridad Baja

- **Animaciones complejas.**
- **Ciclos largos.**
- **Personajes animados.**

El protagonista no aparece en pantalla. [GameOff26-GameIdea-1 | Word]

---

## 11. Efectos modernos

Los efectos NO necesitan respetar pixel perfect estricto.

- Efectos permitidos:
  - Glow suave.
  - Viñeteado.
  - Niebla.
  - Bloom ligero.
  - Aberración cromática muy sutil.
  - Oscurecimiento de pantalla.
- **Especialmente durante:**
  - Eventos.
  - Apariciones.
  - Cambio de realidad.

---

## 12. Transición SHIFT

La transición es el efecto visual más importante del juego.

- **Objetivos**:
  - Comunicar el cambio de realidad.
  - Transmitir inquietud.
  - Ser reconocible inmediatamente.
- **Duración recomendada:**
  - 0,3 - 0,6 segundos
- **Efectos posibles:**
  - Distorsión.
  - Desplazamiento de capas.
  - Cambio de color.
  - Oscurecimiento breve.
  - Aparición de ruido visual.

---

## 13. Reglas de producción

Para cualquier elemento artístico:

- **Si tarda más de una tarde en producirse**: simplificar.
- **Si se necesita una animación compleja**: buscar una alternativa estática.
- **Si existen dos versiones completamente diferentes**: intentar reutilizar una.
- **Si el jugador no interactúa con ello**: reducir detalle.

---

## Resumen técnico definitivo

```plaintext
Render interno:     640 x 360
Salida final:       1920 x 1080
Escalado:           x3 exacto
Fondos:             640 x 360
Objetos pequeños:   16 x 16
Objetos medios:     24 x 24 / 32 x 32
Objetos clave:      32 x 32+
Inventario:         64 x 64
Cámara:             Fija
Arte:               Pixel Art HD
Pixel Perfect:      Parcial
FX:                 Modernos
Transición SHIFT:   Principal efecto visual
```

Esta configuración encaja especialmente bien con el alcance reducido del GameOff26-GameIdea-1.docx y la estructura contenida de GameOff26-PuzzleFlow-v1.docx, porque maximiza la reutilización de escenarios y minimiza la carga artística para un único desarrollador. [GameOff26-GameIdea-1 | Word], [GameOff26-...zleFlow-v1 | Word], [GameOff26-...dea-1-v3 1 | PDF]
