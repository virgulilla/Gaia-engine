# AUD-004 — Music

**Version:** 1.0.0

**Status:** Approved

**Owner:** Gaia Engine Team

**Last Updated:** 2026-06-27

---

# Purpose

Defines the adaptive music system used throughout Gaia Engine.

Music enhances the emotional experience of the simulation while remaining completely independent from gameplay mechanics.

---

# Scope

This specification defines:

- adaptive music
- musical themes
- transitions
- music states
- playback rules

It does not define:

- sound effects
- ambient audio
- gameplay logic
- simulation

---

# Philosophy

Music should support the simulation.

It should never distract from observation.

Silence is sometimes more powerful than music.

---

# Responsibilities

The Music System is responsible for:

- selecting music
- adapting to world state
- transitioning between themes
- controlling music playback

The Music System never:

- modify gameplay
- execute simulation
- affect AI

---

# Music Architecture

```text
Music

├── Main Theme
├── Exploration
├── Discovery
├── Tension
├── World Events
├── Menu
└── Credits
```

---

# Main Theme

Played during:

- Main Menu
- World Loading
- Intro Sequence

Represents the identity of Gaia Engine.

---

# Exploration

Default in-game music.

Characteristics:

- calm
- atmospheric
- unobtrusive
- dynamic

Exploration themes occupy most gameplay time.

---

# Discovery

Played during important discoveries.

Examples:

- New Species
- New Biome
- Major Evolution
- Encyclopedia Milestone

Discovery themes are brief.

---

# Tension

Played during dangerous situations.

Examples:

- Predator Nearby
- Ecosystem Collapse
- Natural Disaster
- Extinction Event

Music intensity depends on event severity.

---

# World Events

Special themes for significant events.

Examples:

- First Speciation
- First Extinction
- Rare Climate Event
- Ancient Species Discovery

These themes have high emotional impact.

---

# Menu Music

Used in:

- Main Menu
- Encyclopedia
- Settings
- Pause Menu

Menu music should remain relaxing.

---

# Credits

Dedicated ending music.

Played during Credits only.

---

# Music States

Possible states:

- Silent
- Ambient
- Exploration
- Discovery
- Tension
- Event
- Menu

Only one primary music state is active at a time.

---

# Adaptive Behaviour

Music adapts according to:

- Current Biome
- Time of Day
- Weather
- Population Density
- Gameplay Events

Transitions are gradual.

---

# Transition Rules

Supported transitions:

- Crossfade
- Fade In
- Fade Out
- Layer Blend

Abrupt transitions should be avoided.

---

# Looping

Exploration themes should loop seamlessly.

Loop points must be inaudible.

---

# Playback Priority

Priority order:

1. Critical World Events
2. Discoveries
3. Tension
4. Exploration
5. Ambient Silence

Higher priority themes temporarily replace lower priority themes.

---

# Performance

The Music System should:

- stream long tracks
- preload transition clips
- minimize memory usage
- avoid playback interruptions

---

# Mobile Constraints

Preferred implementation:

- compressed streaming audio
- limited simultaneous music layers
- configurable quality

---

# Determinism

Music playback never affects gameplay or simulation.

Different music settings produce identical simulation results.

---

# Serialization

Music playback state is not serialized.

Music resumes according to current gameplay state after loading.

---

# Design Constraints

The Music System must remain:

- adaptive
- lightweight
- scalable
- simulation independent

---

# Related Documents

AUD-000 — Audio Module

AUD-001 — Audio Events

AUD-002 — Sound Effects

AUD-003 — Ambient Audio

---

# Acceptance Criteria

- [ ] Supports adaptive music.
- [ ] Supports seamless transitions.
- [ ] Supports event-based themes.
- [ ] Optimized for mobile.
- [ ] Never affects simulation.

---

# Revision History

## 1.0.0

Initial version.
