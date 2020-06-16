using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace sampleserver
{
    public static class EventLogger
    {
        private static string EventLogFilePath;
        public static string ErrorLog;
        static EventLogger()
        {
            OnLoading();
        }
        public static async Task OnLoading()
        {
            DateTime startupTime = DateTime.Now;
            EventLogFilePath=$"eventLog{startupTime.Year}{startupTime.Month}{startupTime.Day}{startupTime.Hour}{startupTime.Minute}.txt";
            string startingLine = $"SAMPLE client started at: {startupTime} \n";
            
            using (StreamWriter writer = new StreamWriter(EventLogFilePath,true))
            {
                
                await writer.WriteLineAsync(startingLine);
            }
            ErrorLog+=startingLine;
        }
        public static async Task LogExceptionToFile(string eventMessage,string exMessage, string stackTrace)
        {
            using (StreamWriter writer = new StreamWriter(EventLogFilePath, true))
            {
                await writer.WriteLineAsync($"{DateTime.Now}: {eventMessage}");
                await writer.WriteLineAsync(exMessage);
                await writer.WriteLineAsync(stackTrace);
            }
        }
        public static async Task LogEventToFile(string eventMessage)
        {
            using (StreamWriter writer = new StreamWriter(EventLogFilePath, true))
            {
                await writer.WriteLineAsync($"{DateTime.Now}: {eventMessage}");
            }
        }
        public static async Task LogForUser(string eventMessage)
        {
            
            await LogEventToFile(eventMessage);
            ErrorLog+=$"{DateTime.Now}: {eventMessage} \n";

        }
    }
}
