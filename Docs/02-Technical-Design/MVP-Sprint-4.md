# MVP Sprint 4 - Pulido final, validación y entrega

- Inicio: 2026-11-23
- Objetivo: 2026-12-01
- Estado: 🟡 ACTIVO - último sprint del MVP
- Milestone de entrada: `0.1.1-proto`
- Milestone de salida: `0.1.4-proto`

## Objetivo

Cerrar la versión jugable existente sin ampliar el proyecto. Este sprint transforma
`0.1.3-proto` en la candidata `0.1.4-proto`, estable para WebGL, Windows e itch.io.

No se añaden rooms, managers, mecánicas de combate, movimiento del jugador ni
infraestructuras nuevas. Una petición entra en el sprint solo si corrige un
bloqueo, mejora la claridad, elimina un riesgo de entrega o completa contenido
ya diseñado.

Las ideas visuales y de presentación son invitadas al sprint, pero el sistema
base se congela antes de añadir cualquier extensión experimental.

## Punto de partida

Sprint 3 entregó:

- loop completo Lobby → Hallway → Bedroom / Library → Basement → Forest;
- carta, tres finales y reinicio limpio al Lobby;
- cinco Memorias opcionales y sus sprites;
- HUD unificado con inventario, feedback, SHIFT, carta y Memorias;
- primeros assets de música y perfil base de WebGL / Windows.

Todo el código y el arte heredados se consideran baseline. Los cambios del
Sprint 4 deben ser conservadores y medibles.

## Ruta de implementación por bloques

Comenzar siempre por el bloque con estado más bajo. No abrir el siguiente hasta
cerrar los criterios marcados con **Gate**. Cada bloque debe terminar con una
captura o vídeo corto para comprobar el cambio frente a la baseline.

### Bloque 0 - Congelar y diagnosticar

- **Rama recomendada:** `develop`
- [x] Crear una build WebGL candidata.
- [x] Crear una build Windows x64 candidata.
- [x] Jugar el recorrido completo sin trucos ni ayuda de depuración.
- [x] Registrar bloqueos, errores de consola y pasos que no sean claros.
  - Cuesta ver el Inventario y los Items sin tener gráficos propios.
- [ ] Corregir solo bugs P0 que impidan jugar, reiniciar o salir al lobby.
- [ ] Guardar una captura del Lobby, HUD y Ending como referencia.

**Gate:** el loop pasa de Lobby a Ending y Restart sin bloquear ni desincronizar
estado en Editor, WebGL y Windows.

### Bloque 1 - Fundaciones visuales compartidas

- **Rama recomendada:** `feat/s4-ui-foundations`
- **Estado:** ✅ COMPLETADO
- **Versión de salida:** `0.1.2-proto`
- [x] Importar y probar una fuente retro legible.
- [x] Sustituir los placeholders de slots e iconos prioritarios del inventario.
- [x] Añadir sprites propios a Staff Key, Library Key, Worn Book, Floppy Disk,
      Basement Code y Valve Handle.
- [x] Aplicar la fuente a HUD, Memorias y carta sin cortar texto.
- [x] Mantener contraste y fallback si una fuente o sprite todavía no está listo.
- [x] Actualizar documentación pertinente.
- [x] Actualizar Changelog.
- [x] PR hacia `develop` y después hacia `main`
- [x] Crear nuevas Builds, verificar funcionamiento en WebGL y Windows y subir a Itch.io junto con el Changelog nuevo.

**Gate:** HUD e inventario siguen siendo legibles a `640 x 360` y en el embed
WebGL `640 x 360`; ningún texto nuevo corta controles.

**Hallazgos:**

