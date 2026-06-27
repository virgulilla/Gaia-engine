# ART-004 — Animations

**Version:** 1.0.0

**Status:** Approved

**Owner:** Gaia Engine Team

**Last Updated:** 2026-06-27

---

# Purpose

Defines the animation system used to represent organisms and environmental elements in Gaia Engine.

Animations visualize simulation state.

They never modify simulation state.

---

# Scope

This specification defines:

- animation architecture
- animation clips
- animation states
- procedural animation
- animation events

It does not define:

- AI
- gameplay
- rendering pipeline
- simulation logic

---

# Philosophy

Animations communicate information.

The player should understand an organism's current behaviour simply by observing its movement.

---

# Responsibilities

The Animation System is responsible for:

- selecting animation clips
- blending animations
- procedural animation adjustments
- animation events

The Animation System never:

- execute gameplay
- modify organisms
- change AI decisions

---

# Animation Pipeline

```text
Behaviour

↓

Current Action

↓

Animation State

↓

Animation Controller

↓

Animation Clip

↓

Renderer
```

---

# Animation Categories

```text
Animations

├── Idle
├── Locomotion
├── Interaction
├── Combat
├── Reproduction
├── Damage
└── Death
```

---

# Idle Animations

Examples:

- Idle
- Look Around
- Breathing
- Sleeping

Idle animations should introduce subtle variation.

---

# Locomotion

Examples:

- Walk
- Run
- Swim
- Climb (Future)
- Fly (Future)

Movement speed is synchronized with simulation speed.

---

# Interaction

Examples:

- Eat
- Drink
- Harvest
- Inspect
- Build Nest

---

# Combat

Examples:

- Attack
- Bite
- Charge
- Defend
- Escape

---

# Reproduction

Examples:

- Courtship
- Mating
- Egg Laying
- Birth

---

# Damage

Examples:

- Hurt
- Stunned
- Exhausted
- Falling

---

# Death

Examples:

- Collapse
- Float
- Decay Start

Death animations never delay simulation.

---

# Animation State Machine

Supported states:

- Idle
- Moving
- Working
- Fighting
- Resting
- Dead

Transitions are controlled by Behaviour Execution.

---

# Procedural Animation

Procedural adjustments may include:

- head tracking
- tail movement
- breathing
- body sway
- look direction

Procedural animation complements authored animations.

---

# Animation Events

Animations may publish events.

Examples:

- Footstep
- Bite
- Drink
- Hit
- Death

Events notify Simulation Systems.

Animation timing never affects simulation determinism.

---

# Blending

Animations support smooth transitions.

Blend parameters include:

- Speed
- Direction
- Action Priority

---

# Performance

Animations should:

- minimize CPU usage
- reuse animation clips
- support animation sharing
- avoid unnecessary updates

---

# Mobile Constraints

Preferred limits:

- Shared animation controllers
- Lightweight rigs
- Limited bone count
- Minimal animation layers

---

# Determinism

Animation selection must be deterministic.

Visual interpolation does not affect simulation.

---

# Serialization

Animation runtime state is not serialized.

Animation state is reconstructed from simulation state after loading.

---

# Design Constraints

The Animation System must remain:

- deterministic
- modular
- renderer independent
- platform independent

---

# Related Documents

ART-001 — Style Guide

ART-002 — Procedural Assets

ART-003 — Materials

ART-005 — Visual Effects

AI-006 — Behaviour Execution

---

# Acceptance Criteria

- [ ] Supports procedural animation.
- [ ] Supports animation events.
- [ ] Optimized for mobile.
- [ ] Animation never affects simulation.
- [ ] Runtime animation state is reconstructable.

---

# Revision History

## 1.0.0

Initial version.
