using sampleserver.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace sampleserver.Models
{
    
   public class TelemetryInformationContainer
    {
        private IPictureFetcher pictureFetcher;
        private PlotCreator creator;
        public ITelemetryParser DataParser { get; }
        public List<DateTime> LastTimestamps { get; private set; }
        private Dictionary<string, IDataItem> Measures;
        private readonly int countOfRecordsToStore;

        public TelemetryInformationContainer(ITelemetryParser telemetryParser, IPictureFetcher pictureFetcher)
        {
            countOfRecordsToStore = 20;
            this.pictureFetcher=pictureFetcher;
            DataParser = telemetryParser;
            creator = new PlotCreator();
            Measures = new Dictionary<string, IDataItem>();
            LastTimestamps = new List<DateTime>();
        }
        public async Task UpdateItems()
        {
            if (LastTimestamps.Count>countOfRecordsToStore)
                FlushOldRecords();
            
            await DataParser.UpdateData();
            
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
        public string FetchPicture()
        {
            pictureFetcher.FetchPicture();
            return pictureFetcher.LastPictureFetchedPath;
        }
        private void FlushOldRecords()
        {
            LastTimestamps.RemoveAt(0);
            foreach (var measure in Measures)
            {
                var key = measure.Key;
                Measures[key].LastMeasures.RemoveAt(0);
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
