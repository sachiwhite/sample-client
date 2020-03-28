using Avalonia.Threading;
using ReactiveUI;
using sampleserver.Models;
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
        private DispatcherTimer timer;
        private static readonly HttpClient client = new HttpClient();
        private string greeting = "";
        private string RequestUri = "192.168.0.19:2137";
        private TelemetryInformationContainer telemetryInformationContainer;
        private string humidityPicPath;
        public string HumidityPicPath
        {
            get { return humidityPicPath; }
            set { humidityPicPath = value; }
        }


        //for debugging purposes
        int i = 0;
        public MainWindowViewModel()
        {
            humidityPicPath=@"C:\Users\lewon\source\repos\sample-client\sampleserver\Assets\Humidity.png";
            SendTelemetryCommand = ReactiveCommand.CreateFromTask<string>(SendTelemetry);
             GetDataFromIndex = ReactiveCommand.Create(() =>
            {
                
                Greeting = String.Empty;
                var builder = new UriBuilder(RequestUri);
                var uri = builder.Uri;
                WebClient client = new WebClient();
                var data = client.OpenRead(uri);
                using (StreamReader reader = new StreamReader(data))
                {
                    Greeting = reader.ReadToEnd();
                }
            });
            telemetryInformationContainer = new TelemetryInformationContainer();
            timer = new DispatcherTimer();
            // timer.Interval = new TimeSpan(0, 5, 0); //real value
            timer.Interval = new TimeSpan(0, 0, 5); //value for tests
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            telemetryInformationContainer.UpdateItems();
            i++;
            if (i == 10) timer.Stop();
            //TODO: update the model and the view every 5 minutes
        }

        private async Task SendTelemetry(string command)
        {
            var values = new Dictionary<string, string>
            {
                {"telecommand", command }
            };
            var content = new FormUrlEncodedContent(values);
            HttpResponseMessage response = null;
            try
            {
                response = await client.PostAsync(new UriBuilder(RequestUri + "/telecommands").Uri, content);
            }
            //catch (ArgumentNullException ex)
            //{
            //    //TODO: displaying errors
                
            //}
            catch(HttpRequestException hex)
            {
                //TODO: displaying errors
            }
            if (response!=null)
            {
                var responseString = await response.Content.ReadAsStringAsync();
                Greeting = responseString;
            }
            
        }

        public string Greeting
        {
            get { return greeting; }
            set { greeting = this.RaiseAndSetIfChanged(ref greeting, value); }
        }
        public ReactiveCommand<string, Unit> SendTelemetryCommand { get; }
        public ReactiveCommand<Unit, Unit> GetDataFromIndex { get; } 
        public ReactiveCommand<Unit, Unit> GetDataFromXYZ { get; }
    }
}

