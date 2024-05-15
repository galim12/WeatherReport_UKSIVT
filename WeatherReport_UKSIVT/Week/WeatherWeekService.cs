using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using WeatherReport_UKSIVT.API;

namespace WeatherReport_UKSIVT.Week
{
    public class WeatherWeekService
    {
        public async Task<List<WeatherDayOfWeek>> GetWeatherForecastAsync()
        {
            using (var client = new HttpClient())
            {
                try
                {
                    var response = await client.GetAsync(WeatherUrls.WeekForecast("UFA"));
                    response.EnsureSuccessStatusCode();
                    var responseBody = await response.Content.ReadAsStringAsync();
                    var weatherForecast = ParseWeatherForecast(responseBody);
                    return weatherForecast;
                }
                catch (HttpRequestException ex)
                {

                    MessageBox.Show($"Ошибка HTTP-запроса: {ex.Message}");
                    return null;
                }
            }
        }
        
        private List<WeatherDayOfWeek> ParseWeatherForecast(string responseBody)
        {
            var weatherForecast = new List<WeatherDayOfWeek>();
            List<string> dates = new List<string>();

            dynamic json = JsonConvert.DeserializeObject(responseBody);
            foreach (var item in json.list)
            {
                string dateString = item.dt_txt.ToString();
                string englishDayOfWeek = DateTime.Parse(dateString).DayOfWeek.ToString();

                if (dates.Contains(englishDayOfWeek))
                {
                    continue;
                }
                dates.Add(englishDayOfWeek);

                string russianDayOfWeek = Helper.TranslateDayOfWeekToRussian(englishDayOfWeek);
                string maxTemp = $"{item.main.temp_max:0}°";
                string minTemp = $"{item.main.temp_min:0}°";
                string iconCode = item.weather[0].icon.ToString();
                string iconPath = Helper.GetImagePathByIcon(iconCode);
                int wind = item.wind.speed;
                int windDeg = item.wind.deg;
                int humidity = item.main.humidity;
                int visibility = item.visibility;
                weatherForecast.Add(
                    new WeatherDayOfWeek {
                        DayOfWeek = russianDayOfWeek,
                        MaxTemperature = maxTemp, 
                        MinTemperature = minTemp, 
                        IconPath = iconPath, 
                        Wind = wind, 
                        WindDeg = windDeg, 
                        Humidity = humidity, 
                        Visibility = visibility 
                    }
                );
            }

            return weatherForecast;
        }
    }
}
