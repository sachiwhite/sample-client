using System.Threading.Tasks;

namespace sampleserver.Infrastructure
{
    public interface IPictureFetcher
    {
        string LastPictureFetchedPath { get; }

        Task<bool> FetchPicture();
    }
}