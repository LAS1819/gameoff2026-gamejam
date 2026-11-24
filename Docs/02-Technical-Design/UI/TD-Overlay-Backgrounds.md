# TD-Overlay-Backgrounds - Backgrounds gráficos de overlays

- **Versión:** 1.0
- **Creado:** 2026-11-24
- **Actualizado:** 2026-11-24
- **Autor:** @LAS1819
- **Estado:** Implementado en el prototipo; arte provisional propio

---

## Objetivo

Definir una familia visual compartida para:

- el panel de descubrimiento de Memorias;
- la Carta Revelación;
- el Resumen Final.

Los tres background son assets separados, aunque comparten proporción, corte
pixelado, profundidad de borde y 9-slice. Los textos no se incluyen dentro de
los PNG.

---

## Sistema gráfico recomendado

Usar 9-slice en los tres panels. Permite ampliar el rectángulo desde
`420-620 px` sin deformar esquinas, marcos ni remaches.

Canvas recomendado:

```text
320 x 320 px
```

Slices:

```text
24 px izquierda
24 px derecha
24 px arriba
24 px abajo
```

Estas medidas dejan un borde ornamental estable de 24 px y un centro grande
que puede estirarse sin cambios visibles en las esquinas.

No es necesario crear una altura diferente por tipo de contenido. Los paneles
pueden variar entre `220-320 px` según el texto, manteniendo el mismo asset.

---

## Memorias

### Concepto

Panel ceniza con sensación de archivo, recuerdo congelado o fotografía antigua.
Debe sentirse distinto del HUD y de la madera cálida de Carta/Resumen.

Paleta recomendada:

- fondo: `#BFC6C2`;
- superficie: `#D6D2C1`;
- sombra: `#4A4D50`;
- acento: `#8D9DA1`;
- detalle metálico: `#C8C2A2`.

### Composición

- borde exterior oscuro de 2-3 px;
- esquinas cortadas o recortadas;
- borde interno fino;
- cuatro marcas archivísticas pequeñas;
- centro casi plano para legibilidad;
- textura mínima y sin contraste fuerte detrás del copy.

El concepto de cada Memoria sigue siendo un sprite horizontal independiente:

```text
320 x 112 px
```

El background no debe contener el título ni el botón.

Jerarquía:

```text
memory-discovery-overlay
├── memory-discovery-backdrop
└── memory-discovery-panel
    ├── memory-discovery-image
    ├── memory-discovery-title
    ├── memory-discovery-body
    └── memory-discovery-close
```

### Asset

```text
Assets/Visuals/UI/Memory/
└── sprite-ui-memory-panel-background-v1.png
```

Import Settings:

- Texture Type = `Sprite (2D and UI)`;
- Sprite Mode = `Single`;
- Pixels Per Unit = `16`;
- Filter Mode = `Point`;
- mipmaps desactivados;
- Compression = `None`;
- 9-slice en USS: `24 px` por cada lado.

---

## Carta Revelación

### Concepto

La Carta debe parecer papel realista/pixelado, no un panel de madera. Debe
destacar por superficie clara, bordes irregulares suaves y una pequeña zona
de envejecimiento alrededor de los bordes.

Paleta recomendada:

- papel: `#E7D3A2`;
- papel claro: `#F1DFC0`;
- sombra: `#A88A5A`;
- mancha: `#C0A477`;
- tinta: `#34231A`.

### Composición

- esquinas ligeramente irregulares;
- 1 borde externo oscuro tenue;
- degradado imposible en pixel art: usar bandas o dither controlado;
- manchas limitadas a los bordes;
- centro limpio;
- pliegue opcional muy tenue;
- sin sello grande que compita con el título.

La textura puede llegar a 320 px para que el `dither` o el patrón de papel no
se vea repetido. Los bordes reciben ornamento pero el centro se estira.

### Asset

```text
Assets/Visuals/UI/Story/
└── sprite-ui-story-letter-background-v1.png
```

Import Settings:

- Texture Type = `Sprite (2D and UI)`;
- Sprite Mode = `Single`;
- Pixels Per Unit = `16`;
- Filter Mode = `Point`;
- mipmaps desactivados;
- Compression = `None`;
- 9-slice en USS: `24 px` por cada lado.

