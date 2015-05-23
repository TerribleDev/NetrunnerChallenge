using System;
using System.Collections.Generic;
using System.Linq;
using WebGrease.Css.Extensions;

namespace NetrunnerChallenge.Extensions
{
    public static class CollectionExtension
    {
        public static T RandomElement<T>(this IEnumerable<T> enumerable)
        {
            return enumerable.RandomElementUsing(new Random());
        }

        public static T RandomElementUsing<T>(this IEnumerable<T> enumerable, Random rand)
        {
            var enumerable1 = enumerable as IList<T> ?? enumerable.ToList();
            var index = rand.Next(0, enumerable1.Count);
            return enumerable1.ElementAt(index);
        }

        public static int Combine(this IEnumerable<int> em)
        {
            var total = 0;
            em.ForEach(a=> { total += a; });
            return total;
        }
    }
}