namespace SW_MVC.Models;

public class City
{
    public int Id { get; init; }
    public string? Name { get; set; }
    public double Longitude { get; set; }
    public double Latitude { get; set; }
    public string? State { get; set; }
    public string? Country { get; set; }
    public SunriseSunsetTimes SunriseSunsetTimes { get; set; }

    public City(string? name, double longitude, double latitude, string? state, string? country)
    {
        Name = name;
        Longitude = longitude;
        Latitude = latitude;
        State = state;
        Country = country;
    }

    public void ChangeCityData(string name, double longitude, double latitude, string state, string county)
    {
        Name = name;
        Longitude = longitude;
        Latitude = latitude;
        State = state;
        Country = county;
    }
}