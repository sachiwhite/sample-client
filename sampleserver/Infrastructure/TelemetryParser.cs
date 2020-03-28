using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace sampleserver.Infrastructure
{
    public class TelemetryParser :ITelemetryParser
    {
        private DataFetcher dataFetcher;
        public Dictionary<string, string> parsedData { get; set; }
        public TelemetryParser()
        {
            dataFetcher = new DataFetcher();
            parsedData = new Dictionary<string, string>();
        }
        public void UpdateData()
        {
                var textToParse = dataFetcher.UpdateData();
                var parsedData = new Dictionary<string, string>();
                for (int i = 0; i < textToParse.Count; i++)
                {
                    var toProcess = textToParse[i];
                    if (string.IsNullOrWhiteSpace(toProcess)) continue;
                    toProcess = new string(toProcess.SkipWhile(c => !char.IsLetterOrDigit(c)).ToArray());
                    var name = new string(toProcess.TakeWhile(c => (c == '_' || char.IsLetterOrDigit(c))).ToArray());
                    var data = new string(toProcess.TrimEnd().SkipWhile(c => !char.IsNumber(c)).ToArray());
                    parsedData.Add(name, data);
                }
              
           
        }
        public DateTime GetTimestamp()
        {
            return DateTime.Parse(parsedData["Timestamp"]);
        }

        public Dictionary<string, double> FetchNumericData()
        {
            var DataFetched = new Dictionary<string, double>();
            foreach (var item in parsedData)
            {
                var key = item.Key;
                var value = item.Value;
                if (double.TryParse(value, NumberStyles.Number, CultureInfo.InvariantCulture, out double num))
                    DataFetched.Add(key, num);
            }
            return DataFetched;
        }
    }

    public interface ITelemetryParser
    {
        void UpdateData();
        DateTime GetTimestamp();
        Dictionary<string, double> FetchNumericData();
    }
}
