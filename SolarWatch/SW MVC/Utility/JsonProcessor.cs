using System.Text.Json;
using SW_MVC.Models;

namespace SW_MVC.Utility;

public class JsonProcessor : IJsonProcessor
{
    public City Process(string data)
    {
        var json = JsonDocument.Parse(data).RootElement[0];
        
        if (json.TryGetProperty("state", out var stateElement))
        {
            return new City(json.GetProperty("name").ToString(),
                json.GetProperty("lon").GetDouble(),
                json.GetProperty("lat").GetDouble(),
                stateElement.ToString(),
                json.GetProperty("country").ToString());
        }
        else
        {
            return new City(json.GetProperty("name").ToString(),
                json.GetProperty("lon").GetDouble(),
                json.GetProperty("lat").GetDouble(),
                "-",
                json.GetProperty("country").ToString()
            );
        }
    }

    public SunriseSunsetTimes SunTimeProcess(string data)
    {
        string[] array = data.Split("   ");
        
        var json = JsonDocument.Parse(data);
        var results = json.RootElement.GetProperty("results");

        return new SunriseSunsetTimes(results.GetProperty("sunrise").ToString(), results.GetProperty("sunset").ToString());
    }
}