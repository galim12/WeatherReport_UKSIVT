using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static WeatherReport_UKSIVT.FavouritesSpisok;

public static class FavoriteCitiesManager
{
    private static readonly string filePath = "favoriteCities.txt";

    public static void SaveFavoriteCities(List<string> cities)
    {
        File.WriteAllLines(filePath, cities);
    }

    public static List<string> LoadFavoriteCities()
    {
        if (File.Exists(filePath))
        {
            return new List<string>(File.ReadAllLines(filePath));
        }
        return new List<string>();
    }

    public static List<FavoriteCity> LoadFavoriteCitiesAsObjects()
    {
        var cities = LoadFavoriteCities();
        return cities.Select(city => new FavoriteCity { CityName = city }).ToList();
    }

    public static void AddCity(string city)
    {
        var cities = LoadFavoriteCities();
        if (!cities.Contains(city))
        {
            cities.Add(city);
            SaveFavoriteCities(cities);
        }
    }
    public static List<string> LoadCities()
    {
        if (File.Exists(filePath))
        {
            return new List<string>(File.ReadAllLines(filePath));
        }

        return new List<string>();
    }

    public static void RemoveCity(string city)
    {
        var cities = LoadFavoriteCities();
        if (cities.Contains(city))
        {
            cities.Remove(city);
            SaveFavoriteCities(cities);
        }
    }
}
