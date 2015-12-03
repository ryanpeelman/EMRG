using System;

namespace EMRG.Console.Helpers
{
    internal class NDCGenerator
    {
        public static string GetRandomNDC(Random randomizer)
        {
            var first = randomizer.Next(1, 100000);
            var second = randomizer.Next(1, 10000);
            return $"{first.ToString("#0000")}-{second.ToString("0000")}";
        }
    }
}
