using System;
using System.Collections.Generic;
using System.Linq;

namespace WebApplication1
{
    public static class DictionaryExtensions
    {
        public static TValue GetOrAdd<TKey, TValue>(this Dictionary<TKey, TValue> @this, TKey key, Func<TValue> create)
        {
            return @this.ContainsKey(key) ? @this[key] : (@this[key] = create());
        }

        public static TValue GetOrAdd<TKey, TValue>(this Dictionary<TKey, TValue> @this, TKey key)
            where TValue : new()
        {
            return GetOrAdd(@this, key, () => new TValue());
        }
    }
}