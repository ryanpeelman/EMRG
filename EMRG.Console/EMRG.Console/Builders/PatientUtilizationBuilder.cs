using EMRG.Console.Models;
using Ploeh.AutoFixture;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EMRG.Console.Builders
{
    internal class PatientUtilizationBuilder
    {
        public static PatientUtilizationBuilder Instance { get; } = new PatientUtilizationBuilder();

        private PatientUtilizationBuilder() { }

        public IEnumerable<PatientUtilization> GetPatientUtilizations(Fixture fixture, Random randomizer, PatientDemographics demographic, int maxNumberOfPatientUtilizations)
        {
            fixture.Customize<PatientUtilization>(pu => pu.With(x => x.PatientId, demographic.PatientId));

            var utilizations = fixture.CreateMany<PatientUtilization>(randomizer.Next(0, maxNumberOfPatientUtilizations + 1));

            return utilizations.OrderBy(x => x.ActivityDate);
        }
    }
}
