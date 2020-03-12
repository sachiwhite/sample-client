using sampleserver.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace sampleserver.Models
{
    public enum ValuesMeasured
    {
        Humidity,
        Pressure,
        Light_Intensity,
        Lights,
        Temperature,
        Airfan,
        Heater
    }
   public class TelemetryInformationContainer
    {
        private PlotCreator creator;
        public DataFetcher DataFetcher { get; }
        public List<DateTime> LastTimestamps { get; private set; }
        private Dictionary<string, IDataItem> Measures;

        public TelemetryInformationContainer()
        {
            DataFetcher = new DataFetcher();
            creator = new PlotCreator();
            Measures = new Dictionary<string, IDataItem>();
            //for debugging purpose, it will be deleted when timestamps can be 
            LastTimestamps = new List<DateTime>();
            LastTimestamps.Add(new DateTime(2019, 3, 12, 0, 0, 0));
        }
        public void UpdateItems()
        {
            var dataToProcess = DataFetcher.GetFetchNumericData();
            foreach (var item in dataToProcess)
            {
                var key = item.Key;
                var value = item.Value;
                if(Measures.ContainsKey(key))
                {
                    Measures[key].LastMeasures.Add(value);
                }
                else
                {
                    var measureToAdd = new IDataItem(value);
                    Measures.Add(key, measureToAdd);
                }
            }
            foreach (var measure in Measures)
            {
                creator.ReturnPlot(LastTimestamps[0], measure.Value.LastMeasures, measure.Key);
            }  
        }

    }
}
