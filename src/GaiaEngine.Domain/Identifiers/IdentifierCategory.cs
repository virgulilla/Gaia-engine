namespace GaiaEngine.Domain.Identifiers;

/// <summary>
/// Defines the persistent identifier categories used by the engine.
/// </summary>
internal enum IdentifierCategory : byte
{
    World = 1,
    Chunk = 2,
    Organism = 3,
    Species = 4,
    Genome = 5,
    Resource = 6,
    Biome = 7,
    Event = 8,
    Action = 9,
}
