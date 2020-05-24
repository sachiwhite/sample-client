using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Text;

namespace sampleserver.Infrastructure
{
    public class PictureFetcher : IPictureFetcher
    {
        private string FileName = @"C:\Users\lewon\source\repos\sample-client\sampleserver\Assets\downloaded_photos\";

        public string LastPictureFetchedPath{get =>$"{FileName}{fileNameCore}{fileNumber}{extension}"; }
        private const string fileNameCore= "downloaded_photo";
        private int fileNumber = 1;
        private const string extension = ".png";
        public PictureFetcher()
        {
            SetLastPictureFetchedPath();
        }

        private void SetLastPictureFetchedPath()
        {
            while (File.Exists(LastPictureFetchedPath))
            {
                fileNumber++;
            }
        }

        public bool FetchPicture()
        {
            using (WebClient client = new WebClient())
            {

                Uri address = new UriBuilder(@"http://192.168.1.100:2137/photo").Uri;
                var imageData = client.DownloadData(address);
                using (MemoryStream memoryStream = new MemoryStream(imageData))
                {
                    using (var downloadedImage = Image.FromStream(memoryStream))
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
