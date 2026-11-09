# Audio System Technical Design

- **Autor:** @LAS1819
- **Versión:** 1.0
- **Fecha:** 2026-11-09
- **Actualización:** 2026-11-09

## Resumen

El sistema de audio se encarga de gestionar la reproducción de música, ambientes y efectos sonoros dentro del juego.

Su objetivo principal es reforzar la atmósfera, comunicar cambios entre realidades y acompañar la progresión emocional de la historia, especialmente los temas de memoria, pérdida, negación y aceptación.

El audio constituye uno de los pilares fundamentales de la experiencia y complementa las limitaciones visuales inherentes a una producción retro de pequeño alcance.

---

## Componentes del Sistema de Audio

### Audio Manager (AudioManager)

Responsable de la gestión global del sistema.

Funciones:

- Reproducción de música.
- Reproducción de ambientes.
- Reproducción de efectos de sonido.
- Control de volumen.
- Gestión de transiciones.
- Comunicación con otros sistemas.

### Music Manager (MusicManager)

Responsable de la reproducción musical.

Funciones:

- Reproducir temas musicales.
- Gestionar transiciones entre versiones.
- Gestionar cambios entre Realidad Normal y El Otro Lado.
- Controlar volumen y mezcla.

### Ambient Manager (AmbientManager)

Responsable de los sonidos ambientales.

Funciones:

- Reproducción de ambientes por sala.
- Transiciones ambientales.
- Variaciones según la realidad activa.
- Capas ambientales dinámicas.

### SFX Manager (SFXManager)

Responsable de los efectos sonoros.

Funciones:

- Interacciones.
- Inventario.
- Eventos.
- Sistema SHIFT.
- Feedback del jugador.

---

## Filosofía Técnica

El sistema debe favorecer la reutilización de contenido.

Siempre que sea posible:

- Un tema musical tendrá varias versiones derivadas.
- Los ambientes compartirán capas y variaciones.
- Los efectos utilizarán modificaciones mediante volumen, pitch y filtros.
- Las diferencias entre realidades se apoyarán en la mezcla y el procesamiento antes que en la creación de contenido completamente nuevo.

---

## Formatos de Audio

### Música (Music)

Formato final:

- MP3
- 192 kbps

Objetivos:

- Reducir tamaño final.
- Mantener calidad suficiente.
- Conservar una ligera degradación coherente con la estética retro.

### Ambiente (Ambient)

Formato final:

- MP3
- 192 kbps

Objetivos:

- Mantener coherencia con la música.
- Reducir espacio ocupado.
- Simplificar el pipeline de producción.

### Efectos de Sonido (SFX)

Formato final:

- OGG

Objetivos:

- Mejor respuesta para sonidos cortos.
- Menor aparición de artefactos de compresión.
- Tamaño reducido respecto a WAV.

---

## Pipeline de Exportación

Todos los audios generados desde Sonic Pi serán exportados inicialmente en formato WAV.

Posteriormente se procesarán mediante Audacity para realizar:

1. Eliminación de silencios innecesarios.
2. Ajuste de inicio y final.
3. Aplicación de Fade In.
4. Aplicación de Fade Out.
5. Normalización.
6. Exportación al formato final correspondiente.

---

## Normalización de Volumen

Todos los archivos finales serán normalizados a:

- -3 dB

Objetivos:

- Mantener coherencia entre pistas.
- Evitar diferencias bruscas de volumen.
- Disponer de margen para mezcla posterior dentro del juego.

---

## Fade In

Recomendaciones:

- Música: 0,5 - 2 segundos
- Ambiente: 1 - 4 segundos
- Efectos de sonido: 0,1 - 0,5 segundos

---

## Fade Out

Recomendaciones:

- Música: 2 - 8 segundos
- Ambiente: 2 - 8 segundos
- Efectos de sonido: 0,1 - 0,5 segundos

---

## Integración con SHIFT

La transición entre Realidad Normal y El Otro Lado constituye uno de los aspectos más importantes del diseño sonoro.

Siempre que sea posible:

- Se reutilizarán los mismos temas base.
- Se reutilizarán las mismas capas ambientales.
- Las diferencias surgirán de cambios de mezcla, filtros, efectos y procesamiento.

El objetivo es que el jugador no perciba un cambio brusco de pista, sino la sensación de que el propio mundo está transformándose a su alrededor.

---

## Consideraciones Narrativas

El tema principal del juego representa la búsqueda del padre por parte del protagonista.

Durante la partida, dicho tema podrá aparecer fragmentado, incompleto o parcialmente oculto.

En el Final Verdadero, el motivo principal podrá reproducirse por primera vez de forma completa para reforzar la reconstrucción de la memoria y la aceptación final de la verdad.