using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WeatherReport_UKSIVT
{
    public class WindyApiClient
    {
        private const string ApiKey = "U5SV6iqiwkm7RUs675NWbaZze0ZUtzdF";
        private const string BaseUrl = "https://api.windy.com/api/webcams/v2/";

        public async Task<string> GetPrecipitationMapAsync(string cityName)
        {
            try
            {
                string apiUrl = $"https://api.windy.com/api/webcams/v2/list/nearby=55.7558,37.6176,key={ApiKey}";
                using (var client = new HttpClient())
                {
                    var response = await client.GetAsync(apiUrl);
                    response.EnsureSuccessStatusCode();
                    var responseBody = await response.Content.ReadAsStringAsync();
                    dynamic jsonData = JsonConvert.DeserializeObject(responseBody);
                    string mapUrl = jsonData["result"]["webcams"][0]["player"]["year"]["cdn_url"];
                    return mapUrl;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при получении карты осадков: {ex.Message}");
                return null;
            }
        }
    }
}
