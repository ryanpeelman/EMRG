using System;

namespace EMRG.Console.Models
{
    class PatientLab
    {
        public string PatientId { get; set; }
        public DateTime LabDate { get; set; }
        public string LabDescription { get; set; }
        public string LabName { get; set; }
        public string Value { get; set; }
    }
}
