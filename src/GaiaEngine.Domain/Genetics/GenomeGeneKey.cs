namespace GaiaEngine.Domain.Genetics;

/// <summary>
/// Identifies one supported deterministic genome gene.
/// </summary>
public enum GenomeGeneKey
{
    /// <summary>
    /// Morphology: body size.
    /// </summary>
    BodySize = 0,

    /// <summary>
    /// Morphology: limb count.
    /// </summary>
    LimbCount = 1,

    /// <summary>
    /// Morphology: neck length.
    /// </summary>
    NeckLength = 2,

    /// <summary>
    /// Morphology: tail length.
    /// </summary>
    TailLength = 3,

    /// <summary>
    /// Morphology: body shape.
    /// </summary>
    BodyShape = 4,

    /// <summary>
    /// Morphology: skeletal density.
    /// </summary>
    SkeletalDensity = 5,

    /// <summary>
    /// Morphology: muscle distribution.
    /// </summary>
    MuscleDistribution = 6,

    /// <summary>
    /// Physiology: metabolism.
    /// </summary>
    Metabolism = 100,

    /// <summary>
    /// Physiology: growth rate.
    /// </summary>
    GrowthRate = 101,

    /// <summary>
    /// Physiology: lifespan.
    /// </summary>
    Lifespan = 102,

    /// <summary>
    /// Physiology: heat resistance.
    /// </summary>
    HeatResistance = 103,

    /// <summary>
    /// Physiology: cold resistance.
    /// </summary>
    ColdResistance = 104,

    /// <summary>
    /// Physiology: water efficiency.
    /// </summary>
    WaterEfficiency = 105,

    /// <summary>
    /// Physiology: digestion efficiency.
    /// </summary>
    DigestionEfficiency = 106,

    /// <summary>
    /// Reproduction: maturity age.
    /// </summary>
    MaturityAge = 200,

    /// <summary>
    /// Reproduction: fertility.
    /// </summary>
    Fertility = 201,

    /// <summary>
    /// Reproduction: gestation time.
    /// </summary>
    GestationTime = 202,

    /// <summary>
    /// Reproduction: egg count.
    /// </summary>
    EggCount = 203,

    /// <summary>
    /// Reproduction: breeding cooldown.
    /// </summary>
    BreedingCooldown = 204,

    /// <summary>
    /// Senses: vision range.
    /// </summary>
    VisionRange = 300,

    /// <summary>
    /// Senses: night vision.
    /// </summary>
    NightVision = 301,

    /// <summary>
    /// Senses: smell sensitivity.
    /// </summary>
    SmellSensitivity = 302,

    /// <summary>
    /// Senses: hearing range.
    /// </summary>
    HearingRange = 303,

    /// <summary>
    /// Senses: threat detection.
    /// </summary>
    ThreatDetection = 304,

    /// <summary>
    /// Adaptation: desert adaptation.
    /// </summary>
    DesertAdaptation = 400,

    /// <summary>
    /// Adaptation: cold adaptation.
    /// </summary>
    ColdAdaptation = 401,

    /// <summary>
    /// Adaptation: wetland adaptation.
    /// </summary>
    WetlandAdaptation = 402,

    /// <summary>
    /// Adaptation: mountain adaptation.
    /// </summary>
    MountainAdaptation = 403,

    /// <summary>
    /// Adaptation: aquatic affinity.
    /// </summary>
    AquaticAffinity = 404,

    /// <summary>
    /// Appearance: primary color.
    /// </summary>
    PrimaryColor = 500,

    /// <summary>
    /// Appearance: secondary color.
    /// </summary>
    SecondaryColor = 501,

    /// <summary>
    /// Appearance: pattern.
    /// </summary>
    Pattern = 502,

    /// <summary>
    /// Appearance: fur density.
    /// </summary>
    FurDensity = 503,

    /// <summary>
    /// Appearance: scale density.
    /// </summary>
    ScaleDensity = 504,

    /// <summary>
    /// Appearance: horn shape.
    /// </summary>
    HornShape = 505,

    /// <summary>
    /// Appearance: ear shape.
    /// </summary>
    EarShape = 506,

    /// <summary>
    /// Behaviour bias: curiosity.
    /// </summary>
    Curiosity = 600,

    /// <summary>
    /// Behaviour bias: aggression bias.
    /// </summary>
    AggressionBias = 601,

    /// <summary>
    /// Behaviour bias: social bias.
    /// </summary>
    SocialBias = 602,

    /// <summary>
    /// Behaviour bias: risk tolerance.
    /// </summary>
    RiskTolerance = 603,

    /// <summary>
    /// Behaviour bias: exploration bias.
    /// </summary>
    ExplorationBias = 604,
}
