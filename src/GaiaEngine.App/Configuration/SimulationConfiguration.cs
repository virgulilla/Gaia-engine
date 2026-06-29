using System;

namespace GaiaEngine.App.Configuration;

/// <summary>
/// Represents the immutable startup configuration of the Gaia Engine simulation module.
/// </summary>
public sealed record SimulationConfiguration
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SimulationConfiguration"/> class.
    /// </summary>
    /// <param name="ticksPerDay">The number of deterministic simulation ticks contained in one world day.</param>
    /// <param name="daysPerSeason">The number of world days contained in one season.</param>
    /// <param name="startingDay">The initial world day.</param>
    /// <param name="startingSeason">The initial season name.</param>
    /// <param name="startingYear">The initial world year.</param>
    /// <param name="mutation">The immutable mutation configuration.</param>
    /// <param name="speciesRecognition">The immutable species recognition configuration.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when numeric values are outside their valid ranges.</exception>
    /// <exception cref="ArgumentException">Thrown when <paramref name="startingSeason"/> is empty.</exception>
    public SimulationConfiguration(
        int ticksPerDay,
        int daysPerSeason,
        int startingDay,
        string startingSeason,
        int startingYear,
        MutationConfiguration mutation,
        SpeciesRecognitionConfiguration speciesRecognition)
    {
        if (ticksPerDay <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(ticksPerDay), "The ticks-per-day value must be greater than zero.");
        }

        if (daysPerSeason <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(daysPerSeason), "The days-per-season value must be greater than zero.");
        }

        if (startingDay < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(startingDay), "The starting day must be zero or greater.");
        }

        if (string.IsNullOrWhiteSpace(startingSeason))
        {
            throw new ArgumentException("The starting season must contain a value.", nameof(startingSeason));
        }

        if (startingYear < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(startingYear), "The starting year must be zero or greater.");
        }

        TicksPerDay = ticksPerDay;
        DaysPerSeason = daysPerSeason;
        StartingDay = startingDay;
        StartingSeason = startingSeason;
        StartingYear = startingYear;
        Mutation = mutation ?? throw new ArgumentNullException(nameof(mutation));
        SpeciesRecognition = speciesRecognition ?? throw new ArgumentNullException(nameof(speciesRecognition));
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SimulationConfiguration"/> class.
    /// </summary>
    /// <param name="ticksPerDay">The number of deterministic simulation ticks contained in one world day.</param>
    /// <param name="daysPerSeason">The number of world days contained in one season.</param>
    /// <param name="startingDay">The initial world day.</param>
    /// <param name="startingSeason">The initial season name.</param>
    /// <param name="startingYear">The initial world year.</param>
    public SimulationConfiguration(
        int ticksPerDay,
        int daysPerSeason,
        int startingDay,
        string startingSeason,
        int startingYear)
        : this(ticksPerDay, daysPerSeason, startingDay, startingSeason, startingYear, MutationConfiguration.Default, SpeciesRecognitionConfiguration.Default)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SimulationConfiguration"/> class.
    /// </summary>
    /// <param name="ticksPerDay">The number of deterministic simulation ticks contained in one world day.</param>
    /// <param name="daysPerSeason">The number of world days contained in one season.</param>
    /// <param name="startingDay">The initial world day.</param>
    /// <param name="startingSeason">The initial season name.</param>
    /// <param name="startingYear">The initial world year.</param>
    /// <param name="mutation">The immutable mutation configuration.</param>
    public SimulationConfiguration(
        int ticksPerDay,
        int daysPerSeason,
        int startingDay,
        string startingSeason,
        int startingYear,
        MutationConfiguration mutation)
        : this(ticksPerDay, daysPerSeason, startingDay, startingSeason, startingYear, mutation, SpeciesRecognitionConfiguration.Default)
    {
    }

    /// <summary>
    /// Gets the number of deterministic simulation ticks contained in one world day.
    /// </summary>
    public int TicksPerDay { get; }

    /// <summary>
    /// Gets the number of world days contained in one season.
    /// </summary>
    public int DaysPerSeason { get; }

    /// <summary>
    /// Gets the initial world day.
    /// </summary>
    public int StartingDay { get; }

    /// <summary>
    /// Gets the initial season name.
    /// </summary>
    public string StartingSeason { get; }

    /// <summary>
    /// Gets the initial world year.
    /// </summary>
    public int StartingYear { get; }

    /// <summary>
    /// Gets the immutable mutation configuration.
    /// </summary>
    public MutationConfiguration Mutation { get; }

    /// <summary>
    /// Gets the immutable species recognition configuration.
    /// </summary>
    public SpeciesRecognitionConfiguration SpeciesRecognition { get; }
}
