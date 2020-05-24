namespace sampleserver.Infrastructure
{
    public interface IPictureFetcher
    {
        string LastPictureFetchedPath { get; }

        public bool FetchPicture();
    }
}