# MVP Sprint 4 - Pulido final, validación y entrega

- Inicio: 2026-11-23
- Objetivo: 2026-12-01
- Estado: 🟡 ACTIVO - último sprint del MVP
- Milestone de entrada: `0.1.1-proto`
- Milestone de salida: candidata final de Game Off 2026

## Objetivo

Cerrar la versión jugable existente sin ampliar el proyecto. Este sprint transforma
`0.1.1-proto` en una candidata estable para WebGL, Windows e itch.io.

No se añaden rooms, managers, mecánicas de combate, movimiento del jugador ni
infraestructuras nuevas. Una petición entra en el sprint solo si corrige un
bloqueo, mejora la claridad, elimina un riesgo de entrega o completa contenido
ya diseñado.

## Punto de partida

Sprint 3 entregó:

- loop completo Lobby → Hallway → Bedroom / Library → Basement → Forest;
- carta, tres finales y reinicio limpio al Lobby;
- cinco Memorias opcionales y sus sprites;
- HUD unificado con inventario, feedback, SHIFT, carta y Memorias;
- primeros assets de música y perfil base de WebGL / Windows.

Todo el código y el arte heredados se consideran baseline. Los cambios del
Sprint 4 deben ser conservadores y medibles.

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
- [ ] Añadir únicamente SFX de alto valor para interacción, SHIFT, error y carta.
- [ ] Comprobar que el audio inicia, se mantiene y no se duplica tras reiniciar.
- [ ] Confirmar el enrutado al mixer `Master / Music / Ambient / SFX`.
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

## Tareas MVP Sprint 4 - IDEAS (por organizar y priorizar)

- GRÁFICOS: mejorar Backgrounds, Interactables y demás elementos gráficos de cada Room.
- FUENTE: importar fuente retro para los textos, coherente con la estética del juego.
- FUENTE: buscar fuente retro o escalada a resolución adecuada para los textos del juego en Carta de Revelación y/o Memorias.
- INVENTARIO UI: mejorar apartado gráfico del inventario con sprites propios (ahora mismo son genéricos o placeholders).
- ITEMS: añadir sprites a cada Item para que se represente en el inventario.
- DEDICATORIA: añadir texto final con dedicatoria especial a Luki.
- ROOMS: mejorar transición entre Rooms.
- SFX: añadir (crear con Sonic Pi) los SFX base para el MVP, incluyendo interacciones, transiciones y eventos clave en cada Room.
- LINTERNA: añadir sistema de mecánica e iluminación de Room mediante Linterna, usando, como se había planeado, un sistema de Luz Global y un sprite light 2D para simular el efecto de la linterna alrededor del cursor del jugador.
- LUZ GLOBAL: implementar un sistema de iluminación global que afecte a todas las Rooms, permitiendo simular cambios de luz ambiental y reforzar la mecánica de la Linterna por cada una de las Rooms.
- MAIN MENU: diseñar sistema para Menú Principal, incluyendo opciones de inicio, configuración y salida del juego. De momento, para la Jam, podemos incluirlo en la misma escena Main que estamos usando para todo el juego.
- SFX MEMORIA RECORDADA: añadir efectos de sonido específicos para cuando una Memoria es recordada, reforzando la retroalimentación auditiva del jugador.
- INTERACTABLES: ampliar número de Interactables por Room para añadir más narrativa y más hotspots que investigar.
- GAMEPLAY: complicar un poco más el flujo de puzles para alargar un poco la duración del juego y aumentar el desafío para el jugador.
- CURSOR: crear un diseño personalizado para el cursor del jugador, coherente con la estética retro del juego y que se destaque sobre los elementos interactuables en cada Room.
- NOMBRE INTERACTABLES: podría aparecer el nombre del Interactable cuando el cursor del jugador pase sobre él, proporcionando retroalimentación visual y ayudando a identificar los elementos interactuables en cada Room.
- MEMORIAS: crear background específico para las Memorias, que sea diferente a otros backgrounds de ventanas/modales, reforzando la sensación de que el jugador está accediendo a un recuerdo especial.
- LUCES: en caso de añadir la Luz Global, añadir elementos de iluminación específicos en cada Room para reforzar la atmósfera y la narrativa visual del juego.
- INTRO: añadir introducción simple con algo de contexto inicial, con background único. Usar el mismo que podamos crear para el fondo del Main Menu.
- FOREST - LUCES: en caso de añadir Luz Global, añadir luces 2D Light parpadeando que vayan escalando tras cada avance del jugador en la Room, reforzando la sensación de progresión y tensión.