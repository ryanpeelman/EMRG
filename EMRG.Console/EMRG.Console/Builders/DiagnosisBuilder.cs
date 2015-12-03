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
        public const int ChanceOfHavingFavoredCondition = 30;

        private static List<string> _exclusionConditionCodes;
        private static List<string> _inclusionConditionCodes;
        private static List<string> _favoredConditionCodes;
        private static List<ICD9Entry> _icd9Entries;

        public static DiagnosisBuilder Instance { get; } = new DiagnosisBuilder();

        private DiagnosisBuilder()
        {
            _icd9Entries = ICD9Repository.Instance.GetEntries().ToList();

            _exclusionConditionCodes = new List<string> { "160", "161", "162", "163", "164", "165", "277.0", "491", "492", "496" };
            _inclusionConditionCodes = new List<string> { "493" };
            _favoredConditionCodes = new List<string> { "280", "281", "282", "283", "284", "285", "300.4", "301.12", "309.0", "309.1", "311", "327.23", "428", "401", "402", "403", "404", "405", "714" };
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
                var hasFavoredCondition = randomizer.NextPercent() <= ChanceOfHavingFavoredCondition;
                var entry = hasFavoredCondition ? GetSubsetEntry(randomizer, _favoredConditionCodes) : randomizer.NextListElement(_icd9Entries);
                DecoratePatientDiagnosisFromICD9Entry(diagnosis, entry);
            }

            var hasInclusionCondition = randomizer.NextPercent() <= ChanceOfHavingInclusionCondition;
            if (hasInclusionCondition)
            {
                var entry = GetSubsetEntry(randomizer, _inclusionConditionCodes);
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
                var entry = GetSubsetEntry(randomizer, _exclusionConditionCodes);
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

        private static ICD9Entry GetSubsetEntry(Random randomizer, List<string> conditionCodes)
        {
            var code = randomizer.NextListElement(conditionCodes);
            var entries = _icd9Entries.Where(x => x.ICD9Code.StartsWith(code)).ToList();
            return randomizer.NextListElement(entries);
        }
    }
}
