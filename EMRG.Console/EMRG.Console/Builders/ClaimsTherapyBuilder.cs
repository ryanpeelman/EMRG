using EMRG.Console.Models;
using Ploeh.AutoFixture;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EMRG.Console.Builders
{
    internal class ClaimsTherapyBuilder
    {
        public static ClaimsTherapyBuilder Instance { get; } = new ClaimsTherapyBuilder();

        private ClaimsTherapyBuilder() { }

        public IEnumerable<ClaimsTherapy> GetClaimsTherapies(Fixture fixture, Random randomizer, PatientDemographics demographic, bool alcoholAbuse, bool drugAbuse, int maxNumberOfClaimsTherapies)
        {
            fixture.Customize<ClaimsTherapy>(ct => ct.With(x => x.PatientId, demographic.PatientId)
                                                     .With(x => x.Age, demographic.Age)
                                                     .With(x => x.Gender, demographic.Gender)
                                                     .With(x => x.Region, demographic.Region)
                                                     .With(x => x.AlcoholAbuse, alcoholAbuse)
                                                     .With(x => x.DrugAbuse, drugAbuse)
                                                     .Without(x => x.DDID)
                                                     .Without(x => x.RXNorm));

            var claimsTherapies = fixture.CreateMany<ClaimsTherapy>(randomizer.Next(0, maxNumberOfClaimsTherapies + 1)).OrderBy(x => x.StartDate);
            foreach (var claimTherapy in claimsTherapies)
            {
                var daysOnTherapy = randomizer.Next(1, 365);
                claimTherapy.StopDate = new DateTime(Math.Min(claimTherapy.StartDate.AddDays(daysOnTherapy).Ticks, DateTime.Now.Ticks));
            }

            return claimsTherapies.OrderBy(x => x.StartDate);
        }
    }
}
