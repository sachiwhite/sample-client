using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace sampleserver.Infrastructure
{
    public class TelecommandSender
    {
        private string RequestUri = "192.168.0.18:2137";
        private static readonly HttpClient client = new HttpClient();
        public async Task SendTelecommandAsync(string command)
        {
            var values = new Dictionary<string, string>
            {
                {"telecommand", command }
            };
            var content = new FormUrlEncodedContent(values);
            HttpResponseMessage response = null;
            try
            {
                response = await client.PostAsync(new UriBuilder(RequestUri + "/telecommands").Uri, content);
            }
            //catch (ArgumentNullException ex)
            //{
            //    //TODO: displaying errors

            //}
            catch (HttpRequestException hex)
            {
                //TODO: displaying errors
            }
            if (response != null)
            {
                var responseString = await response.Content.ReadAsStringAsync();
                Console.WriteLine(responseString);
            }

        }
    }
}
