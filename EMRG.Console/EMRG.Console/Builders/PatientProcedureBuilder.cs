using EMRG.Console.Models;
using Ploeh.AutoFixture;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EMRG.Console
{
    internal class PatientProcedureBuilder
    {
        public static PatientProcedureBuilder Instance { get; } = new PatientProcedureBuilder();

        private PatientProcedureBuilder() { }

        public IEnumerable<PatientProcedure> GetPatientProcedures(Fixture fixture, Random randomizer, PatientDemographics demographic, int maxNumberOfPatientProcedures)
        {
            fixture.Customize<PatientProcedure>(pp => pp.With(x => x.PatientId, demographic.PatientId));

            var procedures = fixture.CreateMany<PatientProcedure>(randomizer.Next(0, maxNumberOfPatientProcedures + 1));

            return procedures.OrderBy(x => x.ProcedureDate);
        }
    }
}
