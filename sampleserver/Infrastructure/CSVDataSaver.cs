using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace sampleserver.Infrastructure
{
    public class CSVDataSaver :IDataSaver
    {
        private string CSVFilePath;

        public CSVDataSaver()
        {
            CSVFilePath = string.Empty;
            SetFilePath();
        }

        private async Task SetFilePath()
        {
            DateTime startupTime = DateTime.Now;
            string nameToSet=$"telemetry_{startupTime.Year}-{startupTime.Month}-{startupTime.Day} {startupTime.Hour}{startupTime.Minute}.csv";
          
            try
            {
                using (StreamWriter writer = new StreamWriter(nameToSet,true))
                {
                    await writer.WriteLineAsync("Timestamp,Temperature,Humidity,Pressure");
                }
                
            }
            catch (Exception ex)
            {
               await EventLogger.LogExceptionForUserAndToFile(
                    "Could not create CSV file for storing data. Data would not be stored.", ex);
            }

            CSVFilePath = nameToSet;
        }

        public async Task SaveTelemetryToFile(TelemetryStorage telemetry)
        {
            string lineToSave = $"{telemetry.Timestamp},{telemetry.Temperature},{telemetry.Humidity},{telemetry.Pressure}";
            try
            {
                using (StreamWriter writer =new StreamWriter(CSVFilePath,true))
                {
                    await writer.WriteLineAsync(lineToSave);
                }
            }
            catch (Exception ex)
            {
                await EventLogger.LogExceptionForUserAndToFile(
                    $"Could not write this line: {lineToSave} to CSV file. This data would not be stored.", ex);
            }
        }
    }
}
