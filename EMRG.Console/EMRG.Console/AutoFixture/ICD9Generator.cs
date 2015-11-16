using Ploeh.AutoFixture.Kernel;
using System;

namespace EMRG.Console.AutoFixture
{
    class ICD9Generator : ISpecimenBuilder
    {
        private readonly Random _randomizer = new Random();

        public object Create(object request, ISpecimenContext context)
        {
            var randomValue = _randomizer.NextDouble() * 1000;
            return randomValue.ToString("000.0");
        }
    }
}
