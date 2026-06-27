# ORG-010 — Lifecycle

**Version:** 1.0.0

**Status:** Approved

**Owner:** Gaia Engine Team

**Last Updated:** 2026-06-27

---

# Purpose

Defines the biological lifecycle of every organism.

The Lifecycle describes the progression of an organism from creation to removal from the simulation.

---

# Scope

This specification defines:

- lifecycle stages
- lifecycle transitions
- biological milestones
- aging
- death
- removal

It does not define:

- reproduction logic
- physiology
- behaviour
- AI
- rendering

---

# Philosophy

Every organism follows the same abstract lifecycle.

Species determine how each stage behaves.

The engine determines when transitions occur.

---

# Lifecycle Pipeline

```text
Genome Created

↓

Morphogenesis

↓

Birth

↓

Juvenile

↓

Adult

↓

Aging

↓

Death

↓

Decomposition

↓

Removal
```

---

# Lifecycle Stages

## Embryonic

Organism is under development.

Characteristics:

- Not autonomous
- No behaviour
- Morphogenesis active

---

## Newborn

Recently created organism.

Characteristics:

- Limited capabilities
- High dependency
- Rapid development

---

## Juvenile

Growing organism.

Characteristics:

- Active growth
- Learning enabled (future)
- Reproduction disabled

---

## Adult

Fully developed organism.

Characteristics:

- Maximum capabilities
- Reproduction enabled
- Stable physiology

---

## Aging

Late-life stage.

Characteristics:

- Progressive decline
- Reduced fertility
- Increased mortality risk

---

## Dead

Biological functions have stopped.

Characteristics:

- No simulation behaviour
- No physiology updates
- No AI execution

---

## Decomposition

Dead organism remains in the world.

Characteristics:

- Resource generation
- Environmental interaction
- Decay progression

---

## Removed

Final stage.

Characteristics:

- Removed from active simulation
- Memory released
- Statistics preserved

---

# Lifecycle Transitions

Transitions are controlled by dedicated simulation systems.

Transitions never occur directly inside the Organism.

---

# Transition Conditions

Examples:

Embryonic → Newborn

- Development completed

Newborn → Juvenile

- Minimum growth reached

Juvenile → Adult

- Sexual maturity reached

Adult → Aging

- Aging threshold reached

Aging → Dead

- Survival failure

Dead → Decomposition

- Death processed

Decomposition → Removed

- Decomposition completed

---

# Lifecycle State

Every organism stores:

- Current Stage
- Stage Start Tick
- Age
- Expected Lifespan

---

# Aging

Aging is continuous.

It is never event-based.

Biological age increases every Simulation Tick.

---

# Death

Death is a lifecycle transition.

Death does not immediately remove the organism.

The body may continue interacting with the environment.

---

# Decomposition

Decomposition may influence:

- Resources
- Soil Fertility
- Disease Spread
- Scavenger Behaviour

Implementation depends on simulation configuration.

---

# Removal

Removal permanently deletes the organism from active simulation.

Historical information may remain available in Statistics.

---

# Inputs

Lifecycle receives updates from:

- Physiology System
- Survival System
- Reproduction System
- Aging System

---

# Outputs

Lifecycle provides information to:

- Behaviour System
- Rendering
- Statistics
- Encyclopedia
- Evolution

---

# Serialization

Lifecycle state must be fully serializable.

Loading a saved simulation must preserve the current lifecycle stage.

---

# Design Constraints

Lifecycle must remain:

- deterministic
- species-independent
- extensible
- data-driven

---

# Related Documents

ORG-001 — Organism

ORG-004 — Body Schema

ORG-006 — Physiology

ORG-008 — Health

GEN-003 — Morphogenesis

---

# Acceptance Criteria

- [ ] Supports complete biological lifecycle.
- [ ] Stage transitions are system-driven.
- [ ] Dead organisms remain in simulation until removed.
- [ ] Fully serializable.
- [ ] Independent from AI and rendering.

---

# Revision History

## 1.0.0

Initial version.