- **Karq - WebGL:**
  - *Sensación general:* "Está guapo, tío, muy emotivo si sabes de qué va realmente. Me gusta. La música queda genial, la verdad, acompaña muy bien la experiencia. Me ha costado un ratillo pasármelo eh? Buen puzle."
  - *Bugs/Sugerencias/Ideas encontrados:*
    - [x] **Memorias**: igual no es un bug, pero sólo se pueden consultar una vez las Memorias. Luego, ya no más. Además, desaparecen solas las ventanas de Memorias haciendo que el botón Continue tenga poco sentido y que puedas perder info, además.
      - *Solución Luxus:* bien visto, quizá sería mejor mantener las ventanas de Memorias abiertas hasta que el jugador decida cerrarlas, evitando así la pérdida de información y dando más sentido al botón Continue.
    - [x] **Memorias:** estarían bien que pudiesen volver a verse las Memorias. En estos momentos sólo se ve una vez y ya está. Esto mejora la experiencia y da más tiempo a apreciar los detalles de cada Memoria (gráfico y texto).
      - *Solución Luxus:* implementar la posibilidad de volver a consultar las Memorias al pulsar sobre el Interactuable que las contiene.
    - [x] **Cuarto de la Derecha (Bedroom)**: cuando inspeccionas la Ventana, el texto no se ve con el fondo blanco.
      - *Solución Luxus:* considerar cambiar el color del texto o añadir un fondo semitransparente al background del texto para mejorar la legibilidad.
    - [x] **Items de Inventario:** los objetos del inventario se tienen que poder desseleccionar pinchando otra vez (o algo por el estilo) porque a veces se queda seleccionado algo del inventario y "te raya" no poder dejar de usarlo.
      - *Solución Luxus:* permitir que los objetos del inventario se desseleccionen al hacer clic nuevamente sobre ellos o al interactuar con otro objeto, evitando así que el jugador quede bloqueado con un objeto seleccionado. NOTA: esto se había resuelto anteriormente, pero parece que no se ha solucionado en todos los Interactables.
    - [x] **Manivela:** he podido obtener la Manivela 2 veces, una vez puesta en la tubería, en la realidad alternativa se puede volver a recoger.
    - [x] **Monstruo del Sótano:** tras el game loop completo, no me ha vuelto a salir el Monstruo del Sótano. Aún así, cuando he instalado la manivela en la tubería, y activado la Otra Realidad, se ha visto cómo desaparecía a causa del vapor.
      - *Solución Luxus:* revisar la lógica de aparición y desaparición del Monstruo del Sótano para asegurar que se comporte de manera consistente en todas las realidades y tras completar el game loop.
    - [x] **UI Carta Revelación**: los textos de la Carta Revelación no se encuadran en el espacio destinado, causando que parte del texto se corte o quede fuera de la vista.
      - *Solución Luxus:* ajustar el layout de la Carta Revelación para que los textos se encuadren correctamente dentro del espacio disponible, asegurando que toda la información sea visible y legible.
    - [x] **UI Resumen Final**: los textos del Resumen Final no se encuadran correctamente en el espacio destinado, causando que parte del texto se corte o quede fuera de la vista.
      - *Solución Luxus:* ajustar el layout del Resumen Final para que los textos se encuadren correctamente dentro del espacio disponible, asegurando que toda la información sea visible y legible.
    - [x] **Interactable Carta Revelación:** ahora mismo, la Carta Revelación queda detrás del panel del Inventario, sólo se ve unos píxeles y es difícil de interactuar con ella.
      - *Solución Luxus:* subir la posición Y de los elementos de la Mesa y la Carta para que queden por encima del panel del Inventario, asegurando que sean completamente visibles e interactuables.
      - *Solución Luxus 2:* bajar unos píxeles (8 o así) la posición del Panel del Inventario para que no interfiera con la visibilidad de la Carta Revelación ni con la Personal Effects Box.
      - *Idea:* podríamos añadir un poco de transparencia al background del panel, pero como algo estético, no como solución al problema de visibilidad de la Carta Revelación.
    - [ ] **Linterna:** he encontrado la Linterna en la habitación, pensaba que servía para algo, pero no he tenido ocasión de saber para qué.
      - *Comentario Luxus:* tranquilo, la mecánica de la Linterna todavía no está implementada. Tenemos pendiente implementar un sistema de luz global para que la Linterna tenga que usarse en el Sótano, mediante uso de Sprites Light 2D alrededor del cursor del ratón.

### Bloque 2 - Interacción y feedback compartido

