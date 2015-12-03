using EMRG.Console.Helpers;
using EMRG.Console.Models;
using Ploeh.AutoFixture;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EMRG.Console
{
    internal class PatientAllergyBuilder
    {
        public static PatientAllergyBuilder Instance { get; } = new PatientAllergyBuilder();

        private PatientAllergyBuilder() { }

        public IEnumerable<PatientAllergy> GetPatientAllergies(Fixture fixture, Random randomizer, PatientDemographics demographic, int maxNumberOfPatientAllergies)
        {
            fixture.Customize<PatientAllergy>(pa => pa.With(x => x.PatientId, demographic.PatientId)
                                                      .Without(x => x.DDID)
                                                      .Without(x => x.NDC)
                                                      .Without(x => x.RXNorm));

            var allergies = fixture.CreateMany<PatientAllergy>(randomizer.Next(0, maxNumberOfPatientAllergies + 1));
            foreach(var allergy in allergies)
            {
                if (allergy.AllergyType == Enumerations.Allergy.Drug)
                {
                    allergy.NDC = NDCGenerator.GetRandomNDC(randomizer);
                }
            }

            return allergies.OrderBy(x => x.DiagnosisDate);
        }
    }
}
