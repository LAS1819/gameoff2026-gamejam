# TD-Music - Música principal y transiciones

- **Versión:** 1.0
- **Creado:** 2026-11-25
- **Estado:** Tema principal integrado; variaciones pendientes

---

## 1. Estado actual

Existe el clip:

```text
Assets/Audio/Music/father-theme-progressive-v1.mp3
```

`MusicManager`:

- lo reproduce en loop;
- usa el grupo `Music`;
- aplica pitch/volume distintos en Normal y Other Side;
- evita reiniciarlo si ya está reproduciendo;
- escucha cambios de realidad.

---

## 2. Reglas

- mantener una sola pista base en el MVP;
- no cargar temas completamente nuevos por Room;
- cambiar mezcla y pitch antes que componer piezas alternativas;
- no cortar la música al cambiar de room;
- no duplicar fuentes al reiniciar;
- detener y reiniciar solo en Ending/Restart si se valida el flujo.

---

## 3. Mezcla Normal / Other Side

Valores actuales:

```text
Normal:
- pitch: 1.00
- volume: 0.80

Other Side:
- pitch: 1.08
- volume: 0.90
```

La mezcla debe mantenerse sutil. El cambio debe sentirse como una
transformación de percepción, no como una versión hostil del tema.

---

## 4. Variaciones futuras

Componer solo si el tema base necesita diferenciarse más:

- `music_main_normal.ogg`;
- `music_main_otherside.ogg`;
- `music_forest_acceptance.ogg`;
- `music_ending.ogg`.

No crear todas al inicio. El tema actual puede conservarse y alternar mezcla.

---

## 5. Producción en Sonic Pi

El tema debe producirse como composición modular:

- 4-5 capas independientes;
- drone/bass estable;
- motivo melódico simple;
- ritmo mínimo;
- ruido industrial discreto;
- tail con fade natural.

Duración recomendada:

- loop base: `4-8 minutos`;
- intro: `1-2 minutos`;
- Ending: `2-3 minutos` si se crea una pieza separada.

---

## 6. Importación

- formato: MP3 para compatibilidad o OGG para tamaño;
- Load Type: `Streaming`;
- Preload: `false`;
- Spatial Blend: `0`;
- loop: `true`;
- group: `Music`.

---

## 7. Validación

- WebGL no corta ni reinicia en una transición de room;
- el cambio SHIFT no produce silencio;
- el pitch no produce artefactos audibles;
- el restart no deja dos temas activos;
- el volumen mantiene legibles los SFX críticos.
