using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace sampleserver.Infrastructure
{
    public class TelecommandSender :ITelecommandSender
    {
        private string RequestUri = "192.168.1.100:80/post_telecommand";
        private ConnectionConfiguration configuration;

        public TelecommandSender(ConnectionConfiguration configuration)
        {
            this.configuration = configuration;
            RequestUri = $"{this.configuration.RequestUri}/post_telecommand";
        }
        public async Task SendTelecommandAsync(string command)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(new UriBuilder(RequestUri).Uri);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";
            try
            {
                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    string json = "{\"telecommand\":"+"\""+command+"\"}";

                    await streamWriter.WriteAsync(json);
                }
            }
            catch (WebException webEx)
            {
                Debug.WriteLine(webEx.Message);
                Debug.WriteLine(webEx.Response);
               
            }
            try
            {
                var webResponse = await httpWebRequest.GetResponseAsync();
                 var httpResponse = (HttpWebResponse)webResponse;
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = await streamReader.ReadToEndAsync();
                    Debug.WriteLine(result);
                }
            }
            catch (WebException webEx)
            {
                Debug.WriteLine(webEx.Message);
                Debug.WriteLine(webEx.Response);

            }        

            
        }
       
    }
}
