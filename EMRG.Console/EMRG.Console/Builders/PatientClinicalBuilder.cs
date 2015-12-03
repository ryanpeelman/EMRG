using EMRG.Console.Models;
using Ploeh.AutoFixture;
using System;
using System.Collections.Generic;
using System.Linq;
using EMRG.Console.Helpers;

namespace EMRG.Console.Builders
{
    internal class PatientClinicalBuilder
    {
        public const double AverageHeightInMeters = 1.75;

        public static PatientClinicalBuilder Instance { get; } = new PatientClinicalBuilder();

        private PatientClinicalBuilder() { }

        public IEnumerable<PatientClinical> GetPatientClinicals(Fixture fixture, Random randomizer, PatientDemographics demographic, bool alcoholAbuse, bool drugAbuse, bool smoker, int maxNumberOfPatientClinicals)
        {
            fixture.Customize<PatientClinical>(pc => pc.With(x => x.PatientId, demographic.PatientId)
                                                       .With(x => x.AlcoholAbuse, alcoholAbuse)
                                                       .With(x => x.DrugAbuse, drugAbuse)
                                                       .With(x => x.Smoker, smoker)
                                                       .With(x => x.Weight, WeightGenerator.GetWeight())
                                                       .Without(x => x.BMI));

            var clinicals = fixture.CreateMany<PatientClinical>(randomizer.Next(0, maxNumberOfPatientClinicals + 1)).ToList();
            foreach(var clinical in clinicals)
            {
                clinical.Weight = WeightGenerator.GetWeight(clinical.Weight);
                clinical.BMI = Math.Round(clinical.Weight / Math.Pow(AverageHeightInMeters, 2), 2);
            }

            return clinicals.OrderBy(x => x.ObservationDate);
        }
    }
}
