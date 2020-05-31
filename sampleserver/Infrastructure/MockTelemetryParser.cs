using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace sampleserver.Infrastructure
{
    public class MockTelemetryParser : ITelemetryParser
    {
        public Dictionary<string, string> parsedData
        {
            get
            {
                return new Dictionary<string, string>()
        {
                    { "Timestamp", "2020-03-21 13:57:31"},
            { "Temperature", "27.5"},
            { "Humidity", "98.0"},
            { "Pressure", "100.0"},
            { "Light_intensity", "0.4"},
            { "No_of_lamps", "2"},
            { "No_of_airfans", "1"},
            { "No_of_heaters", "0"},
            { "Photo", " TBD "}
                };
            }
        }

        public Dictionary<string, double> FetchNumericData()
        {
            return new Dictionary<string, double>()
            {
                    {"Temperature", 27.5},
                    {"Humidity", 98.0},
                    {"Pressure", 100.0},
                    {"Light_intensity", 0.4},
                    {"No_of_lamps", 2},
                    {"No_of_airfans", 1},
                    {"No_of_heaters", 0},

            };
        }

        public DateTime? GetTimestamp()
        {
            return new DateTime(2020, 3, 21, 13, 57, 31);
        }

        public string ParsePhotoLink()
        {
            return " TBD ";
        }

        public async Task UpdateData()
        {
        }
    }
}
