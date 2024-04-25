using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

public class WeatherApiClient
{
    private const string WeatherMapApiKey = "f92c20202c6ad67391b1116545dc8561";
    private const string WeatherMapApiUrl = "http://api.openweathermap.org/data/2.5/weather?q=UFA&units=metric&lang=ru&appid=" + WeatherMapApiKey;

    public async Task<WeatherData> GetWeatherDataAsync()
    {
        using (var client = new HttpClient())
        {
            try
            {
                var response = await client.GetAsync(WeatherMapApiUrl);
                response.EnsureSuccessStatusCode();
                var responseBody = await response.Content.ReadAsStringAsync();
                var weatherData = JsonConvert.DeserializeObject<WeatherData>(responseBody);
                return weatherData;
            }
            catch (HttpRequestException ex)
            {               
                Console.WriteLine($"Ошибка HTTP-запроса: {ex.Message}");
                return null;          
            }
        }

    }
}
public class WeatherData
{
    public MainData main { get; set; }
    public Weather[] weather { get; set; }
    public Clouds clouds { get; set; }
    public Wind wind {  get; set; }
    public long dt { get; set; }
  
    public int timezone { get; set; }
}

public class MainData
{
    public double temp { get; set; }
    public int humidity { get; set; }
}

public class Weather
{
    public string icon { get; set; }
    public string description { get; set; }
}

public class Clouds
{
    public int all { get; set; }
}
public class Wind
{
    public double speed { get; set; }
    public int deg {  get; set; }
}

