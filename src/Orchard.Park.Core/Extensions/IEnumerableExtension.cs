using System;
using System.Collections.Generic;
using System.Linq;

namespace Orchard.Park.Core.Extensions;

public static class IEnumerableExtension
{
    public static int PatternAt<T>(this T[] source, IReadOnlyCollection<T> pattern) where T : IEquatable<T>
    {
        var segment = new ArraySegment<T>(source);

        for (var i = 0; i < source.Length - pattern.Count + 1; i++)
        {
            if (segment.Slice(i, pattern.Count).SequenceEqual(pattern))
            {
                return i;
            }
        }

        return -1;
    }

    public static void UpdateValues<TSource, TTarget>(this List<TSource> sourceData,
        List<TTarget> targetData,
        Func<TSource, TTarget, bool> wherePredicate,
        Action<TSource, TTarget> updater)
    {
        if (sourceData == null || sourceData.Count < 0) return;
        if (targetData == null || targetData.Count < 0) return;

        foreach (var item in sourceData)
        {
            var matchingTargetData = targetData.FirstOrDefault(target => wherePredicate(item, target));
            if (matchingTargetData == null)
                continue;

            updater(item, matchingTargetData);
        }
    }
}