﻿using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

namespace sampleserver.Infrastructure
{
    public class DataFetcher : IDataFetcher
    {
        private readonly string RequestUri;
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
                    
                    await EventLogger.LogForUser("Connection established.");
                }
                catch (ArgumentNullException ex)
                {
                    string EventMessage= "The request Uri was null. ";
                    await EventLogger.LogExceptionForUserAndToFile(EventMessage,ex);

                }
                catch (HttpRequestException ex)
                {
                    string EventMessage= "The HTTP request failed. ";
                    await EventLogger.LogExceptionForUserAndToFile(EventMessage,ex);
                     
                }
                catch (Exception ex)
                {
                    string EventMessage= "An unknown error occurred in DataFetcher while fetching data. ";
                    await EventLogger.LogExceptionForUserAndToFile(EventMessage,ex);
                     

                }
                
            }
           
            return json;
        }
    }
}
