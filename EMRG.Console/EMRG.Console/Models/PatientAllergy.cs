using System;
using EMRG.Console.Enumerations;

namespace EMRG.Console.Models
{
    class PatientAllergy
    {
        public string PatientId { get; set; }
        public Allergy AllergyType { get; set; }
        public string DDID { get; set; }
        public DateTime DiagnosisDate { get; set; }
        public string DrugName { get; set; }
        public string NDC { get; set; }
        public string RXNorm { get; set; }
    }
}
