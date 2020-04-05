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

namespace sampleserver.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private TelemetryInformationContainer telemetryInformationContainer;
        private TelecommandData telecommandData;
        
        private async Task ChangeImage(IChangeImages window)
        {
            while (true)
            {
                await Task.Run(async () =>
                {
                    telemetryInformationContainer.UpdateItems();
                    await Task.Delay(4000);
                });
                if (window != null)
                {
                    for (int i = 0; i < 7; i++)
                        window.ChangePic(i);
                }

            }
            
        }

        public MainWindowViewModel()
        {
            
            telecommandData = new TelecommandData();
            ChangePictureCommand = ReactiveCommand.CreateFromTask<IChangeImages>(ChangeImage);
            SendTelemetryCommand = ReactiveCommand.CreateFromTask<string>(SendTelemetry);
            telemetryInformationContainer = new TelemetryInformationContainer();
        }

        private async Task SendTelemetry(string command)
        {
            await telecommandData.SendTelecommand(command);
        }

        public ReactiveCommand<IChangeImages, Unit> ChangePictureCommand { get; }
        public ReactiveCommand<string, Unit> SendTelemetryCommand { get; }
    }
}


