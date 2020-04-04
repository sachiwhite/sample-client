using sampleserver.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace sampleserver.Models
{
    
   public class TelemetryInformationContainer
    {
        
        private PlotCreator creator;
        public ITelemetryParser DataParser { get; }
        public List<DateTime> LastTimestamps { get; private set; }
        private Dictionary<string, IDataItem> Measures;

        public TelemetryInformationContainer()
        {
            DataParser = new TelemetryParser(new DataFetcher());
            creator = new PlotCreator();
            Measures = new Dictionary<string, IDataItem>();
            LastTimestamps = new List<DateTime>();
        }
        public TelemetryInformationContainer(ITelemetryParser telemetryParser)
        {
            DataParser = telemetryParser;
            creator = new PlotCreator();
            Measures = new Dictionary<string, IDataItem>();
            LastTimestamps = new List<DateTime>();
        }
        public void UpdateItems()
        {
            DataParser.UpdateData();
            
            var timestampToAdd = DataParser.GetTimestamp();
            var dataToProcess = DataParser.FetchNumericData();
            if (timestampToAdd!=null || dataToProcess.Count!=0)
            {
                LastTimestamps.Add((DateTime)timestampToAdd);
                foreach (var item in dataToProcess)
                {
                    var key = item.Key;
                    var value = item.Value;
                    if (Measures.ContainsKey(key))
                    {
                        Measures[key].LastMeasures.Add(value);
                    }
                    else
                    {
                        var measureToAdd = new IDataItem(value);
                        Measures.Add(key, measureToAdd);
                    }
                }
                CreatePlots();
            }
            
        }
        private void CreatePlots()
        {
            foreach (var measure in Measures)
            {
                creator.ReturnPlot(LastTimestamps[0], measure.Value.LastMeasures, measure.Key);
            }
        }
    }
}
