using System.Collections.Generic;

namespace GaiaEngine.Engine.Components;

/// <summary>
/// Provides helpers for exposing deterministic read-only component views.
/// </summary>
internal static class ReadOnlyListExtensions
{
    public static IReadOnlyList<T> AsReadOnlyList<T>(this ICollection<T> source)
    {
        List<T> list = new(source.Count);
        foreach (T item in source)
        {
            list.Add(item);
        }

        return list;
    }
}
