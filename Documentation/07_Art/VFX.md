# ART-005 — Visual Effects

**Version:** 1.0.0

**Status:** Approved

**Owner:** Gaia Engine Team

**Last Updated:** 2026-06-27

---

# Purpose

Defines the Visual Effects (VFX) system used by Gaia Engine.

Visual Effects provide visual feedback for simulation events without affecting gameplay or simulation logic.

---

# Scope

This specification defines:

- particle effects
- environmental effects
- biological effects
- weather effects
- event visualization

It does not define:

- rendering pipeline
- shaders
- gameplay logic
- simulation logic

---

# Philosophy

Visual Effects communicate change.

Every effect should improve readability.

Effects never alter simulation results.

---

# Responsibilities

The Visual Effects System is responsible for:

- spawning effects
- managing effect lifetimes
- visual event feedback
- environmental ambience

The Visual Effects System never:

- modify gameplay
- modify organisms
- execute AI
- modify world state

---

# Effect Pipeline

```text
Simulation Event

↓

VFX Event

↓

Effect Selection

↓

Effect Instance

↓

Renderer
```

---

# Effect Categories

```text
Visual Effects

├── Biological
├── Environment
├── Climate
├── Water
├── Interaction
├── Combat
└── Ambient
```

---

# Biological Effects

Examples:

- Breathing
- Pollen
- Spores
- Growth
- Death
- Decay

---

# Environment Effects

Examples:

- Falling Leaves
- Dust
- Sand
- Mud Splash
- Rock Fragments

---

# Climate Effects

Examples:

- Rain
- Snow
- Fog
- Lightning
- Wind Gusts

---

# Water Effects

Examples:

- Ripples
- Splashes
- Waterfalls
- Foam
- Bubbles

---

# Interaction Effects

Examples:

- Eating
- Harvesting
- Digging
- Nest Building
- Discovery Highlight

---

# Combat Effects

Examples:

- Hit
- Blood (Configurable)
- Impact Dust
- Bite
- Shockwave

Effects should remain stylized.

---

# Ambient Effects

Examples:

- Fireflies
- Floating Seeds
- Insects
- Floating Dust
- Floating Pollen

Ambient effects improve world immersion.

---

# Effect Structure

Every Visual Effect contains:

```text
Visual Effect

├── EffectId
├── Category
├── Trigger
├── Duration
├── Loop
├── Priority
└── LOD Rules
```

---

# Trigger Types

Effects may be triggered by:

- Simulation Events
- Animation Events
- Weather
- Organism State
- World State

---

# Lifetime

Effects may be:

- Instant
- Timed
- Continuous

Lifetime is managed automatically.

---

# LOD

Effects support multiple quality levels.

Examples:

- High
- Medium
- Low
- Disabled

Quality depends on device capabilities.

---

# Performance

Effects should:

- reuse particle pools
- minimize draw calls
- avoid allocations
- limit simultaneous instances

---

# Mobile Constraints

Preferred limits:

- pooled particles
- GPU-friendly effects
- minimal overdraw
- configurable quality

---

# Determinism

Visual Effects never affect simulation.

Simulation remains deterministic regardless of VFX quality.

---

# Serialization

Visual Effects are never serialized.

They are regenerated from simulation state after loading.

---

# Design Constraints

The Visual Effects System must remain:

- renderer independent
- lightweight
- scalable
- simulation independent

---

# Related Documents

ART-001 — Style Guide

ART-003 — Materials

ART-004 — Animations

AUD-001 — Audio Events

---

# Acceptance Criteria

- [ ] Visual effects never modify simulation.
- [ ] Supports effect pooling.
- [ ] Supports quality levels.
- [ ] Runtime effects are not serialized.
- [ ] Optimized for mobile.

---

# Revision History

## 1.0.0

Initial version.
