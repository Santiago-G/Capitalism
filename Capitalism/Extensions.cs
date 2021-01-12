using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capitalism
{
    public static class Extensions
    {
        static Random gen = new Random();
        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> enumerable)
        {
            var list = new List<T>(enumerable);

            for (int i = 0; i < list.Count; i++)
            {
                int newIndex = gen.Next(i, list.Count);

                // swap whatever is at i with newindex

                var temp = list[i];
                list[i] = list[newIndex];
                list[newIndex] = temp;

            }

            return list;

        }
    }
}
