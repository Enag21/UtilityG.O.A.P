using System;
using System.Collections.Generic;

namespace UGOAP.CommonUtils.ExtensionMethods;

public static class EnumerableExtensions
{
    // Iterates through each element in the enumerable and performs the specified action on each element.
    //
    // Parameters:
    //   enumerable: The IEnumerable to iterate through.
    //   action: The action to perform on each element.
    //
    // Returns:
    //   void
    public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
    {
        foreach (var item in enumerable)
        {
            action(item);
        }
    }
}