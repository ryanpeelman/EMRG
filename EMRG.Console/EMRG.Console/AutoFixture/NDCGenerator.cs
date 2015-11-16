using Ploeh.AutoFixture.Kernel;
using System;

namespace EMRG.Console.AutoFixture
{
    class NDCGenerator : ISpecimenBuilder
    {
        private readonly Random _randomizer = new Random();

        public object Create(object request, ISpecimenContext context)
        {
            var first = _randomizer.Next(1, 100000);
            var second = _randomizer.Next(1, 10000);
            return $"{first.ToString("#0000")}-{second.ToString("0000")}";
        }
    }
}
