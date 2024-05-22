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
using System.Windows.Media;
using OxyPlot;
using OxyPlot.Series;
using OxyPlot.Axes;
using OxyPlot.Legends;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;

namespace WeatherReport_UKSIVT
{
    public partial class MainWindow : Window
    {
        private readonly WeatherUIUpdater _weatherUIUpdater;
        private readonly WeatherWeekService _weatherWeekService;
        private readonly WeatherApiClient _weatherApiClient;
        private List<WeatherDayOfWeek> weatherForecast;
        private bool _isCelsius = true;



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
            
            _weatherUIUpdater = new WeatherUIUpdater(LeftStack, ImageFon, WebView, CurrentTemperatureTB, DayOfWeekTextBlock, CloudsTB, CloudsImage, PrecipitationTB, windSpeedTextBlock, windDirectionTextBlock, SunriseTxt, SunsetTxt, feelsTxt, grndTxt, rainTxt, windSpeedMaxTextBlock);
            celsiusButton.Click += CelsiusButton_Click;
            fahrenheitButton.Click += FahrenheitButton_Click;

            string defaultCity = "Уфа";
            cityNameView.Text = defaultCity;
            LoadWeatherData(defaultCity);
            UpdateWeather(defaultCity);
            _weatherApiClient = new WeatherApiClient();
            _weatherWeekService = new WeatherWeekService();

        }
        

        

        public async void LoadWeatherData(string cityName)
        {
            WeatherWeekService weatherService = new WeatherWeekService();
            List<WeatherDayOfWeek> weatherData = await weatherService.GetWeatherForecastAsync(cityName);

            if (weatherData != null)
            {
                PlotModel plotModel = new PlotModel { Title = "Как будет меняться погода в течении 5 дней" };

                // Настройка осей
                var valueAxis = new LinearAxis
                {
                    Position = AxisPosition.Left,
                    Title = "Температура °C",
                    MajorGridlineStyle = LineStyle.Solid,
                    MinorGridlineStyle = LineStyle.Dot
                };
                plotModel.Axes.Add(valueAxis);

                var dateAxis = new DateTimeAxis
                {
                    Position = AxisPosition.Bottom,
                    StringFormat = "dd/MM",
                    Title = "Дата",
                    MajorGridlineStyle = LineStyle.Solid,
                    MinorGridlineStyle = LineStyle.Dot
                };
                plotModel.Axes.Add(dateAxis);

                // Настройка серий данных
                LineSeries maxTempSeries = new LineSeries
                {
                    Title = "Макс. температура",
                    MarkerType = MarkerType.Circle,
                    MarkerSize = 4,
                    MarkerStroke = OxyColors.Red,
                    Color = OxyColors.Red
                };

                LineSeries minTempSeries = new LineSeries
                {
                    Title = "Мин. температура",
                    MarkerType = MarkerType.Circle,
                    MarkerSize = 4,
                    MarkerStroke = OxyColors.Blue,
                    Color = OxyColors.Blue
                };

                
                foreach (var day in weatherData)
                {
                    if (double.TryParse(day.MaxTemperature.TrimEnd('°', 'C', 'F'), out double maxTemp) &&
                        double.TryParse(day.MinTemperature.TrimEnd('°', 'C', 'F'), out double minTemp))
                    {
                        maxTempSeries.Points.Add(new DataPoint(DateTimeAxis.ToDouble(day.Date), maxTemp));
                        minTempSeries.Points.Add(new DataPoint(DateTimeAxis.ToDouble(day.Date), minTemp));
                    }
                    else
                    {
                        
                        MessageBox.Show($"Не удалось преобразовать строку температуры для даты {day.Date}");
                    }
                }

                plotModel.Series.Add(maxTempSeries);
                plotModel.Series.Add(minTempSeries);

                
                var legend = new Legend
                {
                    LegendPosition = LegendPosition.TopRight,
                    LegendPlacement = LegendPlacement.Outside,
                    LegendOrientation = LegendOrientation.Horizontal,
                    LegendBackground = OxyColors.White,
                    LegendBorder = OxyColors.Black,
                    LegendBorderThickness = 1,
                    LegendMargin = 10,
                    LegendPadding = 10,
                };
                plotModel.Legends.Add(legend);

                WeatherPlot.Model = plotModel;
            }
            else
            {
                MessageBox.Show("Не удалось загрузить данные о погоде.");
            }
        }

       
        private void CelsiusButton_Click(object sender, RoutedEventArgs e)
        {
            _isCelsius = true;
            _weatherUIUpdater.ToggleCelsiusUnit(sender, e);

            celsiusButton.Background = Brushes.Black;
            celsiusButton.Foreground = Brushes.White;

            fahrenheitButton.ClearValue(Button.BackgroundProperty);
            fahrenheitButton.ClearValue(Button.ForegroundProperty);

            UpdateTemperatureUnits();
        }

