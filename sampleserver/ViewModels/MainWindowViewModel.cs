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
using sampleserver.Infrastructure;

namespace sampleserver.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly TelemetryInformationContainer telemetryInformationContainer;
        private readonly TelecommandData telecommandData;
        private readonly DelayProvider delayProvider;

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
                    
                    await Task.Delay(millisecondsDelay: (int)delayProvider.DelayInMiliseconds);
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
        public MainWindowViewModel(DelayProvider delayProvider, TelecommandData telecommandData, TelemetryInformationContainer telemetryInformationContainer = null)
        {
            this.delayProvider = delayProvider;
            this.telecommandData = telecommandData;
            this.telemetryInformationContainer = telemetryInformationContainer;
            ChangePictureCommand = ReactiveCommand.CreateFromTask<IChangeImages>(ChangeImage);
            SendTelemetryCommand = ReactiveCommand.CreateFromTask<string>(SendTelemetry);
            DownloadPictureCommand = ReactiveCommand.CreateFromTask<IChangeImages>(DownloadPicture);
            SetTimeCommand = ReactiveCommand.CreateFromTask<string>(SetDelayInMinutes);

        }

        private async Task SetDelayInMinutes(string delay)
        {
            if (double.TryParse(delay, out double minutesDelay))
            {
                delayProvider.ChangeDelayFromDelayInMinutes(minutesDelay);
            }
            else
            {
                await EventLogger.LogForUser("Changing time between measurements was unsuccesful.");
                this.RaisePropertyChanged("EventLog");
            }

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

        public ReactiveCommand<string,Unit> SetTimeCommand { get;}
        public ReactiveCommand<IChangeImages, Unit> DownloadPictureCommand { get; }
        public ReactiveCommand<IChangeImages, Unit> ChangePictureCommand { get; }
        public ReactiveCommand<string, Unit> SendTelemetryCommand { get; }
    }
}


