# UI-005 — Encyclopedia UI

**Version:** 1.0.0

**Status:** Approved

**Owner:** Gaia Engine Team

**Last Updated:** 2026-06-27

---

# Purpose

Defines the user interface used to browse the Encyclopedia.

The Encyclopedia UI presents player knowledge accumulated through discoveries.

It never generates or modifies Encyclopedia data.

---

# Scope

This specification defines:

- encyclopedia navigation
- category browsing
- search
- filtering
- comparison
- progression display

It does not define:

- encyclopedia content
- gameplay logic
- simulation
- rendering

---

# Philosophy

The Encyclopedia is a scientific archive.

It should encourage curiosity and exploration.

Information is revealed progressively as the player discovers more about the world.

---

# Responsibilities

The Encyclopedia UI is responsible for:

- presenting Encyclopedia entries
- browsing categories
- searching entries
- filtering information
- comparing discovered knowledge

The Encyclopedia UI never:

- unlock entries
- modify discoveries
- update gameplay
- modify simulation

---

# Screen Layout

```text
Encyclopedia

├── Navigation Panel
├── Category List
├── Search Bar
├── Entry List
├── Entry Details
├── Related Entries
└── Progress Summary
```

---

# Navigation Panel

Allows navigation between major categories.

Supported categories:

- Species
- Traits
- Biomes
- Resources
- Climate
- Behaviours
- Evolution
- World History

---

# Entry List

Displays all discovered entries within the selected category.

Sorting options:

- Alphabetical
- Discovery Date
- Observation Count
- Completion Percentage

---

# Entry Details

Displays detailed information.

Examples:

Species

- Description
- Habitat
- Diet
- Lifespan
- Population History
- Traits

Biome

- Climate
- Resources
- Typical Species

---

# Search

Search supports:

- Name
- Category
- Trait
- Resource
- Biome

Search updates results immediately.

---

# Filters

Available filters include:

- Discovered
- Undiscovered
- Complete
- Incomplete
- Recently Updated
- Favorites (Future)

Filters may be combined.

---

# Related Entries

Each entry displays automatically generated links.

Examples:

Species →

Traits

Biome

Predators

Prey

Evolution Line

---

# Comparison Mode

Players may compare multiple entries.

Examples:

- Species vs Species
- Trait vs Trait
- Biome vs Biome

Comparison is read-only.

---

# Progress Summary

Displays completion statistics.

Examples:

- Species Discovered
- Traits Observed
- Biomes Visited
- Encyclopedia Completion

---

# Locked Entries

Undiscovered entries remain hidden or partially visible depending on player settings.

Locked entries never reveal hidden information.

---

# Navigation

Supports:

- Mouse
- Touch
- Keyboard
- Controller

Navigation order must remain deterministic.

---

# Accessibility

Supports:

- scalable fonts
- screen readers
- high contrast mode
- controller navigation

---

# Performance

The Encyclopedia UI should:

- load entries lazily
- virtualize long lists
- cache thumbnails
- reuse UI components

---

# Determinism

The Encyclopedia UI is presentation only.

It never affects player progression or simulation.

---

# Serialization

UI state is not serialized.

Window preferences may be stored in Player Settings.

---

# Design Constraints

The Encyclopedia UI must remain:

- responsive
- accessible
- data-driven
- simulation independent

---

# Related Documents

GAME-003 — Encyclopedia

UI-001 — Visual Language

UI-003 — Menus

GAME-002 — Discovery

---

# Acceptance Criteria

- [ ] Supports category navigation.
- [ ] Supports search and filtering.
- [ ] Supports entry comparison.
- [ ] Never modifies Encyclopedia data.
- [ ] Fully accessible.

---

# Revision History

## 1.0.0

Initial version.
