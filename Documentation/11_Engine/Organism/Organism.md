# ORG-001 — Organism

**Version:** 1.0.0

**Status:** Approved

**Owner:** Gaia Engine Team

**Last Updated:** 2026-06-27

---

# Purpose

Defines the central concept of every living entity inside Gaia Engine.

All life forms are represented as Organisms.

No alternative representation exists.

---

# Definition

An Organism is any living entity capable of participating in the simulation.

Examples include:

- Plants
- Animals
- Fungi
- Coral
- Algae

Future versions may include:

- Microorganisms
- Alien organisms
- Artificial biological life

---

An Organism represents a living state.

It stores identity and biological information.

It does not execute behaviour.

Behaviour belongs to simulation systems.

---

# Design Principles

## Living Entity

Every organism:

- is born
- ages
- consumes resources
- interacts with the environment
- reproduces (when possible)
- dies

---

## Behaviour-Free

Organisms never contain simulation logic.

Forbidden:

Move()

Eat()

Think()

Attack()

Sleep()

Reproduce()

These actions belong to dedicated systems.

---

## Persistent Identity

Each organism owns a permanent identifier.

The identifier never changes during its lifetime.

---

## Biological Representation

Every organism is composed of several logical components.

At minimum:

- Identity
- Genome
- Physiology
- Needs
- Health
- Relationships

Additional components may be introduced without modifying existing systems.

---

# Life Cycle

Every organism progresses through the same high-level lifecycle.

```text
Created

↓

Born

↓

Juvenile

↓

Adult

↓

Aging

↓

Dead

↓

Removed from Simulation
```

Individual species may skip or extend stages.

The engine supports flexible lifecycle definitions.

---

# Interaction Model

Organisms interact only through systems.

Examples:

MovementSystem

CombatSystem

PollinationSystem

ReproductionSystem

DiseaseSystem

No organism directly invokes another organism.

---

# Ownership

An organism owns:

- biological state
- identity
- genome reference
- species reference
- physiological data

The world owns:

- terrain
- climate
- resources

The simulation owns:

- execution

---

# Biological Neutrality

The engine makes no assumptions about:

- number of legs
- reproduction method
- metabolism
- intelligence
- movement type

These properties emerge from data and systems.

---

# Persistence

Every organism must be serializable.

Saving and loading an organism must preserve:

- identity
- genome
- lifecycle stage
- physiological state
- relationships

---

# Extensibility

Future biological systems should integrate without changing the Organism definition.

Examples:

Parasites

Symbiosis

Colonies

Hives

Microbial ecosystems

Marine organisms

---

# Constraints

An organism must never:

- update itself
- access rendering
- access UI
- own global state
- schedule execution

---

# Related Documents

ORG-002 — Organism Schema

ORG-003 — Organism State

GEN-001 — Genome

SIM-001 — Simulation Philosophy

PAT-002 — Component Pattern

---

# Acceptance Criteria

- [ ] Represents every living entity.
- [ ] Contains no behaviour.
- [ ] Is fully serializable.
- [ ] Owns biological state only.
- [ ] Supports future organism types.

---

# Revision History

## 1.0.0

Initial version.
