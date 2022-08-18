﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace AutoForm.Analysis
{
    internal static class Extensions
    {
        public static void ThrowOnDuplicate<T>(this IEnumerable<T> values, String name)
        {
            EqualityComparer<T> comparer = EqualityComparer<T>.Default;
            T duplicate = values.FirstOrDefault(v1 => values.Where(v2 => comparer.Equals(v1, v2)).Count() > 1);
            if (!comparer.Equals(duplicate, default))
            {
                throw new ArgumentException($"Cannot register {name} {duplicate} multiple times.");
            }
        }
    }
}
