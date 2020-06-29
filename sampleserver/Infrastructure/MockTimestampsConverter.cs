using System;
using System.Collections.Generic;

namespace sampleserver.Infrastructure
{
    public class MockTimestampsConverter : ITimestampsConverter
    {
        public List<double> ConvertTimestamps(List<DateTime> timestampsToConvert)
        {
            List<double> convertedTimestamps = new List<double>();
            for (int i = 0; i < timestampsToConvert.Count; i++)
                convertedTimestamps.Add(timestampsToConvert[i].AddSeconds(i).ToOADate());

            return convertedTimestamps;
        }
    }
}