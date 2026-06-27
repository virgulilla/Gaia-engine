# PAT-002 — Component Pattern

**Version:** 1.0.0

**Status:** Approved

**Owner:** Technical Director

**Last Updated:** 2026-06-27

---

# Purpose

Defines the official data model used by Gaia Engine components.

Components are the primary containers of simulation state.

They never contain simulation behaviour.

---

# Philosophy

Components represent data.

Systems represent behaviour.

This separation is mandatory throughout Gaia Engine.

---

# Responsibilities

A Component may:

- store state
- expose immutable identifiers
- expose mutable simulation data
- support serialization

A Component must never:

- execute gameplay logic
- update itself
- subscribe to events
- access rendering
- access UI
- access audio
- schedule work

---

# Component Granularity

Gaia Engine uses **Macro Components**.

Components represent coherent groups of related data.

Example:

OrganismComponent

Contains:

- identity
- age
- energy
- health
- body reference

Instead of:

AgeComponent

EnergyComponent

HealthComponent

IdentityComponent

BodyComponent

---

# Why Macro Components?

Benefits:

- simpler debugging
- easier documentation
- better readability
- fewer files
- easier AI generation
- lower maintenance cost

---

# Standard Folder Structure

```text
ComponentName/

README.md

ComponentName.cs

ComponentNameSchema.md

ComponentNameTests.cs
```

---

# Component Categories

## Identity Components

Store immutable identifiers.

Examples:

- Organism ID
- Species ID
- Genome ID

---

## Simulation Components

Store mutable simulation state.

Examples:

- Energy
- Temperature
- Hunger
- Age

---

## Configuration Components

Store static configuration loaded from data files.

Runtime systems never modify them.

---

# Serialization

Every component must support serialization.

Persistent state belongs inside components.

Transient execution state does not.

---

# Ownership

Each component has exactly one owner.

Example:

Organism owns:

Identity

Genome

Body

Health

Needs

Relationships

No other entity owns those components.

---

# Mutability

Immutable:

IDs

Creation timestamps

Configuration references

Mutable:

Health

Energy

Position

Needs

Relationships

---

# References

Components reference other entities only through IDs.

Never through direct object ownership unless explicitly justified.

---

# Events

Components never publish events.

Systems publish events after modifying components.

---

# Validation

Components may validate their own data consistency.

They must never modify the simulation.

---

# Performance

Components should:

- minimize allocations
- use cache-friendly layouts
- avoid unnecessary nesting
- avoid runtime reflection

---

# Thread Safety

Components should assume future parallel execution.

Shared mutable state is forbidden.

---

# Example

OrganismComponent

Contains:

Identity

Genome Reference

Species Reference

Current Energy

Current Health

Current Age

Body Size

Current Position

Nothing else.

Movement belongs to MovementSystem.

Evolution belongs to EvolutionSystem.

Health calculations belong to HealthSystem.

---

# Anti-Patterns

Forbidden:

```text
OrganismComponent.Move()
```

Forbidden:

```text
OrganismComponent.Update()
```

Forbidden:

```text
OrganismComponent.Eat()
```

Forbidden:

```text
OrganismComponent.Reproduce()
```

Those belong to Systems.

---

# Design Checklist

Before creating a new component:

- Does it store state only?
- Does it avoid behaviour?
- Can it be serialized?
- Is it reusable?
- Does it belong to exactly one owner?

If not, redesign it.

---

# Related Documents

PAT-001 — System Pattern

PAT-003 — Event Pattern

CORE-001 — Core Architecture

REF-001 — Canonical Vocabulary

---

# Acceptance Criteria

- [ ] Stores state only.
- [ ] No behaviour.
- [ ] Serializable.
- [ ] Clearly owned.
- [ ] Easy to inspect.
- [ ] Suitable for deterministic simulation.

---

# Revision History

## 1.0.0

Initial version.
