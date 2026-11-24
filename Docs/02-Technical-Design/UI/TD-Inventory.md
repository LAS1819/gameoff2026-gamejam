# TD-Inventory - Inventario gráfico Sprint 4

- **Version:** 1.1
- **Creado:** 2026-11-23
- **Actualizado:** 2026-11-24
- **Autor:** @LAS1819
- **Estado:** 6 iconos, background y estados gráficos de slots implementados

---

## Objetivo

Reemplazar los placeholders visuales de los tres slots del inventario sin
cambiar el comportamiento actual de selección, inspección y eliminación.

El sistema debe seguir usando `PanelRenderer + UXML + USS + HUDInventoryView`;
no requiere una escena nueva, un manager nuevo ni migración a ScriptableObjects.

## Estado actual

La UI ya existe en:

- [HUD.uxml](../../../Assets/UI/HUD.uxml);
- [HUD-style-sheet.uss](../../../Assets/UI/HUD-style-sheet.uss);
- [HUDInventoryView.cs](../../../Assets/Scripts/UI/HUDInventoryView.cs);
- [InventoryManager.cs](../../../Assets/Scripts/Inventory/InventoryManager.cs).

El HUD contiene tres slots:

```text
inventory-panel
├── slot-0
├── slot-1
└── slot-2
```

Cada slot usa:

- clase `empty` o `full`;
- clase `selected` al seleccionar;
- una etiqueta `slot-label`;
- left click para seleccionar;
- right click o context click para inspeccionar.

Actualmente están conectados por `item.id`:

- `staff_key` -> icono gráfico;
- `library_key` -> icono gráfico;
- `worn_book` -> icono gráfico;
- `floppy_disk` -> icono gráfico;
- `basement_code` -> icono gráfico;
- `valve_handle` -> icono gráfico;
- cualquier ID sin sprite asignado -> fallback textual.

---

## Decisión técnica

### Representación visual

El inventario ya contiene exactamente tres slots creados en
[HUD.uxml](../../../Assets/UI/HUD.uxml):

```text
inventory-panel
├── slot-0
├── slot-1
└── slot-2
```

No se debe crear un nuevo slot por cada item. El código actual recibe los items
de `InventoryManager`, ocupa el primer slot libre y reutiliza el mismo
`VisualElement` cuando cambia el inventario.

Cada slot mantiene la misma estructura runtime:

La jerarquia runtime debe ser:

```text
inventory-slot
├── slot-icon
└── slot-label
```

Dentro de él:

- `slot-icon` muestra el sprite del item;
- `slot-label` se usa solo como fallback textual.

`slot-label` queda oculto cuando existe icono válido y se conserva como
fallback de nombre corto si falta el sprite.

### Qué debe crear el diseñador o artista

Crear seis PNG independientes y transparentes:

- Staff Key;
- Library Key;
- Worn Book;
- Floppy Disk;
- Basement Code;
- Valve Handle.

No hace falta crear:

- una malla de tres slots;
- una plantilla prefabricada por item;
- un GameObject o prefab por slot;
- un sprite separado para el estado vacío;
- un sprite separado para selección.

Los estados vacío, lleno y seleccionado se mantienen en `HUDInventoryView`
mediante USS y las clases `empty`, `full` y `selected`.

### Datos

No hace falta crear un catálogo nuevo ni ampliar `InventoryItemData` con
referencias a Assets. La opción recomendada mantiene el dato del item como
información de gameplay y resuelve su icono por `item.id` en la capa visual.

No se recomienda almacenar rutas de Assets como strings en el modelo runtime.
Las referencias de Sprite se mantienen serializadas en `HUDController` y se
entregan a `HUDInventoryView`.

### Asignación desde HUDController

`HUDController` puede seguir siendo el punto de configuración visual:

- serializar los Sprites prioritarios;
- pasarles una referencia o resolver al construir `HUDInventoryView`;
- no crear un IconManager ni un ScriptableObject por este bloque.

La asignación exacta se detalla en [TD-Item-Sprites.md](TD-Item-Sprites.md).

## Dimensión y layout

Usar como propuesta final `48 x 48 px` para cada slot e icono y `8 px` de
margen horizontal. Esta reducción mejora la separación visual sin cambiar el
número de slots ni modificar la lógica.

