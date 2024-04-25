using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WeatherReport_UKSIVT.Week
{
    public class WeatherWeekService
    {
        private const string WeatherMapApiKey = "f92c20202c6ad67391b1116545dc8561";
        private const string WeatherMapApiUrl = "http://api.openweathermap.org/data/2.5/forecast?q=Ufa&units=metric&cnt=7&appid=" + WeatherMapApiKey;

        public async Task<List<WeatherDayOfWeek>> GetWeatherForecastAsync()
        {
            using (var client = new HttpClient())
            {
                try
                {
                    var response = await client.GetAsync(WeatherMapApiUrl);
                    response.EnsureSuccessStatusCode();
                    var responseBody = await response.Content.ReadAsStringAsync();
                    var weatherForecast = ParseWeatherForecast(responseBody);
                    return weatherForecast;
                }
                catch (HttpRequestException ex)
                {
                    // Обработка ошибок запроса к API
                    MessageBox.Show($"Ошибка HTTP-запроса: {ex.Message}");
                    return null;
                }
            }
        }

        private List<WeatherDayOfWeek> ParseWeatherForecast(string responseBody)
        {
            var weatherForecast = new List<WeatherDayOfWeek>();

            dynamic json = JsonConvert.DeserializeObject(responseBody);
            foreach (var item in json.list)
            {
                string dateString = item.dt_txt.ToString(); // Преобразуем dt_txt в строку
                string dayOfWeek = DateTime.Parse(dateString).ToString("dddd");
                string maxTemp = $"{item.main.temp_max:0}°";
                string minTemp = $"{item.main.temp_min:0}°";
                string iconPath = $"/Images/{item.weather[0].icon}.png";

                weatherForecast.Add(new WeatherDayOfWeek { DayOfWeek = dayOfWeek, MaxTemperature = maxTemp, MinTemperature = minTemp, IconPath = iconPath });
            }

            return weatherForecast;
        }
    }
}