- **Rama recomendada:** `feat/s4-interaction-feedback`
- **Estado:** ✅ COMPLETADO
- **Versión final:** `0.1.3-proto`
- [x] Corrección de hallazgos de bugs encontrados en versión `0.1.2-proto`
  - [x] Inventario: seleccionar otra vez el mismo item deselecciona el item.
  - [x] Inventario: las acciones resueltas limpian la selección obsoleta.
  - [x] Manivela: el Personal Effects Box evita una segunda recolección.
  - [x] Feedback sobre fondos claros: fondo semitransparente aplicado a los textos.
  - [x] Memorias: el overlay permanece abierto hasta cerrar manualmente.
  - [x] Carta Revelación: layout flex relativo revisado para evitar cortes por resolución o loop.
  - [x] Resumen Final: layout flex relativo revisado para evitar cortes por resolución o loop.
  - [x] Carta Revelación y Mesa elevadas en Y y sobre la interferencia del inventario.
- [x] Añadir cursor personalizado con cursor sistema como fallback.
  - [x] Hook técnico disponible.
  - [x] Cursor personalizado implementado con fallback al sistema.
- [x] Mostrar el nombre del interactable solo al hacer hover, sin ensuciar la vista.
- [POSPUESTO] Revisar el fade de transición entre rooms.
- [POSPUESTO] Revisar el pulse de cambio de realidad.
- [x] Mantener el background visual del overlay de Memorias con panel legible y close manual.
- [x] Mantener Continue, timeout y cierre por Escape / Enter / Space.
  - [x] Continue cerrado por botón.
  - [x] Escape / Enter / Space disponibles en Memorias.
  - [x] Timeout eliminado de Memorias.
- [x] Añadidos backgrounds para ventanas de Memorias, Carta y Resumen Final.

**Gate:** hover, cursor, transiciones y Memorias funcionan con ratón y teclado,
sin ocultar hotspots ni bloquear la room.

### Bloque 3 - Menu e introducción ligeros

- **Rama recomendada:** `feat/s4-menu-intro`
- **Versión final:** `0.1.4-proto`
- [x] Tratar el Main Menu como una Room más dentro del flujo. Es la primera room antes del Lobby.
- [x] Reutilizar un único background compartido para menu e intro.
- [x] Añadir Start, Settings y Exit sin crear una escena nueva.
- [x] Implementar Settings como feedback dentro del flujo existente; se pospone configuración real.
- [x] Añadir una intro breve con Continue por ratón y teclado.
- [x] Diseñar con Aseprite y crear gráficos (v1) para el Background y los Interactables y Visuales.
- [x] Importar los gráficos diseñados en Aseprite al proyecto y verificar su correcta visualización.
- [x] Sustituir sprites de test con los gráficos importados desde Aseprite.
- [x] Comprobar que Start devuelve al estado inicial limpio.
- [x] Comprobar que Exit funciona en Windows, informa en WebGL y no pretende persistir estado.
- [x] Comprobar que Restart vuelve a Main Menu y limpia flags/runtime.
- [x] Comprobar hover/tint/scale en Start, Settings y Exit.
- [x] Comprobar que la Intro de dos páginas no solapa título y cuerpo.

**Gate:** el camino `Menu -> Intro -> Lobby -> Restart -> Main Menu` funciona.
Si Settings o Exit depende de infraestructura nueva, se recorta.

### Bloque 4 - SFX y mezcla base

- **Rama recomendada:** `feat/s4-sfx-base`
- **Versión final prevista:** `0.1.5-proto`
- [x] Confirmar plan sonoro y catálogo mínimo antes de producir clips.
- [x] Añadir cues para clic, SHIFT, transición, pickup/use y carta.
- [x] Añadir un cue específico al descubrir una Memoria.
- [x] Respetar `Master / Music / Ambient / SFX`.
- [x] Evitar duplicados al cambiar de room o reiniciar.
- [x] Ajustar volumen mediante volumen raíz, volumen por clip y mixer SFX.
- [x] Documentar producción en Sonic Pi y importación en Unity.

Plan técnico acordado:

- mantener `AudioManager` como fachada pública;
- mantener `MusicManager` para el tema principal existente;
- añadir `SFXManager` solo si se necesitan AudioSources reutilizables;
- añadir `AmbientManager` solo si hay varias pistas ambientales simultáneas;
- escuchar eventos existentes: `RoomChanged`, `RealityChanged`, `InteractionResolved`
  y descubrimiento de Memorias;
- no llamar `AudioSource.Play()` desde cada Room o Interactable;
- usar los cuatro grupos existentes del `MainAudioMixer`: `Master`, `Music`,
  `Ambient` y `SFX`;
