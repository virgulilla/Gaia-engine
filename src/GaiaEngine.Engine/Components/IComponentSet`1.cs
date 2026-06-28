using System.Collections.Generic;

namespace GaiaEngine.Engine.Components;

/// <summary>
/// Represents a deterministic collection of components owned by a single entity.
/// </summary>
/// <typeparam name="TId">The owning entity identifier type.</typeparam>
public interface IComponentSet<TId>
    where TId : struct
{
    /// <summary>
    /// Gets the identifier of the entity that owns the components.
    /// </summary>
    public TId OwnerId { get; }

    /// <summary>
    /// Gets the number of registered component types.
    /// </summary>
    public int Count { get; }

    /// <summary>
    /// Registers or replaces a component in the set.
    /// </summary>
    /// <typeparam name="TComponent">The component type.</typeparam>
    /// <param name="component">The component instance to register.</param>
    public void SetComponent<TComponent>(TComponent component)
        where TComponent : class, IComponent<TId>;

    /// <summary>
    /// Tries to get a component from the set.
    /// </summary>
    /// <typeparam name="TComponent">The component type.</typeparam>
    /// <param name="component">The resolved component when present.</param>
    /// <returns><see langword="true"/> when the component exists; otherwise <see langword="false"/>.</returns>
    public bool TryGetComponent<TComponent>(out TComponent? component)
        where TComponent : class, IComponent<TId>;

    /// <summary>
    /// Gets a component from the set.
    /// </summary>
    /// <typeparam name="TComponent">The component type.</typeparam>
    /// <returns>The resolved component.</returns>
    public TComponent GetComponent<TComponent>()
        where TComponent : class, IComponent<TId>;

    /// <summary>
    /// Removes a component from the set.
    /// </summary>
    /// <typeparam name="TComponent">The component type.</typeparam>
    /// <returns><see langword="true"/> when the component existed; otherwise <see langword="false"/>.</returns>
    public bool RemoveComponent<TComponent>()
        where TComponent : class, IComponent<TId>;

    /// <summary>
    /// Returns the registered components in deterministic order.
    /// </summary>
    /// <returns>The registered components.</returns>
    public IReadOnlyList<IComponent<TId>> GetComponents();
}
