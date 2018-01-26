using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pure.DataMining
{
    public static class DictionaryExtension
    {
        public static void AddOrIncrease<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key,
                                                       TValue initValue,
                                                       Func<TValue, TValue> increaseValue)
        {
            if (dict.ContainsKey(key))
            {
                dict[key] = increaseValue(dict[key]);
            }
            else
            {
                dict.Add(key, initValue);
            }
        }

        public static void AddOrIncrease<TKey>(this IDictionary<TKey, int> dict, TKey key)
        {
            AddOrIncrease(dict, key, 1, o => o + 1);
        }

        public static void AddOrIncrease<TKey>(this IDictionary<TKey, long> dict, TKey key)
        {
            AddOrIncrease(dict, key, 1, o => o + 1);
        }
    }
}
