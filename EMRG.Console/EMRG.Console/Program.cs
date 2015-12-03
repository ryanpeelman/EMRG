using EMRG.Console.AutoFixture;
using EMRG.Console.CSV;
using EMRG.Console.Models;
using Ploeh.AutoFixture;
using System;
using System.Collections.Generic;
using System.Linq;
using EMRG.Console.Builders;
using EMRG.Console.Helpers;

namespace EMRG.Console
{
    class Program
    {
        private static readonly Random Randomizer = new Random((int)DateTime.Now.Ticks);

        static void Main(string[] args)
        {
            var allClaimsTherapies = new List<ClaimsTherapy>();
            var allClaimsUtilizations = new List<ClaimsUtilization>();
            var allPatientAllergies = new List<PatientAllergy>();
            var allPatientClinicals = new List<PatientClinical>();
            var allPatientDemographics = new List<PatientDemographics>();
            var allPatientDiagnoses = new List<PatientDiagnosis>();
            var allPatientLabs = new List<PatientLab>();
            var allPatientProcedures = new List<PatientProcedure>();
            var allPatientTherapies = new List<PatientTherapy>();
            var allPatientUtilizations = new List<PatientUtilization>();

            var fixture = new Fixture();
            fixture.Customizations.Add(new PatientSpecimenBuilder());

            int maxNumberOfClaimsTherapies = 5;
            int maxNumberOfClaimsUtilizations = 10;
            int maxNumberOfPatientAllergies = 5;
            int maxNumberOfPatientClinicals = 10;
            int maxNumberOfPatientDiagnoses = 10;
            int maxNumberOfPatientLabs = 20;
            int maxNumberOfPatientProcedures = 5;
            int maxNumberOfPatientTherapies = 20;
            int maxNumberOfPatientUtilizations = 15;
            int numberOfPatients = 50;
            for (int i = 0; i < numberOfPatients; i++)
            {
                var demographic = fixture.Create<PatientDemographics>();
                
                var alcoholAbuse = fixture.Create<bool>();
                var drugAbuse = fixture.Create<bool>();
                var smoker = fixture.Create<bool>();
                fixture.Customize<PatientClinical>(pc => pc.With(x => x.PatientId, demographic.PatientId)
                                                           .With(x => x.AlcoholAbuse, alcoholAbuse)
                                                           .With(x => x.DrugAbuse, drugAbuse)
                                                           .With(x => x.Smoker, smoker)
                                                           .With(x => x.Weight, WeightGenerator.GetWeight())
                                                           .Without(x => x.BMI));
                var clinicals = fixture.CreateMany<PatientClinical>(Randomizer.Next(0, maxNumberOfPatientClinicals + 1)).OrderBy(x => x.ObservationDate);
                clinicals.ToList().ForEach(RedecoratePatientClinical);

                fixture.Customize<PatientUtilization>(pu => pu.With(x => x.PatientId, demographic.PatientId));
                var utilizations = fixture.CreateMany<PatientUtilization>(Randomizer.Next(0, maxNumberOfPatientUtilizations + 1)).OrderBy(x => x.ActivityDate);

                var diagnoses = DiagnosisBuilder.Instance.GetPatientDiagnoses(fixture, Randomizer, demographic, maxNumberOfPatientDiagnoses);

                fixture.Customize<PatientLab>(pl => pl.With(x => x.PatientId, demographic.PatientId));
                var labs = fixture.CreateMany<PatientLab>(Randomizer.Next(0, maxNumberOfPatientLabs + 1)).OrderBy(x => x.LabDate);
                labs.ToList().ForEach(RedecoratePatientLab);

                fixture.Customize<PatientTherapy>(pt => pt.With(x => x.PatientId, demographic.PatientId)
                                                          .Without(x => x.DDID)
                                                          .Without(x => x.RXNorm)
                                                          .Without(x => x.StopDate));
                var therapies = fixture.CreateMany<PatientTherapy>(Randomizer.Next(0, maxNumberOfPatientTherapies + 1));
                therapies.ToList().ForEach(RedecoratePatientTherapy);
                therapies = therapies.OrderBy(x => x.StartDate);


                fixture.Customize<PatientAllergy>(pa => pa.With(x => x.PatientId, demographic.PatientId)
                                                          .Without(x => x.DDID)
                                                          .Without(x => x.RXNorm));
                var allergies = fixture.CreateMany<PatientAllergy>(Randomizer.Next(0, maxNumberOfPatientAllergies + 1)).OrderBy(x => x.DiagnosisDate);
                allergies.ToList().ForEach(RedecoratePatientAllergy);

                fixture.Customize<PatientProcedure>(pp => pp.With(x => x.PatientId, demographic.PatientId));
                var procedures = fixture.CreateMany<PatientProcedure>(Randomizer.Next(0, maxNumberOfPatientProcedures + 1)).OrderBy(x => x.ProcedureDate);

                fixture.Customize<ClaimsUtilization>(cu => cu.With(x => x.PatientId, demographic.PatientId)
                                                             .With(x => x.Age, demographic.Age)
                                                             .With(x => x.Gender, demographic.Gender)
                                                             .With(x => x.Region, demographic.Region)
                                                             .With(x => x.AlcoholAbuse, alcoholAbuse)
                                                             .With(x => x.DrugAbuse, drugAbuse)
                                                             .Without(x => x.ICD10));
                var claimsUtilizations = fixture.CreateMany<ClaimsUtilization>(Randomizer.Next(0, maxNumberOfClaimsUtilizations + 1)).OrderBy(x => x.ActivityDate);

                fixture.Customize<ClaimsTherapy>(ct => ct.With(x => x.PatientId, demographic.PatientId)
                                                         .With(x => x.Age, demographic.Age)
                                                         .With(x => x.Gender, demographic.Gender)
                                                         .With(x => x.Region, demographic.Region)
                                                         .With(x => x.AlcoholAbuse, alcoholAbuse)
                                                         .With(x => x.DrugAbuse, drugAbuse)
                                                         .Without(x => x.DDID)
                                                         .Without(x => x.RXNorm));
                var claimsTherapies = fixture.CreateMany<ClaimsTherapy>(Randomizer.Next(0, maxNumberOfClaimsTherapies + 1)).OrderBy(x => x.StartDate);
                claimsTherapies.ToList().ForEach(RedecorateClaimsTherapy);
                claimsTherapies = claimsTherapies.OrderBy(x => x.StartDate);

                allClaimsTherapies.AddRange(claimsTherapies);
                allClaimsUtilizations.AddRange(claimsUtilizations);
                allPatientAllergies.AddRange(allergies);
                allPatientClinicals.AddRange(clinicals);
                allPatientDemographics.Add(demographic);
                allPatientDiagnoses.AddRange(diagnoses);
                allPatientLabs.AddRange(labs);
                allPatientProcedures.AddRange(procedures);
                allPatientTherapies.AddRange(therapies);
                allPatientUtilizations.AddRange(utilizations);
            }

            string FOLDERPATH = "c:\\temp";
            string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
            var writer = new CsvWriter();
            writer.WriteToFile($"{FOLDERPATH}\\{timestamp}.unofficial.patientdemographics.csv", allPatientDemographics);
            writer.WriteToFile($"{FOLDERPATH}\\{timestamp}.unofficial.patientclinicals.csv", allPatientClinicals);
            writer.WriteToFile($"{FOLDERPATH}\\{timestamp}.unofficial.patientutilizations.csv", allPatientUtilizations);
            writer.WriteToFile($"{FOLDERPATH}\\{timestamp}.unofficial.patientdiagnoses.csv", allPatientDiagnoses);
            writer.WriteToFile($"{FOLDERPATH}\\{timestamp}.unofficial.patientlabs.csv", allPatientLabs);
            writer.WriteToFile($"{FOLDERPATH}\\{timestamp}.unofficial.patienttherapies.csv", allPatientTherapies);
            writer.WriteToFile($"{FOLDERPATH}\\{timestamp}.unofficial.patientallergies.csv", allPatientAllergies);
            writer.WriteToFile($"{FOLDERPATH}\\{timestamp}.unofficial.patientprocedures.csv", allPatientProcedures);
            writer.WriteToFile($"{FOLDERPATH}\\{timestamp}.unofficial.claimstherapies.csv", allClaimsTherapies);
            writer.WriteToFile($"{FOLDERPATH}\\{timestamp}.unofficial.claimsutilizations.csv", allClaimsUtilizations);
        }

