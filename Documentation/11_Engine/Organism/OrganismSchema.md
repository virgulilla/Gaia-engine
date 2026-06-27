# ORG-002 — Organism Schema

**Version:** 1.0.0

**Status:** Approved

**Owner:** Gaia Engine Team

**Last Updated:** 2026-06-27

---

# Purpose

Defines the complete logical structure of an Organism.

This document specifies what information an Organism stores.

It does not describe implementation details.

It does not describe behaviour.

---

# Overview

Every Organism contains exactly seven major sections.

```text
Organism

├── Identity

├── Genome

├── Body

├── Physiology

├── Needs

├── Health

└── Relationships
```

Future versions may extend this schema without breaking compatibility.

---

# Identity

Represents immutable information.

Fields:

- OrganismId
- BirthTick
- ParentIds
- SpeciesId
- Generation
- WorldId

Identity never changes during the organism lifetime.

---

# Genome

References the complete genetic description.

Fields:

- GenomeId
- ActiveGenomeVersion
- MutationHistory

Genome data itself is defined by GEN-001.

The Organism only stores references.

---

# Body

Represents physical characteristics.

Fields:

- BodySize
- BodyMass
- BodyTemperature
- LocomotionType
- DietType
- SensorProfile
- BodyColor
- LimbConfiguration

The Body describes what the organism is.

It does not describe what the organism does.

---

# Physiology

Represents internal biological processes.

Fields:

- Energy
- Metabolism
- GrowthStage
- Fertility
- FatReserve
- OxygenLevel

Physiology changes continuously during the simulation.

---

# Needs

Represents internal motivations.

Typical needs:

- Hunger
- Thirst
- Rest
- Safety
- Reproduction

Needs are inputs to Behaviour Systems.

Needs never contain decisions.

---

# Health

Represents physical condition.

Fields:

- HealthPoints
- Diseases
- Injuries
- PoisonLevel
- Stress
- ImmuneStrength

Health does not contain medical logic.

Health stores state only.

---

# Relationships

Represents links to other organisms.

Examples:

- Parent
- Child
- Mate
- Predator
- Prey
- Colony
- Symbiosis

Relationships are identified by OrganismId.

Never by direct object references.

---

# Optional Future Sections

Future versions may introduce:

Memory

Learning

Personality

Hormones

Microbiome

These additions must preserve backward compatibility.

---

# Ownership Rules

Each section has exactly one owner.

Example:

Body belongs only to the Organism.

Genome belongs only to the Organism.

Health belongs only to the Organism.

Shared mutable ownership is forbidden.

---

# Serialization

Every section must be serializable independently.

The engine must support partial serialization for debugging tools.

---

# Validation Rules

Every Organism must always contain:

Identity

Genome

Body

Physiology

Needs

Health

Relationships

None of these sections may be null.

---

# Design Constraints

Schema evolution must be additive.

Removing existing fields requires a new major version.

Renaming existing fields is forbidden.

---

# Related Documents

ORG-001 — Organism

ORG-003 — Organism Lifecycle

GEN-001 — Genome

PAT-002 — Component Pattern

---

# Acceptance Criteria

- [ ] Seven mandatory sections exist.
- [ ] Behaviour is absent.
- [ ] Serialization is supported.
- [ ] Identity is immutable.
- [ ] Relationships use identifiers.
- [ ] Schema supports future extensions.

---

# Revision History

## 1.0.0

Initial version.
