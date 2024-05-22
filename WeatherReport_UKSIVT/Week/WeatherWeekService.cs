using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using WeatherReport_UKSIVT.API;

namespace WeatherReport_UKSIVT.Week
{
    public class WeatherWeekService
    {
        public async Task<List<WeatherDayOfWeek>> GetWeatherForecastAsync(string cityName)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    var response = await client.GetAsync(WeatherUrls.WeekForecast(cityName));

                    if (response.IsSuccessStatusCode)
                    {
                        var responseBody = await response.Content.ReadAsStringAsync();
                        Console.WriteLine(responseBody);
                        var weatherForecast = ParseWeatherForecast(responseBody);
                        return weatherForecast;
                    }
                    else if (response.StatusCode == HttpStatusCode.NotFound)
                    {
                        MessageBox.Show("Город не найден");
                        return null;
                    }
                    else
                    {
                        MessageBox.Show($"Ошибка HTTP-запроса: {response.StatusCode}");
                        return null;
                    }
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
                DateTime date = DateTime.Parse(dateString);
                string englishDayOfWeek = date.DayOfWeek.ToString();

                if (dates.Contains(englishDayOfWeek))
                {
                    continue;
                }

                dates.Add(englishDayOfWeek);
                string russianDayOfWeek = Helper.TranslateDayOfWeekToRussian(englishDayOfWeek);
                double maxTempCelsius = item.main.temp_max;
                double minTempCelsius = item.main.temp_min;
                string iconCode = item.weather[0].icon.ToString();
                string iconPath = Helper.GetImagePathByIcon(iconCode);
                int wind = item.wind.speed;
                int gustWind = item.wind.gust;
                int windDeg = item.wind.deg;
                int humidity = item.main.humidity;
                int visibility = item.visibility;
                double grnd_level = item.main.grnd_level * 0.75;
                double feels_like = item.main.feels_like;
                double _3h = item.rain?._3h ?? 0;

                weatherForecast.Add(
                    new WeatherDayOfWeek
                    {
                        DayOfWeek = russianDayOfWeek,
                        Date = date,
                        MaxTemperatureCelsius = maxTempCelsius,
                        MinTemperatureCelsius = minTempCelsius,
                        MaxTemperature = $"{maxTempCelsius:0}°C",
                        MinTemperature = $"{minTempCelsius:0}°C",
                        IconPath = iconPath,
                        Wind = wind,
                        WindDeg = windDeg,
                        GustWind = gustWind,
                        Humidity = humidity,
                        Visibility = visibility,
                        Grnd_level = grnd_level,
                        Feels_like = feels_like,
                        _3h = _3h
                    }
                );
            }

            return weatherForecast;
        }
    }
}
