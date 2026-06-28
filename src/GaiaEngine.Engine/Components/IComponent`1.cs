namespace GaiaEngine.Engine.Components;

/// <summary>
/// Defines a data-only component owned by a specific entity identifier.
/// </summary>
/// <typeparam name="TId">The owning entity identifier type.</typeparam>
public interface IComponent<TId> : IComponent
    where TId : struct
{
    /// <summary>
    /// Gets the identifier of the entity that owns the component.
    /// </summary>
    public TId OwnerId { get; }
}
