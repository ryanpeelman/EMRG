using System;

namespace EMRG.Console.Models
{
    class PatientDiagnosis
    {
        public string PatientId { get; set; }
        public DateTime DiagnosisDate { get; set; }
        public string DiagnosisDescription { get; set; }
        public string ICD9 { get; set; }
        public string ICD10 { get; set; }
    }
}
