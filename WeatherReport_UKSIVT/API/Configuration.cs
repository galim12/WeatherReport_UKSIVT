namespace WeatherReport_UKSIVT.API
{
    public class Configuration
    {
        public const string WeatherMapApiKey = "f92c20202c6ad67391b1116545dc8561";
    }

    public class WeatherUrls
    {
        public const string PrecipiationMap = "https://openweathermap.org/weathermap?basemap=map&cities=false&layer=precipitation&lat=30&lon=-20&zoom=3";

        public static string WeekForecast(string zoneName)
        {
            return "https://api.openweathermap.org/data/2.5/forecast?q=" + zoneName + "&units=metric&appid=" + Configuration.WeatherMapApiKey;
        }
        public static string SearchForecast(string zoneName)
        {
            return "http://api.openweathermap.org/data/2.5/weather?q=" + zoneName + "&units=metric&lang=ru&appid=" + Configuration.WeatherMapApiKey;
        }

        public static string PrecipitationMap(string lat,  string lon)
        {
            
            return "https://yandex.ru/weather/maps/nowcast?ll=37.657869_55.672620&z=12&lat=" + lat + "&lon=" + lon;
        }
    }
}
