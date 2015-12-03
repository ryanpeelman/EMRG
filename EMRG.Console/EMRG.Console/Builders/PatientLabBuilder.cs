using EMRG.Console.Helpers;
using EMRG.Console.Models;
using Ploeh.AutoFixture;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EMRG.Console.Builders
{
    internal class PatientLabBuilder
    {
        public const int ChanceOfHavingFavoredLab = 30;

        private Dictionary<string, Func<Random, string>> _favoredLabs;

        public static PatientLabBuilder Instance { get; } = new PatientLabBuilder();

        private PatientLabBuilder()
        {
            _favoredLabs = new Dictionary<string, Func<Random, string>>()
            {
                ["A1C[NORMAL]"] = randomizer => randomizer.Next(4, 9).ToString(),
                ["A1C[ABNORMAL]"] = randomizer => randomizer.Next(8, 14).ToString()
            };
        }

        public IEnumerable<PatientLab> GetPatientLabs(Fixture fixture, Random randomizer, PatientDemographics demographic, int maxNumberOfPatientLabs)
        {
            fixture.Customize<PatientLab>(pl => pl.With(x => x.PatientId, demographic.PatientId));

            var labs = fixture.CreateMany<PatientLab>(randomizer.Next(0, maxNumberOfPatientLabs + 1)).OrderBy(x => x.LabDate);
            foreach (var lab in labs)
            {
                var hasFavoredLab = randomizer.NextPercent() <= ChanceOfHavingFavoredLab;
                if (hasFavoredLab)
                {
                    var pair = randomizer.NextDictionaryPair(_favoredLabs);
                    lab.LabName = pair.Key.Replace("[NORMAL]", string.Empty).Replace("[ABNORMAL]", string.Empty);
                    lab.Value = pair.Value.Invoke(randomizer);
                }
            }

            return labs;
        }
    }
}
