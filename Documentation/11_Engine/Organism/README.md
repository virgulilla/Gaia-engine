# ORG-000 — Organism Module

**Version:** 1.0.0

**Status:** Approved

**Owner:** Gaia Engine Team

**Last Updated:** 2026-06-27

---

# Purpose

The Organism module defines every concept related to living entities inside Gaia Engine.

It acts as the bridge between Genetics, Simulation and Artificial Intelligence.

---

# Scope

This module defines:

- organism structure
- anatomy
- biological state
- physiology
- needs
- health
- relationships
- lifecycle

It does not define:

- evolution
- genome structure
- AI decision making
- movement
- combat
- rendering

---

# Module Architecture

```text
Genome
        │
        ▼
Morphogenesis
        │
        ▼
Body Schema
        │
        ▼
Body Instance
        │
        ▼
Physiology
        │
        ▼
Needs
        │
        ▼
Behaviour
```

---

# Responsibilities

The Organism module is responsible for representing living entities.

Simulation Systems operate on the Organism.

The Organism itself never executes behaviour.

---

# Design Principles

## State over Behaviour

Organisms contain state only.

Behaviour belongs to Systems.

---

## Immutable Structure

The anatomical blueprint never changes.

Runtime changes belong to Body Instance.

---

## Biological Separation

The following concepts are independent:

- Genome
- Body
- Physiology
- Needs
- Health
- Behaviour

Each concept owns a single responsibility.

---

## One Source of Truth

Every piece of information has exactly one owner.

Examples:

| Information   | Owner         |
| ------------- | ------------- |
| Genome        | Genome        |
| Anatomy       | Body Schema   |
| Injuries      | Body Instance |
| Energy        | Physiology    |
| Hunger        | Needs         |
| Diseases      | Health        |
| Relationships | Relationships |
| Lifecycle     | Lifecycle     |

Duplicated state is forbidden.

---

# Folder Structure

```text
Organism/

README.md

Organism.md

OrganismSchema.md

BodySchema.md

BodyInstance.md

Physiology.md

Needs.md

Health.md

Relationships.md

Lifecycle.md
```

---

# Runtime Ownership

```text
Organism

├── Identity
├── Genome Reference
├── Body Schema
├── Body Instance
├── Physiology
├── Needs
├── Health
├── Relationships
└── Lifecycle
```

---

# Simulation Ownership

Simulation Systems are responsible for updating Organism state.

Examples:

- Physiology System
- Needs System
- Health System
- Aging System
- Reproduction System

No data is modified directly by the Organism.

---

# Dependencies

Incoming:

- Genetics
- World
- Climate
- Resources
- Simulation

Outgoing:

- AI
- Rendering
- Statistics
- Encyclopedia
- Gameplay

---

# Design Constraints

The Organism module must remain:

- deterministic
- modular
- serializable
- data-oriented
- platform independent

---

# Related Documents

GEN-000 — Genetics Module

GEN-002 — Genome

GEN-003 — Morphogenesis

SIM-001 — Simulation Philosophy

PAT-001 — System Pattern

PAT-002 — Component Pattern

---

# Acceptance Criteria

- [ ] Defines every living entity.
- [ ] Contains no simulation logic.
- [ ] Fully serializable.
- [ ] Uses single ownership for every state.
- [ ] Independent from AI and rendering.

---

# Revision History

## 1.0.0

Initial version.
