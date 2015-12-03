using EMRG.Console.Helpers;
using EMRG.Console.Models;
using Ploeh.AutoFixture;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EMRG.Console
{
    internal class PatientTherapyBuilder
    {
        public const int ChanceOfHavingFavoredCondition = 80;

        private Dictionary<string, string> _favoredTherapies;

        public static PatientTherapyBuilder Instance { get; } = new PatientTherapyBuilder();

        private PatientTherapyBuilder()
        {
            _favoredTherapies = new Dictionary<string, string>()
            {
                ["21695-198"] = "ALBUTEROL",
                ["0310-0800"] = "ACLIDINIUM BROMIDE",
                ["69097-173"] = "IPRATROPIUM-ALBUTEROL",
                ["63402-911"] = "ARFORMOTEROL TARTRATE",
                ["59310-202"] = "BECLOMETHASONE DIPROPIONATE",
                ["0186-0702"] = "BUDESONIDE-FORMOTEROL FUMARATE",
                ["10122-901"] = "ZILEUTON",
                ["0143-1020"] = "AMINOPHYLLINE",
                ["0037-0678"] = "CROMOLYN SODIUM",
                ["0023-8842"] = "NEDOCROMIL SODIUM",
                ["50242-040"] = "OMALIZUMAB",
                ["69299-202"] = "BETAMETHASONE"
            };
        }

        public IEnumerable<PatientTherapy> GetPatientTherapies(Fixture fixture, Random randomizer, PatientDemographics demographic, int maxNumberOfPatientTherapies)
        {
            fixture.Customize<PatientTherapy>(pt => pt.With(x => x.PatientId, demographic.PatientId)
                                                      .Without(x => x.DDID)
                                                      .Without(x => x.RXNorm)
                                                      .Without(x => x.StopDate));

            var therapies = fixture.CreateMany<PatientTherapy>(randomizer.Next(0, maxNumberOfPatientTherapies + 1));
            foreach (var therapy in therapies)
            {
                var daysOnTherapy = randomizer.Next(1, 365);
                therapy.StopDate = new DateTime(Math.Min(therapy.StartDate.AddDays(daysOnTherapy).Ticks, DateTime.Now.Ticks));

                var hasFavoredTherapy = randomizer.NextPercent() <= ChanceOfHavingFavoredCondition;
                if (hasFavoredTherapy)
                {
                    var pair = randomizer.NextDictionaryPair(_favoredTherapies);
                    therapy.NDC = pair.Key;
                    therapy.DrugName = pair.Value;
                }
                else
                {
                    therapy.NDC = NDCGenerator.GetRandomNDC(randomizer);
                }
            }

            return therapies.OrderBy(x => x.StartDate);
        }
    }
}
