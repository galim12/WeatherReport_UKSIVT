using Newtonsoft.Json;
using System;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using WeatherReport_UKSIVT.UserControls;
using static System.Net.Mime.MediaTypeNames.Image;

public class WeatherUIUpdater
{
    private readonly StackPanel _stackPanel;
    private readonly WeatherApiClient _weatherApiClient;
    private readonly Image _image;
    private readonly TextBlock _temperatureTextBlock;
    private readonly TextBlock _dayOfWeekTextBlock;
    private readonly TextBlock _cloudsTextBlock;
    private readonly Image _cloudsImage;
    private readonly TextBlock _precipitationTB;
    private readonly TextBlock _windSpeedTextBlock;
    private readonly TextBlock windDirectionTextBlock;
    private readonly Image _mapImage;


    public WeatherUIUpdater(StackPanel stackPanel, Image image, Image mapImage, TextBlock temperatureTextBlock, TextBlock dayOfWeekTextBlock, TextBlock cloudsTextBlock, Image cloudsImage, TextBlock precipitationTB, TextBlock windSpeedTextBlock, TextBlock windDirectionTextBlock)
    {
        _mapImage = mapImage;
        _stackPanel = stackPanel;
        _image = image;
        _temperatureTextBlock = temperatureTextBlock;
        _dayOfWeekTextBlock = dayOfWeekTextBlock;
        _cloudsTextBlock = cloudsTextBlock;
        _cloudsImage = cloudsImage;
        _precipitationTB = precipitationTB;
        _windSpeedTextBlock = windSpeedTextBlock;
        _weatherApiClient = new WeatherApiClient();
    }
    

    public async Task UpdateWeatherUI(Image image, TextBlock temperatureTextBlock, TextBlock dayOfWeekTextBlock, TextBlock cloudsTextBlock, TextBlock precipitationTB, TextBlock windSpeedTextBlock, TextBlock windDirectionTextBlock)
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
        string imagePath;

        // Подбираем путь к изображению в зависимости от значений icon
        switch (icon)
        {
            case "01d": // ясно (день)
                imagePath = "/Images/sun.png";
                break;
            //case "01n": // ясно (ночь)
            //    imagePath = "/Images/clear_night.png";

            case "02d": // немного облаков (день)
                imagePath = "/Images/sun_cloud.png";
                break;
            case "03d": // облачно (день)
            case "04d": // облачно (день)
                imagePath = "/Images/cloud.png";
                break;
            //case "02n": // немного облаков (ночь)
            //case "03n": // облачно (ночь)
            //case "04n": // облачно (ночь)
            //    imagePath = "/Images/cloudy_night.png";
            //    break;
            case "09d": // дождь (день)
                imagePath = "/Images/rain_cloud.png";
                break;
            case "10d": // ливень (день)
                imagePath = "/Images/rain.png";
                break;
            case "11d": // гроза (день)
                imagePath = "/Images/storm.png";
                break;
            case "09n": // дождь (ночь)
            case "10n": // ливень (ночь)
            case "11n": // гроза (ночь)
                imagePath = "/Images/storm.png";
                break;
            case "13d": // снег (день)
                imagePath = "/Images/snow.png";
                break;
            case "13n": // снег (ночь)
                imagePath = "/Images/snow.png";
                break;
            case "50d": // туман (день)
            case "50n": // туман (ночь)
                imagePath = "/Images/fog.png";
                break;
            default:
                imagePath = "/Images/default.png"; // Если погода неопределенна
                break;
        }

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
        return localDateTime.ToString("dddd, HH:mm");
    }
    
}
