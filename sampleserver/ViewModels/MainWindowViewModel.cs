using Avalonia.Threading;
using ReactiveUI;
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
        private string RequestUri = "127.0.0.1:2137";
        public MainWindowViewModel()
        {
            
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
            
            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 5, 0);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
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
            catch (ArgumentNullException ex)
            {
                //TODO: displaying errors
                
            }
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

