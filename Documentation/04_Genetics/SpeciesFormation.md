# GEN-006 — Species Formation

**Version:** 1.1.0

**Status:** Approved

**Owner:** Gaia Engine Team

**Last Updated:** 2026-06-29

---

# Purpose

Defines how populations diverge into distinct species over evolutionary time.

Species Formation is an emergent process.

No system explicitly creates a new species.

---

# Scope

This specification defines:

- speciation
- genetic divergence
- reproductive isolation
- lineage tracking
- extinction

It does not define:

- mutation
- inheritance
- behaviour
- natural selection

---

# Philosophy

Species are discovered.

They are never manually created.

The simulation continuously evaluates populations.

When divergence exceeds configured thresholds, a new species is identified.

---

# Speciation Pipeline

```text
Population

↓

Genetic Drift

↓

Mutation

↓

Selection

↓

Divergence

↓

Reproductive Isolation

↓

Species Recognition
```

---

# Species Definition

A species is defined by:

- shared ancestry
- compatible reproduction
- genetic similarity

Species are classifications.

They are not gameplay entities.

---

# Divergence Metrics

The engine continuously evaluates:

- Genome Similarity
- Trait Similarity
- Morphological Distance
- Reproductive Compatibility

All recognition metrics use the normalized deterministic score range:

- 1000 = identical
- 0 = maximally different

---

# Eligible Organisms

Species recognition evaluates only organisms that are:

- alive
- Adult or Elder

Juveniles never create a new species.

Dead organisms never create a new species.

---

# Founder Reference

Every species is compared against its recorded Founder Population.

For recognition purposes:

- each Founder Organism is resolved from the persistent organism collection
- its Genome is resolved from the persistent genome collection
- the candidate organism is compared against every Founder Genome
- the Founder Genome with the highest Genome Similarity becomes the reference for that evaluation

This keeps lineage explicit and deterministic.

---

# Metric Definitions

## Genome Similarity

Genome Similarity is evaluated gene by gene across the complete Genome schema.

For every matched gene:

- Value Similarity = `1000 - abs(A.Value - B.Value)`
- Dominance Similarity = `1000` when dominance matches, otherwise `0`
- Activation Similarity = `1000` when activation matches, otherwise `0`

Gene Score:

`(Value Similarity * 7 + Dominance Similarity * 2 + Activation Similarity) / 10`

Genome Similarity is the arithmetic mean of all Gene Scores.

---

## Trait Similarity

Trait Similarity is evaluated after deterministic trait expression.

For every Trait:

- Trait Score = `1000 - abs(A.Trait - B.Trait)`

Trait Similarity is the arithmetic mean of all Trait Scores.

---

## Morphological Distance

Morphological Distance is evaluated through deterministic Morphogenesis under neutral development conditions:

- Average Temperature = `18`
- Food Availability = `500`
- Humidity = `500`
- Altitude = `0`
- Season = `Spring`

Both organisms generate a deterministic Body Plan from those neutral conditions.

For every Body Plan field:

- Body Field Score = `1000 - abs(A.Field - B.Field)`

Morphological Similarity is the arithmetic mean of:

- Proportions
- Mass
- Limb Configuration
- Body Covering
- Coloration
- Sensory Structures
- Locomotion Profile

Morphological Distance is considered high when Morphological Similarity falls below its configured threshold.

---

## Reproductive Compatibility

Reproductive Compatibility is evaluated using the Reproduction Gene Group only.

The following genes are required:

- Maturity Age
- Fertility
- Gestation Time
- Egg Count
- Breeding Cooldown

Each reproduction gene uses the same Gene Score formula defined by Genome Similarity.

Reproductive Compatibility is the arithmetic mean of those five reproduction Gene Scores.

---

# Reproductive Isolation

Species separation begins when successful reproduction falls below configured thresholds.

Isolation may be caused by:

- genetics
- geography
- behaviour
- environment

---

# Species Recognition

When all requirements are satisfied:

- a new SpeciesId is generated
- lineage is preserved
- ancestry is recorded

No organisms are modified.

Only their classification changes.

The deterministic recognition rule is:

1. evaluate the candidate organism against the best Founder Reference of its current Species
2. compute:
   - Genome Similarity
   - Trait Similarity
   - Morphological Similarity
   - Reproductive Compatibility
3. count how many of these metrics fail their configured minimum thresholds
4. create a new Species only when:
   - Reproductive Compatibility fails its threshold
   - the number of failed metrics is greater than or equal to `Required Failed Metric Count`

This makes reproductive isolation mandatory while still considering broader biological divergence.

---

# New Species Allocation

When a new Species is recognized:

- ParentSpeciesId = candidate organism current SpeciesId
- Origin Tick = current simulation tick
- Extinction Tick = null
- Founder Population = candidate organism identifier only

The new SpeciesId sequence is assigned deterministically:

- resolve the highest existing SpeciesId sequence
- allocate the next sequence value
- if multiple organisms create new Species during the same evaluation pass, allocation follows organism identifier order

This guarantees stable Species trees across identical runs.

---

# Configuration

Recognition thresholds are configuration-driven.

Required configuration values:

- Evaluation Frequency
- Minimum Genome Similarity
- Minimum Trait Similarity
- Minimum Morphology Similarity
- Minimum Reproductive Compatibility
- Required Failed Metric Count

Hardcoded recognition thresholds are forbidden.

---

# Extinction

A species becomes extinct when no living organisms remain.

Species records are never deleted.

Historical information remains available.

---

# Lineage

Every species stores:

- SpeciesId
- ParentSpeciesId
- Origin Tick
- Extinction Tick (optional)
- Founder Population

This enables complete evolutionary trees.

---

# Evolution Tree

Species relationships form a directed tree.

Future tools may visualize this tree in the Encyclopedia.

---

# Determinism

Species recognition must be deterministic.

Given the same simulation history, the same species tree must always be produced.

---

# Future Extensions

Future versions may support:

- hybrid species
- ring species
- convergent evolution
- adaptive radiation

---

# Related Documents

GEN-002 — Genome

GEN-004 — Mutation

GEN-005 — Inheritance

ORG-001 — Organism

---

# Acceptance Criteria

- [ ] Species emerge automatically.
- [ ] Lineages are preserved.
- [ ] Extinct species remain in history.
- [ ] Classification is deterministic.
- [ ] Species never contain behaviour.

---

# Revision History

## 1.1.0

Defined deterministic recognition metrics, founder reference resolution, neutral morphology comparison and configurable threshold rules.

## 1.0.0

Initial version.
