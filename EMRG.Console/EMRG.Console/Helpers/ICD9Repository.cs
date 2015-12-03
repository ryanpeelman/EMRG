using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace EMRG.Console.Helpers
{
    public class ICD9Repository
    {
        public static ICD9Repository Instance { get; } = new ICD9Repository();

        public IEnumerable<ICD9Entry> GetEntries()
        {
            var commaDelimitedExpression = ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)";
            var modelProperties = typeof(ICD9Entry).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var lines = File.ReadAllLines("ICD9.csv");
            var header = lines.First().Split(',');
            return lines.Skip(1).Select(x => Regex.Split(x, commaDelimitedExpression)).Select(x =>
            {
                var model = new ICD9Entry();
                foreach (var field in header)
                {
                    var property = modelProperties.First(p => p.Name == field);
                    var propertyType = property.PropertyType;
                    var stringValue = x[Array.IndexOf(header, property.Name)];
                    var value = (propertyType.IsEnum) ? Enum.Parse(propertyType, stringValue) : Convert.ChangeType(stringValue, propertyType);
                    property.SetValue(model, value);
                }
                return model;
            });
        }
    }
}
