# Audio System Technical Design

## Resumen

El sistema de audio se encarga de gestionar todos los aspectos relacionados con la reproducción de sonidos, ambiente y música dentro del juego. Esto incluye la carga y reproducción de efectos de sonido, la gestión de pistas de música de fondo, la aplicación de efectos de audio y la coordinación con otros sistemas del juego para asegurar una experiencia auditiva coherente y envolvente.

## Componentes del Sistema de Audio

El sistema de audio se compone de varios módulos que trabajan en conjunto para gestionar la reproducción y manipulación del sonido:

- **Gestor de Audio (Audio Manager):** Supervisa y coordina todos los aspectos del sistema de audio, incluyendo la gestión de efectos de sonido, música y audio ambiental. Se asegura de que los diferentes módulos trabajen de manera coherente y eficiente.
- **Gestor de Efectos de Sonido (SFX Manager):** Se encarga de la carga y reproducción de efectos de sonido individuales, como disparos, pasos o interacciones del jugador.
- **Gestor de Música (Music Manager):** Administra las pistas de música de fondo, incluyendo la reproducción, pausa, cambio de pista y ajuste de volumen.
- **Gestor de Audio Ambiental (Ambient Audio Manager):** Controla los sonidos ambientales que contribuyen a la inmersión del jugador, como el viento, la lluvia o el ruido de la ciudad.
- **Sistema de Efectos de Audio (Audio Effects System):** Aplica efectos de audio como reverberación, eco o filtros de sonido para mejorar la experiencia auditiva.
- **Interfaz con Otros Sistemas:** Coordina con otros sistemas del juego, como el sistema de eventos o el motor de físicas, para reproducir sonidos en respuesta a acciones del jugador o eventos del juego.

## Formatos de Audio

- **Música (Music):** el formato final de los clips de música será **MP3** con compresión de 192 kbps.
- **Ambiente (Ambient):** el formato final de los clips de audio ambiental será **MP3** con compresión de 192 kbps.
- **Efectos de Sonido (SFX):** el formato final de los clips de efectos de sonido será **OGG** con compresión de 192 kbps.