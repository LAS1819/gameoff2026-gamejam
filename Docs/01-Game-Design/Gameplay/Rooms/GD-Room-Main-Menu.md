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
- el cartel `HORIZON` se considera encendido;
- la luz global aumenta la intensidad y mezcla un tono amarillo cálido sobre
  la base neutral de la Room;
- se abre el modal de introducción.

El texto introductorio debe ser breve y accionable por `Next` / `Continue`.
No debe duplicar la Carta final ni iniciar mecánicas nuevas.

El texto se muestra en dos páginas para mantener legibilidad:

**Página 1 — Después de seis meses:**

> `For six months, I believed my father had disappeared and would return to explain everything.`
>
> `Today, a message arrived in his handwriting.`

**Página 2 — La invitación:**

> `It asked me to meet him inside the building marked HORIZON.`
>
> `I came here to find answers. I have not yet understood that I came to say goodbye.`

El primer botón dice `Next` y el segundo `Continue`.

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

En `0.1.6-proto`, `Settings` no introduce un sistema global nuevo.

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

## 6. Inicio limpio y Reinicio

El arranque normal carga `Main Menu` y limpia flags/runtime.

El Restart desde el final mantiene el flujo acordado:

```text
Menu -> Intro -> Lobby -> ... -> Ending -> Restart -> Main Menu
```

Al seleccionar `Restart`, se fuerza el retorno al `Main Menu` para reiniciar el flujo de introducción y limpiar flags/runtime previos.

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
