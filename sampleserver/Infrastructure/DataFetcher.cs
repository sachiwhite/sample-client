using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

namespace sampleserver.Infrastructure
{
    public class DataFetcher : IDataFetcher
    {
        private string RequestUri = "192.168.1.100:80/get_telemetry";
        private ConnectionConfiguration configuration;

        public DataFetcher(ConnectionConfiguration configuration)
        {
            this.configuration = configuration;
            RequestUri = $"{configuration.RequestUri}/get_telemetry";
        }
        public async Task<string> UpdateData()
        {
            string json = string.Empty;
            using (var httpClient = new HttpClient())
            {
                try
                {
                    var Uri = new UriBuilder(RequestUri).Uri;
                    json = await httpClient.GetStringAsync(Uri);
                }
                catch(ArgumentNullException anex)
                {
                    Debug.WriteLine(anex.Message);
                    Debug.WriteLine(anex.StackTrace);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    Debug.WriteLine(ex.StackTrace);

                }
                
            }
            await EventLogger.LogForUser("Connection established.");
            return json;
        }
    }
}
