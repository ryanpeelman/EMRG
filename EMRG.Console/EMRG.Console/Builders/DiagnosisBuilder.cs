using System;
using System.Collections.Generic;
using System.Linq;
using EMRG.Console.Helpers;
using EMRG.Console.Models;
using Ploeh.AutoFixture;

namespace EMRG.Console.Builders
{
    internal class DiagnosisBuilder
    {
        public const int ChanceOfHavingExclusionCondition = 10;
        public const int ChanceOfHavingInclusionCondition = 80;

        private static List<string> _exclusionConditionCodes;
        private static List<string> _inclusionConditionCodes;
        private static List<ICD9Entry> _icd9Entries;

        public static DiagnosisBuilder Instance { get; } = new DiagnosisBuilder();

        private DiagnosisBuilder()
        {
            _icd9Entries = ICD9Repository.Instance.GetEntries().ToList();

            _exclusionConditionCodes = new List<string> { "160", "161", "162", "163", "164", "165", "277.0", "491", "492", "496" };
            _inclusionConditionCodes = new List<string> { "493" };
        }

        public IEnumerable<PatientDiagnosis> GetPatientDiagnoses(Fixture fixture, Random randomizer, PatientDemographics demographic, int maxNumberOfPatientDiagnoses)
        {
            fixture.Customize<PatientDiagnosis>(pd => pd.With(x => x.PatientId, demographic.PatientId)
                                                        .Without(x => x.DiagnosisDescription)
                                                        .Without(x => x.ICD9)
                                                        .Without(x => x.ICD10));

            var diagnoses = fixture.CreateMany<PatientDiagnosis>(randomizer.Next(0, maxNumberOfPatientDiagnoses + 1)).ToList();
            foreach (var diagnosis in diagnoses)
            {
                var index = randomizer.Next(0, _icd9Entries.Count);
                var entry = _icd9Entries.ElementAt(index);
                DecoratePatientDiagnosisFromICD9Entry(diagnosis, entry);
            }

            var hasInclusionCondition = randomizer.NextPercent() <= ChanceOfHavingInclusionCondition;
            if (hasInclusionCondition)
            {
                var inclusionCode = randomizer.NextListElement(_inclusionConditionCodes);
                var entries = _icd9Entries.Where(x => x.ICD9Code.StartsWith(inclusionCode)).ToList();
                var entry = randomizer.NextListElement(entries);
                if (entry != null)
                {
                    var diagnosis = fixture.Create<PatientDiagnosis>();
                    DecoratePatientDiagnosisFromICD9Entry(diagnosis, entry);
                    diagnoses.Add(diagnosis);
                }
            }

            var hasExclusionCondition = randomizer.NextPercent() <= ChanceOfHavingExclusionCondition;
            if (hasExclusionCondition)
            {
                var exclusionCode = randomizer.NextListElement(_exclusionConditionCodes);
                var entries = _icd9Entries.Where(x => x.ICD9Code.StartsWith(exclusionCode)).ToList();
                var entry = randomizer.NextListElement(entries);
                if (entry != null)
                {
                    var diagnosis = fixture.Create<PatientDiagnosis>();
                    DecoratePatientDiagnosisFromICD9Entry(diagnosis, entry);
                    diagnoses.Add(diagnosis);
                }
            }

            return diagnoses.OrderBy(x => x.DiagnosisDate);
        }
        
        private static void DecoratePatientDiagnosisFromICD9Entry(PatientDiagnosis diagnosis, ICD9Entry entry)
        {
            diagnosis.ICD9 = entry.ICD9Code;
            diagnosis.DiagnosisDescription = entry.DisplayName;
        }
    }
}
