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

            var allPatientDemographics = new List<PatientDemographics>();
            var allPatientClinicals = new List<PatientClinical>();
            var allPatientUtilizations = new List<PatientUtilization>();
            var allPatientDiagnoses = new List<PatientDiagnosis>();
            var allPatientLabs = new List<PatientLab>();
            var allPatientTherapies = new List<PatientTherapy>();
            var allPatientAllergies = new List<PatientAllergy>();
            var allPatientProcedures = new List<PatientProcedure>();

            var fixture = new Fixture();
            fixture.Customizations.Add(new PatientSpecimenBuilder());

            int numberOfPatients = 5;
            int maxNumberOfPatientClinicals = 10;
            int maxNumberOfPatientUtilizations = 15;
            int maxNumberOfPatientDiagnoses = 10;
            int maxNumberOfPatientLabs = 10;
            int maxNumberOfPatientTherapies = 15;
            int maxNumberOfPatientAllergies = 10;
            int maxNumberOfPatientProcedures = 15;
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

                fixture.Customize<PatientDiagnosis>(pd => pd.With(x => x.PatientId, demographic.PatientId));
                var diagnoses = fixture.CreateMany<PatientDiagnosis>(randomizer.Next(0, maxNumberOfPatientDiagnoses + 1)).OrderBy(x => x.DiagnosisDate);

                fixture.Customize<PatientLab>(pl => pl.With(x => x.PatientId, demographic.PatientId));
                var labs = fixture.CreateMany<PatientLab>(randomizer.Next(0, maxNumberOfPatientLabs + 1)).OrderBy(x => x.LabDate);

                fixture.Customize<PatientTherapy>(pt => pt.With(x => x.PatientId, demographic.PatientId));
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


                fixture.Customize<PatientAllergy>(pa => pa.With(x => x.PatientId, demographic.PatientId));
                var allergies = fixture.CreateMany<PatientAllergy>(randomizer.Next(0, maxNumberOfPatientAllergies + 1)).OrderBy(x => x.DiagnosisDate);

                fixture.Customize<PatientProcedure>(pp => pp.With(x => x.PatientId, demographic.PatientId));
                var procedures = fixture.CreateMany<PatientProcedure>(randomizer.Next(0, maxNumberOfPatientProcedures + 1)).OrderBy(x => x.ProcedureDate);

                allPatientDemographics.Add(demographic);
                allPatientClinicals.AddRange(clinicals);
                allPatientUtilizations.AddRange(utilizations);
                allPatientDiagnoses.AddRange(diagnoses);
                allPatientLabs.AddRange(labs);
                allPatientTherapies.AddRange(therapies);
                allPatientAllergies.AddRange(allergies);
                allPatientProcedures.AddRange(procedures);
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
        }
    }
}
