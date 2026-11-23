# TD-Inventory - Inventario gráfico Sprint 4

- **Version:** 1.0
- **Creado:** 2026-11-23
- **Actualizado:** 2026-11-23
- **Autor:** @LAS1819
- **Estado:** Implementación parcial (3 iconos conectados)

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
- resto de items -> fallback textual hasta completar sprites.

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

No se utiliza un sprite de background del slot: el estilo compartido se
resuelve en USS y mantiene el fallback textual para items sin icono.

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

Ruta recomendada:

```text
Assets/Visuals/UI/Inventory/inventory-background-v1.png
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
8. Completar iconos pendientes (`floppy_disk`, `basement_code`, `valve_handle`).
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
