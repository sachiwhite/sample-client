using System.Threading.Tasks;

namespace sampleserver.Infrastructure
{
    public class MockPictureFetcher :IPictureFetcher
    {
        public string LastPictureFetchedPath => "..\\Assets\\downloaded_photos\\downloaded_photo10.png";
        public async Task<bool> FetchPicture()
        {
            return true;
        }
    }
}