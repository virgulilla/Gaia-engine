namespace GaiaEngine.Domain.Entities;

/// <summary>
/// Defines a domain entity that acts as an aggregate root.
/// </summary>
/// <typeparam name="TId">The immutable identifier type.</typeparam>
public interface IAggregateRoot<TId> : IEntity<TId>
    where TId : struct
{
}