El panel completo usa un fondo oscuro, borde ámbar y padding interior para
mantener contraste sobre rooms con suelos claros.

Import settings de los iconos:

- Texture Type = Sprite (2D and UI);
- Filter Mode = Point;
- mipmaps desactivados;
- `16` pixels per unit, coherente con `PanelSettings`;
- preservar transparencia;
- evitar escalado no entero.

El slot usa `scale-to-fit` para que el sprite completo quede visible.

## Presentación aplicada

El tratamiento visual mantiene el minimalismo del HUD:

- panel principal: fondo violeta oscuro casi opaco;
- marco: ámbar suave;
- slot vacío: gris violeta;
- slot con item: borde marfil;
- slot seleccionado: fondo ocre y borde amarillo;
- icono: representación completa a `48 x 48 px`, usando un `Image` de UI
  Toolkit para respetar el tamaño nativo del sprite.

El panel usa ahora
`sprite-ui-inventory-background-v1.png` como background 9-slice. Los slots
siguen resueltos en USS y mantienen el fallback textual para items sin icono.

### Ajuste recomendado del marco

Mantener el tamaño fijo de los tres slots en `48 x 48 px` y aumentar el
espacio exterior del panel, no los slots. La configuración aplicada usa:

```uss
padding: 9px 12px;
```

Resultado aproximado:

- padding horizontal: `12 px`;
- padding vertical: `9 px`;
- separación entre slots: `8 px`;
- panel: aproximadamente `184 x 66 px`.

Este ajuste crea aproximadamente `6 px` adicionales en cada dimensión sobre
la composición anterior y evita que los marcos de los slots se superpongan al
9-slice decorativo del panel.

Para alternar rápidamente entre una ampliación menor o mayor, cambiar solo
el padding vertical y horizontal:

```text
+4 px: padding: 8px 10px
+6 px: padding: 9px 12px
+8 px: padding: 10px 12px
```

No cambiar `width`, `height`, `margin-right` o PPU de los slots para agrandar
el panel. El panel crece por su contenido y padding, mientras los slots siguen
siendo píxeles exactos y los iconos conservan `48 x 48 px`.

## Background decorativo del inventario

El background futuro pertenece únicamente a `#inventory-panel`. No debe
reemplazar ni dibujar `slot-0`, `slot-1` y `slot-2`: esos elementos siguen
siendo UI interactiva y conservan la lógica de selección, inspección y
fallback textual.

### Responsabilidades

El PNG decorativo puede incluir:

- placa trasera;
- marco exterior;
- esquinas, remaches o adornos;
- profundidad visual;
- pequeños detalles de textura.

No debe incluir:

- el dibujo de los tres slots;
- iconos de items;
- estados de selección;
- texto;
- elementos interactivos;
- sombras importantes sobre el contenido central.

Los slots deben poder cambiar de tamaño o estado sin obligar a redibujar el
background completo.

### Dimensiones y área segura

La base actual del panel es aproximadamente `180 x 64 px`, con una proporción
cercana a `2.8:1`:

```text
180 x 64 px
┌────────────────────────────────────┐
│  margen                            │
│  [ slot 0 ] [ slot 1 ] [ slot 2 ] │
│  margen                            │
└────────────────────────────────────┘
```

Crear el PNG inicial también a `180 x 64 px`. Reservar aproximadamente
`12 px` en los bordes y un área central limpia para los slots. El alto exacto
puede revisarse posteriormente si cambia el padding del panel, pero la
proporción debe mantenerse.

El background puede quedar a un solo pixel de color o textura en el centro
si el centro es una zona estirable. Los detalles que no deben deformarse deben
colocarse cerca de las cuatro esquinas.

### Import settings

Asset implementado:

```text
Assets/Visuals/UI/Inventory/sprite-ui-inventory-background-v1.png
```

Configuración inicial:

- Texture Type = `Sprite (2D and UI)`;
- Sprite Mode = `Single`;
- Filter Mode = `Point`;
- mipmaps desactivados;
- PPU = `16`, coherente con `PanelSettings`;
- compresión sin pérdida;
- transparencia preservada;
- bordes sin antialias en el píxel art.

### Técnica 9-slice

Un 9-slice divide el sprite en nueve regiones:

