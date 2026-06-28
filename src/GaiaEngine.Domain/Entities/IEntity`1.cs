namespace GaiaEngine.Domain.Entities;

/// <summary>
/// Defines a persistent domain entity with an immutable identity.
/// </summary>
/// <typeparam name="TId">The immutable identifier type.</typeparam>
public interface IEntity<TId>
    where TId : struct
{
    /// <summary>
    /// Gets the immutable entity identity.
    /// </summary>
    public TId Id { get; }
}
