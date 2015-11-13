using System;

namespace EMRG.Console.Models
{
    class ClaimTherapy
    {
        public string PatientId { get; set; }
        public int Age { get; set; }
        public bool AlcoholAbuse { get; set; }
        public string DDID { get; set; }
        public bool DrugAbuse { get; set; }
        public string DrugName { get; set; }
        public string Gender { get; set; }
        public string NDC { get; set; }
        public string Region { get; set; }
        public string RXNorm { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime StopDate { get; set; }
    }
}
