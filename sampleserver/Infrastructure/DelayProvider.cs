using System;
using System.Collections.Generic;
using System.Text;

namespace sampleserver.Infrastructure
{
    public class DelayProvider
    {
        public double DelayInMiliseconds { get; private set; }
        public double DelayInSeconds { get; private set; }
        public double DelayInMinutes { get; private set; }

        public DelayProvider()
        {
            ChangeDelayFromDelayInMinutes(0.016);
        }
        public void ChangeDelayFromDelayInMinutes(double delayInMinutes)
        {
            DelayInMinutes = delayInMinutes;
            DelayInSeconds = delayInMinutes * 60;
            DelayInMiliseconds = DelayInSeconds * 1000; 
        }
    }
}
