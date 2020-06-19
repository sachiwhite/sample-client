using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using System.Diagnostics;

namespace sampleserver.Views
{
    public class MainWindow : Window, IChangeImages
    {
        private const string FileName = @"..\Assets\";
        private Image[] images;
        private Image downloadedPicture;
        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            
        }
    
        public async void ChangePic(int number)
        {
            var filename = FileName + (Parameters)(number) + ".png";
            try
            {
                Bitmap bitmap = new Bitmap(filename);
                images[number].Source = bitmap;
            }
            catch (System.Exception ex)
            {
                string EventMessage= "An unknown error while updating photo. ";
                await EventLogger.LogForUser(EventMessage);
                await EventLogger.LogExceptionToFile(EventMessage, ex.Message, ex.StackTrace);
            }
           
        }

        public async void UpdatePicture(string filename)
        {
            try
            {
                Bitmap bitmap = new Bitmap(filename);
                downloadedPicture.Source = bitmap;
            }
            catch (System.Exception ex)
            {
                string EventMessage= "An unknown error while updating photo. ";
                await EventLogger.LogForUser(EventMessage);
                await EventLogger.LogExceptionToFile(EventMessage, ex.Message, ex.StackTrace);
            }
           
            
        }


        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
            images = new Image[7];
            images[0] = this.FindControl<Image>("humidity");
            images[1] = this.FindControl<Image>("pressure");
            images[2] = this.FindControl<Image>("light_intensity");
            images[3] = this.FindControl<Image>("no_of_lamps");
            images[4] = this.FindControl<Image>("temperature");
            images[5] = this.FindControl<Image>("no_of_airfans");
            images[6] = this.FindControl<Image>("no_of_heaters");
            downloadedPicture = this.FindControl<Image>("downloaded_photo");
            
            
        }
    }
}