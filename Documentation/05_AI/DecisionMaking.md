# AI-004 — Decision Making

**Version:** 1.0.0

**Status:** Approved

**Owner:** Gaia Engine Team

**Last Updated:** 2026-06-27

---

# Purpose

Defines how organisms select one action from all available alternatives.

Decision Making consumes Utility AI evaluations and produces a single executable decision.

---

# Scope

This specification defines:

- action selection
- tie resolution
- decision validation
- execution requests
- decision lifecycle

It does not define:

- action execution
- movement
- animation
- rendering

---

# Philosophy

Decision Making does not calculate utility.

Decision Making only selects the best available action.

---

# Responsibilities

Decision Making is responsible for:

- collecting utility scores
- validating candidate actions
- selecting one action
- requesting execution

Decision Making never:

- execute actions
- modify the world
- update physiology
- move organisms

---

# Decision Pipeline

```text
Utility AI

↓

Candidate Actions

↓

Validation

↓

Priority Resolution

↓

Selected Action

↓

Execution Request
```

---

# Candidate Actions

Every candidate contains:

- Action Id
- Utility Score
- Estimated Cost
- Estimated Duration
- Preconditions

---

# Validation

Before selection every action is validated.

Examples:

- Target still exists
- Enough energy
- Action is reachable
- Environmental conditions are valid

Invalid actions are discarded.

---

# Selection

The valid action with the highest Utility Score is selected.

Selection is deterministic.

---

# Tie Resolution

When two actions have identical scores:

Priority order:

1. Lowest Energy Cost
2. Shortest Duration
3. Lowest Action Id

Random selection is forbidden.

---

# Action Lock

Some actions cannot be interrupted.

Examples:

- Eating
- Sleeping
- Giving Birth

Interruptibility is defined by the Action.

---

# Re-evaluation

Actions may be re-evaluated when:

- environment changes
- target disappears
- higher priority need appears
- action completes

---

# Decision Output

Decision Making returns:

- Selected Action
- Target
- Expected Duration
- Utility Score
- Decision Tick

---

# Failure Handling

If no valid action exists:

Default Action:

Idle

Idle allows continuous re-evaluation.

---

# Determinism

Decision selection must be deterministic.

Identical inputs always produce identical decisions.

---

# Serialization

Current decision must be serializable.

Loading restores the active decision.

---

# Design Constraints

Decision Making must remain:

- deterministic
- stateless where possible
- data-driven
- renderer independent

---

# Related Documents

AI-001 — Utility AI

AI-002 — Perception

AI-003 — Memory

AI-005 — Actions

---

# Acceptance Criteria

- [ ] Selects exactly one action.
- [ ] Uses deterministic tie resolution.
- [ ] Validates actions before execution.
- [ ] Supports action interruption.
- [ ] Fully serializable.

---

# Revision History

## 1.0.0

Initial version.
