using System;

namespace WeatherReport_UKSIVT.Week
{
    public class WeatherDayOfWeek
    {
        public string DayOfWeek { get; set; }
        public DateTime Date { get; set; }
        public string MaxTemperature { get; set; }
        public string MinTemperature { get; set; }
        public string IconPath { get; set; }
        public int Wind { get; set; }
        public int WindDeg { get; set; }
        public int GustWind { get; set; }
        public int Humidity { get; set; }
        public int Visibility { get; set; }
        public double Grnd_level { get; set; }
        public double Feels_like { get; set; }
        public double _3h { get; set; }


    }
}
