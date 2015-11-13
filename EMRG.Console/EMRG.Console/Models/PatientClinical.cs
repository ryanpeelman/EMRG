using System;

namespace EMRG.Console.Models
{
    class PatientClinical
    {
        public string PatientId { get; set; }
        public bool AlcoholAbuse { get; set; }
        public double BMI { get; set; }
        public bool DrugAbuse { get; set; }
        public DateTime ObservationDate { get; set; }
        public bool Smoker { get; set; }
        public double Weight { get; set; }
    }
}
