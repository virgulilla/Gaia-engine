# GEN-005 — Inheritance

**Version:** 1.0.0

**Status:** Approved

**Owner:** Gaia Engine Team

**Last Updated:** 2026-06-29

---

# Purpose

Defines how two parent Genomes are combined to produce a new offspring Genome.

Inheritance is the deterministic genetic combination stage that occurs before Mutation.

It never modifies parent Genomes.

---

# Scope

This specification defines:

- parent genome combination
- gene-by-gene inheritance
- dominance resolution
- activation inheritance
- offspring genome identity

It does not define:

- reproduction triggers
- mutation generation
- morphogenesis
- physiology
- behaviour

---

# Philosophy

Inheritance combines biological potential.

The offspring never receives a direct copy of a complete parent Genome.

Instead, each supported gene is resolved independently using deterministic inheritance rules.

---

# Inheritance Pipeline

```text
Parent Genome A

+ 

Parent Genome B

↓

Schema Validation

↓

Gene Group Combination

↓

Gene Value Resolution

↓

Dominance Resolution

↓

Activation Resolution

↓

Offspring Genome
```

---

# Parent Requirements

Both parent Genomes must:

- use the same Genome schema
- contain the same Gene Groups
- contain the same Gene Keys inside each group

Parent order must not affect the final result.

To guarantee this, parent references are canonically ordered by `GenomeId`.

The lower `GenomeId` is always stored as `ParentGenomeA`.

The higher `GenomeId` is always stored as `ParentGenomeB`.

---

# Responsibilities

Inheritance is responsible for:

- validating parent compatibility
- combining gene values
- resolving offspring dominance
- resolving offspring activation state
- creating offspring genome identity

Inheritance never:

- mutate genes
- create runtime behaviour
- execute morphogenesis
- modify parent genomes

---

# Gene Combination

Every offspring Genome contains the same supported Gene Groups as its parents:

- Morphology
- Physiology
- Reproduction
- Senses
- Adaptation
- Appearance
- Behaviour Bias

Each gene is inherited independently.

No gene may be skipped.

No new arbitrary field may be introduced during inheritance.

---

# Value Resolution

Gene values are resolved deterministically according to parent dominance:

- Dominant + Recessive → dominant parent value
- Dominant + Dominant → arithmetic mean
- Recessive + Recessive → arithmetic mean
- Co-dominant + any value → arithmetic mean
- Blended + any value → arithmetic mean

If both parent values are identical, the offspring keeps that value.

All resulting values remain normalized.

---

# Dominance Resolution

Offspring dominance is resolved deterministically:

- same dominance on both parents → keep that dominance
- Blended paired with any dominance → Blended
- Co-dominant paired with Dominant or Recessive → Co-dominant
- Dominant paired with Recessive → Dominant

Random dominance selection is forbidden.

---

# Activation Resolution

Optional gene activation is resolved deterministically:

- active + active → active
- inactive + inactive → inactive
- active + inactive → active

When only one parent is active, the active parent also provides the inherited value.

---

# Offspring Identity

Every inherited Genome receives:

- a new GenomeId
- the current schema Version
- ParentGenomeA
- ParentGenomeB
- MutationCount initialized to 0 before Mutation
- Generation = max(parent generation) + 1

Identity is created during inheritance.

Mutation may update the resulting Genome afterwards.

---

# Biological Constraints

Inheritance must preserve biological coherence.

Examples:

Allowed:

- combining body size tendencies
- combining sensory strengths
- combining environmental tolerances

Forbidden:

- removing required gene groups
- producing unsupported gene keys
- generating values outside the normalized range

---

# Performance

Inheritance should:

- avoid reflection
- avoid recursion
- avoid unnecessary allocations
- scale linearly with gene count

---

# Determinism

Inheritance is deterministic.

Given identical:

- parent genomes
- offspring GenomeId
- schema version

the resulting offspring Genome must always be identical.

---

# Serialization

Inheritance is runtime logic only.

The resulting offspring Genome is serialized through normal Genome serialization.

Parent Genomes remain unchanged.

---

# Design Constraints

Inheritance must remain:

- deterministic
- data-driven
- schema-safe
- platform independent
- independent from AI

---

# Related Documents

GEN-001 — Evolution Pipeline

GEN-002 — Genome

GEN-003 — Morphogenesis

GEN-004 — Mutation

---

# Acceptance Criteria

- [ ] Combines two parent genomes deterministically.
- [ ] Offspring never receives a direct parent copy.
- [ ] Dominance resolution is deterministic.
- [ ] Activation resolution is deterministic.
- [ ] Parent genomes remain immutable.
- [ ] Offspring identity stores canonical parent references.

---

# Revision History

## 1.0.0

Initial version.
