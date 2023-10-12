using System.Globalization;
using System.Text.Json;

namespace SolarWatch.Controllers;

public class JsonProcessor : IJsonProcessor
{
    public SolarWatch Process(string data, bool picker)
    {
        SolarWatch solarWatch = new SolarWatch();

        JsonDocument json = JsonDocument.Parse(data);
        if (picker)
        {
            //Access the first element in the JSON array
            JsonElement city = json.RootElement.EnumerateArray().FirstOrDefault();

            solarWatch.Latitude = (float)city.GetProperty("lat").GetDecimal();
            solarWatch.Longitude = (float)city.GetProperty("lon").GetDecimal();
            solarWatch.City = city.GetProperty("name").GetString();
        }
        else
        {
            //Access the "results" object in the JSON
            JsonElement results = json.RootElement.GetProperty("results");

            solarWatch.Sunrise = ParseTimeString(results.GetProperty("sunrise").GetString());
            solarWatch.Sunset = ParseTimeString(results.GetProperty("sunset").GetString());
        }

        return solarWatch;
    }

    private static string ParseTimeString(string? timeString)
    {
        DateTime dateTime = DateTime.ParseExact(timeString, "h:mm:ss tt", CultureInfo.InvariantCulture);
    
        //Format the DateTime as a string with the desired format
        return dateTime.ToString("h:mm:ss tt", CultureInfo.InvariantCulture);
    }

}
