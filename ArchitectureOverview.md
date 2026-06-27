ArchitectureOverview.md

Version: 1.0.0

Status: Approved

---

Purpose

Este documento describe la arquitectura global de Gaia Engine.

No describe clases.

No describe código.

Describe cómo está organizado el motor.

---

Philosophy

Gaia Engine está construido mediante capas.

Cada capa tiene responsabilidades claras.

Cada capa solo conoce las capas inferiores.

Nunca las superiores.

Esto evita dependencias circulares.

---

High Level Architecture

Core

↓

Simulation Engine

↓

Gameplay Layer

↓

Presentation Layer

↓

Platform Layer

---

Core Layer

Responsabilidad

Infraestructura.

Nunca contiene lógica del juego.

Incluye:

Logging

Configuration

Math

Random

Utilities

Collections

Serialization

Dependency Injection

Time

Scheduler

Events

---

Simulation Engine

Es el corazón del motor.

Aquí ocurre toda la simulación.

No conoce Unity.

No conoce UI.

No conoce Audio.

No conoce Render.

Contiene:

Simulation

World

Climate

Life

Evolution

Behaviour

Resources

Statistics

---

Gameplay Layer

Convierte la simulación en un juego.

Ejemplos

Poderes del jugador

Progresión

Desbloqueos

Objetivos

Logros

Tutorial

Economía

Monetización

Todo esto vive aquí.

---

Presentation Layer

Representa visualmente la simulación.

Incluye

Render

Animaciones

Sonido

Interfaz

Partículas

Shaders

HUD

Nunca modifica directamente la simulación.

---

Platform Layer

Responsable de:

Android

iOS

Steam

Guardado

Compras

Notificaciones

Servicios externos

El motor nunca depende de esta capa.

---

Core Modules

Core contiene únicamente módulos reutilizables.

Ejemplo

Time

Scheduler

Events

Pooling

Configuration

Math

Profiler

Debug

Nunca contiene organismos.

Nunca contiene biomas.

---

Simulation Modules

Simulation

↓

World

↓

Climate

↓

Resources

↓

Plants

↓

Organisms

↓

Evolution

↓

AI

↓

Statistics

Cada módulo actualiza únicamente su dominio.

---

Presentation Modules

Camera

UI

Audio

Rendering

Lighting

Animation

Effects

Debug Overlay

---

Communication

Los módulos nunca se llaman directamente.

Siempre utilizan:

Interfaces

o

Event Bus

Esto reduce el acoplamiento.

---

Update Order

Boot

↓

Load Config

↓

Initialize Core

↓

Initialize Simulation

↓

Initialize Gameplay

↓

Initialize Presentation

↓

Game Ready

---

Cada Tick

Time

↓

Weather

↓

World

↓

Plants

↓

Organisms

↓

AI

↓

Evolution

↓

Gameplay

↓

Rendering

↓

Statistics

↓

Save Check

---

Dependencies

Core

↓

Simulation

↓

Gameplay

↓

Presentation

↓

Platform

Nunca al revés.

---

Forbidden Dependencies

Presentation → Simulation

permitido

Simulation → Presentation

prohibido

Gameplay → Core

permitido

Core → Gameplay

prohibido

World → UI

prohibido

Genome → Audio

prohibido

---

Data Flow

Input

↓

Gameplay

↓

Simulation

↓

Events

↓

Presentation

↓

Output

Nunca existe flujo inverso.

---

Folder Structure

Engine/

Core/

Simulation/

Gameplay/

Presentation/

Platform/

Editor/

Tests/

---

Engine Goals

Reutilizable.

Determinista.

Modular.

Escalable.

Portable.

Testeable.

Data Driven.

---

Non Goals

No pretende competir con Unity.

No pretende ser un motor gráfico.

No pretende gestionar escenas.

No pretende sustituir ECS.

Su única misión es ejecutar simulaciones complejas.

---

Success Criteria

Añadir un nuevo sistema debe requerir:

Crear módulo.

Registrar módulo.

Escribir tests.

Actualizar documentación.

Nada más.

---

Revision History

v1.0.0

Versión inicial.