- conservar el 2D audio y el loop de música por defecto;
- no crear ambientes por room antes de validar el base building drone;
- priorizar 12 SFX reconocibles y 2-4 ambientes reutilizables.

Documentación de referencia:

- [GD-Audio.md](../01-Game-Design/Audio/GD-Audio.md) - intención narrativa;
- [TD-Audio-System.md](./Audio/TD-Audio-System.md) - arquitectura vigente;
- [TD-SFX.md](./Audio/TD-SFX.md) - catálogo de efectos;
- [TD-Music.md](./Audio/TD-Music.md) - música y transiciones;
- [TD-Ambience.md](./Audio/TD-Ambience.md) - ambientes y realidad.

**Gate:** acciones críticas tienen audio coherente, el tema no se duplica y el
reinicio deja una mezcla limpia.

### Bloque 5 - Luz Global y mecánica de acceso con Linterna al Basement

- **Rama recomendada:** `feat/s4-flashlight-basement`
- [x] Definir perfiles de luz explícitos por Room y Realidad.
- [x] Añadir Global Light 2D a `Main.unity`.
- [x] Implementar controlador de iluminación sobre `RoomChanged` y `RealityChanged`.
- [x] Implementar estado separado `FlashlightActive` sin añadir batería ni inventario.
- [x] Añadir toggle de Linterna desde el icono del HUD y tecla `F`.
- [ ] Cursor-follow Light 2D diferido para una iteración posterior.
- [x] Desactivar automáticamente la Linterna al cambiar de Room o reiniciar.
- [x] Añadir `requiresLantern` a los interactables clave del Basement.
- [x] Bloquear `Use` y `Examine` oscuros antes de consumir items o activar flags.
- [x] Reutilizar `UiDenied` y `LightSwitch` sin crear cues nuevos.
- [x] Mostrar mensajes narrativos "demasiado oscuro" para caja, tuberías, estanterías y válvula.
- [x] Mantener Library/Forest exit operativas como navegación cuando sus flags correspondan.
- [ ] Validar transición Library -> Basement -> Library y acceso final al Forest.
- [ ] Validar Editor, WebGL y Windows a `640 x 360`.

Documentación técnica:

- [TD-Lighting-System.md](./TD-Lighting-System.md)
- [TD-Room-Basement.md](./Gameplay/Rooms/TD-Room-Basement.md)
- [GD-Room-Basement.md](../01-Game-Design/Gameplay/Rooms/GD-Room-Basement.md)
- [TD-HUD.md](./UI/TD-HUD.md)

**Gate:**

- Las Rooms tienen su propia iluminación independiente, controlada por el sistema de luz global. Variando un poco los parámetros de luz global (intensidad, color) según la Room y la Realidad, se puede mantener la coherencia visual sin perder referencias.
- La Linterna permite explorar el Basement sin perder referencias visuales.
- Los interactables clave no resuelven ni consumen items mientras están a oscuras.
- El estado activo no permanece encendido después de salir del Basement o reiniciar.

### Bloque 6 - Barrido visual de Lobby

- **Rama recomendada:** `art/s4-lobby-polish`
- [ ] Mejorar fondo y composición sin mover los hotspots.
- [ ] Revisar Staff Key, Front Door y Hallway Entrance.
- [ ] Revisar Memory 01 en Realidad Normal.
- [ ] Revisar menu e intro sobre el nuevo fondo.
- [ ] Validar entrada a Hallway y retorno por reinicio.

**Gate:** una captura Normal y una captura final deben ser legibles en un vistazo.

### Bloque 7 - Barrido visual de Hallway

- **Rama recomendada:** `art/s4-hallway-polish`
- [ ] Mejorar fondos Normal / Other Side.
- [ ] Revisar obstáculo, SHIFT y puertas de Bedroom / Library.
- [ ] Revisar hitboxes después de cualquier ajuste visual.
- [ ] Revisar Memory 02 en Other Side.
- [ ] Validar acceso de ida y regreso a Lobby, Bedroom y Library.

**Gate:** SHIFT cambia la percepción sin confundir el camino ni ocultar hotspots.

### Bloque 8 - Barrido visual de Bedroom

