using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using WeatherReport_UKSIVT.API;
using WeatherReport_UKSIVT.Week;
using WeatherReport_UKSIVT;
using System;
using System.IO;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestWeekForecast()
        {

            string zoneName = "London";
            string expectedUrl = "https://api.openweathermap.org/data/2.5/forecast?q=London&units=metric&appid=f92c20202c6ad67391b1116545dc8561";

            string actualUrl = WeatherUrls.WeekForecast(zoneName);

            Assert.AreEqual(expectedUrl, actualUrl);
        }


        [TestMethod]
        public async Task ParseWeatherForecast_ShouldReturnCorrectWeatherData()
        {
            string jsonResponse = @"
    {
        'list': [
            {
                'dt_txt': '2024-11-20 12:00:00',
                'main': {
                    'temp_max': 16,
                    'temp_min': 7.2,
                    'grnd_level': 1013,
                    'humidity': 50,
                    'feels_like': 12.0
                },
                'weather': [
                    { 'icon': '10d' }
                ],
                'wind': {
                    'speed': 5,
                    'deg': 270,
                    'gust': 10
                },
                'visibility': 10000,
                'rain': { '_3h': 0.2 }
            }
        ]
    }";

            var service = new WeatherWeekService();

            var result = service.ParseWeatherForecast(jsonResponse);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);

            var dayForecast = result[0];
            Assert.AreEqual("среда", dayForecast.DayOfWeek.ToLower());
            Assert.AreEqual("/Images/rain.png", dayForecast.IconPath);
            Assert.AreEqual("16°C", dayForecast.MaxTemperature);
            Assert.AreEqual("7°C", dayForecast.MinTemperature);
            Assert.AreEqual(0.2, dayForecast._3h);
        }

        
        private static readonly string filePath = "favoriteCities.txt";
        [TestMethod]
        public void TestAddCity_ShouldAddCityToFavorites()
        {
            string cityToAdd = "London";
            FavoriteCitiesManager.AddCity(cityToAdd);

            Assert.IsTrue(File.Exists(filePath), "Файл с избранными городами не был создан.");

            var cities = File.ReadAllLines(filePath);
            Assert.IsTrue(Array.Exists(cities, city => city == cityToAdd), "Город не был добавлен в файл.");
        }
    }
}