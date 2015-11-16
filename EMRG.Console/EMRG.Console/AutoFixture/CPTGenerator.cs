using Ploeh.AutoFixture.Kernel;
using System;

namespace EMRG.Console.AutoFixture
{
    class CPTGenerator : ISpecimenBuilder
    {
        private readonly Random _randomizer = new Random();

        public object Create(object request, ISpecimenContext context)
        {
            var randomValue = _randomizer.Next(100, 99500);
            return randomValue.ToString("00000");
        }
    }
}
