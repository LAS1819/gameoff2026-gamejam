# Game Off 2026 - HABITACIÓN / BEDROOM - SPRITES Y POSICIONAMIENTO

- Versión 0.1 - Layout de producción
- Creado: 10/11/2026
- Actualizado: 10/11/2026

---

## 1. Objetivo del documento

Este documento marca el punto en el que dejamos de hablar de la room como concepto y pasamos a la fase de producción artística.

Aquí definimos:

- qué sprites necesitamos;
- dónde van colocados;
- cómo se organizan en capas;
- qué elementos son props, qué interactuables y qué variaciones tienen entre realidades.

La intención es que sea suficiente para producir el art y dejarlo listo para Unity sin ambigüedad.

---

## 2. Resolución base

Usamos la misma base establecida en la guía visual general:

- resolución interna: 640 x 360
- formato: 16:9
- escala: x3 exacto para salida final
- cámara fija
- fondo único por room

Coordenadas recomendadas en escena:

- centro de la escena: (0, 0, 0)
- eje horizontal: desde -320 a +320
- eje vertical: desde -180 a +180

Se recomienda mantener la composición dentro de estos límites para evitar que los sprites se salgan del marco.

---

## 3. Estructura de capas

La room debe dividirse en capas simples y claras:

```text
Bedroom
├── Background
│   ├── BackgroundNormal
│   └── BackgroundOtherSide
├── Props
│   ├── WallDecor
│   ├── FloorTexture
│   ├── BedFrame
│   ├── Nightstand
│   ├── WindowFrame
│   ├── WardrobeBody
│   ├── DoorFrame
│   └── LampBody
├── Interactables
│   ├── Wardrobe
│   ├── Lamp
│   ├── Switch
│   ├── Nightstand
│   ├── Window
│   ├── HallwayExit
│   ├── LibraryKey
│   └── FlashlightTool
└── Effects
    ├── LightVolume
    ├── MonsterPresence
    └── RealityNoise
```

### Regla de capas

- Background: Sorting Order 0
- Props decorativos: Sorting Order 1
- Interactables: Sorting Order 2
- Effects: Sorting Order 3
- UI / overlays: fuera de la room, en UI

---

## 4. Composición espacial recomendada

La calidad de la room depende de que la composición se lea en un segundo.

### Layout principal

```text
┌────────────────────────────────────────────┐
│                                            │
│                 [Wall Decor]               │
│                                            │
│                                            │
│       [Window]           [Lamp / Switch]   │
│                                            │
│                                            │
│     [Bed]                [Wardrobe]        │
│                                            │
│             [Nightstand]                    │
│                                            │
│                [Door / Exit]               │
│                                            │
└────────────────────────────────────────────┘
```

### Distribución orientativa

- La cama debe quedar dominante en el lado izquierdo o centro-inferior.
- El armario debe vivir en un lado derecho, grande y reconocible.
- La ventana debe estar en la pared superior izquierda o central.
- La mesita debe quedar junto a la cama, en una zona cercana a la luz.
- La salida hacia el pasillo debe estar en el lado derecho o lateral inferior.

La room debe sentirse personal, no simétrica ni fría. Es un espacio vivido.

---

## 5. Sprites necesarios

Estos son los sprites mínimos para producir la room y dejarla jugable.

### 5.1 Backgrounds

#### BackgroundNormal

- tipo: fondo completo
- resolución recomendada: 640x360
- contenido: pared, cama a medio fondo, ventana, armario, suelo
- objetivo: que la room sea inmediatamente reconocible como dormitorio

#### BackgroundOtherSide

- tipo: versión distorsionada del mismo fondo
- mismo layout base
- cambia color, iluminación y presencia extraña
- debe conservar la geometría general para que el “cambio” sea emocional y no estructural

---

### 5.2 Props / fijo

#### WallDecor

- tamaño sugerido: 120x80 o similar
- usa una silueta simple de pósters/fotos/pequeños objetos colgados
- no requerir interactuar

#### BedFrame

- tamaño sugerido: 200x110
- debe leer muy rápido como cama
- puede ir como forma sólida con detalles mínimos
- si el asset está muy grande, conviene que el sprite tenga una base sólida y clara

#### Nightstand

- tamaño sugerido: 56x56
- pequeño y legible
- puede tener una lámpara encima o un objeto anecdótico

#### WindowFrame

- tamaño sugerido: 90x100
- debe ser evidente como ventana
- en Otro Lado se tapa con ladrillos o textura extraña

#### DoorFrame

- tamaño sugerido: 80x130
- puerta de salida hacia el pasillo
- debe ser distinguible de un simple bloque de pared

#### WardrobeBody

- tamaño sugerido: 100x120
- el sprite más importante de la room junto con la cama
- debe tener una silueta clara y sólida

#### LampBody

- tamaño sugerido: 32x32 o 40x48
- puede ser una lámpara de mesa, pared o de pie
- su valor visual es mayor si la luz es visible o si se puede encender con un efecto sutil

#### FloorTexture

- pantalla completa o tramo central del suelo
- sirve para anclar la habitación y dar sensación de profundidad

---

### 5.3 Interactables

#### Wardrobe

- sprite principal del armario interactuable
- tamaño: 100x120
- collider: proporcional a la silueta
- estado de normal / otro lado: mismo sprite base con variación de sombra o de “presencia”

#### Lamp

- sprite de lámpara o foco
- tamaño: 32x48
- collider: pequeño pero claro
- puede activarse con click o ser un punto de luz que se enciende

#### Switch

- sprite del interruptor
- tamaño: 16x16 o 20x20
- muy pequeño y muy legible
- ideal si se coloca junto a la lámpara

#### NightstandInteractable

