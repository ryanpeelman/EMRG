using System;
using EMRG.Console.Enumerations;

namespace EMRG.Console.Models
{
    class ClaimsUtilization
    {
        public string PatientId { get; set; }
        public DateTime ActivityDate { get; set; }
        public UtilizationActivity ActivityType { get; set; }
        public int Age { get; set; }
        public bool AlcoholAbuse { get; set; }
        public DateTime ClaimDate { get; set; }
        public string DiagnosisDescription { get; set; }
        public bool DrugAbuse { get; set; }
        public string Gender { get; set; }
        public string ICD9 { get; set; }
        public string ICD10 { get; set; }
        public int LengthOfStayInDays { get; set; }
        public Specialty PhysicianSpecialty { get; set; }
        public string Region { get; set; }
    }
}
