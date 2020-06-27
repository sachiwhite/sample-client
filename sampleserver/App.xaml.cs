using System;
using System.IO;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Ninject;
using Ninject.Syntax;
using sampleserver.Infrastructure;
using sampleserver.Models;
using sampleserver.ViewModels;
using sampleserver.Views;
using Splat;

namespace sampleserver
{
    public class App : Application
    {
        private IKernel container;   
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
            ConfigureContainer();
            ConfigureDirectories();
       
        }

        
        private void ConfigureDirectories()
        {
            if (!Directory.Exists("..\\Assets"))
                Directory.CreateDirectory("..\\Assets");
         
            if (!Directory.Exists("..\\Assets\\downloaded_photos"))
                Directory.CreateDirectory("..\\Assets\\downloaded_photos");
        }
        private void ConfigureContainer()
        {
            container = new StandardKernel();
            container.Bind<ConnectionConfiguration>().ToSelf().InSingletonScope();
            container.Bind<DelayProvider>().ToSelf().InSingletonScope();
            #warning using mock
            container.Bind<IDataFetcher>().To<MockDataFetcher>();
            container.Bind<IPictureFetcher>().To<MockPictureFetcher>();
            
            container.Bind<IDataSaver>().To<CSVDataSaver>();
            container.Bind<ITelemetryParser>().To<TelemetryParser>();
            container.Bind<ITelecommandSender>().To<TelecommandSender>();
            container.Bind<TelecommandData>().ToSelf();
            container.Bind<TelemetryInformationContainer>().ToSelf();
        }

        public override async void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                var telecommandData = container.Get<TelecommandData>();
                var delayProvider = container.Get<DelayProvider>();
                var telemetryInformationContainer = container.Get<TelemetryInformationContainer>();
                desktop.MainWindow = new MainWindow
                {
                    DataContext = new MainWindowViewModel(delayProvider, telecommandData, telemetryInformationContainer),
                };
            }
            
            base.OnFrameworkInitializationCompleted();
        }
    }
}
