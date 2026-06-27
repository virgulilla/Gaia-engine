# ORG-009 — Relationships

**Version:** 1.0.0

**Status:** Approved

**Owner:** Gaia Engine Team

**Last Updated:** 2026-06-27

---

# Purpose

Defines the persistent and temporary relationships between organisms.

Relationships allow organisms to recognize biological, social and ecological connections without embedding behaviour.

---

# Scope

This specification defines:

- family relationships
- reproductive relationships
- social relationships
- ecological relationships
- relationship lifecycle

It does not define:

- behaviour
- AI decisions
- combat
- movement
- diplomacy

---

# Philosophy

Relationships describe connections.

Behaviour interprets those connections.

Relationships never force actions.

---

# Responsibilities

Relationships are responsible for storing:

- known connections
- relationship types
- relationship strength
- relationship status
- timestamps

Relationships never:

- evaluate decisions
- modify behaviour
- execute interactions
- update automatically

---

# Runtime Structure

```text
Relationships

├── Family
├── Reproduction
├── Social
├── Ecological
└── Memory Links
```

---

# Family

Represents biological lineage.

Examples:

- Parent
- Child
- Sibling
- Ancestor

Fields:

- OrganismId
- Relationship Type
- Generation Distance

---

# Reproduction

Represents reproductive history.

Examples:

- Current Mate
- Previous Mate
- Offspring
- Pair Bond

Fields:

- OrganismId
- Bond Strength
- Last Reproduction Tick

---

# Social

Represents social interactions.

Examples:

- Ally
- Rival
- Leader
- Follower
- Group Member

Fields:

- OrganismId
- Affinity
- Trust
- Dominance

---

# Ecological

Represents ecological interactions.

Examples:

- Predator
- Prey
- Competitor
- Host
- Parasite
- Symbiont

Fields:

- OrganismId
- Interaction Strength
- Last Encounter

---

# Memory Links

Represents remembered organisms.

Examples:

- Recently Seen
- Dangerous
- Familiar
- Unknown

Memory Links are optional.

They may expire over time.

---

# Relationship Strength

Every relationship has a normalized strength.

Range:

0.0 → 1.0

Examples:

0.10 = Weak

0.50 = Moderate

1.00 = Strong

---

# Lifetime

Relationships may be:

- Permanent
- Temporary
- Expiring

Lifetime is defined by simulation systems.

---

# Ownership

Every organism owns its own relationship graph.

Relationships are directional.

Example:

A may recognize B.

B may not recognize A.

---

# References

Relationships always reference organisms using OrganismId.

Direct object references are forbidden.

---

# Serialization

Relationships must be fully serializable.

Relationship integrity must be preserved after loading.

---

# Inputs

Relationships are updated by:

- Reproduction System
- Social System
- Interaction System
- Ecology System
- Memory System (future)

---

# Outputs

Relationships provide information to:

- Behaviour System
- Utility AI
- Statistics
- Encyclopedia
- Evolution Analytics

---

# Design Constraints

Relationships must remain:

- deterministic
- data-oriented
- scalable
- independent from AI
- independent from rendering

---

# Related Documents

ORG-001 — Organism

ORG-006 — Physiology

ORG-007 — Needs

AI-001 — Utility AI

---

# Acceptance Criteria

- [ ] Supports biological relationships.
- [ ] Supports ecological relationships.
- [ ] Uses OrganismId references only.
- [ ] Fully serializable.
- [ ] Contains no behaviour logic.

---

# Revision History

## 1.0.0

Initial version.
