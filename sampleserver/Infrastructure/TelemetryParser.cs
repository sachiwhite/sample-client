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
                string EventMessage = "An error occurred in TelemetryParser while updating data. An object to populate with values from JSON file was null.";
                await EventLogger.LogForUser(EventMessage);
                await EventLogger.LogExceptionToFile(EventMessage,ex.Message,ex.StackTrace);
            }
            catch (JsonException ex)
            {
                string EventMessage= "An error occurred in TelemetryParser while updating data. Downloaded JSON file is invalid.";
                await EventLogger.LogForUser(EventMessage);
                await EventLogger.LogExceptionToFile(EventMessage, ex.Message, ex.StackTrace);
            }
            catch (Exception ex)
            {
                string EventMessage= "An unknown error occurred in TelemetryParser while updating data. ";
                await EventLogger.LogForUser(EventMessage);
                await EventLogger.LogExceptionToFile(EventMessage, ex.Message, ex.StackTrace);
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
