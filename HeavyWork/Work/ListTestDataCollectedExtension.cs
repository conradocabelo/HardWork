using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using HeavyWork.Entities;

namespace HeavyWork.Work
{
    public static class ListTestDataCollectedExtension
    {
        public static string CreateFilePath(TestConfiguration testConfiguration, string FileName)
        {
            string fullpath = Path.Combine(testConfiguration.PathOutReport, FileName);

            if (!Directory.Exists(testConfiguration.PathOutReport))
                Directory.CreateDirectory(testConfiguration.PathOutReport);

            return fullpath;
        }

        public static void ExportCsv(this List<TestDataCollected> data, TestConfiguration testConfiguration, string FileName, string separator = ",")
        {
            List<string> lines = new List<string>();

            var properties = typeof(TestDataCollected).GetProperties();
            var header = string.Join(separator, properties.Select(t => t.Name).ToArray());

            lines.Add(header);

            foreach (var lineData in data)
            {
                List<string> values = new List<string>();
                foreach (var propertie in properties)
                {
                    var value = propertie.GetValue(lineData);
                    values.Add(value == null ? "" : value.ToString());
                }
                lines.Add(string.Join(separator, values.ToArray()));
            }

            File.WriteAllLines(CreateFilePath(testConfiguration, FileName), lines.ToArray());
        }

        public static void ExportJSON(this List<TestDataCollected> data, TestConfiguration testConfiguration, string FileName)
        {
            var response = JsonConvert.SerializeObject(data);
            File.WriteAllText(CreateFilePath(testConfiguration, FileName), response);
        }
    }
}
