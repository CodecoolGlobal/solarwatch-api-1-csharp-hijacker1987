namespace SolarWatchMinimal.Model;

public class SolarWatch
{

    public float Latitude { get; set; }
    public float Longitude { get; set; }
    public string Sunrise { get; set; }
    public string Sunset { get; set; }
    public string? City { get; set; }
    public string State { get; set; }
    public string Country { get; set; }
    
    public SolarWatch(float latitude, float longitude, string sunrise, string sunset, string? city, string state, string country)
    {
        Latitude = latitude;
        Longitude = longitude;
        Sunrise = sunrise;
        Sunset = sunset;
        City = city;
        State = state;
        Country = country;
    }
}