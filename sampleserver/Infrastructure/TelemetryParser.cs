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
        private readonly JsonSerializerOptions jsonSerializerOptions;
        public TelemetryParser(IDataFetcher dataFetcher)
        {
            this.dataFetcher = dataFetcher;
            telemetry = new TelemetryStorage();
            jsonSerializerOptions=new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive=true,
            };
        }
        public async Task<bool> UpdateData()
        {
            var json = await dataFetcher.UpdateData();
            try
            {
                telemetry = JsonSerializer.Deserialize<TelemetryStorage>(json,jsonSerializerOptions);
            }
            catch (ArgumentNullException ex)
            {
                string EventMessage = "An error occurred in TelemetryParser while updating data. An object to populate with values from JSON file was null.";
                await EventLogger.LogExceptionForUserAndToFile(EventMessage,ex);
                await EventLogger.LogExceptionToFile(EventMessage,ex.Message,ex.StackTrace);
                return false;
            }
            catch (JsonException ex)
            {
                string EventMessage= "An error occurred in TelemetryParser while updating data. Downloaded JSON file is invalid.";
                await EventLogger.LogExceptionForUserAndToFile(EventMessage,ex);
                 
                return false;
            }
            catch (Exception ex)
            {
                string EventMessage= "An unknown error occurred in TelemetryParser while updating data. ";
                await EventLogger.LogExceptionForUserAndToFile(EventMessage,ex);
                 
                return false;
            }

            return true;
        }
        public async Task<DateTime?> GetTimestamp()
        {
            try
            {
                var date = DateTime.Parse(telemetry.Timestamp);
                return date;

            }
            catch(ArgumentNullException ex)
            {
                string EventMessage= "Timestamp was null. ";
                await EventLogger.LogExceptionForUserAndToFile(EventMessage,ex);
                 
            }
            catch(FormatException ex)
            {
                string EventMessage= "Timestamp was in improper format";
                await EventLogger.LogExceptionForUserAndToFile(EventMessage,ex);
                 
            }
            catch(Exception ex)
            {
                string EventMessage= "An unknown error occurred in TelemetryParser while updating data. ";
                await EventLogger.LogExceptionForUserAndToFile(EventMessage,ex);
                 
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
