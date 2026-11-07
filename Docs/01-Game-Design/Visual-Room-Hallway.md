# Game Off 2026 - PASILLO PRINCIPAL

- Versión 0.1 - Diseño Visual
- Creado: 05/11/2026
- Actualizado: 05/11/2026

---

## 1. Objetivo del escenario

El Pasillo Principal es la primera zona donde el jugador experimenta el cambio de realidad de forma explícita.

Su función es:

- introducir la mecánica SHIFT con una amenaza visual clara;
- mostrar que la ruta no es segura ni estática;
- reforzar la sensación de que el edificio “no está bien”;
- conectar la salida del vestíbulo con el siguiente bloque de progresión;
- preparar la transición hacia la `Biblioteca` y el `Dormitorio`.

El jugador debe entender enseguida:

- que este pasillo no es un espacio neutral;
- que hay una obstrucción real que debe resolverse;
- que la realidad cambia la geometría del espacio;
- que el camino solo queda claro cuando se activa SHIFT.

---

## 2. Composición general

### Cámara

- Cámara fija.
- Fondo único en una sola vista.
- Sin desplazamiento.
- Sin personaje visible.
- Enfoque en la sensación de corredor y bloqueo temporal.

### Encuadre

La cámara debe mostrar un pasillo largo y desolado, con sensación de recorrido sin llegar a ser un mapa abierto.

Distribución aproximada:

```plaintext
┌──────────────────────────────────────────┐
│                                          │
│   Puerta del Vestíbulo      Pasillo      │
│                                          │
│     [Ruta bloqueada]         [Ruta       │
│         Obstrucción        visible]      │
│                                          │
│          Pared / Estantería              │
│                                          │
│      Señal de transición / SHIFT         │
│                                          │
│    Entrada a Biblioteca / Dormitorio     │
│                                          │
└──────────────────────────────────────────┘
```

La composición debe ser clara: el pasillo debe sentirse estrecho y opresivo, con una ruta claramente bloqueada en una realidad y abierta en la otra.

---

## 3. Historia ambiental

El pasillo principal del sanatorio debe sugerir un lugar que fue vital en su funcionamiento, pero que el paso del tiempo y lo ocurrido lo han convertido en un lugar inestable.

Elementos recomendados:

- pasillo largo con suelo de baldosas rotas;
- paredes gastadas y parcialmente desconchadas;
- puertas laterales cerradas o semi-deterioradas;
- iluminación débil y sesgada;
- estanterías, cajas o mobiliario desvencijado;
- huellas de tránsito o marcas de manos en las paredes;
- restos de actividad médica o administrativa;
- sensación de presencia constante, aunque no visible.

No debe verse como un sitio “limpio” ni ordenado. Debe dar la impresión de que el edificio intenta seguir funcionando aunque ya no pueda.

---

## 4. Paleta visual

### Realidad Normal

Colores dominantes:

- gris sucio;
- verde apagado;
- marrón descolorido;
- amarillo oxidado.

Sensación:

- abandono;
- rutina rota;
- tensión baja pero constante.

### El Otro Lado

Colores dominantes:

- morado oscuro;
- azul profundo;
- negro;
- rojo muy tenue o anomalía esporádica.

Sensación:

- distorsión;
- amenaza psicológica;
- espacio casi vivo.

### Iluminación

- iluminación general reducida;
- sombras largas y duras;
- una fuente de luz puntual (lámpara, ventana o foco) para anclar la composición;
- cambios de intensidad al alternar realidades;
- el contraste debe ayudar a que la diferencia entre ambos estados sea inmediata.

---

## 5. Interactivos principales

### Obstrucción del Pasillo - Hallway Obstacle

Función:

- bloquea la ruta principal temporalmente;
- exige el uso de SHIFT para poder continuar.

Visualmente:

- debe ser grande, reconocible y claramente no estético;
- puede ser una estantería caída, un bloque de pared, un mueble o una estructura deformada;
- la forma debe leerse rápidamente como obstáculo.

Comportamiento visual:

- en Realidad Normal: aparece con presencia real y bloquea la ruta;
- en El Otro Lado: desaparece o queda semitransparente, dejando paso libre.

---

### Punto de SHIFT - Shift Trigger

Función:

- actúa como espacio de transición entre realidades;
- marca el punto en el que el jugador toma conciencia de la mecánica.

Visualmente:

- una zona de luz distinta, brillo, huella o marca en el suelo;
- símbolo sutil y no demasiado literal;
- debe invitar a la exploración sin romper la ambientación.

Feedback adicional:

- el Icono del Amuleto (UI) en la esquina superior derecha debe estar **parpadeando o mostrando feedback visual** para indicar que SHIFT está disponible;
- ver [SHIFT-Mechanic.md](./SHIFT-Mechanic.md) para detalles completos sobre el icono y su activación.

---

### Entrada al Siguiente Sector - Transition Marker

Función:

- marcar la ruta libre o parcialmente abierta después de resolver la obstrucción.

Visualmente:

- disposición de fondo clara y simple;
- puede ser un hueco, una puerta interrumpida o un corredor que se abre más allá del bloqueo;
- debe estar conectado visualmente con la idea de que la estructura del edificio cambia según la realidad.

---

## 6. Producción artística

### Nivel de detalle

La sala no necesita ser extremadamente compleja, pero debe tener un fuerte impacto visual.

Prioridad:

- siluetas claras;
- lectura rápida del bloqueo;
- contraste suficiente para diferenciar ambas realidades;
- objetos con trazado mínimo pero legible.

### Regla principal

El pasillo debe ser una habitación más “narrativa” que “decorativa”:

- transmite tensión;
- comunica el cambio de realidad;
- da sentido a la progresión.

Si un elemento no ayuda a estas tres cosas, no hace falta incluirlo.

---

## 7. Versión "Otro Lado"

La composición general debe mantenerse casi igual para reforzar la sensación de continuidad espacial.

### Cambios visuales recomendados

- oscurecimiento general del pasillo;
- más contraste entre luces y sombras;
- materialidad más extraña en muros y suelo;
- presencia de detalles no naturales: marcas, grietas, descamación imposible;
- obstrucción reducida o eliminada en la ruta principal;
- iluminación más fría, con dominantes moradas, azules y negras.

La idea no es cambiar completamente el lugar, sino mostrar que el mismo espacio se ha “distorsionado” sin perder identidad.

---

## 8. Función narrativa

El Pasillo Principal funciona como primer gran “punto de ruptura” del juego.

Su propósito narrativo es doble:

1. demostrar que el edificio ya no funciona como una construcción normal;
2. introducir una lógica de progresión donde el jugador debe adaptarse al cambio de realidad.

Es la primera sala que no se resuelve solo con “ver” o “recoger”, sino con realizar una acción concreta sobre el espacio: SHIFT.

---

## 9. Resultado esperado

La habitación debe transmitir:

- tensión contenida;
- sensación de avance bloqueado;
- primera interacción clara con la mecánica SHIFT;
- diferencia visual inmediata entre ambas realidades;
- transició n natural hacia la siguiente zona del diseño del juego.

En resumen: el Pasillo Principal debe sentirse como un paso de “no volver atrás”, aunque la progresión real aún no sea completa.