```text
┌─────────┬───────────────┬──────────┐
│ esquina │ borde sup.    │ esquina  │
├─────────┼───────────────┼──────────┤
│ borde   │ centro        │ borde    │
│ lateral │ extensible    │ lateral  │
├─────────┼───────────────┼──────────┤
│ esquina │ borde inf.    │ esquina  │
└─────────┴───────────────┴──────────┘
```

Las cuatro esquinas conservan su tamaño original. Los bordes se estiran en una
sola dirección y el centro puede crecer en ambas. Así el marco no se deforma
cuando el panel cambia de tamaño, aunque cambien sus márgenes o en el futuro se
añada un cuarto slot.

Para este asset, el primer valor recomendado es `12 px` por cada lado:

- left: `12`;
- top: `12`;
- right: `12`;
- bottom: `12`.

Estos valores no son un requisito artístico fijo. Deben elegirse según el ancho
del detalle de cada esquina: si un remache, curva o columna ocupa `8 px`,
normalmente el borde de slice debe ser igual o mayor que esos `8 px`.

### Dibujar el sprite para 9-slice

1. Dibujar el asset completo a `180 x 64 px`.
2. Colocar los ornamentos importantes dentro de las cuatro regiones de esquina.
3. Mantener un tramo repetible y simple entre la primera y segunda división.
4. Evitar detalles únicos o líneas verticales dentro del centro extensible.
5. Comprobar que el centro puede repetirse o ampliarse sin formar patrones
   discontinuos.
6. Cambiar provisionalmente el ancho del panel en una captura para comprobar
   que las esquinas conservan tamaño.
7. Volver al tamaño real del panel y validar el resultado a `640 x 360`.

Ejemplo conceptual:

```text
12 px              centro extensible              12 px
<---><--------------------------------------------><--->
```

### Configuración en UI Toolkit

Primero importar el PNG como Sprite y definir el slice en el Sprite Editor.
Después, aplicar el asset sobre `#inventory-panel`:

```uss
#inventory-panel {
    background-image: url("project://database/Assets/Visuals/UI/Inventory/inventory-background-v1.png");
    background-color: rgba(22, 20, 32, 0.28);
    bottom: 10px;
    -unity-background-scale-mode: stretch-to-fill;
    -unity-slice-left: 12;
    -unity-slice-top: 12;
    -unity-slice-right: 12;
    -unity-slice-bottom: 12;
}
```

En UI Toolkit, `-unity-slice-*` funciona en píxeles del sprite. Las esquinas
quedan fijas y el centro se extiende. `stretch-to-fill` permite que el panel
ocupe el rectángulo calculado por el layout; no se debe usar `scale-to-fit`
para este background porque dejaría bandas vacías si cambia la relación.

No declarar el background en UXML mediante un `style`; debe permanecer en USS
para que se pueda sustituir posteriormente sin tocar la lógica.

### Estado implementado

El asset actual mide `180 x 64 px`, usa PPU `16`, Filter Mode Point, sin
mipmaps y tiene los bordes 9-slice configurados a `12 px` por lado. La
asignación se realiza en `#inventory-panel`; no modifica los slots ni su
comportamiento.

### Convivencia con el estilo actual

Durante la transición:

1. mantener el color de fondo actual como fallback;
2. añadir el sprite;
3. reducir gradualmente la opacidad del fondo CSS;
4. conservar primero los tres slots CSS para no mezclar dos cambios visuales;
5. sustituir más adelante el estilo de slot por sprites, usando validaciones
   separadas para los estados `empty`, `full` y `selected`.

El PNG del panel no sustituye el borde del estado seleccionado. Ese borde debe
seguir siendo visible sobre o dentro del background.

### Validación del 9-slice

Comprobar:

- las cuatro esquinas conservan el mismo grosor al redimensionar;
- no aparecen líneas dobladas ni repetidas;
- el contenido central no invade el marco;
- el texto fallback sigue siendo legible;
- los tres slots y sus iconos no cambian de tamaño;
- el background mantiene nitidez en `640 x 360`;
- el panel sigue ocupando el rectángulo esperado sobre fondos claros y oscuros.

## Sprites gráficos de los slots

Los slots deben tratarse como una pieza de interfaz independiente del sprite
del item. La misma ranura conserva su estado visual y el contenido cambia
por `item.id`; no se debe dibujar un slot distinto dentro de cada icono.

