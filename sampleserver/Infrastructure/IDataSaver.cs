using System.Threading.Tasks;

namespace sampleserver.Infrastructure
{
    public interface IDataSaver
    {
        Task SaveTelemetryToFile(TelemetryStorage telemetry);
    }
}