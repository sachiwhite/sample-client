using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace sampleserver.Infrastructure
{
    public class MockDataFetcher : IDataFetcher
    {
        public async Task<string> UpdateData()
        {
            await EventLogger.LogForUser("Mock connection established.");
            return "{\"humidity\": 40.0, \"temperature\": 25.0, \"pressure\": 1000.0, \"luminosity\": 0.0, \"lamps\": 1, \"airfans\": 0, \"heaters\": 0, \"timestamp\": \"2020 - 05 - 31 15:05:17\"}";
        }
    }
}
