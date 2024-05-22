using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Shapes;

namespace WeatherReport_UKSIVT
{
    /// <summary>
    /// Логика взаимодействия для FavouritesSpisok.xaml
    /// </summary>
    public partial class FavouritesSpisok : Window
    {
        public event Action<string> CitySelected;
        private List<FavoriteCity> _favoriteCities;
        public FavouritesSpisok()
        {
            InitializeComponent();
            LoadFavoriteCities();
            listView.SelectionChanged += ListView_SelectionChanged;

        }
        private void LoadFavoriteCities()
        {
            _favoriteCities = FavoriteCitiesManager.LoadFavoriteCitiesAsObjects();
            listView.ItemsSource = _favoriteCities;
        }
        private async void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            var selectedCity = (sender as ListView).SelectedItem as FavoriteCity;

            if (selectedCity != null)
            {
                try
                {
                    MainWindow mainWindow = Application.Current.MainWindow as MainWindow;
                    mainWindow.cityNameView.Text = selectedCity.CityName;


                    mainWindow.LoadWeatherData(selectedCity.CityName);
                    mainWindow.UpdateWeather(selectedCity.CityName);
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при смене города: " + ex.Message);
                }
            }
        }
        private void DeleteImage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var city = ((FrameworkElement)sender).DataContext as FavoriteCity;
            if (city != null)
            {
                FavoriteCitiesManager.RemoveCity(city.CityName);
                LoadFavoriteCities();
            }
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
            Close();
        }

        
        public class MyData
        {
            public string IconSource { get; set; }
            public string ButtonContent { get; set; }
            public string DeleteSource { get; set; }
        }
        public class FavoriteCity
        {
            public string CityName { get; set; }
            public string IconSource { get; set; } = "/Images/city.png"; 
            public string DeleteSource { get; set; } = "/Images/delete_city.png"; 
        }

        
    }
}

        
   
