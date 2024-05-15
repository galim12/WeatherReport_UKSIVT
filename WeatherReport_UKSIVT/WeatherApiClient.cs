using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WeatherReport_UKSIVT.API;
public class WeatherApiClient
{
    public async Task<WeatherData> GetWeatherDataAsync()
    {
        using (var client = new HttpClient())
        {
            try
            {
                var response = await client.GetAsync(WeatherUrls.SearchForecast("UFA"));
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
    public Coordinates coord { get; set; }
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

public class Coordinates
{
    public string lon { get; set; }
    public string lat { get; set; }
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

