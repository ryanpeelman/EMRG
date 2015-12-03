using EMRG.Console.Models;
using Ploeh.AutoFixture;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EMRG.Console
{
    internal class ClaimsUtilizationBuilder
    {
        public static ClaimsUtilizationBuilder Instance { get; } = new ClaimsUtilizationBuilder();

        private ClaimsUtilizationBuilder() { }

        public IEnumerable<ClaimsUtilization> GetClaimsUtilizations(Fixture fixture, Random randomizer, PatientDemographics demographic, bool alcoholAbuse, bool drugAbuse, int maxNumberOfClaimsUtilizations)
        {
            fixture.Customize<ClaimsUtilization>(cu => cu.With(x => x.PatientId, demographic.PatientId)
                                                         .With(x => x.Age, demographic.Age)
                                                         .With(x => x.Gender, demographic.Gender)
                                                         .With(x => x.Region, demographic.Region)
                                                         .With(x => x.AlcoholAbuse, alcoholAbuse)
                                                         .With(x => x.DrugAbuse, drugAbuse)
                                                         .Without(x => x.ICD10));
            var claimsUtilizations = fixture.CreateMany<ClaimsUtilization>(randomizer.Next(0, maxNumberOfClaimsUtilizations + 1));

            return claimsUtilizations.OrderBy(x => x.ActivityDate);
        }
    }
}
