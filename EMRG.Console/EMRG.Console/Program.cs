using EMRG.Console.AutoFixture;
using EMRG.Console.CSV;
using EMRG.Console.Models;
using Ploeh.AutoFixture;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EMRG.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var AVERAGE_HEIGHT_IN_METERS = 1.75;
            var randomizer = new Random((int)DateTime.Now.Ticks);

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
            int maxNumberOfPatientDiagnoses = 5;
            int maxNumberOfPatientLabs = 10;
            int maxNumberOfPatientProcedures = 5;
            int maxNumberOfPatientTherapies = 5;
            int maxNumberOfPatientUtilizations = 15;
            int numberOfPatients = 5;
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
                var clinicals = fixture.CreateMany<PatientClinical>(randomizer.Next(0, maxNumberOfPatientClinicals + 1)).OrderBy(x => x.ObservationDate);
                clinicals.ToList().ForEach(clinical => 
                {
                    clinical.Weight = WeightGenerator.GetWeight(clinical.Weight);
                    clinical.BMI = Math.Round(clinical.Weight / Math.Pow(AVERAGE_HEIGHT_IN_METERS, 2), 2);
                });

                fixture.Customize<PatientUtilization>(pu => pu.With(x => x.PatientId, demographic.PatientId));
                var utilizations = fixture.CreateMany<PatientUtilization>(randomizer.Next(0, maxNumberOfPatientUtilizations + 1)).OrderBy(x => x.ActivityDate);

                fixture.Customize<PatientDiagnosis>(pd => pd.With(x => x.PatientId, demographic.PatientId)
                                                            .Without(x => x.ICD10));
                var diagnoses = fixture.CreateMany<PatientDiagnosis>(randomizer.Next(0, maxNumberOfPatientDiagnoses + 1)).OrderBy(x => x.DiagnosisDate);

                fixture.Customize<PatientLab>(pl => pl.With(x => x.PatientId, demographic.PatientId));
                var labs = fixture.CreateMany<PatientLab>(randomizer.Next(0, maxNumberOfPatientLabs + 1)).OrderBy(x => x.LabDate);

                fixture.Customize<PatientTherapy>(pt => pt.With(x => x.PatientId, demographic.PatientId)
                                                          .Without(x => x.DDID)
                                                          .Without(x => x.RXNorm));
                var therapies = fixture.CreateMany<PatientTherapy>(randomizer.Next(0, maxNumberOfPatientTherapies + 1));
                therapies.ToList().ForEach(therapy =>
                {
                    if (therapy.StartDate > therapy.StopDate)
                    {
                        var holder = therapy.StartDate;
                        therapy.StartDate = therapy.StopDate;
                        therapy.StopDate = holder;
                    }
                });
                therapies = therapies.OrderBy(x => x.StartDate);


                fixture.Customize<PatientAllergy>(pa => pa.With(x => x.PatientId, demographic.PatientId)
                                                          .Without(x => x.DDID)
                                                          .Without(x => x.RXNorm));
                var allergies = fixture.CreateMany<PatientAllergy>(randomizer.Next(0, maxNumberOfPatientAllergies + 1)).OrderBy(x => x.DiagnosisDate);
                allergies.ToList().ForEach(allergy =>
                {
                    if (allergy.AllergyType != Enumerations.Allergy.Drug)
                    {
                        allergy.NDC = null;
                    }
                });

                fixture.Customize<PatientProcedure>(pp => pp.With(x => x.PatientId, demographic.PatientId));
                var procedures = fixture.CreateMany<PatientProcedure>(randomizer.Next(0, maxNumberOfPatientProcedures + 1)).OrderBy(x => x.ProcedureDate);

                fixture.Customize<ClaimsUtilization>(cu => cu.With(x => x.PatientId, demographic.PatientId)
                                                             .With(x => x.Age, demographic.Age)
                                                             .With(x => x.Gender, demographic.Gender)
                                                             .With(x => x.Region, demographic.Region)
                                                             .With(x => x.AlcoholAbuse, alcoholAbuse)
                                                             .With(x => x.DrugAbuse, drugAbuse)
                                                             .Without(x => x.ICD10));
                var claimsUtilizations = fixture.CreateMany<ClaimsUtilization>(randomizer.Next(0, maxNumberOfClaimsUtilizations + 1)).OrderBy(x => x.ActivityDate);

                fixture.Customize<ClaimsTherapy>(ct => ct.With(x => x.PatientId, demographic.PatientId)
                                                         .With(x => x.Age, demographic.Age)
                                                         .With(x => x.Gender, demographic.Gender)
                                                         .With(x => x.Region, demographic.Region)
                                                         .With(x => x.AlcoholAbuse, alcoholAbuse)
                                                         .With(x => x.DrugAbuse, drugAbuse)
                                                         .Without(x => x.DDID)
                                                         .Without(x => x.RXNorm));
                var claimsTherapies = fixture.CreateMany<ClaimsTherapy>(randomizer.Next(0, maxNumberOfClaimsTherapies + 1)).OrderBy(x => x.StartDate);
                claimsTherapies.ToList().ForEach(therapy =>
                {
                    if (therapy.StartDate > therapy.StopDate)
                    {
                        var holder = therapy.StartDate;
                        therapy.StartDate = therapy.StopDate;
                        therapy.StopDate = holder;
                    }
                });
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
            var writer = new CsvWriter();
            writer.WriteToFile($"{FOLDERPATH}\\demos.csv", allPatientDemographics);
            writer.WriteToFile($"{FOLDERPATH}\\clinicals.csv", allPatientClinicals);
            writer.WriteToFile($"{FOLDERPATH}\\utilizations.csv", allPatientUtilizations);
            writer.WriteToFile($"{FOLDERPATH}\\diagnoses.csv", allPatientDiagnoses);
            writer.WriteToFile($"{FOLDERPATH}\\labs.csv", allPatientLabs);
            writer.WriteToFile($"{FOLDERPATH}\\therapies.csv", allPatientTherapies);
            writer.WriteToFile($"{FOLDERPATH}\\allergies.csv", allPatientAllergies);
            writer.WriteToFile($"{FOLDERPATH}\\procedures.csv", allPatientProcedures);
            writer.WriteToFile($"{FOLDERPATH}\\claimstherapies.csv", allClaimsTherapies);
            writer.WriteToFile($"{FOLDERPATH}\\claimsutilizations.csv", allClaimsUtilizations);
        }
    }
}
