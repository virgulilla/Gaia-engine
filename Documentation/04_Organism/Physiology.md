# ORG-006 — Physiology

**Version:** 1.0.0

**Status:** Approved

**Owner:** Gaia Engine Team

**Last Updated:** 2026-06-29

---

# Purpose

Defines the internal biological operating state of an organism.

Physiology transforms inherited biological potential into runtime upkeep and resilience values.

---

# Scope

This specification defines:

- metabolism
- growth rate
- lifespan
- water efficiency
- digestion efficiency
- body temperature

It does not define:

- needs themselves
- AI decisions
- genome mutation

---

# Philosophy

Physiology is runtime biological state.

It is updated by dedicated Organism Systems.

---

# Fields

- Metabolism Rate
- Growth Rate
- Lifespan Ticks
- Water Efficiency
- Digestion Efficiency
- Body Temperature

All values use deterministic integer ranges interpreted by systems.

---

# Determinism

Physiology updates must never depend on rendering, frame rate or platform-specific timing.

---

# Serialization

Physiology state is fully serializable.

---

# Acceptance Criteria

- [ ] Physiology stores deterministic biological state.
- [ ] Physiology contains data only.
- [ ] Physiology is serializable.

---

# Revision History

## 1.0.0

Initial version.
