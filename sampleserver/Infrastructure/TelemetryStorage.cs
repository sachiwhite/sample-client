namespace sampleserver.Infrastructure
{
    public class TelemetryStorage
    {
        public double Humidity { get; set; }
        public double Temperature { get; set; }
        public double Pressure { get; set; }
        public double Luminosity { get; set; }
        public double Lamps { get; set; }
        public double Airfans { get; set; }
        public double Heaters { get; set; }
        public string Timestamp { get; set; }

        public double[] ReturnArrayOfParameters()
        {
            return new double[]
            {
                 Humidity,
                 Pressure,
                 Luminosity,
                 Lamps,
                 Temperature,
                 Airfans,
                 Heaters
            };
        }
    }
}