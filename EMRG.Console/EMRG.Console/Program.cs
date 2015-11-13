using EMRG.Console.AutoFixture;
using EMRG.Console.Models;
using Ploeh.AutoFixture;

namespace EMRG.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var fixture = new Fixture();
            fixture.Customizations.Add(new PatientDemographicsSpecimenBuilder());

            var demos = fixture.CreateMany<PatientDemographics>(20);
            foreach (var demo in demos)
            {
                System.Console.WriteLine($"{demo.PatientId},{demo.Age},{demo.Gender},{demo.Race},{demo.Region}");
            }

            System.Console.ReadLine();
        }
    }
}
