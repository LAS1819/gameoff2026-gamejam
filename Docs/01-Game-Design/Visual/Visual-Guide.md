# Game Off 2026 - GUÍA VISUAL

- Versión 0.2 - Dirección Artística y Escala
- Creado: 03/11/2026
- Actualizado: 22/11/2026

Esta guía define las bases visuales del proyecto para mantener una producción coherente, rápida y realista para un único desarrollador, alineada con la estética pixel art establecida en el GDD y con los escenarios definidos en el Puzzle Flow.

## 1. Filosofía Visual

El juego utilizará una estética: Retro Pixel Art + Presentación Moderna

- **Objetivos**:
  - Evocar aventuras clásicas de los 90.
  - Mantener una producción asumible para una Game Jam.
  - Aprovechar efectos modernos para potenciar la atmósfera.
  - Evitar las limitaciones extremas del pixel perfect clásico.
- **Principio general**: los gráficos deben ser suficientemente simples para producirlos rápido y suficientemente detallados para sostener la narrativa ambiental.

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
  - Fondos más manejables.

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
  - Producción más rápida.

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
  - **Ejemplos**: Llave, Disquete, Manivela, Objetos clave de puzle
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
  - **Espacio suficiente para objetos complejos.**

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
- **estados modificados**.

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

## 14. Gráficos de las Memorias

Las imágenes de descubrimiento deben representarse como momentos simbólicos en lugar de ilustraciones completas.

Su composición debe seguir una regla visual común:

- formato horizontal;
- protagonista y padre como sombras;
- un único elemento identificativo;
- fondo sencillo;
- paleta limitada;
- sin texto dentro del sprite;
- sin detalles faciales realistas;
- sin revelar directamente la muerte del padre.

### Dimensiones y escala

- resolución interna: `640 x 360`;
- canvas recomendado para el sprite: `320 x 112`;
- posición principal: dentro de `MemoryDiscoveryOverlay`;
- escala visual: conservar proporción sin estirar;
- fondo del panel: puede oscurecerse para separar el recuerdo del escenario.

El tamaño `320 x 112` permite construir las cinco imágenes con la misma estructura y encaja aproximadamente con el área visual actual del overlay. Cada recurso puede contener:

- una silueta pequeña del protagonista;
- una silueta del padre;
- una acción o objeto de apoyo;
- uno o dos planos de fondo.

### Pautas Aseprite

- activar cuadrícula con tamaño 8 x 8 o 16 x 16;
- dibujar a 1x sin zoom durante la creación;
- usar líneas rectas y formas geométricas simples;
- limitar contornos a 1-2 píxeles;
- no usar antialiasing manual;
- no generar rotaciones arbitrarias;
- evitar degradados y texturas de alta frecuencia;
- exportar PNG a tamaño original;
- conservar transparencias solo si ayudan a integrar el fondo.

### Pixel scale en Unity

Estos sprites pertenecen al `overlay` de UI y deben conservar bordes nítidos:

- `Texture Type`: `Sprite (2D and UI)`;
- `Sprite Mode`: `Single`;
- `Pixels Per Unit`: utilizar un valor coherente con el resto del proyecto;
- `Filter Mode`: `Point (no filter)`;
- `Mip Maps`: desactivados;
- `Compression`: `None` en las builds iniciales si es necesario para preservar nitidez;
- `Generate Lighting`: desactivado;
- `Alpha is Transparency`: activado solo si existe transparencia real.

El elemento visual no debe interpolarse con píxeles fraccionarios. Mantener una posición par y dimensiones pares en Aseprite evita desplazamientos horizontales o verticales no deseados.

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
