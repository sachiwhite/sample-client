using ScottPlot;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace sampleserver.Infrastructure
{
    public class PlotCreator
    {
       public void ReturnPlot(DateTime start, List<double> lastMeasures, string name)
        {
            var plot = new Plot(400, 200);
            double points = 24 * 60 / 5;
            plot.PlotSignal(lastMeasures.ToArray(), sampleRate: points, xOffset: start.ToOADate(), yOffset:lastMeasures.Min()-1) ;
            plot.Ticks(dateTimeX: true, displayTickLabelsX: true, displayTicksXminor: true);
            plot.YLabel(name);
            plot.SaveFig(@$"C:\Users\lewon\source\repos\sample-client\sampleserver\Assets\{name}.png");
        }
    }
}
