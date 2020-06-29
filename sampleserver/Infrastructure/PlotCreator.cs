using ScottPlot;
using ScottPlot.Config;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Linq;
using Avalonia.Controls.Platform;

namespace sampleserver.Infrastructure
{
    public class PlotCreator
    {
        
        private readonly DelayProvider delayProvider;
        private readonly ITimestampsConverter timestampsConverter;

        public PlotCreator(DelayProvider delayProvider, ITimestampsConverter timestampsConverter)
        {
            this.delayProvider = delayProvider;
            this.timestampsConverter = timestampsConverter;
        }
        public void ReturnPlot(List<DateTime> timestampOfMeasures, List<double> lastMeasures, string name, double minimumValueToBeShownOnPlot, double maximumValueToBeShownOnPlot)
        {
            var plot = new Plot(300, 150);
            int pointCount = 20;
            List<double> dates = new List<double>();
            List<double> ys = new List<double>();
            var start = timestampOfMeasures[0];

            for (int i = pointCount - lastMeasures.Count; i >= 0; i--)
            {
                ys.Add(0);
                dates.Add(start.AddSeconds(-i * delayProvider.DelayInSeconds - 1).ToOADate());
            }

            List<double> convertedTimestamps = new List<double>();
            List<double> timestampsOfMeasuresConvertedToADate = timestampsConverter.ConvertTimestamps(timestampOfMeasures);
            convertedTimestamps.AddRange(timestampsOfMeasuresConvertedToADate);

            ys.AddRange(lastMeasures);
            dates.AddRange(convertedTimestamps);

            plot.PlotScatter(dates.ToArray(), ys.ToArray());
            plot.Ticks(dateTimeX: true);
            plot.YLabel(name);
            plot.Axis(y1: minimumValueToBeShownOnPlot, y2: maximumValueToBeShownOnPlot);
            plot.SaveFig(@$"..\Assets\{name}.png");

        }

    }
}
