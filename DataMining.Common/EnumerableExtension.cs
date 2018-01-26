using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pure.DataMining
{
    public static class EnumerableExtension
    {
        public static HashSet<TSource> ToHashSet<TSource>(this IEnumerable<TSource> source)
        {
            return new HashSet<TSource>(source);
        }

        public static bool IsExactSingle<TSource>(this IEnumerable<TSource> source)
        {
            return source.Any() && !source.Skip(1).Any();
        }
    }
}
