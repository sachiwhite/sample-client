using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;

namespace sampleserver.Views
{
    public enum Images
    {
        Humidity,
        Pressure,
        Light_intensity,
        No_of_lamps,
        Temperature,
        No_of_airfans,
        No_of_heaters
    }
    public class MainWindow : Window, IChangeImages
    {
        private const string FileName = @"C:\Users\lewon\source\repos\sample-client\sampleserver\Assets\";
        private Image[] images;
        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            
        }
    
        public void ChangePic(int number)
        {
            var filename = FileName + (Images)(number) + ".png";
            Bitmap bitmap = new Bitmap(filename);
            images[number].Source = bitmap;
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
        }
    }

    public interface IChangeImages
    {
        void ChangePic(int number);
    }
}
