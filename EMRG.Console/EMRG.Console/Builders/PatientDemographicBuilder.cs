using EMRG.Console.Helpers;
using EMRG.Console.Models;
using System;
using System.Collections.Generic;

namespace EMRG.Console.Builders
{
    internal class PatientDemographicBuilder
    {
        private static List<string> _gender;
        private static List<string> _race;
        private static List<string> _regions;

        public static PatientDemographicBuilder Instance { get; } = new PatientDemographicBuilder();

        private PatientDemographicBuilder()
        {
            _gender = new List<string> { "Female", "Male" };
            _race = new List<string> { "American Indian and Alaska Native", "Asian", "Black or African American", "Native Hawaiian and Other Pacific Islander", "White" };
            _regions = new List<string> { "North", "South", "East", "West", "Central" };
        }

        public PatientDemographics GetPatientDemographic(Random randomizer)
        {
            return new PatientDemographics()
            {
                PatientId = Guid.NewGuid().ToString(),
                Age = randomizer.Next(18, 90),
                Gender = randomizer.NextListElement(_gender),
                Race = randomizer.NextListElement(_race),
                Region = randomizer.NextListElement(_regions)
            };
        }
    }
}
