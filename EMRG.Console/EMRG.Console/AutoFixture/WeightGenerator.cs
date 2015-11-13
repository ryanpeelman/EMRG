using System;

namespace EMRG.Console.AutoFixture
{
    class WeightGenerator
    {
        private const double MIN_WEIGHT = 45.0;
        private const double MAX_WEIGHT = 135.0;
        private const double MAX_DELTA = 10.0;

        private static readonly Random _randomizer = new Random();

        public static double GetWeight(double? baseWeight = null)
        {
            var direction = _randomizer.NextDouble() > 0.5 ? 1 : -1;
            var delta = _randomizer.NextDouble() * MAX_DELTA * direction;
            return Math.Round((baseWeight ?? _randomizer.NextDouble() * (MAX_WEIGHT - MIN_WEIGHT) + MIN_WEIGHT) + delta, 2);
        }
    }
}
