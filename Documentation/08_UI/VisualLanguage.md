# UI-001 — Visual Language

**Version:** 1.0.0

**Status:** Approved

**Owner:** Gaia Engine Team

**Last Updated:** 2026-06-27

---

# Purpose

Defines the visual language used throughout the User Interface.

The Visual Language establishes a consistent, readable and accessible interface across the entire game.

---

# Scope

This specification defines:

- visual hierarchy
- typography
- spacing
- color usage
- icons
- interaction feedback

It does not define:

- gameplay
- rendering
- UI implementation
- simulation

---

# Philosophy

The interface should disappear behind the experience.

Players should focus on the simulation, not the UI.

---

# Design Principles

## Clarity

Every element should communicate one purpose.

Avoid decorative elements that reduce readability.

---

## Consistency

Identical actions always look identical.

Identical information always uses the same presentation.

---

## Simplicity

Prefer fewer interface elements.

Progressive disclosure is preferred over crowded screens.

---

## Feedback

Every interaction must produce immediate feedback.

Examples:

- hover
- pressed
- selected
- disabled
- completed

---

# Visual Hierarchy

Priority order:

1. Critical Warnings
2. Active Gameplay
3. Current Selection
4. Context Information
5. Background Information

---

# Typography

Text Levels:

- Display
- Heading
- Section
- Body
- Caption

Use a single font family throughout the project.

Avoid decorative fonts.

---

# Color System

Colors communicate categories.

Examples:

Green

- healthy
- positive
- available

Blue

- water
- information
- selection

Yellow

- warning
- attention

Red

- danger
- failure
- critical

Color must never be the only communication channel.

---

# Icons

Every important action should have an icon.

Icons reinforce text.

Icons never replace text completely.

---

# Spacing

Use a consistent spacing system.

Base unit:

```text
8 px
```

Recommended scale:

- 8
- 16
- 24
- 32
- 48
- 64

---

# Panels

Panels should:

- group related information
- maintain consistent padding
- avoid excessive nesting

---

# Buttons

Supported button types:

- Primary
- Secondary
- Tertiary
- Icon
- Toggle

Buttons communicate importance visually.

---

# States

Interactive elements support:

- Normal
- Hover
- Pressed
- Selected
- Disabled
- Focused

---

# Animations

UI animations should be:

- subtle
- fast
- informative

Recommended duration:

100–250 ms

---

# Notifications

Notifications use consistent styling.

Priority:

- Information
- Success
- Warning
- Error

---

# Accessibility

The interface must support:

- scalable fonts
- high contrast mode
- color-blind friendly palettes
- keyboard navigation
- controller navigation
- touch interaction

---

# Mobile Guidelines

The interface should support:

- one-handed operation
- large touch targets
- responsive layouts
- landscape orientation

Minimum touch target:

```text
48 x 48 dp
```

---

# Design Constraints

The Visual Language must remain:

- platform independent
- scalable
- accessible
- consistent

---

# Related Documents

UI-000 — User Interface Module

UI-002 — HUD

ART-001 — Style Guide

ART-006 — Icons

---

# Acceptance Criteria

- [ ] Consistent typography.
- [ ] Consistent spacing.
- [ ] Accessible color usage.
- [ ] Mobile-friendly layouts.
- [ ] Supports controller and touch navigation.

---

# Revision History

## 1.0.0

Initial version.