---

## Resumen Final

### Concepto

El Resumen Final debe sentirse definitivo y oscuro. Puede reutilizar el marco
de la Carta pero con fondo de madera negra, azul ceniza o marrón profundo.

La variante creada para el prototipo usa madera oscura con borde dorado.
La diferencia principal respecto a la Carta es el peso del marco y la jerarquía
de tres líneas narrativas.

Paleta recomendada:

- madera oscura: `#3C2B22`;
- sombra profunda: `#241A16`;
- dorado tenue: `#CBAE73`;
- borde claro: `#E6D4A6`;
- texto: `#F1E5C9`.

### Composición

- marco grueso;
- esquinas con remaches o formas escalonadas;
- dos líneas internas doradas;
- centro plano y oscuro;
- ornamento pequeño arriba y abajo;
- sin ilustración grande que choque con los títulos.

### Tipografía

El fondo oscuro requiere texto claro:

- eyebrow: dorado claro `rgba(230,212,166,0.86)`;
- título: crema `rgba(244,228,190,0.98)`;
- cuerpo: crema clara `rgba(241,229,201,0.98)`;
- contador: dorado `rgba(229,201,134,0.96)`;
- créditos: dorado apagado `rgba(218,197,151,0.86)`.

La Carta mantiene sus colores oscuros sobre papel mediante selectores
específicos de `.story-overlay-panel-letter`.

El panel se mantendrá fijo a `420-620 px` de ancho y `220-320 px` de alto.
El 9-slice evita deformar remaches y esquinas al pasar entre tamaños.

### Asset

```text
Assets/Visuals/UI/Story/
└── sprite-ui-story-ending-background-v1.png
```

Import Settings:

- Texture Type = `Sprite (2D and UI)`;
- Sprite Mode = `Single`;
- Pixels Per Unit = `16`;
- Filter Mode = `Point`;
- mipmaps desactivados;
- Compression = `None`;
- 9-slice en USS: `24 px` por cada lado.

---

## Jerarquía y separación de contenido

Los PNG solo contienen decoración. No dibujar dentro:

- títulos;
- cuerpos;
- contador de Memorias;
- créditos;
- botones;
- texto de Continue;
- texto Restart.

El código UI Toolkit mantiene todos los textos para permitir:

- wrap;
- localización;
- ajuste dinámico;
- cambios de font asset;
- test en `640 x 360`.

---

## Aplicación en USS

Memorias:

```uss
.memory-discovery-panel {
    background-image: url("project://database/Assets/Visuals/UI/Memory/sprite-ui-memory-panel-background-v1.png");
    -unity-background-scale-mode: stretch-to-fill;
    -unity-slice-left: 24;
    -unity-slice-top: 24;
    -unity-slice-right: 24;
    -unity-slice-bottom: 24;
}
```

Carta:

```uss
#story-overlay-panel.story-overlay-panel-letter {
    background-image: url("project://database/Assets/Visuals/UI/Story/sprite-ui-story-letter-background-v1.png");
}
```

Resumen:

```uss
#story-overlay-panel {
    background-image: url("project://database/Assets/Visuals/UI/Story/sprite-ui-story-ending-background-v1.png");
}
```

Las reglas deben conservar los slices de `24 px`; no reemplazarlas por
`scale-to-fit`, que dejaría proporciones no deseadas al cambiar el tamaño.

---

## Medidas de UI

- panel Story: `420 px` mínimo, `620 px` máximo;
- panel Story: `220-320 px` de alto;
- padding: `14-28 px`;
- título Letter: `18 px`;
- título Ending: `16 px`;
- cuerpo: `15 px`, Pixelify Sans;
- contador: `14 px`, Press Start 2P;
- créditos: `13 px`;
- botones: `176 x 42 px`;
- panel Memoria: `72%` de ancho, máximo `620 px`.

---

## Validación

Comprobar a `640 x 360` y WebGL Desktop:

- esquinas intactas después de cambiar el tamaño;
- centro sin deformar el texto;
- título no invade bordes;
- botones nunca quedan fuera del panel;
- el papel no crea bajo el texto demasiado contraste;
- el Resumen mantiene jerarquía oscura y dorada;
- los tres backgrounds siguen legibles con fondos claros y oscuros.
