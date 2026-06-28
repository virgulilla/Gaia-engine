using System;
using System.Collections.Generic;
using GaiaEngine.Engine.Components;
using GaiaEngine.Engine.Identifiers;
using Xunit;

namespace GaiaEngine.Engine.Tests.Components;

public sealed class ComponentSetTests
{
    [Fact]
    public void SetComponent_ShouldStoreAndResolveTheComponent()
    {
        OrganismId ownerId = OrganismId.FromSequence(new EntitySequence(1));
        ComponentSet<OrganismId> componentSet = new(ownerId);
        HealthComponent component = new(ownerId, 10, 20);

        componentSet.SetComponent(component);

        HealthComponent resolved = componentSet.GetComponent<HealthComponent>();

        Assert.Equal(component, resolved);
        Assert.Equal(1, componentSet.Count);
    }

    [Fact]
    public void SetComponent_ShouldRejectComponentsWithAnotherOwner()
    {
        OrganismId ownerId = OrganismId.FromSequence(new EntitySequence(1));
        OrganismId otherOwnerId = OrganismId.FromSequence(new EntitySequence(2));
        ComponentSet<OrganismId> componentSet = new(ownerId);

        Assert.Throws<ArgumentException>(() => componentSet.SetComponent(new HealthComponent(otherOwnerId, 10, 20)));
    }

    [Fact]
    public void SetComponent_ShouldReplaceThePreviousComponentOfTheSameType()
    {
        OrganismId ownerId = OrganismId.FromSequence(new EntitySequence(1));
        ComponentSet<OrganismId> componentSet = new(ownerId);

        componentSet.SetComponent(new HealthComponent(ownerId, 10, 20));
        componentSet.SetComponent(new HealthComponent(ownerId, 15, 25));

        HealthComponent component = componentSet.GetComponent<HealthComponent>();

        Assert.Equal(15, component.CurrentValue);
        Assert.Equal(1, componentSet.Count);
    }

    [Fact]
    public void GetComponents_ShouldReturnComponentsInStableTypeOrder()
    {
        OrganismId ownerId = OrganismId.FromSequence(new EntitySequence(1));
        ComponentSet<OrganismId> componentSet = new(ownerId);

        componentSet.SetComponent(new PhysiologyComponent(ownerId, 5));
        componentSet.SetComponent(new HealthComponent(ownerId, 10, 20));

        IReadOnlyList<IComponent<OrganismId>> components = componentSet.GetComponents();

        Assert.IsType<HealthComponent>(components[0]);
        Assert.IsType<PhysiologyComponent>(components[1]);
    }

    [Fact]
    public void RemoveComponent_ShouldDeleteTheRegisteredType()
    {
        OrganismId ownerId = OrganismId.FromSequence(new EntitySequence(1));
        ComponentSet<OrganismId> componentSet = new(ownerId);
        componentSet.SetComponent(new HealthComponent(ownerId, 10, 20));

        bool removed = componentSet.RemoveComponent<HealthComponent>();

        Assert.True(removed);
        Assert.False(componentSet.TryGetComponent<HealthComponent>(out _));
    }

    private sealed record HealthComponent(OrganismId OwnerId, int CurrentValue, int MaximumValue) : IComponent<OrganismId>;

    private sealed record PhysiologyComponent(OrganismId OwnerId, int BodyMass) : IComponent<OrganismId>;
}
