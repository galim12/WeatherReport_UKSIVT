using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using WeatherReport_UKSIVT.UserControls;
using WeatherReport_UKSIVT.Week;
using WeatherReport_UKSIVT.API;
using CefSharp;

namespace WeatherReport_UKSIVT
{
    public partial class MainWindow : Window
    {
        private readonly WeatherUIUpdater _weatherUIUpdater;
        private readonly WeatherWeekService _weatherWeekService;
        private readonly WeatherApiClient _weatherApiClient;
        private List<WeatherDayOfWeek> weatherForecast;


        public MainWindow()
        {
            InitializeComponent();

            WebView.FrameLoadEnd += (sender, args) =>
            {
                if (args.Frame.IsMain)
                {
                    WebView.SetZoomLevel(-3);
                }
            };

            _weatherUIUpdater = new WeatherUIUpdater(LeftStack, ImageFon, WebView, CurrentTemperatureTB, DayOfWeekTextBlock, CloudsTB, CloudsImage, PrecipitationTB, windSpeedTextBlock, windDirectionTextBlock);          
            UpdateWeather();
            _weatherApiClient = new WeatherApiClient();
            _weatherWeekService = new WeatherWeekService();
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
            await _weatherUIUpdater.UpdateWeatherUI(CloudsImage, WebView, CurrentTemperatureTB, DayOfWeekTextBlock, CloudsTB, PrecipitationTB, windSpeedTextBlock, windDirectionTextBlock);

            weatherForecast = await _weatherWeekService.GetWeatherForecastAsync();

            if (weatherForecast != null) 
            {
                foreach (WeatherDayOfWeek dayForecast in weatherForecast)
                {
                    CardDay day = new CardDay();
                    day.MouseEnter += (sender, e) => Day_MouseEnter(sender, e, dayForecast);
                    day.Day = dayForecast.DayOfWeek;
                    day.MaxNum = dayForecast.MaxTemperature;
                    day.MinNum = dayForecast.MinTemperature;
                    day.Source = dayForecast.IconPath;
                    DayCardPanel.Children.Add(day);

                }
            }
        }

        private void Day_MouseEnter(object sender, MouseEventArgs e, WeatherDayOfWeek dayForecast)
        {
            WeatherDayOfWeek day = GetDataDayWeather(dayForecast.DayOfWeek);

            ForDay.Text = "На " + Helper.DayOfWeekToCorrect(day.DayOfWeek);

            string[] directions = { "С", "ССВ", "СВ", "ВСВ", "В", "ВЮВ", "ЮВ", "ЮЮВ", "Ю", "ЮЮЗ", "ЮЗ", "ЗЮЗ", "З", "ЗСЗ", "СЗ", "ССЗ" };
            int index = (int)Math.Round(day.WindDeg / 22.5) % directions.Length;
            windDirectionTextBlock.Text = directions[index];
            windSpeedTextBlock.Text = day.Wind.ToString() + "км/ч";

            Humidity.Text = day.Humidity.ToString();
            if (day.Humidity >= 60)
            {
                HumidityText.Text = "Высокая";
                HumidityIcon.Source = new BitmapImage(new Uri("/Images/dislike.png", UriKind.Relative));
            }
            else if (day.Humidity <= 40)
            {
                HumidityText.Text = "Низкая";
                HumidityIcon.Source = new BitmapImage(new Uri("/Images/like.png", UriKind.Relative));
            }
            else 
            {
                HumidityText.Text = "Средняя";
                HumidityIcon.Source = new BitmapImage(new Uri("/Images/like.png", UriKind.Relative));
            }

            double visibilityInKilometers = day.Visibility / 1000.0;
            Visibility.Text = visibilityInKilometers.ToString("0.##");
            if (day.Visibility >= 6000)
            {
                VisibilityName.Text = "Хорошая";
                VisibilityIcon.Source = new BitmapImage(new Uri("/Images/happy.png", UriKind.Relative));
            }
            else if (day.Visibility <= 4000)
            {
                VisibilityName.Text = "Слабая";
                VisibilityIcon.Source = new BitmapImage(new Uri("/Images/dislike.png", UriKind.Relative));
            }
            else
            {
                VisibilityName.Text = "Средняя";
                VisibilityIcon.Source = new BitmapImage(new Uri("/Images/dislike.png", UriKind.Relative));
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var ShowFavorites = new FavouritesSpisok(); 
            ShowFavorites.Show();
        }

        private WeatherDayOfWeek GetDataDayWeather(string dayNameRu)
        {
            if (weatherForecast != null)
            {
                foreach (var day in weatherForecast)
                {
                    if (day.DayOfWeek == dayNameRu)
                    {
                        return day;
                    }
                }
            }

            return null;
        }
    }
}