- puede ser el mismo sprite que la mesita, o un relleno interactivo más claro
- no hace falta un sprite extra si la mesita ya es legible

#### Window

- interactuable/observación visual
- puede ser el mismo marco de la ventana, pero con hitbox clara
- en Otro Lado debe cambiar su estado visual y bloquearse

#### HallwayExit

- sprite de salida o puerta de paso
- tamaño medio
- identificable como salida de habitación

#### LibraryKey

- sprite de llave pequeña
- tamaño recomendado: 18x18 a 24x24
- debe leerse muy rápido en Otro Lado

#### FlashlightTool

- puede no necesitar un sprite en-room si es una herramienta de UI
- si se quiere mostrar como sprite en la room, debe ser un objeto muy pequeño y claro

---

## 6. Posicionamiento recomendado

Estas son posiciones orientativas en escena, listas para producir con comodidad.

### Layout base, valores aproximados

```text
BackgroundNormal: x=0, y=0, z=0
BackgroundOtherSide: x=0, y=0, z=0

WindowFrame: x=-190, y=55, z=0
BedFrame: x=-55, y=-30, z=0
Nightstand: x=115, y=-25, z=0
LampBody: x=145, y=35, z=0
WardrobeBody: x=208, y=-10, z=0
DoorFrame: x=260, y=-8, z=0
WallDecor: x=-75, y=95, z=0
FloorTexture: x=0, y=-120, z=0
```

### Interactables

```text
Wardrobe: x=205, y=-8, z=0
Lamp: x=145, y=35, z=0
Switch: x=185, y=22, z=0
Nightstand: x=108, y=-24, z=0
Window: x=-190, y=56, z=0
HallwayExit: x=260, y=-6, z=0
LibraryKey: x=220, y=12, z=0
```

### Escala sugerida

- Cama: 1.2 a 1.5 escala base
- Armario: 1.0 a 1.2 escala base
- Ventana: 0.9 a 1.0 escala base
- Mesita: 0.8 a 1.0 escala base
- Llave: 0.5 a 0.7 escala base
- Lámpara: 0.7 a 0.9 escala base

Estas proporciones son orientativas. Lo importante es que la room se lea en una sola mirada y que cada interactable tenga una silueta reconocible.

---

## 7. Diferencia entre Realidad Normal y Otro Lado

La diferencia debe ser clara pero no “destruir” la room.

### Normal

- colores cálidos y apagados;
- la ventana se ve normalmente;
- la lámpara llama la atención como foco de seguridad;
- la habitación se ve habitada o vivida;
- la clave no está necesariamente “a la vista” si queremos ofrecer un pequeño misterio.

### Otro Lado

- mismo layout base;
- más sombras y un frío tenue;
- ventana tapiada o bloqueada;
- armario más hostil;
- clave más visible o más “anómala”;
- la luz debe ser un recurso necesario y no solo decoración.

### Cambios mínimos recomendados

No hace falta cambiar toda la sala. Conviene hacer:

- un darkening general;
- cambio de tonos en la pared y suelo;
- oscuridad más agresiva en las esquinas;
- alteración de la ventana;
- luz de lámpara con tono más blanco o enfermizo;
- presencia de sombras/siluetas en el armario.

---

## 8. Producción para artistas y programadores

### Recomendación de asset naming

```text
Bedroom_Background_Normal
Bedroom_Background_OtherSide
Bedroom_Props_WallDecor
Bedroom_Props_Bed
Bedroom_Props_Nightstand
Bedroom_Props_WindowFrame
Bedroom_Props_Wardrobe
Bedroom_Props_DoorFrame
Bedroom_Props_Lamp
Bedroom_Interactable_Wardrobe
Bedroom_Interactable_Lamp
Bedroom_Interactable_Switch
Bedroom_Interactable_Window
Bedroom_Interactable_HallwayExit
Bedroom_Interactable_LibraryKey
```

### Recomendación de tamaño por sprite

- Background: 640x360
- Props grandes: 100x120 a 200x140
- Mesita: 56x56
- Lámpara: 32x48
- llave: 18x18 a 24x24
- ventana: 90x100
- salida: 80x130
- armario: 100x120

### Recomendación de pivots

- Background: pivot central
- Interactables: pivot basado en el centro visual del objeto
- Llave: pivot central
- Armario: pivot central o ligeramente inferior

---

## 9. Checklist para producción

### Assets mínimos

- [ ] Background Normal
- [ ] Background OtherSide
- [ ] Bed sprite
- [ ] Wardrobe sprite
- [ ] Nightstand sprite
- [ ] Window sprite
- [ ] Door exit sprite
- [ ] Lamp sprite
- [ ] Switch sprite
- [ ] Library key sprite
- [ ] Lighting effect / glow sprite

### Estado listo para Unity

- [ ] todos los sprites en una escala consistente;
- [ ] hitboxes claras para cada interactable;
- [ ] room legible en una sola mirada;
- [ ] versión OtherSide sigue siendo reconocible;
- [ ] la amenaza del armario se entiende sin necesidad de mucha explicación;
- [ ] la posición de los interactables no compite entre sí.

---

## 10. Conclusión

Con esto ya estamos en la fase de producción real: no hablamos de narrativa ni de arquitectura, sino de que el artista pueda crear los assets con una composición clara, tamaños coherentes y un layout resolutivo.

La Bedroom ya está lista para pasar de “idea” a “sprite”:

- tiene composición clara;
- tiene posiciones pensadas;
- tiene capas definidas;
- mantiene la misma geometría en ambas realidades;
- y deja claro qué elementos marcan la diferencia visual entre Normal y Otro Lado.

Es el punto justo antes de empezar a dibujar y posicionar en Unity.
