using System.Collections.Generic;
using System.Reflection;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;

namespace EMRG.Console.AutoFixture
{
    public class PatientDemographicsSpecimenBuilder : ISpecimenBuilder
    {
        private readonly Dictionary<string, ISpecimenBuilder> _builders = new Dictionary<string, ISpecimenBuilder>();

        public PatientDemographicsSpecimenBuilder()
        {
            _builders.Add("Age", new RandomNumericSequenceGenerator(18, 89));
            _builders.Add("Gender", new ConstrainedValueGenerator(new List<string> { "Female", "Male" }));
            _builders.Add("Race", new ConstrainedValueGenerator(new List<string> { "American Indian and Alaska Native", "Asian", "Black or African American", "Native Hawaiian and Other Pacific Islander", "White" }));
            _builders.Add("Region", new ConstrainedValueGenerator(new List<string> { "North", "South", "East", "West", "Central" }));
        }

        public object Create(object request, ISpecimenContext context)
        {
            var info = request as PropertyInfo;
            if (info != null && _builders.ContainsKey(info.Name))
            {
                return _builders[info.Name].Create(info.PropertyType, context);
            }

            return new NoSpecimen(request);
        }
    }
}
