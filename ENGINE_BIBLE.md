ENGINE_BIBLE.md

Version: 1.0.0

Status: Approved

Owner: Technical Director

Last Updated: 2026-06-27

---

Purpose

Este documento define la filosofía y las reglas fundamentales de Gaia Engine.

Todo componente del motor, toda característica del juego y toda decisión técnica deben respetar las reglas aquí descritas.

Si existe una contradicción entre este documento y cualquier otro, este documento tiene prioridad.

---

Mission

Gaia Engine existe para crear simulaciones emergentes.

No existe para crear un juego concreto.

Project Gaia es únicamente la primera implementación del motor.

El motor debe ser capaz de soportar otros proyectos sin modificar su arquitectura.

---

Core Philosophy

Gaia Engine se basa en cinco principios.

1. Systems over Scripts

Nada importante debe estar programado mediante eventos manuales.

Los comportamientos deben surgir de la interacción entre sistemas.

Ejemplo:

Incorrecto:

"Cuando el jugador pulsa un botón aparece una especie."

Correcto:

"El jugador modifica el clima y el ecosistema favorece el nacimiento de una nueva especie."

---

2. Simulation First

Toda mecánica nace de la simulación.

La interfaz nunca debe contener lógica de juego.

La IA nunca conoce la interfaz.

El render nunca modifica la simulación.

---

3. Data Driven

Las reglas pertenecen a los datos.

No al código.

Ejemplos:

- Biomas
- Genes
- Climas
- Recursos
- Enfermedades
- Estadísticas

Todo debe poder modificarse sin recompilar.

---

4. Modular Architecture

Cada sistema debe poder eliminarse sin romper el resto del motor.

Cada módulo tiene una única responsabilidad.

Los módulos se comunican mediante interfaces y eventos.

Nunca mediante dependencias directas innecesarias.

---

5. Emergent Gameplay

El motor nunca crea historias.

El motor crea condiciones.

Las historias aparecen por la interacción entre sistemas.

---

Engine Layers

El motor está dividido en capas.

Layer 0

Core

Utilidades comunes.

Eventos.

Configuración.

Tiempo.

Logging.

---

Layer 1

Simulation

Estado del mundo.

Tick.

Planificador.

Eventos.

---

Layer 2

World

Mapa.

Chunks.

Biomas.

Recursos.

Agua.

Terreno.

---

Layer 3

Life

Organismos.

Plantas.

Genética.

Reproducción.

Enfermedades.

Muerte.

---

Layer 4

Behaviour

IA.

Necesidades.

Migración.

Combate.

Decisiones.

---

Layer 5

Presentation

Render.

Audio.

UI.

Animaciones.

Efectos.

Esta capa nunca modifica directamente la simulación.

---

Simulation Tick

Toda la simulación funciona mediante ticks.

No mediante frames.

Orden de actualización:

1. Time
2. Weather
3. World
4. Plants
5. Organisms
6. AI
7. Reproduction
8. Evolution
9. Statistics
10. Rendering Sync

El orden nunca cambia sin un ADR.

---

Determinism

Siempre que sea posible:

La misma semilla.

Las mismas acciones.

El mismo resultado.

Esto facilita:

- Depuración.
- Repetición de errores.
- Comparación de simulaciones.
- Pruebas automáticas.

---

Organisms

Gaia Engine no conoce especies concretas.

Solo conoce organismos.

Un organismo posee:

- Genome
- Body
- Needs
- Behaviour
- Health
- Lifecycle
- Relationships

Todo lo demás surge de esos sistemas.

---

World

El mundo no contiene lógica de criaturas.

El mundo únicamente proporciona:

- Terreno.
- Clima.
- Recursos.
- Obstáculos.
- Información espacial.

---

Evolution

La evolución nunca tiene objetivos.

No intenta crear organismos mejores.

Solo selecciona los organismos que sobreviven en un entorno concreto.

---

AI

La IA nunca conoce conceptos de juego.

Solo conoce:

Necesidades.

Estímulos.

Memoria.

Objetivos.

Acciones.

---

Performance Budget

El rendimiento forma parte del diseño.

Una característica que reduzca el rendimiento por debajo del objetivo debe replantearse.

Objetivo inicial:

60 FPS en dispositivos Android de gama media.

---

Debugging

Todo sistema importante debe poder visualizarse.

El motor siempre podrá mostrar:

- Chunks.
- Biomas.
- Temperatura.
- Humedad.
- Caminos.
- Genes.
- Necesidades.
- Eventos.
- Estadísticas.

La depuración es una característica del motor, no una herramienta temporal.

---

Engine Rules

Nunca programar excepciones para un caso concreto.

Nunca crear una especie manualmente.

Nunca introducir reglas ocultas.

Nunca modificar datos desde la interfaz.

Nunca mezclar simulación y renderizado.

Nunca optimizar antes de medir.

Nunca sacrificar claridad por microoptimizaciones.

---

Success Criteria

Gaia Engine será considerado estable cuando:

- Sea capaz de ejecutar simulaciones durante horas sin degradación.
- Permita añadir nuevas mecánicas sin modificar el núcleo.
- Mantenga una arquitectura modular.
- Sea completamente documentado.
- Sea fácilmente ampliable mediante datos.

---

Future Vision

Gaia Engine debe evolucionar hasta soportar:

- Ecosistemas terrestres.
- Ecosistemas marinos.
- Organismos voladores.
- Microorganismos.
- Civilizaciones.
- Otros planetas.
- Simulaciones multiescala.

Todo ello sin cambiar los principios definidos en este documento.

---

Revision History

v1.0.0

- Primera versión oficial del documento.
- Congela la filosofía y arquitectura conceptual de Gaia Engine.
