# REF-002 — Naming Conventions

**Version:** 1.0.0

**Status:** Approved

**Owner:** Gaia Engine Team

**Last Updated:** 2026-06-27

---

# Purpose

Defines the official naming conventions used throughout Gaia Engine.

Consistent naming improves readability, maintainability and automated code generation.

---

# Scope

This specification defines:

- project naming
- code naming
- asset naming
- file naming
- documentation naming

It does not define:

- coding style
- formatting rules
- implementation details

---

# Philosophy

Every concept should have one official name.

Names should be descriptive, consistent and predictable.

---

# General Rules

Names should be:

- descriptive
- concise
- unambiguous
- consistent

Avoid:

- abbreviations
- unnecessary prefixes
- unnecessary suffixes

---

# Language

All source code, documentation and assets use English.

Comments should also be written in English.

---

# File Names

Documentation:

```text
World.md
Genome.md
Simulation.md
```

Avoid:

```text
world_doc.md
myGenome.md
simulation-final.md
```

---

# Folder Names

Folders use PascalCase or the established module naming convention.

Examples:

```text
Simulation/
Gameplay/
Audio/
World/
```

---

# Classes

Classes use PascalCase.

Examples:

```text
Organism

SimulationEngine

GenomeBuilder

ClimateSystem
```

---

# Interfaces

Interfaces begin with the letter **I**.

Examples:

```text
IWorldRepository

IGenomeGenerator

IAudioService
```

---

# Methods

Methods use PascalCase.

Examples:

```text
UpdateSimulation()

GenerateGenome()

CalculateTemperature()

SpawnOrganism()
```

Methods should begin with verbs.

---

# Variables

Variables use camelCase.

Examples:

```text
currentTemperature

foodAvailability

populationCount

selectedOrganism
```

---

# Constants

Constants use ALL_CAPS.

Examples:

```text
MAX_ORGANISMS

DEFAULT_TICK_RATE

MAX_CHUNK_SIZE
```

---

# Enumerations

Enumerations use PascalCase.

Enumeration values also use PascalCase.

Example:

```text
ClimateType

Temperate

Desert

Arctic
```

---

# Events

Events use the past tense.

Examples:

```text
OrganismBorn

SpeciesDiscovered

ClimateChanged

SimulationStarted
```

---

# Components

Components end with the word **Component**.

Examples:

```text
HealthComponent

NeedsComponent

PhysiologyComponent
```

---

# Systems

Systems end with the word **System**.

Examples:

```text
MovementSystem

ClimateSystem

NutritionSystem

ReproductionSystem
```

---

# Managers

Managers end with the word **Manager**.

Examples:

```text
ResourceManager

AudioManager

SaveGameManager
```

---

# Identifiers

Identifiers end with **Id**.

Examples:

```text
OrganismId

SpeciesId

ChunkId

WorldId
```

---

# Assets

Assets use descriptive PascalCase.

Examples:

```text
OakTree

WolfBody

GrassMaterial

RainLoop
```

---

# Documentation IDs

Documents use prefixes.

Examples:

```text
SIM-001

GEN-004

WRLD-003

ORG-002

ENG-010
```

---

# Acronyms

Use uppercase for well-known acronyms.

Examples:

- AI
- UI
- HUD
- VFX
- ECS

---

# Forbidden Naming

Avoid:

- temp
- data2
- object
- thing
- manager2
- helper
- utils

Names must communicate intent.

---

# Design Constraints

Naming conventions must remain:

- predictable
- scalable
- implementation independent

---

# Related Documents

REF-001 — Glossary

REF-003 — Coding Standards

ENG-002 — Domain Model

---

# Acceptance Criteria

- [ ] Uses English consistently.
- [ ] Uses descriptive names.
- [ ] Applies consistent suffixes.
- [ ] Avoids ambiguous terminology.
- [ ] Supports automated code generation.

---

# Revision History

## 1.0.0

Initial version.