- **Rama recomendada:** `art/s4-bedroom-polish`
- [ ] Mejorar ordenador, torre, monitor y Floppy Disk.
- [ ] Revisar lámpara, interruptor, armario y monstruo.
- [ ] Revisar ventana y salida a Hallway.
- [ ] Revisar Memory 03 en Other Side.
- [ ] Validar la progresión completa y el retorno a Hallway.

**Gate:** luz, armario, llave y ordenador siguen claros en ambas realidades.

### Bloque 9 - Barrido visual de Library

- **Rama recomendada:** `art/s4-library-polish`
- [ ] Mejorar estantería principal, estantería secreta y Worn Book.
- [ ] Revisar Floppy Disk, panel de código y Basement Door.
- [ ] Revisar salida a Hallway.
- [ ] Revisar Memory 04 en Other Side.
- [ ] Validar Worn Book -> Disk -> código -> acceso a Basement.

**Gate:** el secreto y el panel son visibles sin instrucciones externas.

### Bloque 10 - Barrido visual de Basement

- **Rama recomendada:** `art/s4-basement-polish`
- [ ] Mejorar caja de efectos, válvula, manivela y tuberías.
- [ ] Revisar vapor, manifestación y estado Normal / Other Side.
- [ ] Revisar Memory 05 en Other Side.
- [ ] Revisar salida a Library y salida desbloqueada al Forest.
- [ ] Validar evidencia -> reparación -> vapor -> manifestación -> Forest.

**Gate:** todo el puzzle sigue resoluble sin recurrir a nombres internos o logs.

### Bloque 11 - Barrido visual de Forest

- **Rama recomendada:** `art/s4-forest-polish`
- [ ] Mejorar fondo y las cuatro capas por profundidad.
- [ ] Revisar la sala final, mesa, luz y carta.
- [ ] Revisar Story Overlay, tres finales y dedicatoria.
- [ ] Revisar el restart limpio al Lobby.
- [ ] Validar rangos `0-2`, `3-4` y `5`.

**Gate:** profundidad, carta y finales son legibles sin solapamientos.

### Bloque 12 - Lote experimental

- **Rama recomendada:** `feat/s4-forest-light-progress`
- [ ] No iniciar hasta cerrar el Linterna/Global Light del Bloque 5.
- [ ] Exponer una prueba aislada de luces 2D progresivas en Forest.
- [ ] Mantener la linterna ausente en Forest; no añadir una regla nueva por allí.
- [ ] No obligar luz 2D para resolver puzzle, carta o finales existentes.
- [ ] Mantener fallback visual sin Light 2D.

**Gate:** prueba aislada estable en WebGL y Windows. Si supera el corte del
29/11, crear una rama nueva por extensión: `feat/s4-forest-light-progress`.

## Prioridades

### P0 - Estabilidad y cierre de flujo

- [ ] Ejecutar el recorrido completo sin saltos ni ayudas de depuración.
- [ ] Verificar cada transición de ida, retorno y reinicio.
- [ ] Confirmar que las cinco Memorias solo se descubren una vez.
- [ ] Probar los rangos de final `0-2`, `3-4` y `5`.
- [ ] Corregir bloqueos, null references, objetos que no reciben clic y flags
      desincronizadas.
- [ ] Congelar correcciones no relacionadas cuando exista una alternativa segura.

### P1 - Pulido narrativo y de presentación

- [ ] Revisar copy final de Memorias, carta, tres despedidas y feedbacks clave.
- [ ] Confirmar que el relato mantiene el tono y no revela antes la conclusión.
- [ ] Sustituir o aprobar el tratamiento provisional de la carta antigua.
- [ ] Revisar ortografía, coherencia terminológica y nombres de final.
- [ ] Resolver la presentación final de créditos: línea en el overlay o escena
      breve dedicada, sin duplicar sistemas de restart.
- [ ] Incluir autoría y atribuciones reales de código, arte, música y fuentes.

### P1 - Pulido de escenas y UI

- [ ] Validar posiciones e hitboxes de las cinco Memorias en la escena final.
- [ ] Validar el tamaño y la legibilidad de todos los textos a `640 x 360`.
- [ ] Revisar capa visual, sorting, foco de interacción y feedback en las seis rooms.
- [ ] Verificar que el Story Overlay no corta texto ni controles.
- [ ] Verificar que el overlay de Memoria admite Continue y timeout sin bloquear el
      jugador.
