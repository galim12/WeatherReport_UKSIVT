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
    private readonly TextBlock _SunsetTxt;
    private readonly TextBlock _SunriseTxt;
    private readonly TextBlock _feelsTxt;
    private readonly TextBlock _grndTxt;
    private readonly TextBlock _rainTxt;
    private readonly TextBlock _windSpeedMaxTextBlock;



    public WeatherUIUpdater(StackPanel stackPanel, Image image, ChromiumWebBrowser web, TextBlock temperatureTextBlock, TextBlock dayOfWeekTextBlock, TextBlock cloudsTextBlock, Image cloudsImage, TextBlock precipitationTB, TextBlock windSpeedTextBlock, TextBlock windDirectionTextBlock, TextBlock SunsetTxt, TextBlock SunriseTxt, TextBlock feelsTxt, TextBlock grndTxt, TextBlock rainTxt, TextBlock windSpeedMaxTextBlock)
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
        _SunsetTxt = SunsetTxt;
        _SunriseTxt = SunriseTxt;
        _feelsTxt = feelsTxt;
        _grndTxt = grndTxt;
        _rainTxt = rainTxt;
        _windSpeedMaxTextBlock = windSpeedMaxTextBlock;
        _weatherApiClient = new WeatherApiClient();
    }
   

    public async Task UpdateWeatherUI(Image image, ChromiumWebBrowser web, TextBlock temperatureTextBlock, TextBlock dayOfWeekTextBlock, TextBlock cloudsTextBlock, TextBlock precipitationTB, TextBlock windSpeedTextBlock, TextBlock windDirectionTextBlock, TextBlock SunriseTxt, TextBlock SunsetTxt, TextBlock Humidity, TextBlock Visibility, TextBlock feelsTxt, TextBlock grndTxt, TextBlock rainTxt, TextBlock windSpeedMaxTextBlock)
    {
        var weatherData = await _weatherApiClient.GetWeatherDataAsync();

        if (weatherData != null)
        {
            var visibilityInKilometers = weatherData.visibility / 1000.0;
            var sunrise = UnixTimeStampToDateTime(weatherData.sys.sunrise).ToString("HH:mm");
            var sunset = UnixTimeStampToDateTime(weatherData.sys.sunset).ToString("HH:mm");
            double grndLevelHpa = weatherData.main.grnd_level * 0.75;
            

            UpdateImage(weatherData.weather[0].icon);
            UpdateTextBlock(temperatureTextBlock, weatherData.main.temp.ToString("0") + "°C");
            UpdateTextBlock(dayOfWeekTextBlock, GetDayOfWeek(weatherData.dt, weatherData.timezone));
            UpdateTextBlock(cloudsTextBlock, weatherData.weather[0].description);
            UpdateTextBlock(precipitationTB, "влажность - " + weatherData.main.humidity + "%");
            UpdateTextBlock(windSpeedTextBlock, weatherData.wind.speed.ToString("0") + "км/ч");
            UpdateTextBlock(windSpeedMaxTextBlock, weatherData.wind.gust.ToString("0") + "км/ч");
            UpdateTextBlock(Humidity, weatherData.main.humidity + "%");
            UpdateTextBlock(Visibility, visibilityInKilometers.ToString());
            UpdateTextBlock(SunriseTxt, sunrise);
            UpdateTextBlock(SunsetTxt, sunset);
            UpdateWindDirection(windDirectionTextBlock, weatherData.wind.deg);
            UpdateTextBlock(grndTxt, grndLevelHpa.ToString("0"));

            if (weatherData.rain != null)
            {
                UpdateTextBlock(rainTxt, weatherData.rain._3h.ToString());
            }
            else
            {
                
                UpdateTextBlock(rainTxt, "Осадков нет");
            }

            UpdateTextBlock(feelsTxt, weatherData.main.feels_like.ToString("0"));

            web.Address = WeatherUrls.PrecipitationMap(weatherData.coord.lat, weatherData.coord.lon);
        }
    }
    public static DateTime UnixTimeStampToDateTime(long unixTimeStamp)
    {

        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dateTime = dateTime.AddSeconds(unixTimeStamp).ToLocalTime();
        return dateTime;
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


    ////////
    private bool isCelsius = true;

    public void ToggleCelsiusUnit(object sender, EventArgs e)
    {
        isCelsius = true;
        UpdateTemperatureUnit();
    }

    public void ToggleFahrenheitUnit(object sender, EventArgs e)
    {
        isCelsius = false;
        UpdateTemperatureUnit();
    }

    private async void UpdateTemperatureUnit()
    {
        var weatherData = await _weatherApiClient.GetWeatherDataAsync();
        if (weatherData != null)
        {
            double temperature = isCelsius ? weatherData.main.temp : ConvertCelsiusToFahrenheit(weatherData.main.temp);
            UpdateTextBlock(_temperatureTextBlock, temperature.ToString("0") + (isCelsius ? "°C" : "°F"));
        }
    }

    private double ConvertCelsiusToFahrenheit(double celsius)
    {
    return celsius * 9 / 5 + 32;
    }
    
}
