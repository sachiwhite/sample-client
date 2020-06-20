using System;
using System.Collections.Generic;
using System.Text;

namespace sampleserver.Models
{
    public class IDataItem
    {
        public List<double> LastMeasures { get; }
        public double MinimumValueForPlotting { get; }
        public double MaximumValueForPlotting { get; }


        public static IDataItem ReturnDataItem(string key, double firstValue)
        {
            Enum.TryParse(key, out Parameters p);
            switch (p)
            {
                case Parameters.Humidity:
                        return new IDataItem(firstValue, 0, 100);
                case Parameters.Light_intensity: 
                        return new IDataItem(firstValue, -0.5, 100);
                case Parameters.No_of_airfans: 
                        return new IDataItem(firstValue, -0.5, 5);
                case Parameters.No_of_heaters: 
                    return new IDataItem(firstValue, -0.5, 5); 
                case Parameters.No_of_lamps: 
                    return new IDataItem(firstValue, -0.5, 5);
                case Parameters.Pressure: 
                    return new IDataItem(firstValue, 950, 1050); 
                case Parameters.Temperature: 
                    return new IDataItem(firstValue, 15, 50);
                default: return new IDataItem(firstValue, -1, -1);

            }
        }
        private IDataItem(double firstValue, double minimumValueForPlotting, double maximumValueForPlotting)
        {
            MinimumValueForPlotting = minimumValueForPlotting;
            MaximumValueForPlotting = maximumValueForPlotting;
            LastMeasures = new List<double>();
            LastMeasures.Add(firstValue);
        }

    }
}
