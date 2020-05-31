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
       
        }

        private void ConfigureContainer()
        {
            container = new StandardKernel();
            container.Bind<ConnectionConfiguration>().ToSelf().InSingletonScope();
            container.Bind<IDataFetcher>().To<DataFetcher>();
            container.Bind<IPictureFetcher>().To<PictureFetcher>();
            container.Bind<ITelemetryParser>().To<TelemetryParser>();
            container.Bind<ITelecommandSender>().To<TelecommandSender>();
            container.Bind<TelecommandData>().ToSelf();
            container.Bind<TelemetryInformationContainer>().ToSelf();
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                var telecommandData = container.Get<TelecommandData>();
                var telemetryInformationContainer = container.Get<TelemetryInformationContainer>();
                desktop.MainWindow = new MainWindow
                {
                    DataContext = new MainWindowViewModel(telecommandData, telemetryInformationContainer),
                };
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}
