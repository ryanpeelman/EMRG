using EMRG.Console.AutoFixture;
using EMRG.Console.Builders;
using EMRG.Console.CSV;
using EMRG.Console.Models;
using Ploeh.AutoFixture;
using System;
using System.Collections.Generic;

namespace EMRG.Console
{
    class Program
    {
        static void Main(string[] args)
        {
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
            int maxNumberOfPatientDiagnoses = 10;
            int maxNumberOfPatientLabs = 20;
            int maxNumberOfPatientProcedures = 5;
            int maxNumberOfPatientTherapies = 20;
            int maxNumberOfPatientUtilizations = 15;
            int numberOfPatients = 50;
            for (int i = 0; i < numberOfPatients; i++)
            {
                var alcoholAbuse = fixture.Create<bool>();
                var drugAbuse = fixture.Create<bool>();
                var smoker = fixture.Create<bool>();

                var demographic = fixture.Create<PatientDemographics>();
                var clinicals = PatientClinicalBuilder.Instance.GetPatientClinicals(fixture, randomizer, demographic, alcoholAbuse, drugAbuse, smoker, maxNumberOfPatientClinicals);
                var utilizations = PatientUtilizationBuilder.Instance.GetPatientUtilizations(fixture, randomizer, demographic, maxNumberOfPatientUtilizations);
                var diagnoses = PatientDiagnosisBuilder.Instance.GetPatientDiagnoses(fixture, randomizer, demographic, maxNumberOfPatientDiagnoses);
                var labs = PatientLabBuilder.Instance.GetPatientLabs(fixture, randomizer, demographic, maxNumberOfPatientLabs);
                var therapies = PatientTherapyBuilder.Instance.GetPatientTherapies(fixture, randomizer, demographic, maxNumberOfPatientTherapies);
                var allergies = PatientAllergyBuilder.Instance.GetPatientAllergies(fixture, randomizer, demographic, maxNumberOfPatientAllergies);
                var procedures = PatientProcedureBuilder.Instance.GetPatientProcedures(fixture, randomizer, demographic, maxNumberOfPatientProcedures);
                var claimsUtilizations = ClaimsUtilizationBuilder.Instance.GetClaimsUtilizations(fixture, randomizer, demographic, alcoholAbuse, drugAbuse, maxNumberOfClaimsUtilizations);
                var claimsTherapies = ClaimsTherapyBuilder.Instance.GetClaimsTherapies(fixture, randomizer, demographic, alcoholAbuse, drugAbuse, maxNumberOfClaimsTherapies);

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
    }
}
