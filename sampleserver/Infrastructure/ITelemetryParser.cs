using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace sampleserver.Infrastructure
{
    public interface ITelemetryParser
    {
        Task<bool> UpdateData();
        Task<DateTime?> GetTimestamp();
        Dictionary<string, double> FetchNumericData();
       
    }
}
