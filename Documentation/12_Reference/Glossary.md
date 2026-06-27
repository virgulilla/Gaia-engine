# REF-001 — Glossary

**Version:** 1.0.0

**Status:** Approved

**Owner:** Gaia Engine Team

**Last Updated:** 2026-06-27

---

# Purpose

Defines the official terminology used throughout Gaia Engine.

Every document, specification and implementation should use these definitions consistently.

---

# Scope

This glossary defines:

- engine terminology
- biological terminology
- simulation terminology
- architectural terminology

It does not define:

- implementation details
- algorithms
- gameplay mechanics

---

# Philosophy

One concept.

One name.

Every important concept should have a single official definition.

---

# Engine Terms

## Engine

The complete software platform responsible for running Gaia Engine.

---

## Module

A logical group of related functionality.

Examples:

- World
- AI
- Gameplay
- Audio

---

## System

A processing unit responsible for updating one or more Components.

Systems own behaviour.

---

## Component

A data container attached to an Entity.

Components never contain behaviour.

---

## Entity

A persistent object with a unique identity.

Examples:

- Organism
- World
- Chunk

---

## Value Object

An immutable object defined entirely by its values.

Examples:

- Position
- Temperature
- Humidity

---

# Simulation Terms

## Tick

The smallest deterministic simulation step.

Every simulation update occurs inside a Tick.

---

## Simulation

The deterministic execution of the virtual ecosystem.

---

## Event

A message describing something that happened.

Events are distributed through the Event Bus.

---

## World

The complete simulated environment.

---

## Chunk

A subdivision of the World used for streaming and optimization.

---

# Biological Terms

## Organism

A living entity participating in the simulation.

---

## Species

A biological classification shared by compatible organisms.

---

## Genome

The complete genetic description of an organism.

---

## Trait

A characteristic derived from genetic information.

---

## Phenotype

The observable characteristics produced by Genome and environmental influence.

---

## Morphogenesis

The process that transforms genetic information into body structure.

---

## Physiology

The internal biological state of an organism.

---

## Need

A biological requirement such as hunger or hydration.

---

# Gameplay Terms

## Discovery

A permanent piece of player knowledge obtained through observation.

---

## Encyclopedia

The player's persistent scientific database.

---

## Objective

An optional gameplay goal.

---

## Achievement

A permanent accomplishment stored in the Player Profile.

---

## Progression

The long-term evolution of player knowledge and unlocks.

---

# Engine Architecture Terms

## Aggregate

A consistency boundary inside the Domain Model.

---

## Composition Root

The single location responsible for constructing the application dependency graph.

---

## Dependency Injection

The mechanism used to provide dependencies to systems.

---

## Serialization

The process of converting persistent state into storage.

---

## Save Game

A serialized representation of an entire simulation.

---

## Resource

A non-domain asset managed by the Resource Manager.

Examples:

- Mesh
- Texture
- Audio
- Configuration

---

# Documentation Rules

All future documentation should use these definitions.

Alternative terminology should be avoided unless explicitly documented.

---

# Related Documents

REF-002 — Naming Conventions

REF-003 — Coding Standards

ENG-002 — Domain Model

---

# Acceptance Criteria

- [ ] Defines official project terminology.
- [ ] Eliminates ambiguous naming.
- [ ] Supports all documentation modules.
- [ ] Independent from implementation.

---

# Revision History

## 1.0.0

Initial version.
