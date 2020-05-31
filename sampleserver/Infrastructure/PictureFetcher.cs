using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Net.Http;
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
        private ConnectionConfiguration configuration;
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

        public async Task<bool> FetchPicture()
        {
            using (HttpClient client = new HttpClient())
            {

                Uri address = new UriBuilder($@"{configuration.RequestUri}/photo").Uri;
                using (HttpResponseMessage response =await client.GetAsync(address))
                using (Stream stream = await response.Content.ReadAsStreamAsync())
                {
                    using (var downloadedImage = Image.FromStream(stream))
                    {
                        fileNumber++;
                        downloadedImage.Save(LastPictureFetchedPath, ImageFormat.Png);

                    }
                }
                
            }
            return true;
        }
    }
}
