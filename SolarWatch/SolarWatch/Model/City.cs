namespace SolarWatch.Model;

public class City
{
    public int Id { get; init; }
    public string? Name { get; private set; }
    public double Longitude { get; private set; }
    public double Latitude { get; private set; }
    public string? State { get; private set; }
    public string? Country { get; private set; }

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