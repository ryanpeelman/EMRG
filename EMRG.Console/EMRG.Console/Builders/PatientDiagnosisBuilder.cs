using EMRG.Console.Helpers;
using EMRG.Console.Models;
using Ploeh.AutoFixture;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EMRG.Console.Builders
{
    internal class PatientDiagnosisBuilder
    {
        public const int ChanceOfHavingExclusionDiagnosis = 10;
        public const int ChanceOfHavingInclusionDiagnosis = 80;
        public const int ChanceOfHavingFavoredDiagnosis = 30;

        private static List<string> _exclusionDiagnosisCodes;
        private static List<string> _inclusionDiagnosisCodes;
        private static List<string> _favoredDiagnosisCodes;
        private static List<ICD9Entry> _icd9Entries;

        public static PatientDiagnosisBuilder Instance { get; } = new PatientDiagnosisBuilder();

        private PatientDiagnosisBuilder()
        {
            _icd9Entries = ICD9Repository.Instance.GetEntries().ToList();

            _exclusionDiagnosisCodes = new List<string> { "160", "161", "162", "163", "164", "165", "277.0", "491", "492", "496" };
            _inclusionDiagnosisCodes = new List<string> { "493" };
            _favoredDiagnosisCodes = new List<string> { "280", "281", "282", "283", "284", "285", "300.4", "301.12", "309.0", "309.1", "311", "327.23", "428", "401", "402", "403", "404", "405", "714" };
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
                var hasFavoredDiagnosis = randomizer.NextPercent() <= ChanceOfHavingFavoredDiagnosis;
                var entry = hasFavoredDiagnosis ? GetSubsetEntry(randomizer, _favoredDiagnosisCodes) : randomizer.NextListElement(_icd9Entries);
                DecoratePatientDiagnosisFromICD9Entry(diagnosis, entry);
            }

            var hasInclusionDiagnosis = randomizer.NextPercent() <= ChanceOfHavingInclusionDiagnosis;
            if (hasInclusionDiagnosis)
            {
                var entry = GetSubsetEntry(randomizer, _inclusionDiagnosisCodes);
                if (entry != null)
                {
                    var diagnosis = fixture.Create<PatientDiagnosis>();
                    DecoratePatientDiagnosisFromICD9Entry(diagnosis, entry);
                    diagnoses.Add(diagnosis);
                }
            }

            var hasExclusionDiagnosis = randomizer.NextPercent() <= ChanceOfHavingExclusionDiagnosis;
            if (hasExclusionDiagnosis)
            {
                var entry = GetSubsetEntry(randomizer, _exclusionDiagnosisCodes);
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

        private static ICD9Entry GetSubsetEntry(Random randomizer, List<string> codes)
        {
            var code = randomizer.NextListElement(codes);
            var entries = _icd9Entries.Where(x => x.ICD9Code.StartsWith(code)).ToList();
            return randomizer.NextListElement(entries);
        }
    }
}
