# PAT-001 — System Pattern

**Version:** 1.0.0

**Status:** Approved

**Owner:** Technical Director

**Last Updated:** 2026-06-27

---

# Purpose

Defines the official architecture for every engine system.

Every system implemented inside Gaia Engine must follow this pattern.

No alternative implementations are allowed unless approved through an Architecture Decision Record (ADR).

---

# Philosophy

A System is responsible for processing data.

A System is never responsible for storing long-term state.

Systems transform the current simulation state into a new simulation state.

---

# Responsibilities

A System may:

- read components
- update components
- publish events
- read configuration
- collect metrics

A System must never:

- render graphics
- read UI input
- play audio
- save files directly
- create circular dependencies

---

# Standard Folder Structure

```text
SystemName/

README.md

SystemNameSystem.cs

SystemNameConfig.cs

SystemNameEvents.cs

SystemNameDebugView.cs

SystemNameTests.cs

Documentation.md
```

---

# Standard Class Responsibilities

## SystemNameSystem

Contains simulation logic.

No rendering.

No UI.

No persistence.

---

## SystemNameConfig

Contains tunable parameters.

Loaded during engine initialization.

Never modified by runtime logic.

---

## SystemNameEvents

Defines every event published by the system.

Only event definitions.

No behaviour.

---

## SystemNameDebugView

Provides visualization tools for debugging.

Compiled only in development builds.

Never modifies simulation state.

---

## SystemNameTests

Contains automated tests.

Every public behaviour must be validated.

---

# Lifecycle

Every system follows the same lifecycle.

```text
Initialize

↓

Load Configuration

↓

Subscribe to Events

↓

Execute Tick

↓

Publish Events

↓

Shutdown
```

---

# Dependencies

A System may depend on:

- Core
- Shared Interfaces
- Configuration

A System must never depend on:

- UI
- Rendering
- Audio
- Gameplay
- Platform APIs

---

# State Management

Persistent state belongs to Components.

Transient state belongs to local execution.

Global mutable state is forbidden.

---

# Configuration

Every configurable value must come from configuration files.

Forbidden:

```text
const float HungerRate = 0.18f;
```

Preferred:

```text
OrganismConfig.HungerRate
```

---

# Event Usage

A System publishes facts.

Examples:

OrganismBorn

RainStarted

SpeciesExtinct

A System never publishes commands.

Forbidden:

CreateSpecies

MoveAnimal

SpawnFood

---

# Logging

Systems log only meaningful information.

Avoid excessive logging during simulation.

Development logging must be configurable.

---

# Error Handling

Recoverable errors:

- log warning
- continue execution

Critical errors:

- stop simulation
- preserve consistent state
- generate diagnostic report

---

# Performance Rules

Systems should avoid:

- unnecessary allocations
- reflection during simulation
- LINQ in critical paths
- dynamic type discovery
- hidden memory allocations

---

# Thread Safety

Systems must assume future parallel execution.

Avoid shared mutable state.

Prefer immutable event payloads.

---

# Debug Support

Every system must expose:

- execution time
- processed entities
- generated events
- configuration values
- debug visualization (when applicable)

---

# Unit Testing

Each system must provide tests for:

- normal execution
- edge cases
- invalid input
- deterministic behaviour
- configuration loading

---

# Naming Rules

Every system follows this naming convention.

Examples:

ClimateSystem

MovementSystem

EvolutionSystem

GenomeSystem

StatisticsSystem

Never use:

ClimateManager

MovementController

EvolutionHandler

GenomeProcessor

---

# Design Checklist

Before creating a new system verify:

- Does it have a single responsibility?
- Can it execute independently?
- Is configuration externalized?
- Are events used instead of direct references?
- Can it be unit tested?
- Does it avoid presentation logic?

If any answer is "No", redesign the system.

---

# Related Documents

CORE-001 — Core Architecture

SIM-001 — Simulation Philosophy

SIM-002 — Simulation Loop

REF-001 — Canonical Vocabulary

---

# Acceptance Criteria

- [ ] One responsibility only.
- [ ] Configuration is external.
- [ ] Rendering is absent.
- [ ] UI is absent.
- [ ] Events are immutable.
- [ ] Automated tests exist.
- [ ] Debug tools are available.

---

# Revision History

## 1.0.0

Initial version.
