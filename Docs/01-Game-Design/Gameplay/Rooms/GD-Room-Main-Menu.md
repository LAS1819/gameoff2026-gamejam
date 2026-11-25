# GD Room - Main Menu

- **Versión:** 1.0
- **Creado:** 2026-11-25
- **Estado:** Implementado como Room previa al Lobby

---

## 1. Propósito

El `Main Menu` es la fachada exterior del edificio. No se presenta como un
menú abstracto separado del juego: ocupa la primera Room visible y convierte
la puerta de entrada en el primer interactable narrativo.

Su objetivo es:

- presentar `Silent Feel` con una imagen única;
- ofrecer `Start`, `Settings` y `Exit`;
- explicar solo lo necesario antes de entrar;
- conservar el hábito de interactuar con objetos de las demás Rooms;
- conducir al `Lobby` sin una escena nueva ni un sistema paralelo.

---

## 2. Composición visual

La Room se divide en dos zonas:

```text
┌───────────────────────────────┬──────────────────────┐
│ Cartel HORIZON                │ Título                │
│                               │                       │
│        Puerta de entrada      │ Start                 │
│                               │ Settings              │
│                               │ Exit                  │
└───────────────────────────────┴──────────────────────┘
```

- La puerta y el cartel `HORIZON` ocupan la zona izquierda.
- El título y las tres acciones ocupan la pared derecha.
- La cámara mantiene la misma lógica fija de las Rooms jugables.
- El estado inicial del cartel es apagado.

---

## 3. Secuencia

### 3.1 Estado Menú

Al iniciar:

- se muestra el título;
- aparecen `Start`, `Settings` y `Exit`;
- la puerta está presente visualmente, pero todavía no permite entrar;
- el cartel `HORIZON` permanece apagado.

### 3.2 Start

Al pulsar `Start`:

- desaparecen `Start`, `Settings` y `Exit`;
- el título permanece como grafiti fijo en la pared;
- se abre el modal de introducción.

El texto introductorio debe ser breve, una sola pantalla y accionable por
`Continue`. No debe duplicar la Carta final ni iniciar mecánicas nuevas.

**Texto base provisional:** <- MEJORAR

El texto debe hacer referencia al motivo por el cual está allí. Y es porque el protagonista recibió un mensaje donde decía que debía reunirse en el edificio con el nombre Horizon. Revisar `\Docs\01-Game-Design\Story\GD-Story.md` para más contexto. Implementar "paso de página" en la ventana de introducción en caso de añadir más texto narrativo.

> `I return to the building where every light was switched off too soon.`
> `The entrance is still there, waiting for the explanation I never gave.`

El copy puede reemplazarse sin modificar flujo ni jerarquía.

### 3.3 Continuar introducción

Al pulsar `Continue`:

- se cierra el modal;
- vuelve a verse la Room sin menús;
- el cartel `HORIZON` cambia a encendido;
- la puerta se convierte en hotspot de transición.

`Continue` es compatible con ratón, Enter, Space y Escape por reutilizar el
control modal existente.

### 3.4 Entrada

Al clicar la puerta:

- comienza el HUD de gameplay;
- el juego carga el `Lobby`;
- no se conserva progreso previo;
- la realidad activa se fija en `Normal`.

---

## 4. Settings

En `0.1.4-proto`, `Settings` no introduce un sistema global nuevo.

Comportamiento MVP:

- muestra feedback: `Settings are not available in this prototype.`;
- mantiene visible el menú;
- no modifica flags ni progreso.

Si en una iteración posterior se añaden volumen o vídeo, deben configurarse
mediante el HUD y reutilizando este mismo interactable.

---

## 5. Exit

- En build Windows: cierra la aplicación.
- En WebGL: muestra feedback de que el navegador debe cerrar la pestaña; no
  se simula un cierre que la plataforma no permite.
- En Editor: solo muestra feedback, sin detener Play Mode.

No se guarda progreso. No se muestra una falsa persistencia al salir.

---

## 6. Inicio limpio y Reinicio [MODIFICAR MECÁNICA DE REINICIO]

NOTA: el reinicio del juego, es decir, cuando termina el juego y se selecciona `Restart` desde el Ending, debería volver al Main Menu, reiniciando el flujo de introducción y limpiando flags/runtime previos.

El arranque normal carga `Main Menu` y limpia flags/runtime.

El Restart desde el final mantiene el flujo acordado:

```text
Menu -> Intro -> Lobby -> ... -> Ending -> Restart -> Main Menu
```

Al seleccionar `Restart`, se debe forzar el retorno al `Main Menu` para reiniciar el flujo de introducción y limpiar flags/runtime previos.

---

## 7. Fuera de alcance

- perfiles o guardado;
- fullscreen/configuración gráfica;
- sliders de audio;
- animación de cámara;
- vídeo intro;
- botón de créditos;
- menú de pausa;
- estados de cursor específicos del menú.
