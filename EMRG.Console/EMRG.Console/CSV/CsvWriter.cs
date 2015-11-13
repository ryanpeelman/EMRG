using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace EMRG.Console.CSV
{
    class CsvWriter
    {
        public void WriteToFile<T>(string filepath, IEnumerable<T> records)
        {
            var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var header = string.Join(",", properties.Select(x => x.Name));
            var values = string.Join(Environment.NewLine, records.Select(record => string.Join(",", properties.Select(x => x.GetMethod.Invoke(record, null)))));

            File.WriteAllText(filepath, $"{header}{Environment.NewLine}{values}");
        }
    }
}
