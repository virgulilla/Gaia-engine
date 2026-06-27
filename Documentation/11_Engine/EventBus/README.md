# README.md

**System ID:** CORE-002

**System Name:** Event Bus

**Version:** 1.0.0

**Status:** Approved

---

# Purpose

The Event Bus is the communication backbone of Gaia Engine.

Its only responsibility is to transport events between systems.

It never owns gameplay logic.

It never modifies simulation state.

It never schedules execution.

---

# Responsibilities

The Event Bus is responsible for:

- publishing events
- delivering events
- preserving deterministic order
- decoupling systems

---

# Non Responsibilities

The Event Bus must never:

- execute simulation logic
- modify entities
- store world state
- decide execution order
- trigger rendering

---

# Design Goals

The Event Bus must be:

- deterministic
- lightweight
- thread-safe
- easy to debug
- allocation-free during simulation

---

# Folder Contents

Concept.md

Defines the philosophy behind the Event Bus.

Specification.md

Defines expected behaviour.

ImplementationNotes.md

Guidelines for implementation.

Examples.md

Usage examples.

---

# Related Systems

Scheduler

Simulation Loop

Core

AI

Climate

Genome

Rendering

---

# References

SIM-002

CORE-001

ARCH-001