### Proporción y tamaño

La jerarquía visual y lógica ya validada usa:

- slot: `48 x 48 px`;
- icono del item: `48 x 48 px`;
- separación horizontal: `8 px`;
- panel: `180 x 64 px` aproximadamente;
- relación del slot: `1:1`.

El asset implementado reúne los tres estados en un spritesheet horizontal de
`144 x 48 px`, con slices de `48 x 48 px` y fondo transparente:

```text
Assets/Visuals/UI/Inventory/Slots/spritesheet-ui-inventory-slots.png
```

Índices implementados:

```text
0 = Empty
1 = Full
2 = Selected
```

### Qué contiene cada PNG

#### Empty

- marco exterior completo;
- relleno vacío o transparente;
- superficie neutra sin símbolo de item;
- no incluir letras, números ni botones.

#### Full

- mismo marco y geometría que `Empty`;
- variación reconocible que indique que el slot contiene un item;
- fondo neutro detrás del futuro icono;
- no dibujar Staff Key, Library Key u otro item dentro de esta placa.

#### Selected

- mismo marco y geometría que los otros estados;
- borde, esquinas, brillo o fondo especial que indique selección;
- el estado Selected puede aplicarse sobre un slot Full;
- no dibujar una versión distinta del item seleccionado.

Mantener la geometría común de los tres PNG. Cambiar solo superficie, borde o
brillo evita que el slot parezca desplazarse al cambiar de estado.

### Jerarquía de renderizado

La estructura runtime debe mantener esta profundidad:

```text
inventory-panel
└── inventory-slot
    ├── slot-icon
    └── slot-label
```

Orden visual recomendado:

1. `background-image` del propio `inventory-slot`;
2. `slot-icon`;
3. `slot-label` como fallback superior o oculto.

El icono debe aparecer centrado dentro del marco. El fallback textual solo
debe mostrarse si no existe el Sprite del item.

No añadir el nombre del item dentro del sprite gráfico del slot. El nombre
permanecerá en datos, feedback e inspección.

### Rutas de assets

Usar una feature folder separada para estados de slot:

```text
Assets/Visuals/UI/Inventory/Slots/
```

Conservar los iconos por item en:

```text
Assets/Visuals/UI/Inventory/Items/
```

```text
Assets/Visuals/UI/Inventory/Slots/spritesheet-ui-inventory-slots.png
```

No mezclar slot backgrounds, item sprites y el panel 9-slice en una sola
imagen. El panel y los slots son elementos visuales independientes.

### Import settings de los slots

Para cada uno de los tres PNG:

- Texture Type = `Sprite (2D and UI)`;
- Sprite Mode = `Multiple`;
- Filter Mode = `Point`;
- mipmaps desactivados;
- PPU = `16`, coherente con `PanelSettings`;
- compresión sin pérdida;
- transparencia preservada;
- slicing horizontal con tres sprites de `48 x 48 px`;
- sin 9-slice, porque el estado se usa a tamaño fijo de `48 x 48 px`;
- no recortar transparencia;
- no generar Sprite Sheet salvo que se mida una necesidad real de batching.

### Integración futura en USS

La integración visual debe resolver los tres estados mediante las clases
existentes:

```uss
.inventory-slot.empty {
    background-image: url("project://database/Assets/Visuals/UI/Inventory/Slots/spritesheet-ui-inventory-slots.png#spritesheet-ui-inventory-slots_0");
}

.inventory-slot.full {
    background-image: url("project://database/Assets/Visuals/UI/Inventory/Slots/spritesheet-ui-inventory-slots.png#spritesheet-ui-inventory-slots_1");
}

.inventory-slot.selected {
    background-image: url("project://database/Assets/Visuals/UI/Inventory/Slots/spritesheet-ui-inventory-slots.png#spritesheet-ui-inventory-slots_2");
}
```

La clase `selected` debe tener suficiente especificidad para sustituir al
background de `full` sin cambiar clases ni datos:

```text
empty
full
full + selected
```

Para mantener el fallback CSS mientras el arte final no esté disponible:

1. conservar temporalmente `background-color` y `border-*`;
2. añadir `background-image`;
3. retirar el borde CSS porque el spritesheet incluye el marco completo;
4. mantener la forma `48 x 48` aunque el estado sea placeholder.

