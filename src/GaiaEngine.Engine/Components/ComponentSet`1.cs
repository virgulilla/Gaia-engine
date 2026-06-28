using System;
using System.Collections.Generic;

namespace GaiaEngine.Engine.Components;

/// <summary>
/// Stores data-only components owned by a single entity using stable type ordering.
/// </summary>
/// <typeparam name="TId">The owning entity identifier type.</typeparam>
public sealed class ComponentSet<TId> : IComponentSet<TId>
    where TId : struct
{
    private readonly SortedDictionary<string, IComponent<TId>> components = new(StringComparer.Ordinal);

    /// <summary>
    /// Initializes a new instance of the <see cref="ComponentSet{TId}"/> class.
    /// </summary>
    /// <param name="ownerId">The identifier of the entity that owns the set.</param>
    public ComponentSet(TId ownerId)
    {
        OwnerId = ownerId;
    }

    /// <summary>
    /// Gets the identifier of the entity that owns the components.
    /// </summary>
    public TId OwnerId { get; }

    /// <summary>
    /// Gets the number of registered component types.
    /// </summary>
    public int Count => components.Count;

    /// <summary>
    /// Registers or replaces a component in the set.
    /// </summary>
    /// <typeparam name="TComponent">The component type.</typeparam>
    /// <param name="component">The component instance to register.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="component"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException">Thrown when the component belongs to a different owner.</exception>
    public void SetComponent<TComponent>(TComponent component)
        where TComponent : class, IComponent<TId>
    {
        ArgumentNullException.ThrowIfNull(component);

        if (!EqualityComparer<TId>.Default.Equals(component.OwnerId, OwnerId))
        {
            throw new ArgumentException("The component owner does not match the owning entity.", nameof(component));
        }

        components[GetComponentKey<TComponent>()] = component;
    }

    /// <summary>
    /// Tries to get a component from the set.
    /// </summary>
    /// <typeparam name="TComponent">The component type.</typeparam>
    /// <param name="component">The resolved component when present.</param>
    /// <returns><see langword="true"/> when the component exists; otherwise <see langword="false"/>.</returns>
    public bool TryGetComponent<TComponent>(out TComponent? component)
        where TComponent : class, IComponent<TId>
    {
        bool found = components.TryGetValue(GetComponentKey<TComponent>(), out IComponent<TId>? rawComponent);
        component = found ? (TComponent)rawComponent! : null;
        return found;
    }

    /// <summary>
    /// Gets a component from the set.
    /// </summary>
    /// <typeparam name="TComponent">The component type.</typeparam>
    /// <returns>The resolved component.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when the component type is not registered.</exception>
    public TComponent GetComponent<TComponent>()
        where TComponent : class, IComponent<TId>
    {
        if (!TryGetComponent<TComponent>(out TComponent? component))
        {
            throw new KeyNotFoundException("The requested component type is not registered in the set.");
        }

        return component!;
    }

    /// <summary>
    /// Removes a component from the set.
    /// </summary>
    /// <typeparam name="TComponent">The component type.</typeparam>
    /// <returns><see langword="true"/> when the component existed; otherwise <see langword="false"/>.</returns>
    public bool RemoveComponent<TComponent>()
        where TComponent : class, IComponent<TId>
    {
        return components.Remove(GetComponentKey<TComponent>());
    }

    /// <summary>
    /// Returns the registered components in deterministic order.
    /// </summary>
    /// <returns>The registered components.</returns>
    public IReadOnlyList<IComponent<TId>> GetComponents()
    {
        return components.Values.AsReadOnlyList();
    }

    private static string GetComponentKey<TComponent>()
        where TComponent : class, IComponent<TId>
    {
        return typeof(TComponent).FullName ?? typeof(TComponent).Name;
    }
}
