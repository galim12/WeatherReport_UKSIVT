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
using WeatherReport_UKSIVT.UserControls;
using WeatherReport_UKSIVT.Week;

namespace WeatherReport_UKSIVT
{
    public partial class MainWindow : Window
    {
        private readonly WeatherUIUpdater _weatherUIUpdater;
        private readonly WeatherWeekService _weatherWeekService;
        private readonly WeatherApiClient _weatherApiClient;
        public MainWindow()
        {
            InitializeComponent();
            _weatherUIUpdater = new WeatherUIUpdater(LeftStack, ImageFon,mapImage, CurrentTemperatureTB, DayOfWeekTextBlock, CloudsTB, CloudsImage, PrecipitationTB, windSpeedTextBlock, windDirectionTextBlock);          
            UpdateWeather();
            _weatherApiClient = new WeatherApiClient();
            _weatherWeekService = new WeatherWeekService();
            Loaded += MainWindow_Loaded;
        }
        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            List<WeatherDayOfWeek> weatherForecast = await _weatherWeekService.GetWeatherForecastAsync();

            if (weatherForecast != null)
            {
                Dictionary<string, CardDay> cardDaysDictionary = new Dictionary<string, CardDay>
                {
                    { "Воскресенье", SundayCardDay },
                    { "Понедельник", MondayCardDay },
                    { "Вторник", TuesdayCardDay },
                    { "Среда", WednesdayCardDay },
                    { "Четверг", ThursdayCardDay },
                    { "Пятница", FridayCardDay },
                    { "Суббота", SaturdayCardDay }
                };

                foreach (var day in weatherForecast)
                {
                    if (cardDaysDictionary.ContainsKey(day.DayOfWeek))
                    {
                        var cardDay = cardDaysDictionary[day.DayOfWeek];
                        cardDay.MaxNum = day.MaxTemperature;
                        cardDay.MinNum = day.MinTemperature;
                        cardDay.Source = day.IconPath;
                    }
                }
            }
        }


        // Метод для обновления карты осадков
        public async Task UpdatePrecipitationMap()
        {
            try
            {
                // Получаем URL карты осадков для города Уфа
                string mapUrl = await _weatherApiClient.GetPrecipitationMapAsync("Ufa");

                // Устанавливаем полученный URL как источник для элемента Image
                if (!string.IsNullOrEmpty(mapUrl))
                {
                    mapImage.Source = new BitmapImage(new Uri(mapUrl));
                }
                else
                {
                    MessageBox.Show("Не удалось получить карту осадков для города Уфа.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при обновлении карты осадков: {ex.Message}");
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
            Application.Current.Shutdown();
        }

        private async void UpdateWeather()
        {
            await _weatherUIUpdater.UpdateWeatherUI(CloudsImage, CurrentTemperatureTB, DayOfWeekTextBlock, CloudsTB, PrecipitationTB, windSpeedTextBlock, windDirectionTextBlock);
            // Получаем прогноз погоды
            List<WeatherDayOfWeek> weatherForecast = await _weatherWeekService.GetWeatherForecastAsync();

            if (weatherForecast != null && weatherForecast.Count >= 7) // Проверяем, что список содержит данные для всех 7 дней недели
            {
                // Обновляем свойства элементов CardDay для каждого дня недели
                SundayCardDay.MaxNum = weatherForecast[0].MaxTemperature;
                SundayCardDay.MinNum = weatherForecast[0].MinTemperature;
                SundayCardDay.Source = weatherForecast[0].IconPath;

                MondayCardDay.MaxNum = weatherForecast[1].MaxTemperature;
                MondayCardDay.MinNum = weatherForecast[1].MinTemperature;
                MondayCardDay.Source = weatherForecast[1].IconPath;

                TuesdayCardDay.MaxNum = weatherForecast[2].MaxTemperature;
                TuesdayCardDay.MinNum = weatherForecast[2].MinTemperature;
                TuesdayCardDay.Source = weatherForecast[2].IconPath;

                WednesdayCardDay.MaxNum = weatherForecast[3].MaxTemperature;
                WednesdayCardDay.MinNum = weatherForecast[3].MinTemperature;
                WednesdayCardDay.Source = weatherForecast[3].IconPath;

                ThursdayCardDay.MaxNum = weatherForecast[4].MaxTemperature;
                ThursdayCardDay.MinNum = weatherForecast[4].MinTemperature;
                ThursdayCardDay.Source = weatherForecast[4].IconPath;

                FridayCardDay.MaxNum = weatherForecast[5].MaxTemperature;
                FridayCardDay.MinNum = weatherForecast[5].MinTemperature;
                FridayCardDay.Source = weatherForecast[5].IconPath;

                SaturdayCardDay.MaxNum = weatherForecast[6].MaxTemperature;
                SaturdayCardDay.MinNum = weatherForecast[6].MinTemperature;
                SaturdayCardDay.Source = weatherForecast[6].IconPath;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var ShowFavorites = new FavouritesSpisok(); 
            ShowFavorites.Show();
            
        }       
    }
}