El estado implementado ya completó esta transición: el fondo y el borde CSS
están eliminados, y cada clase muestra su slice del spritesheet.

### Resultado validado

Tras introducir el spritesheet:

- slot vacío usa `_0`;
- slot lleno usa `_1`;
- slot lleno + seleccionado usa `_2`;
- cada slot mantiene `48 x 48 px`;
- cada icono visible mantiene `48 x 48 px`;
- el icono se alinea desde `0,0` porque el marco ya está dibujado en el slice;
- `slot-label` permanece disponible como fallback.

La actualización de clases ocurre en `HUDInventoryView.Refresh()`. UI Toolkit
puede requerir un pase de layout antes de recalcular `resolvedStyle`; la lógica
de producción no necesita esperar manualmente, el frame cambia en el siguiente
render normal.

No modificar `HUD.uxml` para añadir un nuevo estado por cada item.
`HUDInventoryView` continúa cambiando `empty`, `full` y `selected` sobre los
tres slots existentes.

### Capa del item sobre el slot

El `slot-icon` de cada slot debe continuar siendo una imagen independiente:

- estado vacío: `slot-icon` oculto;
- estado full: `slot-icon` visible;
- estado selected: `slot-icon` visible sobre el nuevo frame seleccionado;
- fallback: `slot-label` visible solo si no existe el icono.

El PNG del slot no debe incluir el dibujo del item. La combinación final es:

```text
slot frame PNG + item PNG + optional fallback label
```

Esto permite reutilizar Staff Key, Library Key, Worn Book, Floppy Disk,
Basement Code y Valve Handle sin crear variantes del frame para cada item.

### Validación visual de cada estado

Para el primer lote gráfico comprobar:

- empty a `48 x 48` sin recortes;
- full a `48 x 48` con el icono centrado;
- selected a `48 x 48` claramente distinguible;
- marco sin antialias ni escalado no entero;
- separación de `8 px` entre slots;
- contraste sobre el fondo del panel;
- fallback legible si falta el PNG del estado;
- tres items simultáneamente sin solapamientos;
- resultados a `640 x 360` y en el embed WebGL.

---

## Dirección artística: Compartimentos de expediente

Los slots representan pequeños compartimentos físicos integrados en la placa principal del inventario. No deben interpretarse como bolsas, cofres, espacios mágicos ni ranuras tecnológicas.

**Narrativamente representan:**

- evidencias archivadas;
- recuerdos preservados;
- objetos importantes retenidos temporalmente por el protagonista;
- pruebas relacionadas con la verdad que intenta reconstruirse durante la aventura.

Visualmente deben parecer una extensión natural de la placa del sanatorio.

### Geometría compartida

Los tres estados deben conservar exactamente: 48 x 48 px

Estructura común:

```text
┌────────────┐
│            │
│            │
│            │
│            │
└────────────┘
```

**Componentes permanentes:**

- **marco exterior metálico;**
- **esquinas ligeramente desgastadas;**
- **profundidad interior;**
- **sombras internas consistentes;**
- **misma silueta en los tres estados.**

**No modificar:**

- **tamaño;**
- **grosor del marco;**
- **posición de sombras;**
- **área útil para iconos.**

### Estado EMPTY

**Objetivo visual:** Comunicar "Este compartimento todavía no contiene nada."

**Apariencia:**

- Interior oscuro
- Superficie vacía
- Profundidad visible
- Tonos marrón oscuro y gris envejecido
- Textura muy ligera

**Referencia conceptual:**

```text
┌────────────┐
│▒▒▒▒▒▒▒▒▒▒▒▒│
│▒          ▒│
│▒          ▒│
│▒          ▒│
│▒▒▒▒▒▒▒▒▒▒▒▒│
└────────────┘
```

**Sensación:**

- Archivador vacío
- Hueco preparado para recibir una evidencia.

---

### Estado FULL

**Objetivo visual:** Comunicar "Existe un objeto almacenado en este compartimento."

**Apariencia:** Misma geometría que Empty.

**Cambios permitidos:**

- Fondo ligeramente más claro
- Superficie interior suavemente iluminada
- Textura parecida a papel envejecido
- Sensación de objeto depositado

