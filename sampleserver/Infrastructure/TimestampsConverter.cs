using System;
using System.Collections.Generic;

namespace sampleserver.Infrastructure
{
    public class TimestampsConverter : ITimestampsConverter
    {
        public List<double> ConvertTimestamps(List<DateTime> timestampsToConvert)
        {
            List<double> convertedTimestamps = new List<double>();
            for (int i = 0; i < timestampsToConvert.Count; i++)
                convertedTimestamps.Add(timestampsToConvert[i].ToOADate());   

            return convertedTimestamps;
        }
    }
}