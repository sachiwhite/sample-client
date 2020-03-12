using System;
using System.Collections.Generic;
using System.Text;

namespace sampleserver.Models
{
    public class IDataItem
    {
        public List<double> LastMeasures { get; }
        public IDataItem()
        {
            LastMeasures = new List<double>();
        }
        public IDataItem(double firstValue) 
        {
            LastMeasures = new List<double>();
            LastMeasures.Add(firstValue);
        }
        
    }
}
