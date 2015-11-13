using System;
using EMRG.Console.Enumerations;

namespace EMRG.Console.Models
{
    class PatientUtilization
    {
        public string PatientId { get; set; }
        public DateTime ActivityDate { get; set; }
        public UtilizationActivity ActivityType { get; set; }
        public Specialty PhysicianSpecialty { get; set; }
    }
}
