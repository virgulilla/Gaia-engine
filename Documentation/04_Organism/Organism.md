# ORG-001 — Organism

**Version:** 1.0.0

**Status:** Approved

**Owner:** Gaia Engine Team

**Last Updated:** 2026-06-29

---

# Purpose

Defines the runtime organism aggregate used by Gaia Engine.

An organism is a living entity with persistent identity and mutable biological state.

---

# Scope

This specification defines:

- organism identity
- ownership
- component composition
- references
- serialization rules

It does not define:

- AI behaviour
- movement
- combat
- reproduction resolution

---

# Philosophy

An organism is an entity.

It owns biological components.

It never owns simulation behaviour directly.

---

# Structure

```text
Organism

├── OrganismId
├── SpeciesId
├── GenomeId
├── CurrentChunkId
├── PhysiologyComponent
├── NeedsComponent
├── LifecycleComponent
└── HealthComponent
```

---

# Identity

Every organism stores:

- OrganismId
- SpeciesId
- GenomeId

The OrganismId never changes.

SpeciesId may change only through deterministic species recognition systems.

GenomeId never changes after birth.

---

# Ownership

The organism aggregate owns:

- PhysiologyComponent
- NeedsComponent
- LifecycleComponent
- HealthComponent

Chunks store OrganismId references only.

---

# Chunk Reference

Every organism stores exactly one CurrentChunkId.

Spatial ownership remains explicit and deterministic.

---

# Determinism

Organism state updates must be deterministic.

Given identical world state and identical organism state, future organism state must always be identical.

---

# Serialization

Every organism must be fully serializable.

Serialization stores component state and immutable references.

---

# Acceptance Criteria

- [ ] Organism is a persistent entity.
- [ ] Organism owns runtime biological components.
- [ ] Organism references Chunk, Species and Genome through identifiers.
- [ ] Organism contains no behaviour.
- [ ] Organism is fully serializable.

---

# Revision History

## 1.0.0

Initial version.
