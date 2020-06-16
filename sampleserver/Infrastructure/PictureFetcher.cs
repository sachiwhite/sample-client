using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace sampleserver.Infrastructure
{
    public class PictureFetcher : IPictureFetcher
    {
        private string FileName = @"C:\Users\lewon\source\repos\sample-client\sampleserver\Assets\downloaded_photos\";

        public string LastPictureFetchedPath{get =>$"{FileName}{fileNameCore}{fileNumber}{extension}"; }
        private const string fileNameCore= "downloaded_photo";
        private int fileNumber = 1;
        private readonly ConnectionConfiguration configuration;
        private const string extension = ".png";
        public PictureFetcher(ConnectionConfiguration configuration)
        {
            this.configuration=configuration;
            SetLastPictureFetchedPath();
        }

        private void SetLastPictureFetchedPath()
        {
            while (File.Exists(LastPictureFetchedPath))
            {
                fileNumber++;
            }
        }

        private async Task<HttpResponseMessage> GetResponse()
        {
            HttpResponseMessage response = new HttpResponseMessage(){StatusCode = HttpStatusCode.ServiceUnavailable};
            using (HttpClient client = new HttpClient())
            {
                Uri address = new UriBuilder($@"{configuration.RequestUri}/photo").Uri;
                
                try
                {
                    response = await client.GetAsync(address);
                    return response;
                }
                catch (ArgumentNullException ex)
                {
                    string EventMessage = "An Uri was null. ";
                    await EventLogger.LogForUser(EventMessage);
                    await EventLogger.LogExceptionToFile(EventMessage, ex.Message, ex.StackTrace);
                }
                catch (HttpRequestException ex)
                {
                    string EventMessage = "A request to download the picture failed. ";
                    await EventLogger.LogForUser(EventMessage);
                    await EventLogger.LogExceptionToFile(EventMessage, ex.Message, ex.StackTrace);
                    
                }
                catch (Exception ex)
                {
                    string EventMessage = "An unknown exception occurred during downloading picture. ";
                    await EventLogger.LogForUser(EventMessage);
                    await EventLogger.LogExceptionToFile(EventMessage, ex.Message, ex.StackTrace);
                    
                }
                
            }

            return response;
        }
        public async Task<bool> FetchPicture()
        {

                    var response = await GetResponse();
                    if (!response.IsSuccessStatusCode) return false;

                    using (Stream stream = await response.Content.ReadAsStreamAsync())
                    {
                        using (var downloadedImage = Image.FromStream(stream))
                        {
                            fileNumber++;
                            try
                            {
                                downloadedImage.Save(LastPictureFetchedPath, ImageFormat.Png);
                            }
                            catch (ArgumentNullException ex)
                            {
                                string EventMessage = "An filename or encoder used in PictureFetcher was null. ";
                                await EventLogger.LogForUser(EventMessage);
                                await EventLogger.LogExceptionToFile(EventMessage, ex.Message, ex.StackTrace);
                                return false;
                            }
                            catch (ExternalException ex)
                            {
                                string EventMessage = "There was an attempt to save picture with wrong image format. ";
                                await EventLogger.LogForUser(EventMessage);
                                await EventLogger.LogExceptionToFile(EventMessage, ex.Message, ex.StackTrace);
                                return false;
                            }
                            catch (Exception ex)
                            {
                                string EventMessage =
                                    "An unknown error occurred in PictureFetcher while saving photo. ";
                                await EventLogger.LogForUser(EventMessage);
                                await EventLogger.LogExceptionToFile(EventMessage, ex.Message, ex.StackTrace);
                                return false;
                            }


                        }
                    }
                    return true;
        }
    }
}
