# ART-006 — Icons

**Version:** 1.0.0

**Status:** Approved

**Owner:** Gaia Engine Team

**Last Updated:** 2026-06-27

---

# Purpose

Defines the iconography used throughout Gaia Engine.

Icons provide fast visual communication for gameplay, simulation and user interface elements.

Icons never contain gameplay logic.

---

# Scope

This specification defines:

- icon categories
- icon styles
- icon usage
- icon states
- icon generation

It does not define:

- UI layout
- rendering
- gameplay
- simulation

---

# Philosophy

Icons should be immediately recognizable.

Meaning must be understandable without reading text.

Consistency is more important than artistic complexity.

---

# Responsibilities

The Icon System is responsible for:

- visual identification
- category representation
- status indication
- UI consistency

The Icon System never:

- modifies gameplay
- affects simulation
- stores gameplay state

---

# Icon Categories

```text
Icons

├── Organisms
├── Traits
├── Resources
├── Biomes
├── Climate
├── Actions
├── UI
└── Notifications
```

---

# Organism Icons

Examples:

- Herbivore
- Carnivore
- Omnivore
- Plant
- Fungus

Each icon should emphasize silhouette over detail.

---

# Trait Icons

Examples:

- Venom
- Camouflage
- Night Vision
- Thick Fur
- Fast Growth

Trait icons should remain monochromatic whenever possible.

---

# Resource Icons

Examples:

- Water
- Food
- Wood
- Stone
- Metal
- Seeds

Resources should use simple geometric shapes.

---

# Biome Icons

Examples:

- Forest
- Desert
- Swamp
- Mountain
- Ocean
- Tundra

Biome icons should communicate terrain type immediately.

---

# Climate Icons

Examples:

- Sun
- Rain
- Snow
- Wind
- Storm
- Fog

Animated variants are optional.

---

# Action Icons

Examples:

- Move
- Eat
- Drink
- Sleep
- Explore
- Attack
- Reproduce

Action icons support AI debugging tools.

---

# UI Icons

Examples:

- Save
- Load
- Settings
- Search
- Filter
- Close
- Confirm

UI icons follow platform conventions whenever possible.

---

# Notification Icons

Examples:

- Discovery
- Achievement
- Warning
- Error
- Information

Notifications use color as secondary information only.

---

# Icon Structure

Every icon defines:

```text
Icon

├── IconId
├── Category
├── Variant
├── Size
├── Color Profile
└── Accessibility Variant
```

---

# Sizes

Supported sizes:

- 16 px
- 24 px
- 32 px
- 48 px
- 64 px
- Vector Source

Icons should scale cleanly.

---

# States

Icons may support:

- Default
- Hover
- Selected
- Disabled
- Active

State changes belong to the UI System.

---

# Accessibility

Icons must remain understandable when:

- viewed in grayscale
- viewed with color blindness filters
- displayed at small sizes

Color must never be the only distinguishing element.

---

# Performance

Icons should:

- use sprite atlases
- minimize texture switches
- reuse assets
- support vector source generation

---

# Mobile Constraints

Preferred format:

- SVG source
- Raster export
- Shared atlas
- Power-of-two textures

---

# Determinism

Icons are presentation assets only.

They never influence gameplay or simulation.

---

# Serialization

Icons are static assets.

Runtime icon state is never serialized.

---

# Design Constraints

The Icon System must remain:

- lightweight
- scalable
- accessible
- renderer independent

---

# Related Documents

ART-001 — Style Guide

UI-001 — Visual Language

UI-002 — HUD

GAME-003 — Encyclopedia

---

# Acceptance Criteria

- [ ] Icons follow a unified visual language.
- [ ] Icons are accessible.
- [ ] Icons support multiple sizes.
- [ ] Icons never affect gameplay.
- [ ] Optimized for mobile.

---

# Revision History

## 1.0.0

Initial version.
