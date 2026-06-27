# ORG-005 — Body Instance

**Version:** 1.0.0

**Status:** Approved

**Owner:** Gaia Engine Team

**Last Updated:** 2026-06-27

---

# Purpose

Defines the runtime physical state of an organism.

A Body Instance represents the current condition of a Body Schema during the organism's lifetime.

---

# Scope

This specification defines:

- runtime anatomical state
- injuries
- structural damage
- temporary conditions
- developmental state

It does not define:

- anatomy
- genetics
- behaviour
- physiology calculations

---

# Philosophy

The Body Schema defines what the organism was born with.

The Body Instance defines what has happened to that body since birth.

---

# Relationship

```text
Genome

↓

Morphogenesis

↓

Body Schema

↓

Body Instance

↓

Physiology
```

---

# Runtime Sections

Every Body Instance contains:

```text
Body Instance

├── Integrity
├── Condition
├── Development
├── Reproductive State
├── Active Modifiers
└── Structural Damage
```

---

# Integrity

Represents overall structural integrity.

Fields:

- IntegrityPercentage
- StructuralStability
- MobilityFactor

---

# Condition

Represents temporary body conditions.

Examples:

- Wet
- Frozen
- Exhausted
- Overheated
- Starving
- Dehydrated

Conditions are temporary.

---

# Development

Represents current biological development.

Fields:

- GrowthProgress
- MaturityStage
- GrowthModifier

---

# Reproductive State

Represents current reproductive status.

Examples:

- Fertile
- Pregnant
- Incubating
- Recovering
- Sterile

---

# Active Modifiers

Temporary modifiers affecting the body.

Examples:

- Disease
- Parasite
- Poison
- Infection
- Regeneration
- Mutation Effect

Modifiers expire naturally or through gameplay.

---

# Structural Damage

Represents permanent or semi-permanent body damage.

Examples:

- Broken Limb
- Missing Limb
- Scar Tissue
- Damaged Eye
- Damaged Wing
- Damaged Tail

Structural damage never modifies Body Schema.

---

# Runtime Rules

Body Instance is mutable.

Simulation Systems may update it.

Morphogenesis never updates it after birth.

---

# Ownership

Body Instance belongs exclusively to one Organism.

No shared ownership is allowed.

---

# Serialization

Every runtime field must be serializable.

Body Instance must be restored exactly after loading.

---

# Constraints

Body Instance must never contain:

- AI state
- Genome data
- Behaviour logic
- Rendering information
- Gameplay progression

---

# Design Principles

Body Instance stores only current anatomical state.

Simulation Systems interpret that state.

Rendering Systems visualize that state.

---

# Related Documents

ORG-001 — Organism

ORG-002 — Organism Schema

ORG-004 — Body Schema

ORG-006 — Physiology

GEN-003 — Morphogenesis

---

# Acceptance Criteria

- [ ] Stores runtime anatomical state.
- [ ] Body Schema remains immutable.
- [ ] Supports permanent and temporary changes.
- [ ] Fully serializable.
- [ ] Contains no behaviour.

---

# Revision History

## 1.0.0

Initial version.
