using System;
using System.Globalization;

namespace GaiaEngine.Engine.Identifiers;

/// <summary>
/// Provides shared helpers for building deterministic typed identifier values.
/// </summary>
internal static class IdentifierValue
{
    public static ulong Create(IdentifierCategory category, EntitySequence sequence)
    {
        return ((ulong)category << 56) | sequence.Value;
    }

    public static EntitySequence ExtractSequence(ulong value)
    {
        return new EntitySequence(value & EntitySequence.MAX_VALUE);
    }

    public static string Format(ulong value)
    {
        return value.ToString(CultureInfo.InvariantCulture);
    }

    public static ulong Parse(string value, IdentifierCategory expectedCategory)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("The identifier value must contain a value.", nameof(value));
        }

        bool parsed = ulong.TryParse(value, NumberStyles.None, CultureInfo.InvariantCulture, out ulong parsedValue);
        if (!parsed)
        {
            throw new FormatException("The identifier value must be an unsigned integer.");
        }

        ValidateCategory(parsedValue, expectedCategory);
        return parsedValue;
    }

    public static void ValidateCategory(ulong value, IdentifierCategory expectedCategory)
    {
        IdentifierCategory actualCategory = (IdentifierCategory)(value >> 56);
        if (actualCategory != expectedCategory)
        {
            throw new ArgumentException("The identifier value does not match the expected category.", nameof(value));
        }
    }
}
