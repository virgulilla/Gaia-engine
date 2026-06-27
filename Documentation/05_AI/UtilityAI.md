# AI-001 — Utility AI

**Version:** 1.0.0

**Status:** Approved

**Owner:** Gaia Engine Team

**Last Updated:** 2026-06-27

---

# Purpose

Defines the decision-making model used by Gaia Engine.

Every autonomous organism evaluates its current situation using Utility AI.

No alternative decision model is supported.

---

# Scope

This specification defines:

- utility evaluation
- action scoring
- action selection
- decision frequency
- deterministic evaluation

It does not define:

- movement
- pathfinding
- animation
- rendering
- physiology

---

# Philosophy

Organisms do not execute predefined scripts.

Instead, they continuously evaluate possible actions.

The action with the highest utility score is selected.

---

# Decision Pipeline

```text
Perception

↓

Current World State

↓

Needs

↓

Utility Evaluation

↓

Action Scores

↓

Best Action Selected

↓

Action Execution
```

---

# Responsibilities

Utility AI is responsible for:

- evaluating possible actions
- assigning utility scores
- selecting the best action

Utility AI never:

- execute actions
- modify physiology
- modify the world
- update organisms directly

---

# Utility Formula

Every action produces a normalized score.

Range:

0.0 → 1.0

Higher values indicate higher priority.

---

# Inputs

Utility evaluation receives information from:

- Needs
- Physiology
- Health
- Relationships
- Memory
- Perception
- Climate
- Resources

---

# Outputs

Utility AI returns:

- Selected Action
- Utility Score
- Evaluation Metadata

The Action System performs execution.

---

# Decision Frequency

Every organism evaluates its behaviour every Simulation Tick.

Decision frequency may be reduced for:

- sleeping organisms
- inactive organisms
- distant organisms

Configuration determines update frequency.

---

# Candidate Actions

Typical actions include:

- Eat
- Drink
- Sleep
- Explore
- Flee
- Hunt
- Reproduce
- Follow
- Rest
- Idle

Actions are extensible.

---

# Utility Factors

Each action may consider:

- urgency
- distance
- energy cost
- expected reward
- risk
- success probability

Each factor contributes to the final score.

---

# Priority Resolution

The highest score wins.

If two actions produce identical scores:

- deterministic tie-breaking is applied.

Random selection is forbidden.

---

# Utility Curves

Utility curves are configurable.

Supported curve types:

- Linear
- Exponential
- Logistic
- Custom Curve

Curves are defined in external configuration.

---

# Example

Current Needs

```text
Hunger      0.95

Thirst      0.40

Rest        0.10

Safety      0.20
```

Utility Evaluation

```text
Eat         0.96

Drink       0.41

Sleep       0.18

Explore     0.07
```

Selected Action

```text
Eat
```

---

# Determinism

Utility evaluation must be deterministic.

Identical simulation states must always produce identical decisions.

---

# Performance

Utility evaluation should:

- avoid allocations
- avoid reflection
- avoid recursion
- minimize branching

Evaluation must scale efficiently to thousands of organisms.

---

# Serialization

Utility AI stores no persistent state.

Current evaluations are recalculated after loading.

---

# Design Constraints

Utility AI must remain:

- deterministic
- data-driven
- configurable
- extensible
- renderer independent

---

# Related Documents

AI-002 — Perception

AI-003 — Memory

AI-004 — Decision Making

ORG-007 — Needs

SIM-001 — Simulation Philosophy

---

# Acceptance Criteria

- [ ] Every action produces a normalized score.
- [ ] Highest score is always selected.
- [ ] Decision making is deterministic.
- [ ] Utility curves are configurable.
- [ ] AI stores no persistent decision state.

---

# Revision History

## 1.0.0

Initial version.
