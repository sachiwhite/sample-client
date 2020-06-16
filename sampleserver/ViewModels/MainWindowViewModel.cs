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
        private const int Miliseconds = 1000;
        private readonly int delay = 1*Miliseconds;
        private readonly TelemetryInformationContainer telemetryInformationContainer;
        private readonly TelecommandData telecommandData;
        public string EventLog
        {
            get => EventLogger.ErrorLog;
            set => this.RaiseAndSetIfChanged(ref EventLogger.ErrorLog, value);
        }

        private async Task ChangeImage(IChangeImages window)
        {
            while (true)
            {
                await Task.Run(async () =>
                {
                    await telemetryInformationContainer.UpdateItems();
                    
                    await Task.Delay(millisecondsDelay: delay);
                });
                if (window != null)
                {
                    for (int i = 0; i < 7; i++)
                        window.ChangePic(i);
                }
                this.RaisePropertyChanged("EventLog");

            }

        }
        //for designer purposes
        public MainWindowViewModel()
        {

        }
        public MainWindowViewModel(TelecommandData telecommandData, TelemetryInformationContainer telemetryInformationContainer = null)
        {

            this.telecommandData = telecommandData;
            this.telemetryInformationContainer = telemetryInformationContainer;
            ChangePictureCommand = ReactiveCommand.CreateFromTask<IChangeImages>(ChangeImage);
            SendTelemetryCommand = ReactiveCommand.CreateFromTask<string>(SendTelemetry);
            DownloadPictureCommand = ReactiveCommand.CreateFromTask<IChangeImages>(DownloadPicture);

        }

        private async Task DownloadPicture(IChangeImages window)
        {
            var downloadedImageFilePath = await telemetryInformationContainer.FetchPicture();
            if (downloadedImageFilePath!=string.Empty)
            {
                window?.UpdatePicture(downloadedImageFilePath); 
            }
            this.RaisePropertyChanged("EventLog");
            

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