- [ ] Revisar contraste de HUD, cursor, hover y estados de click.
- [ ] Evitar pixel art borroso, escalados no enteros y transparencias inconsistentes.

### P1 - Audio

- [ ] Validar volumen relativo entre música, UI y futuros SFX.
- [x] Añadir únicamente SFX de alto valor para interacción, SHIFT, error y carta.
- [x] Comprobar que el audio inicia, se mantiene y no se duplica tras reiniciar.
- [x] Confirmar el enrutado al mixer `Master / Music / Ambient / SFX`.
- [ ] Si no hay SFX final, dejar mezclado de forma limpia con la música existente;
      no posponer la entrega por un sistema de audio nuevo.

### P1 - Accesibilidad y localización

- [ ] Confirmar lectura legible sin depender solo del color.
- [ ] Validar que las acciones principales funcionan con ratón y teclado disponible.
- [ ] Probar texto esencial sin cortes en inglés.
- [ ] Limitar avisos de contenido y controles a información visible en itch.io.
- [ ] Español: implementar solo si Unity Localization y la revisión completa caben
      dentro del plazo sin poner en riesgo WebGL y Windows.
- [ ] Si no se completa localización, documentar inglés como idioma de entrega;
      no mezclar idiomas dentro del runtime.

### P0 - QA y performance

- [ ] Playtest nuevo de cada puzzle, sin conocimiento interno del proyecto.
- [ ] Repetir cada transición de realidad después de abandonar y volver a la room.
- [ ] Confirmar que Restart elimina flags, inventario, profundidad de Forest y
      overlays.
- [ ] Probar carga fría y reinicio repetido en WebGL y Windows.
- [ ] Revisar consola, log del Player y warnings del build.
- [ ] Comprobar que no hay controles fuera del viewport o clicks bloqueados por UI.
- [ ] Confirmar que la música no rompe navegación ni reinicio.

### P0 - Builds

- [ ] Limpiar o excluir assets temporales que no entren en la entrega.
- [ ] Generar WebGL de release desde el perfil `Web - Desktop - Release`.
- [ ] Generar Windows x64 de release desde el perfil `Windows`.
- [ ] Mantener escenas de build en `Main.unity`.
- [ ] Validar WebGL a `960 x 540` y embed 16:9.
- [ ] Validar Windows a `1920 x 1080`, modo ventana, ratio 16:9 y ventana no
      redimensionable.
- [ ] Probar ambas builds en una copia limpia, no solo desde el Editor.

### P0 - itch.io y cierre

- [ ] Preparar portada, screenshots, descripción sin spoilers y controles.
- [ ] Mostrar aviso de contenido y versión de la build.
- [ ] Subir WebGL y Windows en un draft privado.
- [ ] Probar la descarga ZIP y el embed en incógnito.
- [ ] Completar el flujo de Lobby a Ending desde itch.io.
- [ ] Verificar créditos, licencia, enlaces al repositorio y metadatos.
- [ ] Publicar solo tras completar el checklist de entrega.
- [ ] Actualizar README, changelog y guías de build con el resultado real.

## Matriz mínima de aceptación

| Caso | Resultado requerido |
| --- | --- |
| Lobby → Hallway | Llave, puerta, transición y SHIFT estables |
| Hallway → Bedroom | Armario, monstruo, llave y ordenador estables |
| Hallway → Library | Libro, disquete, panel y Basement estables |
| Bedroom / Library cruzado | No se pierde la progresión al alternar rooms |
| Basement | Caja, manivela, presión, vapor y manifestación sincronizados |
| Forest | Profundidad 0-6, sala final, carta y salida narrativa |
| Finales | Se alcanzan 0-2, 3-4 y 5 Memorias |
| Restart | Vuelve al Lobby con estado limpio |
| WebGL | Inicia y completa el loop desde itch.io |
| Windows | ZIP limpio inicia, juega y reinicia |

## Fuera de alcance

- nuevas rooms o finales;
- sistema temporal;
- batería, luz 2D o uso activo de linterna;
- combate, muerte, salud, cordura o IA;
- movimiento libre y cámara física;
- guardado persistente;
- colección o contador visible de Memorias durante el recorrido;
- migración completa a ScriptableObjects;
- nuevas dependencias si no resuelven un bloqueante de entrega.
- expansión de puzzles mediante nuevos objetos o cadenas que invaliden el loop
  ya probado;