        private void FahrenheitButton_Click(object sender, RoutedEventArgs e)
        {
            _isCelsius = false;
            _weatherUIUpdater.ToggleFahrenheitUnit(sender, e);

            fahrenheitButton.Background = Brushes.Black;
            fahrenheitButton.Foreground = Brushes.White;

            celsiusButton.ClearValue(Button.BackgroundProperty);
            celsiusButton.ClearValue(Button.ForegroundProperty);

            UpdateTemperatureUnits();
        }
        private void UpdateTemperatureUnits()
        {
            foreach (var day in weatherForecast)
            {
                if (_isCelsius)
                {
                    day.MaxTemperature = $"{day.MaxTemperatureCelsius:0}°C";
                    day.MinTemperature = $"{day.MinTemperatureCelsius:0}°C";
                }
                else
                {
                    day.MaxTemperature = $"{day.MaxTemperatureFahrenheit:0}°F";
                    day.MinTemperature = $"{day.MinTemperatureFahrenheit:0}°F";
                }
            }

            UpdateWeatherCards();
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

        public async void UpdateWeather(string cityName)
        {           
            await _weatherUIUpdater.UpdateWeatherUI(cityName, CloudsImage, WebView, CurrentTemperatureTB, DayOfWeekTextBlock, CloudsTB, PrecipitationTB, windSpeedTextBlock, windDirectionTextBlock, SunriseTxt, SunsetTxt, Humidity, Visibility, feelsTxt, grndTxt, rainTxt, windSpeedMaxTextBlock);

            weatherForecast = await _weatherWeekService.GetWeatherForecastAsync(cityName);

            if (weatherForecast != null)
            {
                for (int i = 0; i < weatherForecast.Count; i++)
                {
                    WeatherDayOfWeek dayForecast = weatherForecast[i];

                    if (i < DayCardPanel.Children.Count)
                    {
                        // Обновляем существующий CardDay
                        CardDay day = (CardDay)DayCardPanel.Children[i];
                        UpdateCardDay(day, dayForecast);
                    }
                    else
                    {
                        // Добавляем новые CardDay, если недостаточно существующих
                        CardDay day = new CardDay();
                        UpdateCardDay(day, dayForecast);
                        day.MouseEnter += (sender, e) => Day_MouseEnter(sender, e, dayForecast);
                        DayCardPanel.Children.Add(day);
                    }
                }

                // Удаляем лишние CardDay, если они есть
                while (DayCardPanel.Children.Count > weatherForecast.Count)
                {
                    DayCardPanel.Children.RemoveAt(DayCardPanel.Children.Count - 1);
                }
            }
        }
        private void UpdateWeatherCards()
        {
            for (int i = 0; i < weatherForecast.Count; i++)
            {
                WeatherDayOfWeek dayForecast = weatherForecast[i];

                if (i < DayCardPanel.Children.Count)
                {
                    CardDay day = (CardDay)DayCardPanel.Children[i];
                    UpdateCardDay(day, dayForecast);
                }
            }
        }
        private void UpdateCardDay(CardDay card, WeatherDayOfWeek dayForecast)
        {
            card.Day = dayForecast.DayOfWeek;
            card.MaxNum = dayForecast.MaxTemperature;
            card.MinNum = dayForecast.MinTemperature;
            card.Source = dayForecast.IconPath;
        }

        private void Day_MouseEnter(object sender, MouseEventArgs e, WeatherDayOfWeek dayForecast)
        {
            WeatherDayOfWeek day = GetDataDayWeather(dayForecast.DayOfWeek);

            ForDay.Text = "На " + Helper.DayOfWeekToCorrect(day.DayOfWeek);
            
            string[] directions = { "С", "ССВ", "СВ", "ВСВ", "В", "ВЮВ", "ЮВ", "ЮЮВ", "Ю", "ЮЮЗ", "ЮЗ", "ЗЮЗ", "З", "ЗСЗ", "СЗ", "ССЗ" };
            int index = (int)Math.Round(day.WindDeg / 22.5) % directions.Length;

            windDirectionTextBlock.Text = directions[index];
            windSpeedTextBlock.Text = day.Wind.ToString() + "км/ч";
            windSpeedMaxTextBlock.Text = day.GustWind.ToString() + "км/ч";
            feelsTxt.Text = day.Feels_like.ToString("0");
            grndTxt.Text = day.Grnd_level.ToString("0");
            Humidity.Text = day.Humidity.ToString();
            
            
            //влажность
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


            //давление
            if (day.Grnd_level >= 770)
            {
                grndTxt.Text = "Повышенное";
                grndIcon.Source = new BitmapImage(new Uri("/Images/dislike.png", UriKind.Relative));
            }
            else if (day.Grnd_level <= 769)
            {
                grndText.Text = "Нормальное";
                grndIcon.Source = new BitmapImage(new Uri("/Images/like.png", UriKind.Relative));
            }
            else if(day.Grnd_level <= 740)
            {
                grndText.Text = "Низкое";
                grndIcon.Source = new BitmapImage(new Uri("/Images/dislike.png", UriKind.Relative));
            }


            //видимость
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

        private void imageBtn_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                string cityName = cityTextBox.Text;
                cityNameView.Text = cityName;
                LoadWeatherData(cityName);
                UpdateWeather(cityName);
                cityTextBox.Text = "";
            }
            catch (Exception ex)
            {
                cityNameView.Text = "";
                MessageBox.Show("Некорректно введен город" + ex.Message);
                
            }
        }

        private void favBtn_MouseDown(object sender, MouseButtonEventArgs e)
        {
            cityTextBox.Text = "";
            var ShowFavorites = new FavouritesSpisok(); 
            ShowFavorites.Show();
            
        }

        private void FavouritesWindow_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string cityName = cityTextBox.Text;
                if (!string.IsNullOrEmpty(cityName))
                {
                    FavoriteCitiesManager.AddCity(cityName);
                    cityTextBox.Text = "";

                }
                else
                {
                    MessageBox.Show("Введите название города");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при добавлении города в избранное: " + ex.Message);
            }
        }
    }
}
