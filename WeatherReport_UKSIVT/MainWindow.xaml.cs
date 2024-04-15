using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WeatherReport_UKSIVT
{
    public partial class MainWindow : Window
    {
        private readonly WeatherUIUpdater _weatherUIUpdater;
        public MainWindow()
        {
            InitializeComponent();
            _weatherUIUpdater = new WeatherUIUpdater(LeftStack, ImageFon, CurrentTemperatureTB, DayOfWeekTextBlock, CloudsTB, CloudsImage, PrecipitationTB, windSpeedTextBlock, windDirectionTextBlock);          
            UpdateWeather();          
        }

        private void CloseImage_MouseEnter(object sender, MouseEventArgs e)
        {
            Image image = sender as Image;
            if (image != null)
            {
                image.Source = new BitmapImage(new Uri("/Images/close_dark.png", UriKind.Relative));
            }
        }

        private void CloseImage_MouseLeave(object sender, MouseEventArgs e)
        {
            Image image = sender as Image;
            if (image != null)
            {
                image.Source = new BitmapImage(new Uri("/Images/close_white.png", UriKind.Relative));
            }
        }

        private void CloseImage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private async void UpdateWeather()
        {
            await _weatherUIUpdater.UpdateWeatherUI(CloudsImage, CurrentTemperatureTB, DayOfWeekTextBlock, CloudsTB, PrecipitationTB, windSpeedTextBlock, windDirectionTextBlock);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var ShowFavorites = new FavouritesSpisok(); 
            ShowFavorites.Show();
            
        }       
    }
}
