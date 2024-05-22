using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WeatherReport_UKSIVT.API;
public class WeatherApiClient
{
    public async Task<WeatherData> GetWeatherDataAsync(string cityName)
    {
        using (var client = new HttpClient())
        {
            try
            {
                var response = await client.GetAsync(WeatherUrls.SearchForecast(cityName));

                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    var weatherData = JsonConvert.DeserializeObject<WeatherData>(responseBody);
                    return weatherData;
                }
                else if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    Console.WriteLine("Город не найден");
                    return null;
                }
                else
                {
                    Console.WriteLine($"Ошибка HTTP-запроса: {response.StatusCode}");
                    return null;
                }
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
    public Rain rain { get; set; }
    public Suns sys { get; set; }
    public long dt { get; set; }
    public int timezone { get; set; }
    public int visibility {  get; set; }
}

public class MainData
{
    public double temp { get; set; }
    public int humidity { get; set; }
    public double feels_like{  get; set; }
    public double grnd_level { get; set; }
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
    public double gust { get; set; }
}
public class Rain
{
    public double _3h { get; set; }
}
public class Suns
{
    public long sunrise { get; set; }

    public long sunset { get; set; }
}

