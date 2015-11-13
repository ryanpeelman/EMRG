using System;

namespace EMRG.Console.Models
{
    class PatientTherapy
    {
        public string PatientId { get; set; }
        public string DDID { get; set; }
        public string DrugName { get; set; }
        public string NDC { get; set; }
        public string RXNorm { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime StopDate { get; set; }
    }
}
