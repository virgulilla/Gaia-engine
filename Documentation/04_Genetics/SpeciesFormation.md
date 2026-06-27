# GEN-006 — Species Formation

**Version:** 1.0.0

**Status:** Approved

**Owner:** Gaia Engine Team

**Last Updated:** 2026-06-27

---

# Purpose

Defines how populations diverge into distinct species over evolutionary time.

Species Formation is an emergent process.

No system explicitly creates a new species.

---

# Scope

This specification defines:

- speciation
- genetic divergence
- reproductive isolation
- lineage tracking
- extinction

It does not define:

- mutation
- inheritance
- behaviour
- natural selection

---

# Philosophy

Species are discovered.

They are never manually created.

The simulation continuously evaluates populations.

When divergence exceeds configured thresholds, a new species is identified.

---

# Speciation Pipeline

```text
Population

↓

Genetic Drift

↓

Mutation

↓

Selection

↓

Divergence

↓

Reproductive Isolation

↓

Species Recognition
```

---

# Species Definition

A species is defined by:

- shared ancestry
- compatible reproduction
- genetic similarity

Species are classifications.

They are not gameplay entities.

---

# Divergence Metrics

The engine continuously evaluates:

- Genome Similarity
- Trait Similarity
- Morphological Distance
- Reproductive Compatibility

---

# Reproductive Isolation

Species separation begins when successful reproduction falls below configured thresholds.

Isolation may be caused by:

- genetics
- geography
- behaviour
- environment

---

# Species Recognition

When all requirements are satisfied:

- a new SpeciesId is generated
- lineage is preserved
- ancestry is recorded

No organisms are modified.

Only their classification changes.

---

# Extinction

A species becomes extinct when no living organisms remain.

Species records are never deleted.

Historical information remains available.

---

# Lineage

Every species stores:

- SpeciesId
- ParentSpeciesId
- Origin Tick
- Extinction Tick (optional)
- Founder Population

This enables complete evolutionary trees.

---

# Evolution Tree

Species relationships form a directed tree.

Future tools may visualize this tree in the Encyclopedia.

---

# Determinism

Species recognition must be deterministic.

Given the same simulation history, the same species tree must always be produced.

---

# Future Extensions

Future versions may support:

- hybrid species
- ring species
- convergent evolution
- adaptive radiation

---

# Related Documents

GEN-002 — Genome

GEN-004 — Mutation

GEN-005 — Inheritance

ORG-001 — Organism

---

# Acceptance Criteria

- [ ] Species emerge automatically.
- [ ] Lineages are preserved.
- [ ] Extinct species remain in history.
- [ ] Classification is deterministic.
- [ ] Species never contain behaviour.

---

# Revision History

## 1.0.0

Initial version.
