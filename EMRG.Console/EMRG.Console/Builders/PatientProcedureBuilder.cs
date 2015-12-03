using EMRG.Console.Helpers;
using EMRG.Console.Models;
using Ploeh.AutoFixture;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EMRG.Console
{
    internal class PatientProcedureBuilder
    {
        public const int ChanceOfHavingFavoredProcedure = 10;
        public const int ChanceOfHavingRecentProcedure = 85;

        public static PatientProcedureBuilder Instance { get; } = new PatientProcedureBuilder();

        private static List<string> _favoredProcedureCodes;

        private PatientProcedureBuilder()
        {
            _favoredProcedureCodes = new List<string> { "82785" };
        }

        public IEnumerable<PatientProcedure> GetPatientProcedures(Fixture fixture, Random randomizer, PatientDemographics demographic, int maxNumberOfPatientProcedures)
        {
            fixture.Customize<PatientProcedure>(pp => pp.With(x => x.PatientId, demographic.PatientId)
                                                        .Without(x => x.CptHcpcs));

            var procedures = fixture.CreateMany<PatientProcedure>(randomizer.Next(0, maxNumberOfPatientProcedures + 1)).ToList();
            foreach (var procedure in procedures)
            {
                procedure.ProcedureDate = procedure.ProcedureDate.Date;

                var hasFavoredProcedure = randomizer.NextPercent() <= ChanceOfHavingFavoredProcedure;
                procedure.CptHcpcs = hasFavoredProcedure ? randomizer.NextListElement(_favoredProcedureCodes) : GetRandomProcedure(randomizer);

                var isProcedureRecent = randomizer.NextPercent() <= ChanceOfHavingRecentProcedure;
                if(isProcedureRecent)
                {
                    var oldestRecentDateTime = DateTime.Today.AddMonths(-6);
                    if(procedure.ProcedureDate < oldestRecentDateTime)
                    {
                        var maximumNumberOfDaysForRecent = (int)(DateTime.Today - oldestRecentDateTime).TotalDays;
                        var daysAgo = randomizer.Next(1, maximumNumberOfDaysForRecent);
                        procedure.ProcedureDate = DateTime.Today.AddDays(-daysAgo);
                    }
                }
            }

            return procedures.OrderBy(x => x.ProcedureDate);
        }

        private static string GetRandomProcedure(Random randomizer)
        {
            var randomValue = randomizer.Next(100, 99500);
            return randomValue.ToString("00000");
        }
    }
}
