using System;
using System.Collections.Generic;
using System.Linq;

namespace EMRG.Console.Helpers
{
    public static class RandomExtensions
    {
        public static T NextListElement<T>(this Random random, List<T> elements)
        {
            var index = random.Next(0, elements.Count);
            return elements.ElementAt(index);
        }

        public static int NextPercent(this Random random)
        {
            return random.Next(0, 101);
        }
    }
}
