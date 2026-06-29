# ORG-005 — Lifecycle

**Version:** 1.0.0

**Status:** Approved

**Owner:** Gaia Engine Team

**Last Updated:** 2026-06-29

---

# Purpose

Defines the lifetime progression of an organism.

Lifecycle tracks age, maturity and death state.

---

# Scope

This specification defines:

- birth
- age tracking
- maturity
- elder state
- death state

It does not define:

- reproduction rules
- corpse simulation
- species formation

---

# Philosophy

Lifecycle is explicit state.

It is updated by deterministic systems during the Organism Update phase.

---

# Lifecycle Stages

Supported stages:

- Juvenile
- Adult
- Elder
- Dead

---

# Fields

- Birth Tick
- Age Ticks
- Maturity Age
- Lifecycle Stage
- Is Alive

---

# Transition Rules

Typical deterministic transitions:

- Juvenile → Adult when maturity age is reached
- Adult → Elder when late-life threshold is reached
- Any stage → Dead when lifespan or health constraints are exhausted

---

# Determinism

Lifecycle transitions must be deterministic and tick-based.

---

# Serialization

Lifecycle state is fully serializable.

---

# Acceptance Criteria

- [ ] Lifecycle stores deterministic age state.
- [ ] Lifecycle contains data only.
- [ ] Lifecycle transitions are tick-based.
- [ ] Lifecycle is serializable.

---

# Revision History

## 1.0.0

Initial version.
