# ENG-004 — Entity Identifiers

**Version:** 1.0.0

**Status:** Approved

**Owner:** Gaia Engine Team

**Last Updated:** 2026-06-27

---

# Purpose

Defines the identifier system used throughout Gaia Engine.

Identifiers uniquely identify every persistent entity in the simulation while preserving determinism and serialization compatibility.

---

# Scope

This specification defines:

- identifier types
- identifier lifetime
- ownership
- uniqueness
- serialization rules

It does not define:

- storage
- networking
- rendering
- gameplay

---

# Philosophy

Identity is permanent.

Data changes.

Identity never does.

Every persistent object is referenced by its identifier.

---

# Responsibilities

The Identifier System is responsible for:

- generating unique identifiers
- preserving identity
- supporting serialization
- enabling cross references

The Identifier System never:

- store entity state
- execute simulation logic
- own entity behaviour

---

# Identifier Categories

```text
Identifiers

├── WorldId
├── ChunkId
├── OrganismId
├── SpeciesId
├── GenomeId
├── ResourceId
├── BiomeId
└── EventId
```

---

# Uniqueness

Every identifier must be globally unique within a save file.

Identifiers are never reused.

Destroyed entities permanently retire their identifiers.

---

# Lifetime

Identifiers are assigned once.

They remain unchanged until the entity is destroyed.

Serialization must preserve identifiers exactly.

---

# References

Entities reference one another exclusively through identifiers.

Example:

```text
Organism

↓

SpeciesId

↓

Species
```

Direct object ownership should only exist inside aggregates.

---

# Equality

Entities are considered identical when their identifiers are equal.

Field values do not determine identity.

---

# Generation

Identifier generation must be deterministic where required.

World generation may derive identifiers from:

- World Seed
- Simulation Tick
- Internal Counters

Implementation details remain internal.

---

# Immutability

Identifiers are immutable value objects.

No system may modify an existing identifier.

---

# Performance

Identifiers should:

- compare efficiently
- serialize efficiently
- minimize memory usage

---

# Determinism

Identifier generation must produce identical results under identical simulation conditions.

---

# Serialization

Every persistent identifier is serialized.

Transient runtime identifiers are forbidden.

---

# Design Constraints

The Identifier System must remain:

- deterministic
- immutable
- lightweight
- platform independent

---

# Related Documents

ENG-002 — Domain Model

ENG-003 — Component System

WRLD-001 — World

ORG-001 — Organism

---

# Acceptance Criteria

- [ ] Every persistent entity has a unique identifier.
- [ ] Identifiers are immutable.
- [ ] Identifiers survive serialization.
- [ ] Supports deterministic generation.
- [ ] Never reused.

---

# Revision History

## 1.0.0

Initial version.
