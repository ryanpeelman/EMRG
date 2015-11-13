using System;

namespace EMRG.Console.Models
{
    class PatientProcedure
    {
        public string PatientId { get; set; }
        public string CptHcpcs { get; set; }
        public DateTime ProcedureDate { get; set; }
        public string ProcedureDescription { get; set; }
    }
}