**Referencia conceptual:**

```text
┌────────────┐
│▓▓▓▓▓▓▓▓▓▓▓▓│
│▓          ▓│
│▓          ▓│
│▓          ▓│
│▓▓▓▓▓▓▓▓▓▓▓▓│
└────────────┘
```

**Importante:** el usuario debe identificar el estado Full incluso si el icono tarda unos fotogramas en aparecer.

---

### Estado SELECTED

**Objetivo visual:**

Comunicar:

"Este es el objeto actualmente activo."

**Apariencia:**

Mantener la misma geometría.

**Añadir únicamente:**

- **borde interior iluminado;**
- **esquinas enfatizadas;**
- **reflejo metálico suave.**

**Referencia conceptual:**

```text
╔════════════╗
║            ║
║            ║
║            ║
╚════════════╝
```

**Color recomendado:**

**No usar:**

azul RPG;
verde tecnológico;
neón moderno.

**Usar:**

- **latón envejecido**
- **ocre claro**
- **dorado antiguo**

## Inspiración

La luz de una linterna iluminando una evidencia encontrada.

### Paleta recomendada

- **Marco**
  - #2A2724
  - #3A3530
  - #514941

- **Interior Empty**
  - #1E1B19
  - #262220

- **Interior Full**
  - #4C433B
  - #66584D

- **Selected**
  - #B79A62
  - #D4B77A


**Coherente con la Realidad Normal del proyecto:**

- **marrones apagados;**
- **grises;**
- **amarillos envejecidos;**
- **decadencia hospitalaria.**
**Reglas de producción**
**Mantener**

✅ borde de 2 px
✅ profundidad interna
✅ lectura clara a 640×360
✅ icono centrado
✅ apariencia de compartimento físico

**Evitar:**

❌ cruces médicas
❌ símbolos RPG
❌ botones modernos
❌ texto incrustado
❌ indicadores numéricos
❌ grandes efectos de brillo
❌ cambiar la silueta entre estados

**Resumen visual:**

```text
EMPTY
┌──────────┐
│ oscuro   │
└──────────┘

FULL
┌──────────┐
│ iluminado│
└──────────┘

SELECTED
╔══════════╗
║  brillo  ║
╚══════════╝
```

La filosofía es muy simple:

El panel completo es una placa del sanatorio.

Los slots son compartimentos de expediente.

Los objetos son evidencias.

El estado seleccionado es la evidencia que el protagonista está examinando ahora mismo.

---

## Estados

### Empty

- fondo placeholder discreto;
- borde visible;
- sin icono;
- sin label;
- picking mode Ignore.

### Full

- icono o fallback;
- borde activo;
- left click disponible;
- right click de inspección disponible.

### Selected

- conserva el icono;
- aplica la clase `selected`;
- el feedback confirma el item seleccionado.

## Fallback

Si falta un sprite:

- el slot permanece `full`;
- se conserva la inicial o abreviatura actual;
- la selección e inspección siguen funcionando;
- no se elimina el item del inventario.

Si falta el label por algún motivo:

- el slot conserva fondo, borde y estado seleccionado;
- el item sigue disponible para selección e inspección.

## Implementación

1. Serializar los Sprites prioritarios en `HUDController`.
2. Pasar las referencias necesarias al construir `HUDInventoryView`.
3. Resolver el icono por `item.id` al refrescar el slot.
4. Reusar los slots existentes y no cambiar el numero ni el layout.
5. Crear o resolver `slot-icon` desde `HUDInventoryView`.
6. Mantener `slot-label` como fallback.
7. No cambiar eventos, flags ni consumo de items.
8. Mantener el fallback textual para IDs futuros.
9. Actualizar [TD-HUD.md](TD-HUD.md) con el estado final del inventario.

## Validación

Probar:

- slots vacíos;
- un, dos y tres items;
- slot seleccionado;
- fallback sin sprite;
- left click;
- right click;
- pickup y removal;
- reset desde Restart;
- resolución `640 x 360`;
- embed WebGL `640 x 360`.

## Criterio de done

El inventario pasa si:

- los tres slots son legibles;
- los iconos no cortan ni deforman;
- el fallback no bloquea acciones del item;
- el reset limpia slots e iconos;
- no se crean cuellos de botella ni managers nuevos.
