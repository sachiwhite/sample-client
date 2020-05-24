using Avalonia.Media.Imaging;
using Avalonia.Threading;
using ReactiveUI;
using sampleserver.Models;
using sampleserver.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using Splat;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;

namespace sampleserver.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private string FileName = @"C:\Users\lewon\source\repos\sample-client\sampleserver\Assets\downloaded_photos\";
        private TelemetryInformationContainer telemetryInformationContainer;
        private TelecommandData telecommandData;
        
        private async Task ChangeImage(IChangeImages window)
        {
            while (true)
            {
                await Task.Run(async () =>
                {
                    telemetryInformationContainer.UpdateItems();
                    await Task.Delay(1000);
                });
                if (window != null)
                {
                    for (int i = 0; i < 7; i++)
                        window.ChangePic(i);
                }

            }
            
        }

        public MainWindowViewModel(TelemetryInformationContainer telemetryInformationContainer=null)
        {

            telecommandData=new TelecommandData();
            this.telemetryInformationContainer = telemetryInformationContainer; 
            ChangePictureCommand = ReactiveCommand.CreateFromTask<IChangeImages>(ChangeImage);
            SendTelemetryCommand = ReactiveCommand.CreateFromTask<string>(SendTelemetry);
            DownloadPictureCommand = ReactiveCommand.CreateFromTask<IChangeImages>(DownloadPicture);
           
        }

        private async Task DownloadPicture(IChangeImages window)
        {
            var downloadedImageFilePath = telemetryInformationContainer.FetchPicture();
            if (window!=null)
            {
                window.UpdatePicture(downloadedImageFilePath);
            }
           
        }

        private async Task SendTelemetry(string command)
        {
            await telecommandData.SendTelecommand(command);
        }
        public ReactiveCommand<IChangeImages, Unit> DownloadPictureCommand { get; }
        public ReactiveCommand<IChangeImages, Unit> ChangePictureCommand { get; }
        public ReactiveCommand<string, Unit> SendTelemetryCommand { get; }
    }
}


