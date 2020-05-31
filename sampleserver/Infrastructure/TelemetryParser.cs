using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace sampleserver.Infrastructure
{
    public class TelemetryParser : ITelemetryParser
    {
        private readonly IDataFetcher dataFetcher;
        private TelemetryStorage telemetry;
        private JsonSerializerOptions jsonSerializerOptions;
        public TelemetryParser(IDataFetcher dataFetcher)
        {
            this.dataFetcher = dataFetcher;
            telemetry = new TelemetryStorage();
            jsonSerializerOptions=new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive=true,
            };
        }
        public async Task UpdateData()
        {
            var json = await dataFetcher.UpdateData();
            try
            {
                telemetry = JsonSerializer.Deserialize<TelemetryStorage>(json,jsonSerializerOptions);
            }
            catch (ArgumentNullException ex)
            {

                #warning todo logging errors
                Debug.WriteLine(ex.Message);
                Debug.WriteLine(ex.StackTrace);
            }
            catch (JsonException ex)
            {
                #warning todo logging errors
                Debug.WriteLine(ex.Message);
                Debug.WriteLine(ex.StackTrace);
            }
            catch (Exception ex)
            {
                #warning todo logging errors
                Debug.WriteLine(ex.Message);
                Debug.WriteLine(ex.StackTrace);
            }

        }
        public DateTime? GetTimestamp()
        {
            try
            {
                var date = DateTime.Parse(telemetry.Timestamp);
                return date;

            }
            catch(ArgumentNullException ex)
            {
#warning todo logging errors
                Debug.WriteLine(ex.Message);
                Debug.WriteLine(ex.StackTrace);
            }
            catch(FormatException ex)
            {
#warning todo logging errors
                Debug.WriteLine(ex.Message);
                Debug.WriteLine(ex.StackTrace);
            }
            catch(Exception ex)
            {
#warning todo logging errors
                Debug.WriteLine(ex.Message);
                Debug.WriteLine(ex.StackTrace);
            }
            return null;
        }

        public Dictionary<string, double> FetchNumericData()
        {
            var DataFetched = new Dictionary<string, double>();
            var numericData = telemetry.ReturnArrayOfParameters();
            for (int i = 0; i < numericData.Length; i++)
            {
                var parameterName = (Parameters)(i);
                DataFetched.Add(
                    parameterName.ToString(),
                    numericData[i]
                    );
            }
            return DataFetched;
        }


    }
}
