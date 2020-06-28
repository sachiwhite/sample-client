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

        public PlotCreator(DelayProvider delayProvider)
        {
            this.delayProvider = delayProvider;
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

            for (int i = 0; i < timestampOfMeasures.Count; i++)
            {
                //if using real data
                convertedTimestamps.Add(timestampOfMeasures[i].ToOADate());
                
                //if using mock
                //convertedTimestamps.Add(timestampOfMeasures[i].AddSeconds(i).ToOADate());
            }

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