- migrar el proyecto completo a ScriptableObjects.

## Lote experimental condicionado

- Luces 2D progresivas específicas del Forest.
- Nuevos puzles o rutas que alarguen el recorrido.
- Propagación de iluminación local a rooms fuera del Basement.

Estos elementos pueden entrar solo después de cerrar P0 y P1 si hay tiempo
realista para probarlos sin tocar el flujo base. La implementación debe ser
ligera, reutilizar `HUDController`, el evento de realidad y los resolvers
existentes, y no crear un manager nuevo por feature.

La mecánica de linterna del Bloque 5 es una fundación obligatoria para el alcance
planeado del sprint. Debe integrarse sin cambiar la secuencia existente del
puzzle del Basement ni obligar a completar una interacción nueva para salir al
Forest.

## Definition of Done

Sprint 4 se considera cerrado cuando:

- no quedan bloqueantes P0;
- el loop completo pasa en Editor, WebGL y Windows;
- la build privada de itch.io reproduce el mismo comportamiento;
- copy, créditos y controles públicos están revisados;
- el changelog identifica la candidata final;
- solo quedan mejoras opcionales que no impiden jugar o entregar.

## Regla de corte

Si una tarea de pulido no termina antes del cierre:

1. conservar una versión estable y probada;
2. retirar la rama incompleta, no un sistema ya validado;
3. documentar la limitación pública;
4. priorizar jugar, ver y entender el loop completo.

---

## Organización de las ideas propuestas

### P0 - No pueden retrasarse

Mantienen intacta la candidata jugable y la entrega:

- estabilizar el loop completo y los reinicios;
- resolver bloqueos, clics inválidos y desincronización de flags;
- validar Memorias y los tres finales;
- generar y probar builds WebGL / Windows;
- probar el draft privado de itch.io;
- actualizar copias, changelog y metadatos necesarios.

Estas tareas tienen prioridad sobre toda mejora visual o experimental.

### P1 - Fundaciones compartidas

Deben completarse antes de producir mejoras Room por Room:

| Entrega | Implementación preferida | Criterio de corte |
| --- | --- | --- |
| Fuente retro | Una familia legible para HUD, Memorias y carta | Reemplazar después si afectase legibilidad |
| Inventario gráfico | Sprites propios en el sistema de slots existente | Mantener slots funcionales aunque falte algún icono |
| Item sprites | Referencia visual por item existente | Priorizar Staff Key, Library Key, Worn Book, Disk, Code y Valve Handle |
| Transiciones de Room | Fade/pulse reutilizado por `HUDController` | No crear un transition manager |
| Cursor personalizado | Textura global o `Cursor.SetCursor` con fallback | Mantener cursor sistema si no aporta claridad |
| Nombre de interactable | Tooltip por hover alimentado por el Interactable actual | Mostrar solo si no causa ruido visual |
| Background de Memoria | Capa visual dentro de `HUDMemoryDiscoveryView` | Mantener el overlay actual si el artwork retrasa |
| Menu principal | Overlay UXML en `Main.unity`: Start, Settings, Exit | Sin escena ni manager nuevo |
| Intro | Overlay breve reutilizable con background compartido del menu | Saltable y omitible si bloquea QA |
| SFX base | `AudioManager` / `MusicManager` + cues cortos en acciones | Probar SFX críticos antes que ambientación |
| SFX de Memoria | Un cue dentro del flujo de descubrimiento existente | Omitir si no existe clip final |
| Dedicatoria | Línea final en Ending / créditos | Texto corto y sin alterar el layout |

No se necesita migración a ScriptableObjects para estos elementos.

### P1 - Mejora por Room

El trabajo visual se realizará después de estabilizar el flujo compartido y en
este orden:

| Room | Prioridad de mejora | Validación obligatoria |
| --- | --- | --- |
| Lobby | Fondo, Staff Key, puerta, entrada, Memory 01, intro/menu | Inicio limpio, entrada a Hallway y Memory 01 |
| Hallway | Fondo Normal/Other Side, obstáculo, SHIFT, Memory 02 | Cambio de realidad y acceso Bedroom/Library |
| Bedroom | Ordenador, armario, monstruo, luz, ventanas, Memory 03 | Puzzle completo y retorno a Hallway |
| Library | Estantería, Worn Book, panel, Memory 04 | Libro, disco, código y acceso a Basement |
| Basement | Caja, válvula, vapor, tuberías, manifestación, Memory 05 | Evidencia, reparación y salida a Forest |
| Forest | Profundidad 0-6, mesas/luz/carta, final | Los tres rangos de final y restart |

