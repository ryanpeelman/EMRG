using System;
using System.Collections.Generic;
using System.Linq;
using Ploeh.AutoFixture.Kernel;

namespace EMRG.Console.AutoFixture
{
    class ConstrainedValueGenerator : ISpecimenBuilder
    {
        private readonly Random _randomizer = new Random();
        private readonly IEnumerable<string> _values;

        public ConstrainedValueGenerator(IEnumerable<string> values)
        {
            _values = values;
        }

        public object Create(object request, ISpecimenContext context)
        {
            var randomIndex = _randomizer.Next(0, _values.Count());
            return _values.ElementAt(randomIndex);
        }
    }
}
