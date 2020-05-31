using System.Threading.Tasks;

namespace sampleserver.Infrastructure
{
    public interface ITelecommandSender
    {
        Task SendTelecommandAsync(string command);
    }
}