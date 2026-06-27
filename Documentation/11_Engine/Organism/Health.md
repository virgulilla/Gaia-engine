# ORG-008 — Health

**Version:** 1.0.0

**Status:** Approved

**Owner:** Gaia Engine Team

**Last Updated:** 2026-06-27

---

# Purpose

Defines the biological health state of an organism.

Health represents the organism's ability to survive, recover and continue normal biological functions.

Health never executes medical logic.

---

# Scope

This specification defines:

- health state
- injuries
- diseases
- healing
- immunity
- death conditions

It does not define:

- anatomy
- physiology
- combat
- behaviour
- reproduction

---

# Philosophy

Health is a consequence of simulation.

Health does not control the simulation.

Simulation Systems modify Health.

---

# Responsibilities

Health is responsible for storing:

- injuries
- diseases
- recovery progress
- immune status
- vitality

Health never:

- heals itself
- applies diseases
- decides death
- performs calculations

---

# Runtime Structure

```text
Health

├── Vitality
├── Injuries
├── Diseases
├── Immunity
├── Recovery
└── Status Effects
```

---

# Vitality

Represents overall biological condition.

Fields:

- CurrentHealth
- MaximumHealth
- HealthPercentage
- CriticalThreshold

---

# Injuries

Represents structural damage.

Examples:

- Fracture
- Burn
- Bite
- Laceration
- Internal Injury

Each injury contains:

- Severity
- Affected Body Part
- Healing Progress

---

# Diseases

Represents biological illnesses.

Examples:

- Infection
- Parasite
- Fungus
- Virus
- Unknown Disease

Each disease contains:

- Identifier
- Severity
- Progress
- Contagious Flag

---

# Immunity

Represents defensive biological capabilities.

Fields:

- ImmuneStrength
- DiseaseResistance
- RecoveryEfficiency

---

# Recovery

Represents natural healing.

Fields:

- HealingRate
- RestBonus
- NutritionBonus
- RecoveryProgress

---

# Status Effects

Temporary health modifiers.

Examples:

- Poisoned
- Bleeding
- Fever
- Exhausted
- Hypothermia
- Heatstroke

Status effects are temporary.

Permanent changes belong to Body Instance.

---

# Inputs

Health receives updates from:

- Combat System
- Physiology System
- Disease System
- Environment System
- Aging System

---

# Outputs

Health influences:

- Physiology
- Needs
- Behaviour
- Reproduction
- Survival

---

# Death Conditions

Health does not decide death.

Death is evaluated by the Survival System.

Health only exposes biological state.

---

# Runtime Rules

Health values are mutable.

Updates occur through dedicated simulation systems only.

---

# Serialization

Every health field must be serializable.

Saving and loading must preserve identical health state.

---

# Design Constraints

Health must remain:

- deterministic
- data-oriented
- independent from rendering
- independent from AI
- independent from gameplay

---

# Related Documents

ORG-005 — Body Instance

ORG-006 — Physiology

ORG-007 — Needs

SIM-001 — Simulation Philosophy

---

# Acceptance Criteria

- [ ] Stores health state only.
- [ ] Contains no simulation logic.
- [ ] Fully serializable.
- [ ] Supports multiple simultaneous diseases.
- [ ] Supports localized injuries.
- [ ] Independent from AI and rendering.

---

# Revision History

## 1.0.0

Initial version.
