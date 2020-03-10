using ScottPlot;
using System;
using System.Collections.Generic;
using System.Text;

namespace sampleserver.Infrastructure
{
    public class PlotCreator
    {

        public void ReturnPlot()
        {
            double[] dataX = new double[] { 1, 2, 3, 4, 5 };
            double[] dataY = new double[] { 1, 4, 9, 16, 25 };
            
            var plt = new ScottPlot.Plot(399, 200);
            plt.PlotScatter(dataX, dataY);
            plt.SaveFig("quickstart1.png");
        }
                
    }
}
