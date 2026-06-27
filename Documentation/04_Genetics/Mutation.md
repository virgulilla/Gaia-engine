# GEN-004 — Mutation

**Version:** 1.0.0

**Status:** Approved

**Owner:** Gaia Engine Team

**Last Updated:** 2026-06-27

---

# Purpose

Defines how genetic mutations occur during reproduction.

Mutations are the primary source of long-term genetic diversity within Gaia Engine.

Mutations never occur during an organism's lifetime.

---

# Scope

This specification defines:

- mutation generation
- mutation categories
- mutation probability
- mutation intensity
- mutation inheritance

It does not define:

- natural selection
- behaviour
- physiology
- body generation

---

# Philosophy

Mutation introduces variation.

Mutation does not guarantee improvement.

Most mutations should have little or no visible effect.

Some mutations may be harmful.

Very few mutations become highly beneficial.

---

# Mutation Pipeline

```text
Parent Genomes

↓

Inheritance

↓

Mutation

↓

New Genome

↓

Morphogenesis

↓

Body
```

---

# Mutation Timing

Mutations occur exactly once.

During genome generation.

Never before.

Never after.

---

# Mutation Categories

## Parameter Mutation

Modifies an existing value.

Example:

Body Size

0.64

↓

0.68

---

## Dominance Mutation

Changes inheritance dominance.

Example:

Recessive

↓

Co-Dominant

---

## Activation Mutation

Enables or disables optional genetic features.

Example:

Tail Pattern

Inactive

↓

Active

---

## Structural Mutation

Introduces a new variation inside an existing Gene Group.

Structural mutations must remain compatible with the Genome schema.

---

# Mutation Targets

Mutation may affect:

- Morphology
- Physiology
- Reproduction
- Senses
- Adaptation
- Appearance
- Behaviour Bias

Mutation never modifies:

- Genome Identity
- Version
- Parent References

---

# Mutation Probability

Every mutation uses configurable probabilities.

Example configuration:

- Global Mutation Chance
- Mutation Weight
- Gene Group Weight
- Mutation Strength

Hardcoded probabilities are forbidden.

---

# Mutation Strength

Mutation intensity is continuous.

Typical range:

0.0 → 1.0

Lower values produce subtle variation.

Higher values produce significant variation.

---

# Multiple Mutations

A single genome may contain multiple mutations.

The maximum number is defined by configuration.

---

# Biological Constraints

Mutations must remain biologically coherent.

Examples:

Allowed:

- Larger ears
- Smaller body
- Better night vision

Forbidden:

- Negative limb count
- Infinite lifespan
- Impossible anatomy

---

# Rare Mutations

Rare mutations are supported.

Examples:

- Albinism
- Melanism
- Giantism
- Dwarfism

Rare mutations are defined through configuration.

---

# Neutral Mutations

Most mutations should be neutral.

Neutral mutations maintain long-term genetic diversity.

---

# Determinism

Mutation is deterministic.

Given:

- identical seed
- identical parents
- identical configuration

The resulting genome must always be identical.

---

# Serialization

Mutation history must be serializable.

Each Genome stores:

- Mutation Count
- Mutation History
- Mutation Version

---

# Future Extensions

Future versions may support:

- Epigenetic modifiers
- Chromosomal events
- Gene duplication
- Regulatory mutations

Without changing the mutation pipeline.

---

# Related Documents

GEN-001 — Evolution Pipeline

GEN-002 — Genome

GEN-003 — Morphogenesis

GEN-005 — Inheritance

---

# Acceptance Criteria

- [ ] Mutations occur only during reproduction.
- [ ] Mutation is deterministic.
- [ ] Mutation probabilities are configurable.
- [ ] Impossible biological results are prevented.
- [ ] Mutation history is serializable.

---

# Revision History

## 1.0.0

Initial version.
