# BAL-004 — Reward Balance

**Version:** 1.0.0

**Status:** Approved

**Owner:** Gaia Engine Team

**Last Updated:** 2026-06-27

---

# Purpose

Defines how rewards are distributed throughout Gaia Engine.

Rewards reinforce exploration and scientific curiosity without encouraging repetitive behaviour.

---

# Scope

This specification defines:

- reward categories
- reward scaling
- reward frequency
- reward sources
- reward balancing

It does not define:

- gameplay progression
- achievements
- simulation rules
- UI presentation

---

# Philosophy

Rewards acknowledge meaningful discoveries.

Players should never feel rewarded simply for waiting.

Observation and experimentation should always be more valuable than repetition.

---

# Responsibilities

The Reward Balance System is responsible for:

- assigning rewards
- scaling rewards
- preventing reward inflation
- maintaining long-term motivation

The Reward Balance System never:

- modify simulation
- alter AI behaviour
- generate discoveries
- unlock gameplay automatically

---

# Reward Flow

```text
Gameplay Event

↓

Reward Evaluation

↓

Scaling

↓

Reward Granted

↓

Player Progress Updated
```

---

# Reward Categories

```text
Rewards

├── Experience
├── Unlocks
├── Encyclopedia
├── Cosmetics
├── Statistics
└── Milestones
```

---

# Experience Rewards

Awarded for:

- Discoveries
- Objectives
- Rare Observations
- Encyclopedia Progress

Experience values are configurable.

---

# Unlock Rewards

Examples:

- New Analysis Tool
- New Statistics Panel
- New Visualization
- New Encyclopedia Features

Unlocks improve understanding.

They never improve organism performance.

---

# Encyclopedia Rewards

Examples:

- New Entry
- Entry Upgrade
- Cross References
- Evolution Timeline

Knowledge is the primary reward.

---

# Cosmetic Rewards

Examples:

- Player Badges
- Themes
- Icons
- Profile Decorations

Cosmetic rewards never affect gameplay.

---

# Statistics Rewards

Examples:

- New Graph
- Historical Timeline
- Advanced Filters
- Population Analytics

---

# Milestone Rewards

Awarded for significant accomplishments.

Examples:

- First Discovery
- 100 Species
- First Speciation
- Complete Encyclopedia

---

# Reward Scaling

Reward values may depend on:

- rarity
- complexity
- uniqueness
- progression stage

Scaling is deterministic.

---

# Duplicate Rewards

Repeated observations produce reduced rewards when appropriate.

Important discoveries always grant full rewards.

---

# Reward Frequency

Rewards should:

- be frequent early
- become increasingly meaningful
- avoid excessive repetition

---

# Reward Inflation

The system should prevent:

- infinite farming
- repetitive grinding
- exponential reward growth

---

# Analytics

The system monitors:

- Reward Frequency
- Reward Distribution
- Average Experience Gain
- Reward Efficiency

Analytics never modify rewards.

---

# Determinism

Reward calculations must be deterministic.

Identical gameplay events always generate identical rewards.

---

# Serialization

Reward history belongs to the Player Profile.

Configuration is stored separately.

---

# Design Constraints

The Reward Balance System must remain:

- deterministic
- configurable
- measurable
- profile-based

---

# Related Documents

GAME-005 — Progression

GAME-006 — Achievements

BAL-003 — Progression Balance

BAL-005 — Analytics

---

# Acceptance Criteria

- [ ] Rewards meaningful discoveries.
- [ ] Prevents reward inflation.
- [ ] Supports configurable scaling.
- [ ] Fully deterministic.
- [ ] Fully measurable.

---

# Revision History

## 1.0.0

Initial version.
