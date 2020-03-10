using System;
using System.Collections.Generic;
using System.Text;

namespace sampleserver.Models
{
   public class TelemetryInformationContainer
    {
        public List<DateTime> LastTimestamps { get; private set; }
        public IDataItem[] Measures { get; set; }

        public TelemetryInformationContainer(int size)
        {
            Measures = new IDataItem[size];
        }

    }
}
