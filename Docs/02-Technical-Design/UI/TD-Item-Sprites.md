# TD-Item-Sprites - Iconos de inventario Sprint 4

- **Version:** 1.0
- **Creado:** 2026-11-23
- **Actualizado:** 2026-11-23
- **Autor:** @LAS1819
- **Estado:** Planificado para el Bloque 1

---

## Objetivo

Definir los sprites prioritarios de los Items que aparecen en el inventario y
establecer una referencia visual consistente por `item.id`.

Prioridad de arte:

1. Staff Key
2. Library Key
3. Worn Book
4. Floppy Disk
5. Basement Code
6. Valve Handle

La Linterna usa actualmente un icono separado y no debe duplicarse dentro del
inventario hasta que la mecánica vuelva a usar el item normal.

## Mapeo de items

| Item ID | Item | Origen actual | Sprite prioritario |
| --- | --- | --- | --- |
| `staff_key` | Staff Key | Lobby pickup | `sprite-ui-item-staff-key-v1.png` |
| `library_key` | Library Key | Bedroom wardrobe / Other Side | `sprite-ui-item-library-key-v1.png` |
| `worn_book` | Worn Book | Library | `sprite-ui-item-worn-book-v1.png` |
| `floppy_disk` | Floppy Disk | Hidden in Worn Book | `sprite-ui-item-floppy-disk-v1.png` |
| `basement_code` | Basement Code | Bedroom computer | `sprite-ui-item-basement-code-v1.png` |
| `valve_handle` | Valve Handle | Personal Effects Box | `sprite-ui-item-valve-handle-v1.png` |

## Ubicación prevista

Usar una feature folder dentro de UI:

```text
Assets/Visuals/UI/Inventory/
```

No mezclar los iconos con backgrounds de Rooms ni con los spritesheets de
Memorias. Mantener un solo PNG por item.

## Convención visual

- canvas transparente;
- imagen final `48 x 48` px;
- silueta identificable a baja resolución;
- alto contraste frente al fondo oscuro del HUD;
- padding interno de al menos 4 px;
- sin texto dentro del sprite;
- fondo propio solo si mejora legibilidad;
- no usar bordes decorativos que parezcan estados del slot.

El sprite debe representar el objeto, no una ilustración de interfaz. Los
detalles finos pueden omitirse si dejan de verse a `640 x 360`.

## Import settings

Para cada PNG:

- Texture Type = `Sprite (2D and UI)`;
- Sprite Mode = `Single`;
- Filter Mode = `Point`;
- Compression = sin perdida en el primer test;
- mipmaps desactivados;
- ppu = `16`;
- preserve alpha;
- no recortar transparencias;
- evitar atlasing temporal hasta medir el resultado.

## Implementación

### Relación ID -> Sprite

El enfoque preferido mantiene la relacion visual dentro de `HUDController`
y se resuelve en `HUDInventoryView` por `item.id`.

No se amplía `InventoryItemData` ni se introduce un catalogo global de sprites:
los seis iconos caben en referencias serializadas y el item ya conserva su ID
como clave estable.

### Asignación por item

Las rutas de construcción actuales son:

- pickups genericos en `InteractionResolver`;
- Worn Book y Valve Handle en `InteractionItemFactory`;
- Basement Code en `BedroomComputerController`;
- Floppy Disk en `HUDInventoryView` al inspeccionar el Worn Book.

La asignación visual debe evitar cambiar estas rutas. Puede resolverse de
forma comun por `item.id` al refrescar el slot, sin modificar flags o puzzle
logic.

## Fallback

El fallback existente es una inicial o abreviatura corta:

- K para Staff Key y Library Key;
- B / W / D / V segun los demas items.

Si el sprite falta:

- el slot conserva el estado `full`;
- el label fallback permanece legible;
- selección e inspección siguen disponibles;
- no se muestra un icono roto.

## Accesibilidad

El icono no sustituye por completo al nombre del item:

- mantener el fallback textual;
- conservar feedback al pickup;
- conservar feedback al seleccionar;
- conservar descripción al inspeccionar.

El color no debe ser el único indicador de estado; el borde y la clase
`selected` deben seguir visibles.

## Validación

Validar cada item tras recibirlo:

- Staff Key: Lobby -> slot -> Front Door;
- Library Key: Bedroom -> slot -> Hallway Library Door;
- Worn Book: Library -> slot -> hidden disk;
- Floppy Disk: inspect Worn Book -> slot;
- Basement Code: Bedroom computer -> slot -> code panel;
- Valve Handle: Personal Effects Box -> slot -> pressure valve.

Capturar `640 x 360` con:

- empty slot;
- full slot;
- selected slot;
- fallback sin sprite;
- inventario con 3 items.

## Criterio de corte

No retrasar el HUD si falta algun icono final. El item sigue funcionando con
placeholder o label fallback. Solo la claridad real obtenida por un sprite
valido justifica reemplazar el arte previo.
