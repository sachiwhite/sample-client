using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace sampleserver.Infrastructure
{
    public class TelemetryParser : ITelemetryParser
    {
        private readonly IDataFetcher dataFetcher;
        public Dictionary<string, string> parsedData { get; private set; }
        public TelemetryParser(IDataFetcher dataFetcher)
        {
            this.dataFetcher = dataFetcher;
            parsedData = new Dictionary<string, string>();
        }
        public void UpdateData()
        {
                parsedData = new Dictionary<string, string>();
                var textToParse = dataFetcher.UpdateData();
                for (int i = 0; i < textToParse.Count; i++)
                {
                    var toProcess = textToParse[i];
                    if (string.IsNullOrWhiteSpace(toProcess)) continue;
                    toProcess = new string(toProcess.SkipWhile(c => !char.IsLetterOrDigit(c)).ToArray());
                    var name = new string(toProcess.TakeWhile(c => (c == '_' || char.IsLetterOrDigit(c))).ToArray());
                if (name == "Photo")
                {
                    var data = new string(toProcess.SkipWhile(c=>c!=' ').ToArray());
                    parsedData.Add(name, data);
                }
                else
                {
                    var data = new string(toProcess.TrimEnd().SkipWhile(c => !char.IsNumber(c)).ToArray());
                    parsedData.Add(name, data);
                                     
                }

                }
           
        }
        public DateTime? GetTimestamp()
        {
            try
            {
                var date = DateTime.Parse(parsedData["Timestamp"]);
                return date;

            }
            catch (KeyNotFoundException)
            {
                #warning todo logging errors
                return null; //return new DateTime();
            }
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

        public string ParsePhotoLink()
        {
            try
            {
                var link = parsedData["Photo"];
                return link;
            }
            catch (Exception)
            {
                #warning todo: logging errors
                return string.Empty;
            }
        }
    }

    public interface ITelemetryParser
    {
        void UpdateData();
        string ParsePhotoLink();
        DateTime? GetTimestamp();
        Dictionary<string, double> FetchNumericData();
        Dictionary<string, string> parsedData { get; }
    }
}
