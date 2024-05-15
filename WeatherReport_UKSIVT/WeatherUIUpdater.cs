using System;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using WeatherReport_UKSIVT.API;
using System.Globalization;
using CefSharp.Wpf;

public class WeatherUIUpdater
{
    private readonly StackPanel _stackPanel;
    private readonly WeatherApiClient _weatherApiClient;
    private readonly Image _image;
    private readonly ChromiumWebBrowser _web;
    private readonly TextBlock _temperatureTextBlock;
    private readonly TextBlock _dayOfWeekTextBlock;
    private readonly TextBlock _cloudsTextBlock;
    private readonly Image _cloudsImage;
    private readonly TextBlock _precipitationTB;
    private readonly TextBlock _windSpeedTextBlock;


    public WeatherUIUpdater(StackPanel stackPanel, Image image, ChromiumWebBrowser web, TextBlock temperatureTextBlock, TextBlock dayOfWeekTextBlock, TextBlock cloudsTextBlock, Image cloudsImage, TextBlock precipitationTB, TextBlock windSpeedTextBlock, TextBlock windDirectionTextBlock)
    {
        _stackPanel = stackPanel;
        _image = image;
        _web = web;
        _temperatureTextBlock = temperatureTextBlock;
        _dayOfWeekTextBlock = dayOfWeekTextBlock;
        _cloudsTextBlock = cloudsTextBlock;
        _cloudsImage = cloudsImage;
        _precipitationTB = precipitationTB;
        _windSpeedTextBlock = windSpeedTextBlock;
        _weatherApiClient = new WeatherApiClient();
    }
    

    public async Task UpdateWeatherUI(Image image, ChromiumWebBrowser web, TextBlock temperatureTextBlock, TextBlock dayOfWeekTextBlock, TextBlock cloudsTextBlock, TextBlock precipitationTB, TextBlock windSpeedTextBlock, TextBlock windDirectionTextBlock)
    {
        var weatherData = await _weatherApiClient.GetWeatherDataAsync();

        if (weatherData != null)
        {
            UpdateImage(weatherData.weather[0].icon);
            UpdateTextBlock(temperatureTextBlock, weatherData.main.temp.ToString("0") + "°C");
            UpdateTextBlock(dayOfWeekTextBlock, GetDayOfWeek(weatherData.dt, weatherData.timezone));
            UpdateTextBlock(cloudsTextBlock, weatherData.weather[0].description);
            UpdateTextBlock(precipitationTB, "влажность - " + weatherData.main.humidity + "%");
            UpdateTextBlock(windSpeedTextBlock, weatherData.wind.speed.ToString("0") + "км/ч");

            UpdateWindDirection(windDirectionTextBlock, weatherData.wind.deg);

            web.Address = WeatherUrls.PrecipitationMap(weatherData.coord.lat, weatherData.coord.lon);
        }
    }

    
    public void UpdateWindDirection(TextBlock windDirectionTextBlock, int deg)
    {
        string direction = GetWindDirection(deg);
        UpdateTextBlock(windDirectionTextBlock, direction);
    }
    public string GetWindDirection(int degree)
    {
        string[] directions = { "С", "ССВ", "СВ", "ВСВ", "В", "ВЮВ", "ЮВ", "ЮЮВ", "Ю", "ЮЮЗ", "ЮЗ", "ЗЮЗ", "З", "ЗСЗ", "СЗ", "ССЗ" };

        int index = (int)Math.Round(degree / 22.5);
        index %= 16;

        return directions[index];
    }
    public WeatherUIUpdater(StackPanel stackPanel)
    {
        _stackPanel = stackPanel;
    }


    private void UpdateImage(string icon)
    {
        string imagePath = Helper.GetImagePathByIcon(icon);

        if (_stackPanel.Children.Count > 0 && _stackPanel.Children[0] is Image image)
        {
            image.Source = new BitmapImage(new Uri(imagePath, UriKind.Relative));
        }       
    }
    private void UpdateTextBlock(TextBlock textBlock, string text)
    {
        if (textBlock != null)
        {
            textBlock.Text = text;
        }
        else
        {
            Console.WriteLine("TextBlock is null");
        }
    }   
    private string GetDayOfWeek(long unixTime, int timezoneOffset)
    {
        var utcDateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(unixTime).ToUniversalTime();
        var localDateTime = utcDateTimeOffset.ToOffset(TimeSpan.FromSeconds(timezoneOffset)).DateTime;

        var russianCulture = new CultureInfo("ru-RU");
        return localDateTime.ToString("dddd, HH:mm", russianCulture);
    }
    
}
