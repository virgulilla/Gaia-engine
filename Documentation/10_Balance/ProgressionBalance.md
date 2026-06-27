# BAL-002 — Difficulty

**Version:** 1.0.0

**Status:** Approved

**Owner:** Gaia Engine Team

**Last Updated:** 2026-06-27

---

# Purpose

Defines the difficulty system used by Gaia Engine.

Difficulty adjusts the player experience without violating the integrity of the simulation.

---

# Scope

This specification defines:

- difficulty presets
- configurable parameters
- player assistance
- progression modifiers
- accessibility interaction

It does not define:

- AI
- rendering
- gameplay rules
- ecosystem simulation

---

# Philosophy

Difficulty should modify player experience.

It should never cheat.

Simulation rules remain identical across all difficulty levels.

---

# Responsibilities

The Difficulty System is responsible for:

- selecting difficulty presets
- configuring gameplay parameters
- exposing customization options

The Difficulty System never:

- modify genomes
- alter AI behaviour
- create resources
- prevent extinction

---

# Difficulty Presets

Supported presets:

```text
Difficulty

├── Relaxed
├── Standard
├── Natural
├── Expert
└── Custom
```

---

# Relaxed

Designed for casual observation.

Characteristics:

- Extended notifications
- Slower progression
- More guidance
- Reduced information overload

Simulation remains unchanged.

---

# Standard

Recommended experience.

Balanced progression with normal guidance.

---

# Natural

Minimal interface assistance.

Players are expected to interpret ecosystem behaviour independently.

---

# Expert

Designed for experienced players.

Characteristics:

- Minimal guidance
- Reduced notifications
- Advanced statistics only
- No gameplay hints

---

# Custom

Players may configure individual parameters.

Examples:

- Notification Frequency
- UI Assistance
- Tutorial Level
- Objective Visibility
- Encyclopedia Hints

---

# Difficulty Parameters

Configurable values include:

- Tutorial Assistance
- Notification Density
- Hint Frequency
- Default Simulation Speed
- Initial Unlocks

No parameter modifies ecosystem behaviour.

---

# Player Assistance

Optional systems include:

- Context Tips
- Objective Suggestions
- Encyclopedia Recommendations
- Interactive Tutorials

---

# Unlock Behaviour

Difficulty never locks gameplay features.

Every feature remains obtainable.

Only the amount of guidance changes.

---

# Accessibility

Accessibility settings are independent from difficulty.

Players may combine:

- Expert Difficulty
- High Accessibility

or

- Relaxed Difficulty
- Minimal Accessibility

---

# Determinism

Changing difficulty never changes simulation outcomes.

Identical worlds remain identical.

---

# Serialization

Difficulty settings belong to the Player Profile.

World Saves reference the active preset used during play.

---

# Design Constraints

The Difficulty System must remain:

- deterministic
- configurable
- simulation independent
- accessibility compatible

---

# Related Documents

BAL-001 — Simulation Balance

GAME-005 — Progression

UI-006 — Accessibility

---

# Acceptance Criteria

- [ ] Supports predefined presets.
- [ ] Supports custom difficulty.
- [ ] Never alters simulation rules.
- [ ] Compatible with accessibility.
- [ ] Fully serializable.

---

# Revision History

## 1.0.0

Initial version.
