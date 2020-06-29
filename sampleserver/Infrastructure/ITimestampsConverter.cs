using System;
using System.Collections.Generic;

namespace sampleserver.Infrastructure
{
    public interface ITimestampsConverter
    {
        List<double> ConvertTimestamps(List<DateTime> timestampsToConvert);
    }
}