        private static void RedecorateClaimsTherapy(ClaimsTherapy therapy)
        {
            var daysOnTherapy = Randomizer.Next(1, 365);
            therapy.StopDate = new DateTime(Math.Min(therapy.StartDate.AddDays(daysOnTherapy).Ticks, DateTime.Now.Ticks));
        }

        private static void RedecoratePatientAllergy(PatientAllergy allergy)
        {
            if (allergy.AllergyType != Enumerations.Allergy.Drug)
            {
                allergy.NDC = null;
            }
        }

        private static void RedecoratePatientClinical(PatientClinical clinical)
        {
            var AVERAGE_HEIGHT_IN_METERS = 1.75;
            clinical.Weight = WeightGenerator.GetWeight(clinical.Weight);
            clinical.BMI = Math.Round(clinical.Weight/Math.Pow(AVERAGE_HEIGHT_IN_METERS, 2), 2);
        }

        private static void RedecoratePatientLab(PatientLab lab)
        {
            var preferredValues = new Dictionary<string, Func<string>>()
            {
                ["A1C[NORMAL]"] = () => Randomizer.Next(4, 9).ToString(),
                ["A1C[ABNORMAL]"] = () => Randomizer.Next(8, 14).ToString()
            };

            var percentageOfPreferredValues = 40;
            var shouldUsePreferredValue = Randomizer.NextPercent() <= percentageOfPreferredValues;
            if (shouldUsePreferredValue)
            {
                var index = Randomizer.Next(0, preferredValues.Count);
                var pair = preferredValues.ElementAt(index);
                lab.LabName = pair.Key.Replace("[NORMAL]", string.Empty).Replace("[ABNORMAL]", string.Empty);
                lab.Value = pair.Value.Invoke();
            }
        }

        private static void RedecoratePatientTherapy(PatientTherapy therapy)
        {
            var daysOnTherapy = Randomizer.Next(1, 365);
            therapy.StopDate = new DateTime(Math.Min(therapy.StartDate.AddDays(daysOnTherapy).Ticks, DateTime.Now.Ticks));

            var preferredValues = new Dictionary<string, string>()
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
            var percentageOfPreferredValues = 80;
            var shouldUsePreferredValue = Randomizer.Next(0, 101) <= percentageOfPreferredValues;
            if (shouldUsePreferredValue)
            {
                var index = Randomizer.Next(0, preferredValues.Count);
                var pair = preferredValues.ElementAt(index);
                therapy.NDC = pair.Key;
                therapy.DrugName = pair.Value;
            }
        }
    }
}
