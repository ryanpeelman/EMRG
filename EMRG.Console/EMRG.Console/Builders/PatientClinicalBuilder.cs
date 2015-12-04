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
        public static PatientClinicalBuilder Instance { get; } = new PatientClinicalBuilder();

        private PatientClinicalBuilder() { }

        public IEnumerable<PatientClinical> GetPatientClinicals(Fixture fixture, Random randomizer, PatientDemographics demographic, bool alcoholAbuse, bool drugAbuse, bool smoker, bool obese, int maxNumberOfPatientClinicals)
        {
            fixture.Customize<PatientClinical>(pc => pc.With(x => x.PatientId, demographic.PatientId)
                                                       .With(x => x.AlcoholAbuse, alcoholAbuse)
                                                       .With(x => x.DrugAbuse, drugAbuse)
                                                       .With(x => x.Smoker, smoker)
                                                       .With(x => x.BMI, BodyMetricGenerator.Instance.GetBMI(randomizer, obese))
                                                       .Without(x => x.Weight));

            var heightInInches = BodyMetricGenerator.Instance.GetRandomHeightInInches(randomizer);

            var clinicals = fixture.CreateMany<PatientClinical>(randomizer.Next(0, maxNumberOfPatientClinicals + 1)).ToList();
            foreach (var clinical in clinicals)
            {
                clinical.BMI = BodyMetricGenerator.Instance.GetBMI(randomizer, obese, clinical.BMI);
                clinical.Weight = BodyMetricGenerator.Instance.GetWeightInKilograms(clinical.BMI, heightInInches);
            }

            return clinicals.OrderBy(x => x.ObservationDate);
        }
    }
}
