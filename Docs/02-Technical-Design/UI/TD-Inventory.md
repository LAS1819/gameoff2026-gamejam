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
