using System;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

public class WeatherUIUpdater
{
    private readonly StackPanel _stackPanel;
    private readonly WeatherApiClient _weatherApiClient;

    public WeatherUIUpdater(StackPanel stackPanel)
    {
        _stackPanel = stackPanel;
        _weatherApiClient = new WeatherApiClient();
    }

    public async Task UpdateWeatherUI()
    {
        var weatherData = await _weatherApiClient.GetWeatherDataAsync();

        if (weatherData != null)
        {
            UpdateImage("/Images/" + weatherData.weather[0].icon + ".png");
            UpdateTextBlock(weatherData.main.temp.ToString("0") + "°C", 0);
            UpdateTextBlock(GetDayOfWeek(weatherData.dt, weatherData.timezone), 1); // Используем timezone
            UpdateTextBlock("Облачность - " + weatherData.clouds.all + "%", 2); // Индекс обновлен
            UpdateTextBlock("Осадки - " + (weatherData.clouds.all / 100.0) + "%", 3); // Индекс обновлен
        }
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

    private void UpdateTextBlock(string text, int index)
    {
        var element = _stackPanel.Children[index + 1];

        // Проверяем, что элемент является TextBlock перед обновлением
        if (element is TextBlock textBlock)
        {
            textBlock.Text = text;
        }
        else
        {
            Console.WriteLine("Элемент не является TextBlock");
        }
    }

    private string GetDayOfWeek(long unixTime, int timezoneOffset)
    {
        var utcDateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(unixTime).ToUniversalTime();
        var localDateTime = utcDateTimeOffset.ToOffset(TimeSpan.FromSeconds(timezoneOffset)).DateTime;
        return localDateTime.ToString("dddd, HH:mm");
    }


}
