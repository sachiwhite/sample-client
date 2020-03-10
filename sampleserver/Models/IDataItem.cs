using System;
using System.Collections.Generic;
using System.Text;

namespace sampleserver.Models
{
    public interface IDataItem
    {
        public List<double> Last20Measures { get; }
        
    }
}