Cada Room debe cerrar con:

1. revisión visual Normal;
2. revisión visual Other Side;
3. revisión de hitboxes;
4. prueba de los interactables existentes;
5. revisión del texto y feedback;
6. captura de referencia antes de pasar a la siguiente Room.

### P2 - Lote experimental

Solo debe implementarse tras confirmar que P0 y P1 están en verde:

- elementos específicos de iluminación por Room;
- luces 2D progresivas del Forest;
- 1-2 hotspots narrativos adicionales por Room;
- ajuste ligero de dificultad o pista adicional.

La extensión debe usar el pipeline URP 2D ya presente. Debe existir un MVP
intermedio: primero una habitación y una realidad, después una segunda prueba,
y solo entonces propagación al resto.

El Bloque 5 valida primero la linterna dentro del Basement. Solo después se
considera propagación visual; no convertirla en una migración global sin una
prueba estable en WebGL y Windows.

### P2 - Probablemente se recorte

- añadir más puzles necesarios para avanzar;
- multiplicar hotspots de lujo sin mejora narrativa clara;
- iluminación compleja en todas las Rooms;
- transición entre cada Room con animaciones largas;
- escenas separadas para menu, intro o créditos;
- opciones de configuración que exijan persistencia externa.

Complicar el flujo principal no alarga de forma fiable el juego; puede romper
la jugabilidad de una semana de test y duplicar casos de QA.

---

## Procedimiento de implementación óptimo

### 1. Congelar baseline

Antes de tocar arte o features nuevas:

- crear una build WebGL y una Windows candidata;
- ejecutar el loop completo;
- registrar los bloqueos reales;
- corregir solo errores que impidan jugar o entregar.

No se permite empezar mejoras gráficas de una Room si el loop aún tiene un bug
P0.

### 2. Completar fundaciones compartidas

Implementar una sola vez:

- fuente;
- catálogo de iconos del inventario;
- cursor;
- hover de nombre;
- estilo de memoria;
- transiciones;
- cues SFX comunes;
- menu e intro ligeros dentro de `Main.unity`.

Estas fundaciones deben entrar sin cambiar las reglas de Room ni las flags.

### 3. Barrido Room por Room

El orden recomendado es:

```text
Lobby -> Hallway -> Bedroom -> Library -> Basement -> Forest
```

Este orden sigue la progresión real y permite reutilizar patrones aprobados
hacia adelante. Nunca desarrollar una room más avanzada sin validar su entrada
desde la room anterior.

### 4. Congelar contenido por iteración

Después de cada Room:

- guardar una captura Normal;
- guardar una captura Other Side;
- ejecutar su puzzle completo;
- cerrar la lista de esa Room;
- no volver a editarla salvo por bug o repetición visual general.

### 5. Puerta de corte experimental

No iniciar linterna, Luz Global o puzles nuevos hasta pasar:

- [ ] loop completo estable en Editor;
- [ ] build WebGL estable;
- [ ] build Windows estable;
- [ ] P1 compartido aprobado;
- [ ] al menos Lobby, Hallway y Bedroom cerrados visualmente.

Si esta puerta no se alcanza antes del 29/11/2026, el lote experimental se
descarta y se mantiene el alcance de `0.1.1-proto`.

## Hitos propuestos

| Fecha | Hito |
| --- | --- |
| 23-24/11 | Baseline estable, bug bloqueantes P0 y build fría |
| 25-26/11 | Fuente, inventario, cursor, hover, memoria, menu e intro |
| 26-27/11 | Barrido Lobby y Hallway |
| 27-28/11 | Barrido Bedroom y Library |
| 28-29/11 | Barrido Basement y Forest |
| 29/11 | Puerta de corte: decidir lote experimental |
| 30/11 | QA final, builds candidatas y draft itch.io |
| 01/12 | Correcciones críticas, publicación y changelog final |

Si el equipo pierde un hito visual, conserva el orden y reduce cantidad de
rooms, no abandona los P0 de estabilidad y entrega.
