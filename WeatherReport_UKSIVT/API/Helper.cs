using System.Collections.Generic;

namespace WeatherReport_UKSIVT.API
{
    public class Helper
    {
        public static string TranslateDayOfWeekToRussian(string englishDayOfWeek)
        {
            var daysOfWeekDictionary = new Dictionary<string, string>
            {
                {"Sunday", "Воскресенье"},
                {"Monday", "Понедельник"},
                {"Tuesday", "Вторник"},
                {"Wednesday", "Среда"},
                {"Thursday", "Четверг"},
                {"Friday", "Пятница"},
                {"Saturday", "Суббота"}
            };

            return daysOfWeekDictionary.TryGetValue(englishDayOfWeek, out var russianDayOfWeek) ? russianDayOfWeek : null;
        }

        public static string GetImagePathByIcon(string iconCode)
        {
            string imagePath;
            switch (iconCode)
            {
                case "01d": // ясно (день)
                    imagePath = "/Images/sun.png";
                    break;
                case "02d": // немного облаков (день)
                    imagePath = "/Images/sun_cloud.png";
                    break;
                case "03d": // облачно (день)
                    imagePath = "/Images/sun_cloud.png";
                    break;
                case "04d": // облачно (день)
                    imagePath = "/Images/sun_cloud.png";
                    break;
                case "09d": // дождь (день)
                    imagePath = "/Images/rain_cloud.png";
                    break;
                case "10d": // ливень (день)
                    imagePath = "/Images/rain.png";
                    break;
                case "11d": // гроза (день)
                    imagePath = "/Images/storm.png";
                    break;
                case "01n": // чистое небо (ночь)
                    imagePath = "/Images/moon.png";
                    break;
                case "02n": // облачно (ночь)
                    imagePath = "/Images/moon_cloud.png";
                    break;
                case "03n": // чуть облачность (ночь)
                    imagePath = "/Images/moon.png";
                    break;
                case "04n": // еще облачно (ночь)
                    imagePath = "/Images/moon_cloud.png";
                    break;               
                case "09n": // дождь (ночь)
                    imagePath = "/Images/cloud.png";
                    break;
                case "10n": // ливень (ночь)
                    imagePath = "/Images/rain.png";
                    break;
                case "11n": // гроза (ночь)
                    imagePath = "/Images/storm.png";
                    break;
                case "13d": // снег (день)
                    imagePath = "/Images/snow.png";
                    break;
                case "13n": // снег (ночь)
                    imagePath = "/Images/snow.png";
                    break;
                case "50d": // туман (день)
                    imagePath = "/Images/fog_day.png";
                    break;
                case "50n": // туман (ночь)
                    imagePath = "/Images/fog_night.png";
                    break;
                default:
                    imagePath = "/Images/rain.png";
                    break;
            }
            return imagePath;
        }
        public static string DayOfWeekToCorrect(string dayOfWeek)
        {
            switch (dayOfWeek)
            {
                case "Понедельник": return "понедельник";
                case "Вторник": return "вторник";
                case "Среда": return "среду";
                case "Четверг": return "четверг";
                case "Пятница": return "пятницу";
                case "Суббота": return "субботу";
                case "Воскресенье": return "воскресенье";
                default: return "";
            }
        }
    }
}
