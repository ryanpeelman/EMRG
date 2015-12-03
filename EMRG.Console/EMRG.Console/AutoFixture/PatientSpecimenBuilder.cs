using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;
using System;
using System.Reflection;

namespace EMRG.Console.AutoFixture
{
    class PatientSpecimenBuilder : ISpecimenBuilder
    {
        private readonly ISpecimenBuilder _datetimeGenerator = new RandomDateTimeSequenceGenerator(DateTime.Today.AddYears(-10), DateTime.Today);

        public object Create(object request, ISpecimenContext context)
        {
            var info = request as PropertyInfo;
            if (info != null && info.PropertyType == typeof(DateTime))
            {
                return _datetimeGenerator.Create(info.PropertyType, context);
            }

            return new NoSpecimen(request);
        }
    }
}
