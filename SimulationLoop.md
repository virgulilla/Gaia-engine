# SimulationLoop.md

Version: 1.0.0

Status: Approved

Owner: Technical Director

---

# Purpose

Este documento define el ciclo completo de ejecución de Gaia Engine.

Toda la simulación depende de este orden.

Nunca debe modificarse sin un ADR.

---

# Philosophy

Gaia Engine no funciona mediante Frames.

Funciona mediante Simulation Ticks.

Un Frame puede contener:

0

1

o varios

Simulation Ticks.

La simulación nunca depende del FPS.

---

# Tick Definition

Un Tick representa una unidad lógica de tiempo.

No equivale necesariamente a tiempo real.

Ejemplo

1 Tick

=

100 ms simulados

La velocidad de simulación puede cambiar.

Los Ticks nunca.

---

# Tick Order

Cada Tick ejecuta exactamente el siguiente orden.

1

TimeSystem

↓

2

SchedulerSystem

↓

3

EventQueue

↓

4

ClimateSystem

↓

5

WaterSystem

↓

6

TerrainSystem

↓

7

PlantGrowthSystem

↓

8

ResourceSystem

↓

9

NeedsSystem

↓

10

MovementSystem

↓

11

InteractionSystem

↓

12

CombatSystem

↓

13

HealthSystem

↓

14

ReproductionSystem

↓

15

EvolutionSystem

↓

16

StatisticsSystem

↓

17

SaveCheckpoint

↓

18

DebugSystem

↓

19

RenderSync

↓

Tick Finalizado

---

# Why This Order

El clima modifica el mundo.

El mundo modifica los recursos.

Los recursos modifican las necesidades.

Las necesidades modifican el movimiento.

El movimiento genera encuentros.

Los encuentros generan combate.

El combate modifica la salud.

La salud afecta la reproducción.

La reproducción genera evolución.

La evolución modifica las estadísticas.

El render siempre ocurre al final.

---

# Simulation Frequency

No todos los sistemas trabajan igual.

Ejemplo

Climate

Cada 10 ticks

Plants

Cada 20 ticks

Movement

Cada Tick

Evolution

Solo cuando existe reproducción

Statistics

Cada 100 ticks

Save

Cada 1000 ticks

Esto reduce enormemente el coste.

---

# Scheduler

El Scheduler decide:

Qué sistema ejecutar.

Cuándo.

Con qué prioridad.

Nunca los sistemas deciden por sí mismos cuándo actualizarse.

---

# Event Queue

Los eventos no se ejecutan inmediatamente.

Todos entran en una cola.

La cola se procesa cuando corresponde.

Esto evita efectos secundarios imprevisibles.

---

# Determinism

El mismo Tick.

La misma Seed.

Las mismas acciones.

Siempre producen exactamente el mismo resultado.

---

# Time Scale

Velocidades soportadas

Pausa

x1

x2

x4

x8

x16

La simulación nunca pierde precisión.

---

# Fast Forward

Durante Fast Forward:

Render puede actualizar menos veces.

La simulación nunca.

---

# Overflow Protection

TickID utiliza entero de 64 bits.

El motor puede ejecutar millones de años simulados sin reiniciarse.

---

# Save Strategy

Guardar únicamente entre Ticks.

Nunca durante un Tick.

Así se garantiza consistencia.

---

# Error Handling

Si un sistema falla:

Registrar error.

Cancelar Tick.

Restaurar último estado consistente.

Nunca continuar una simulación corrupta.

---

# Pause

La pausa detiene:

Scheduler

Ticks

Eventos

No detiene:

UI

Cámara

Debug

---

# Background Simulation

El motor puede ejecutarse sin render.

Modo Headless.

Permite:

Benchmarks.

Pruebas.

Balance.

Servidores futuros.

---

# Performance Budget

Tiempo objetivo por Tick

Inferior a 5 ms.

Si se supera:

Registrar advertencia.

Actualizar Profiler.

---

# Metrics

Cada Tick registra:

Duración.

Eventos.

Organismos.

Plantas.

Memoria.

Chunks activos.

Tiempo por sistema.

---

# Success Criteria

Un Tick debe ser:

Determinista.

Atómico.

Repetible.

Medible.

Independiente del render.

---

# Revision History

v1.0.0

Documento inicial.
