# ORG-006 — Physiology

**Version:** 1.0.0

**Status:** Approved

**Owner:** Gaia Engine Team

**Last Updated:** 2026-06-27

---

# Purpose

Defines the internal biological processes that sustain an organism during the simulation.

Physiology transforms anatomical capabilities into functional biological state.

---

# Scope

This specification defines:

- metabolism
- energy management
- temperature regulation
- hydration
- oxygen usage
- growth
- aging

It does not define:

- anatomy
- genetics
- behaviour
- AI
- injuries

---

# Philosophy

Body Schema defines what the organism can be.

Body Instance defines its current physical condition.

Physiology defines how the organism functions over time.

---

# Biological Pipeline

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

↓

Needs

↓

Behaviour
```

---

# Responsibilities

Physiology is responsible for:

- energy production
- energy consumption
- growth
- aging
- body temperature
- hydration
- oxygen balance

Physiology never makes decisions.

---

# Runtime Sections

```text
Physiology

├── Energy
├── Metabolism
├── Temperature
├── Hydration
├── Respiration
├── Growth
└── Aging
```

---

# Energy

Represents available biological energy.

Fields:

- CurrentEnergy
- MaximumEnergy
- EnergyProduction
- EnergyConsumption

---

# Metabolism

Defines biological efficiency.

Fields:

- MetabolicRate
- DigestionEfficiency
- NutrientEfficiency
- FatStorageEfficiency

---

# Temperature

Represents thermal regulation.

Fields:

- BodyTemperature
- OptimalTemperature
- HeatGain
- HeatLoss

---

# Hydration

Represents water balance.

Fields:

- HydrationLevel
- WaterConsumption
- WaterLoss

---

# Respiration

Represents oxygen usage.

Fields:

- OxygenLevel
- OxygenConsumption
- OxygenRecovery

---

# Growth

Represents physical development.

Fields:

- GrowthRate
- CurrentGrowth
- AdultSizeProgress

---

# Aging

Represents biological aging.

Fields:

- BiologicalAge
- ExpectedLifespan
- AgingRate

---

# Runtime Rules

Physiology changes every Simulation Tick.

Changes are calculated exclusively by Physiology Systems.

---

# Inputs

Physiology receives information from:

- Body Schema
- Body Instance
- Climate
- Resources
- Activity Level

---

# Outputs

Physiology influences:

- Needs
- Health
- Reproduction
- Movement
- Behaviour

---

# Design Constraints

Physiology never:

- modifies the Genome
- modifies Body Schema
- executes AI
- controls movement directly

---

# Serialization

Every physiological variable must be serializable.

Loading a saved organism must restore the exact physiological state.

---

# Related Documents

ORG-004 — Body Schema

ORG-005 — Body Instance

ORG-007 — Needs

ORG-008 — Health

GEN-002 — Genome

---

# Acceptance Criteria

- [ ] Represents biological processes only.
- [ ] Updates every Simulation Tick.
- [ ] Fully serializable.
- [ ] Independent from behaviour.
- [ ] Independent from rendering.

---

# Revision History

## 1.0.0

Initial version.
